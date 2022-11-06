using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : MonoBehaviour
{
    // Targets are the three missile bases and the six cities
    [SerializeField] GameObject[] _targetsArray;
    [SerializeField] Bomb _enemyBomb;
    [SerializeField] GameObject _bombExplosion;
    [SerializeField] int _bombNum = 2;
    [SerializeField] float _interval = 15f;
    [SerializeField] float _startDelay = 5f;

    int _bombsDestroyed = 0;
    bool _waveFinished = false;
    List<Bomb> _bombsDropped;

    private List<GameObject> _targets;

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

    private void Update()
    {
        _bombsDestroyed = 0;
        foreach (Bomb bomb in _bombsDropped)
        {
            if (!bomb.BombActive())
            {
                _bombsDestroyed++;
            }
        }

        if (_bombsDestroyed == _bombNum)
        {
            _waveFinished = true;
        }
    }

    private IEnumerator StartBombWave()
    {
        yield return new WaitForSeconds(_startDelay);

        for (int i = 0; i < _bombNum; i++)
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
            yield return new WaitForSeconds(_interval);
        }
    }

    private GameObject RandomTarget()
    {
        int chosenTargetIdx = Random.Range(0, _targets.Count);
        return _targets[chosenTargetIdx];
    }

    public bool GetWaveFinished()
    {
        return _waveFinished;
    }
}
