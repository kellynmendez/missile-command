using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMissile : MonoBehaviour
{
    [SerializeField] GameObject _particleSystem;
    private Plane _plane;
    private Vector3 _clickedPosition;
    private float _explosionZ = 0f;

    private void Awake()
    {
        _plane = new Plane(Vector3.forward, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out distance))
            {
                Debug.Log("clicked");
                _clickedPosition = ray.GetPoint(distance);
                _clickedPosition.z = _explosionZ;
                Instantiate(_particleSystem);
                _particleSystem.transform.position = _clickedPosition;
            }
        }
        
    }
}
