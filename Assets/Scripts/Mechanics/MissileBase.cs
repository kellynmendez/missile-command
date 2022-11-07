using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBase : MonoBehaviour
{
    [SerializeField] GameObject[] _missiles;
    [SerializeField] GameObject _explosion;
    [SerializeField] GameObject _explosionPoint;
    [SerializeField] int _lowMissileNumber = 3;

    [Header("Feedback")]
    [SerializeField] AudioClip _explodeSFX = null;
    private AudioSource _audioSource;

    private int _index = 0;
    private int _missilesLeft = 10;
    private bool _active;
    private bool _lowOnMissiles = false;

    private void Awake()
    {
        _active = true;
        _missilesLeft = _missiles.Length;
        _audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (_missilesLeft <= _lowMissileNumber)
        {
            _lowOnMissiles = true;
        }
    }

    public void RemoveMissile()
    {
        if (_active)
        {
            GameObject go = _missiles[_index];
            Destroy(go);
            _index++;
            _missilesLeft--;

            if (_index >= _missiles.Length)
            {
                _active = false;
            }
        }
    }

    public void DestroyBase()
    {
        _active = false;
        _missilesLeft = 0;
        // Play explosion
        StartCoroutine(PlayExplosion(_explosionPoint.transform.position));
        // Destroying the missiles at the base
        for (int i = _index; i < _missiles.Length; i++)
        {
            Destroy(_missiles[i]);
        }
    }

    public int GetMissilesLeft()
    {
        return _missilesLeft;
    }

    public bool BaseActive()
    {
        return _active;
    }

    public bool LowOnMissiles()
    {
        return _lowOnMissiles;
    }

    private IEnumerator PlayExplosion(Vector3 position)
    {
        PlayExplodeSFX();
        // Instantiate explosion at given position
        GameObject go = Instantiate(_explosion);
        go.transform.position = position;

        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        // Wait for the particle system to be done playing
        while (ps.isPlaying)
        {
            yield return null;
        }
        // Destroy the particle system object then destroy bomb object
        Destroy(ps.gameObject);
        Destroy(gameObject);
    }

    public void PlayExplodeSFX()
    {
        if (_audioSource != null && _explodeSFX != null)
        {
            _audioSource.PlayOneShot(_explodeSFX, _audioSource.volume);
        }
    }
}
