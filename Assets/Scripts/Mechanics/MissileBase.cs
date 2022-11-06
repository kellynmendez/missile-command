using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBase : MonoBehaviour
{
    [SerializeField] GameObject[] _missiles;
    [SerializeField] GameObject _explosion;
    [SerializeField] GameObject _explosionPoint;
    private int _index = 0;
    private int _missilesLeft;
    private bool _active = true;

    private void Awake()
    {
        _missilesLeft = _missiles.Length;
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
