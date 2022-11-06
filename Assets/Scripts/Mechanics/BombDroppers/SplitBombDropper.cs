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
    int _bombsDestroyed = 0;
    bool _waveFinished = false;
    List<Bomb> _bombsDropped;

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
        _bombsDestroyed = 0;
        foreach (Bomb bomb in _bombsDropped)
        {
            if (!bomb.BombActive())
            {
                _bombsDestroyed++;
            }
        }

        if (_bombsDestroyed == _bombsDropped.Count)
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

        for (int i = 1; i < _bombsDropped.Count; i++)
        {
            // Getting first target
            GameObject cloneTarget = RandomTarget();
            // Removing target from target list
            _targets.Remove(cloneTarget);
            // Dropping both bombs
            if (ogBomb)
            {
                Vector3 startPosition = ogBomb.transform.position;
                // Activating the new bombs
                Bomb cloneBomb = _bombsDropped[i];
                cloneBomb.gameObject.SetActive(true);
                cloneBomb.gameObject.transform.position = startPosition;
                cloneBomb.Drop(startPosition, cloneTarget.transform.position);
            }
        }
    }

    public bool GetWaveFinished()
    {
        return _waveFinished;
    }
}
