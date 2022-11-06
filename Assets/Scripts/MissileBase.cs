using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBase : MonoBehaviour
{
    [SerializeField] GameObject[] _missiles;
    [SerializeField] GameObject _explosion;
    [SerializeField] GameObject _explosionPoint;
    int _index = 0;
    bool _baseDestroyed = false;

    public void MissileLaunched()
    {
        if (!_baseDestroyed)
        {
            GameObject go = _missiles[_index];
            Destroy(go);
            _index++;

            if (_index >= _missiles.Length)
            {
                _baseDestroyed = true;
            }
        }
    }

    public void DestroyBase()
    {
        _baseDestroyed = true;
        // Play explosion
        StartCoroutine(PlayExplosion(_explosionPoint.transform.position));
        // Destroying the missiles at the base
        for (int i = _index; i < _missiles.Length; i++)
        {
            Destroy(_missiles[i]);
        }
    }

    public bool GetBaseDestroyed()
    {
        return _baseDestroyed;
    }

    private IEnumerator PlayExplosion(Vector3 position)
    {
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
}
