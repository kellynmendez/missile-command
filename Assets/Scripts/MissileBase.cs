using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBase : MonoBehaviour
{
    [SerializeField] GameObject[] _missiles;
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
        for (int i = _index; i < _missiles.Length; i++)
        {
            Destroy(_missiles[i]);
        }
    }

    public bool GetBaseDestroyed()
    {
        return _baseDestroyed;
    }
}
