using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField] GameObject _bombExplosion;
    [SerializeField] GameObject _launchPoint;
    [SerializeField] Collider _colliderToDeactivate;
    [SerializeField] GameObject _visualsToDeactivate;
    private bool _active = true;

    public void DestroyBomber(bool playExplosion, bool destroyedByMissile)
    {
        _active = false;
        // Deactivating collider and visuals
        _colliderToDeactivate.enabled = false;
        _visualsToDeactivate.SetActive(false);
        // Increment score
        if (destroyedByMissile)
        {
            UIManager.Instance.BomberHitIncrementScore();
        }
        // Instantiating explosion
        if (playExplosion)
        {
            StartCoroutine(PlayExplosion(transform.position));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator PlayExplosion(Vector3 position)
    {
        // Instantiate explosion at given position
        GameObject go = Instantiate(_bombExplosion);
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

    public Vector3 GetLaunchPoint()
    {
        return _launchPoint.transform.position;
    }

    public bool BomberActive()
    {
        return _active;
    }
}
