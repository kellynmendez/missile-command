using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Particle system prefab
    [SerializeField] GameObject _missileExplosion;
    // Sphere missile prefab that will be shot
    [SerializeField] GameObject _missile;
    // The three missile bases
    [SerializeField] MissileBase[] _missileBases;
    // Launch points of all three mmissile bases
    [SerializeField] GameObject[] _baseLaunchPoints;
    // Speed of missile
    [SerializeField] float _speed = 40f;

    // Plane used to determine mouse click position in 3d world space
    private Plane _plane;
    // Which base was chosen to shoot from
    ChosenBase _base;
    // Whether the bases have been destroyed
    bool _leftBaseDestroyed = false;
    bool _middleBaseDestroyed = false;
    bool _rightBaseDestroyed = false;

    enum ChosenBase
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }

    private void Awake()
    {
        _plane = new Plane(Vector3.forward, 0);
    }

    void Update()
    {
        _leftBaseDestroyed = _missileBases[0].GetBaseDestroyed();
        _middleBaseDestroyed = _missileBases[1].GetBaseDestroyed();
        _rightBaseDestroyed = _missileBases[2].GetBaseDestroyed();

        if (!_leftBaseDestroyed || !_middleBaseDestroyed || !_rightBaseDestroyed)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Launching the missile
                LaunchMissile();
            }
        }
    }

    private void LaunchMissile()
    {
        // Using ray and invisible plane to get position of mouse click
        Vector3 clickedPosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out float distance))
        {
            // Getting position of click
            clickedPosition = ray.GetPoint(distance);
            // Getting closest missile base
            GameObject missileBase = FindClosestMissileBase(clickedPosition);
            // Instantiating explosion at mouse click position TODO: change later
            GameObject missile = Instantiate(_missile);
            missile.transform.position = missileBase.transform.position;
            // Calculating distance for the missile to travel
            float travelDistance = Vector3.Distance(missile.transform.position, clickedPosition);
            // Using distance to determine duration of lerp
            StartCoroutine(LaunchRoutine(
                    missile.transform,
                    missileBase.transform.position, 
                    clickedPosition,
                    travelDistance / _speed,
                    _missileExplosion));
            // Reducing missile number at the chosen base
            ReduceMissileNumberAtBase();
        }
    }

    private GameObject FindClosestMissileBase(Vector3 clickedPosition)
    {
        // Setting distances as very large number so it will never be chosen as a minimum
        //      if not reset; distances will be reset if the base exists
        float leftDist = 1000000;
        float midDist = 1000000;
        float rightDist = 1000000;
        // Getting distance from each base
        if (!_leftBaseDestroyed)
        {
            leftDist = Vector3.Distance(_baseLaunchPoints[0].transform.position, clickedPosition);
        }
        if (!_middleBaseDestroyed)
        {
            midDist = Vector3.Distance(_baseLaunchPoints[1].transform.position, clickedPosition);
        }
        if (!_rightBaseDestroyed)
        {
            rightDist = Vector3.Distance(_baseLaunchPoints[2].transform.position, clickedPosition);
        }

        // Determining which one is smallest and updating chosen base
        float minDist = Mathf.Min(leftDist, midDist, rightDist);
        if (minDist == leftDist)
        {
            _base = ChosenBase.Left;
            return _baseLaunchPoints[0];
        }
        else if (minDist == midDist)
        {
            _base = ChosenBase.Middle;
            return _baseLaunchPoints[1];
        }
        else
        {
            _base = ChosenBase.Right;
            return _baseLaunchPoints[2];
        }
    }

    private void ReduceMissileNumberAtBase()
    {
        if (_base == ChosenBase.Left)
        {
            _missileBases[0].MissileLaunched();
        }
        else if (_base == ChosenBase.Middle)
        {
            _missileBases[1].MissileLaunched();
        }
        else
        {
            _missileBases[2].MissileLaunched();
        }
    }

    private IEnumerator LaunchRoutine(Transform missile, Vector3 from, Vector3 to, 
        float duration, GameObject explosion)
    {
        // Instantiating missile and setting initial position
        missile.position = from;

        // Lerp position
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            missile.position = Vector3.Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroying missile once it reaches the clicked position
        Destroy(missile.gameObject);

        // Instantiate explosion at clicked position
        GameObject go = Instantiate(explosion);
        go.transform.position = to;

        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        // Wait for the particle system to be done playing
        while (ps.isPlaying)
        {
            yield return null;
        }
        // When particle system is done playing, destroy the particle system object
        Destroy(ps.gameObject);

        yield break;
    }
}
