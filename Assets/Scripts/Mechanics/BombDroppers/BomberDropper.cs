using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberDropper : MonoBehaviour
{
    // Targets are the three missile bases and the six cities
    [SerializeField] GameObject[] _targetsArray;
    [SerializeField] Bomber _bomberPrefab;
    [SerializeField] Bomb _enemyBomb;
    [SerializeField] GameObject _bombExplosion;
    [SerializeField] GameObject _endPoint;
    [SerializeField] float _bomberSpeed = 1f;
    [SerializeField] float _startDelay = 15f;
    [SerializeField] float _dropInterval = 6f;

    private List<GameObject> _targets;
    private int _numBombs = 2;
    int _bombsDestroyed = 0;
    bool _waveFinished = false;
    private Bomber _bomber;
    List<Bomb> _bombsDropped;

    private void Awake()
    {
        _targets = new List<GameObject>();
        for (int i = 0; i < _targetsArray.Length; i++)
        {
            _targets.Add(_targetsArray[i]);
        }

        _bombsDropped = new List<Bomb>();
        for (int i = 0; i < _numBombs; i++)
        {
            Bomb bomb = Instantiate(_enemyBomb);
            bomb.gameObject.SetActive(false);
            _bombsDropped.Add(bomb);
        }
    }
    
    void Start()
    {
        StartCoroutine(StartBombWave());
    }

    void Update()
    {
        // Checking if all bombs that have been released have been destroyed
        _bombsDestroyed = 0;
        foreach (Bomb bomb in _bombsDropped)
        {
            if (!bomb.BombActive())
            {
                _bombsDestroyed++;
            }
        }
        // If they have, then the wave of bombs is finished
        if (_bombsDestroyed >= _bombsDropped.Count)
        {
            _waveFinished = true;
        }
    }

    private IEnumerator StartBombWave()
    {
        yield return new WaitForSeconds(_startDelay);
        // Instantiating the bomber
        _bomber = Instantiate(_bomberPrefab);
        // Calculating distance to use for speed of lerp
        float distance = Vector3.Distance(transform.position, _endPoint.transform.position);
        // Move across screen
        StartCoroutine(LerpAcrossScreen(
            _bomber.transform,
            transform.position,
            _endPoint.transform.position,
            distance / _bomberSpeed
            ));
        // Drop bombs
        StartCoroutine(DropBombs());
    }

    private GameObject RandomTarget()
    {
        int chosenTargetIdx = Random.Range(0, _targets.Count);
        return _targets[chosenTargetIdx];
    }

    private IEnumerator LerpAcrossScreen(Transform bomber, Vector3 from, Vector3 to,
        float duration)
    {
        // Instantiating bomb and setting initial position
        bomber.position = from;

        // Lerp position
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if (bomber)
            {
                bomber.position = Vector3.Lerp(from, to, elapsedTime / duration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (bomber)
            bomber.position = to;

        yield break;
    }

    private IEnumerator DropBombs()
    {
        for (int i = 0; i < _bombsDropped.Count; i++)
        {
            yield return new WaitForSeconds(_dropInterval);
            if (_bomber.BomberActive())
            {
                Vector3 startPosition = _bomber.GetLaunchPoint();
                // Activating the new bomb
                Bomb bomb = _bombsDropped[i];
                bomb.gameObject.SetActive(true);
                bomb.gameObject.transform.position = startPosition;
                // Getting target
                GameObject cloneTarget = RandomTarget();
                // Removing target from target list
                _targets.Remove(cloneTarget);
                // Dropping bomb
                bomb.Drop(startPosition, cloneTarget.transform.position);
            }
            // If the bomber was killed before dropping, destroy all instantiated bombs
            else
            {
                for (int k = 0; k < _bombsDropped.Count; k++)
                {
                    if (_bombsDropped[k] && !_bombsDropped[k].gameObject.activeSelf)
                    {
                        Destroy(_bombsDropped[k].gameObject);
                    }
                }

                // Clear the list after destroying all bombs that were isntantiated
                _bombsDropped.Clear();
            }
        }
    }
    public bool GetWaveFinished()
    {
        return _waveFinished;
    }
}
