using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _missileExplosion;
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
            // Launching the missile
            LaunchMissile();
        }
        
    }

    private void LaunchMissile()
    {
        // Using ray and invisible plane to get position of mouse click
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out distance))
        {
            // Getting position of click
            _clickedPosition = ray.GetPoint(distance);
            // Make sure all explosions are happening at same depth
            _clickedPosition.z = _explosionZ;
            // Instantiating explosion at mouse click position TODO: change later
            GameObject go = Instantiate(_missileExplosion);
            go.transform.position = _clickedPosition;
            StartCoroutine(CheckIfParticleDone(go.GetComponent<ParticleSystem>()));
        }
    }

    IEnumerator CheckIfParticleDone(ParticleSystem ps)
    {
        // Wait for the particle system to be done playing
        while (ps.isPlaying)
        {
            yield return null;
        }
        // When particle system is done playing, destroy the particle system object
        Destroy(ps.gameObject);
    }
}
