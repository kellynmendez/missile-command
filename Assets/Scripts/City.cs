using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] GameObject _visualsToDeactivate;
    [SerializeField] Collider _colliderToDeactivate;
    [SerializeField] GameObject _explosionPS;
    [SerializeField] GameObject _explosionPoint;
    bool _active = true;

    public void DestroyCity()
    {
        // Playing explosion
        StartCoroutine(PlayExplosion(_explosionPoint.transform.position, _explosionPS));
        // Deactivating visuals and collider
        _visualsToDeactivate.SetActive(false);
        _colliderToDeactivate.enabled = false;
        // Playing sound effect TODO

    }

    private IEnumerator PlayExplosion(Vector3 position, GameObject explosion)
    {
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
}
