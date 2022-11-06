using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBomber : MonoBehaviour
{
    // Targets are the three missile bases and the six cities
    [SerializeField] GameObject[] _targetsArray;
    [SerializeField] Bomb _enemyBomb;
    [SerializeField] GameObject _bombExplosion;
    [SerializeField] float _interval = 15f;
    [SerializeField] float _startDelay = 5f;
    [SerializeField] float _splitDelay = 10f;

    private List<GameObject> _targets;
    private int _numBombs = 3;
    int _bombsDestroyed = 0;
    bool _waveFinished = false;
    List<Bomb> _bombsDropped;

    private void Awake()
    {
        _bombsDropped = new List<Bomb>();
        _targets = new List<GameObject>();
        for (int i = 0; i < _targetsArray.Length; i++)
        {
            _targets.Add(_targetsArray[i]);
        }
    }

    void Start()
    {
        StartCoroutine(StartBombWave());
    }

    void Update()
    {
        foreach (Bomb bomb in _bombsDropped)
        {
            if (!bomb.BombActive())
            {
                _bombsDestroyed++;
            }
        }

        if (_bombsDestroyed == _numBombs)
        {
            _waveFinished = true;
        }
    }

    private IEnumerator StartBombWave()
    {
        yield return new WaitForSeconds(_startDelay);
        StartCoroutine(DropAndSplitBomb());
        yield return new WaitForSeconds(_interval);
    }

    private GameObject RandomTarget()
    {
        int chosenTargetIdx = Random.Range(0, _targets.Count);
        return _targets[chosenTargetIdx];
    }

    private IEnumerator DropAndSplitBomb()
    {
        // Instantiating the bomb
        Bomb bomb = Instantiate(_enemyBomb);
        bomb.gameObject.transform.position = transform.position;
        _bombsDropped.Add(bomb);
        // Getting the target
        GameObject target = RandomTarget();
        // Removing target from target list
        _targets.Remove(target);
        // Dropping the bomb
        bomb.Drop(transform.position, target.transform.position);

        yield return new WaitForSeconds(_splitDelay);

        /*****Splitting the bomb*****/

        // Getting first target
        GameObject target1 = RandomTarget();
        // Removing target from target list
        _targets.Remove(target1);
        // Getting second target
        GameObject target2 = RandomTarget();
        // Removing target from target list
        _targets.Remove(target2);
        // Dropping both bombs
        if (bomb)
        {
            Vector3 startPosition = bomb.transform.position;
            // Instantiating two new bombs
            Bomb bomb1 = Instantiate(_enemyBomb);
            Bomb bomb2 = Instantiate(_enemyBomb);
            bomb1.gameObject.transform.position = startPosition;
            bomb2.gameObject.transform.position = startPosition;
            bomb1.Drop(startPosition, target1.transform.position);
            bomb2.Drop(startPosition, target2.transform.position);
            _bombsDropped.Add(bomb1);
            _bombsDropped.Add(bomb2);
        }
        
    }

    public bool GetWaveFinished()
    {
        return _waveFinished;
    }
}
