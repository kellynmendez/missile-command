using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] GameObject _visualsToDeactivate;
    [SerializeField] Collider _colliderToDeactivate;
    bool _active = true;

    public void Destroyed()
    {
        // Deactivating visuals and collider
        _visualsToDeactivate.SetActive(false);
        _colliderToDeactivate.enabled = false;
        // Playing sound effect TODO
    }
}
