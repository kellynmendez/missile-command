using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject _bombExplosion;
    [SerializeField] Collider _colliderToDeactivate;
    [SerializeField] GameObject _visualsToDeactivate;
    [SerializeField] GameObject _trailToDeactivate;
    [SerializeField] float _speed = 10f;
    private bool _active = true;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Entered {other.gameObject.name}");
        if (_active)
        {
            if (other.tag == "City")
            {
                // Destroying bomb, don't play explosion
                DestroyBomb(false, false);
                // Destroying city
                other.GetComponent<City>().DestroyCity();
            }
            else if (other.tag == "MissileBase")
            {
                // Destroying bomb, don't play explosion
                DestroyBomb(false, false);
                // Destroying city
                other.GetComponent<MissileBase>().DestroyBase();
            }
            else if (other.tag == "Plane")
            {
                // Destroying bomb, play explosion
                DestroyBomb(true, false);
            }
        }
    }

    public void Drop(Vector3 from, Vector3 to)
    {
        // Calculating distance for the bomb to travel
        float distance = Vector3.Distance(from, to);
        // Using distance to determine duration of lerp
        StartCoroutine(BombRoutine(transform, from, to, distance / _speed));
    }

    // lerps the bomb to target position
    private IEnumerator BombRoutine(Transform bomb, Vector3 from, Vector3 to,
        float duration)
    {
        // Instantiating bomb and setting initial position
        bomb.position = from;

        // Lerp position
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            bomb.position = Vector3.Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bomb.position = to;

        yield break;
    }

    public void DestroyBomb(bool playExplosion, bool destroyedByMissile)
    {
        _active = false;
        // Deactivating collider and visuals
        _colliderToDeactivate.enabled = false;
        _visualsToDeactivate.SetActive(false);
        _trailToDeactivate.SetActive(false);
        // Increment score
        if (destroyedByMissile)
        {
            UIManager.Instance.BombHitIncrementScore();
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

    public bool BombActive()
    {
        return _active;
    }
}
