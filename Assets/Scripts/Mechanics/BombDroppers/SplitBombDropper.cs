using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBombDropper : MonoBehaviour
{
    // Targets are the three missile bases and the six cities
    [SerializeField] GameObject[] _targetsArray;
    [SerializeField] Bomb _enemyBomb;
    [SerializeField] GameObject _bombExplosion;
    [SerializeField] float _startDelay = 5f;
    [SerializeField] float _splitDelay = 10f;

    private List<GameObject> _targets;
    private int _numBombs = 3;
    private int _bombsDestroyed = 0;
    private bool _waveFinished = false;
    private List<Bomb> _bombsDropped;

    private void Awake()
    {
        _targets = new List<GameObject>();
        for (int i = 0; i < _targetsArray.Length; i++)
        {
            _targets.Add(_targetsArray[i]);
        }

        _bombsDropped = new List<Bomb>();
        // First bomb is start bomb, the rest are activated when bomb splits
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
        StartCoroutine(DropAndSplitBomb());
    }

    private GameObject RandomTarget()
    {
        int chosenTargetIdx = Random.Range(0, _targets.Count);
        return _targets[chosenTargetIdx];
    }

    private IEnumerator DropAndSplitBomb()
    {
        // Activating the original bomb
        Bomb ogBomb = _bombsDropped[0];
        ogBomb.gameObject.SetActive(true);
        ogBomb.gameObject.transform.position = transform.position;
        _bombsDropped.Add(ogBomb);
        // Getting the target
        GameObject target = RandomTarget();
        // Removing target from target list
        _targets.Remove(target);
        // Dropping the bomb
        ogBomb.Drop(transform.position, target.transform.position);

        yield return new WaitForSeconds(_splitDelay);

        /*****Splitting the bomb*****/

        if (ogBomb)
        {
            for (int i = 1; i < _bombsDropped.Count; i++)
            {
                Vector3 startPosition = ogBomb.transform.position;
                // Activating the new bomb
                Bomb cloneBomb = _bombsDropped[i];
                cloneBomb.gameObject.SetActive(true);
                cloneBomb.gameObject.transform.position = startPosition;
                // Getting target
                GameObject cloneTarget = RandomTarget();
                // Removing target from target list
                _targets.Remove(cloneTarget);
                // Dropping bomb
                cloneBomb.Drop(startPosition, cloneTarget.transform.position);
            }
        }
        // If the original bmob was killed before splitting, destroy all instantiated bombs
        else
        {
            for (int i = 0; i < _bombsDropped.Count; i++)
            {
                if (_bombsDropped[i] && !_bombsDropped[i].gameObject.activeSelf)
                {
                    Destroy(_bombsDropped[i].gameObject);
                }
            }

            // Clear the list after destroying all bombs that were isntantiated
            _bombsDropped.Clear();
        }
    }

    public bool GetWaveFinished()
    {
        return _waveFinished;
    }
}
