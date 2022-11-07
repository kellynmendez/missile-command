using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] GameObject _visualsToDeactivate;
    [SerializeField] Collider _colliderToDeactivate;
    [SerializeField] GameObject _explosionPS;
    [SerializeField] GameObject _explosionPoint;

    [Header("Feedback")]
    [SerializeField] AudioClip _explodeSFX = null;
    private AudioSource _audioSource;

    private bool _active = true;
    private LevelManager _levelManager;

    private void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void DestroyCity()
    {
        _active = false;
        // Playing explosion
        StartCoroutine(PlayExplosion(_explosionPoint.transform.position, _explosionPS));
        // Deactivating visuals and collider
        _visualsToDeactivate.SetActive(false);
        _colliderToDeactivate.enabled = false;
        // Decrementing city count
        _levelManager.DecrementCityCount();
        // Playing sound effect TODO

    }

    // For UI bonus points routine
    public void RemoveCity()
    {
        _active = false;
        // Deactivating visuals and collider
        _visualsToDeactivate.SetActive(false);
        _colliderToDeactivate.enabled = false;
    }

    private IEnumerator PlayExplosion(Vector3 position, GameObject explosion)
    {
        PlayExplodeSFX();
        // Instantiate explosion at clicked position
        GameObject go = Instantiate(explosion);
        go.transform.position = position;

        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        // Wait for the particle system to be done playing
        while (ps.isPlaying)
        {
            yield return null;
        }
        // When particle system is done playing, destroy the particle system object
        Destroy(ps.gameObject);
    }

    public bool CityActive()
    {
        return _active;
    }

    public void PlayExplodeSFX()
    {
        if (_audioSource != null && _explodeSFX != null)
        {
            _audioSource.PlayOneShot(_explodeSFX, _audioSource.volume);
        }
    }
}