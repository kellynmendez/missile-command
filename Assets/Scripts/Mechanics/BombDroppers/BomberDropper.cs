using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberDropper : MonoBehaviour
{
    // Targets are the three missile bases and the six cities
    [SerializeField] GameObject[] _targetsArray;
    [SerializeField] Bomb _enemyBomb;
    [SerializeField] GameObject _bombExplosion;
    [SerializeField] float _startDelay = 5f;
    [SerializeField] float _dropDelay = 10f;

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
    /*
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
        // Instantiating the bomber
        Bomb bomb = Instantiate(_enemyBomb);
        bomb.gameObject.transform.position = transform.position;
        _bombsDropped.Add(bomb);
        // Getting the target
        GameObject target = RandomTarget();
        // Removing target from target list
        _targets.Remove(target);
        // Dropping the bomb
        bomb.Drop(transform.position, target.transform.position);
        StartCoroutine(DropAndSplitBomb());
    }

    private GameObject RandomTarget()
    {
        int chosenTargetIdx = Random.Range(0, _targets.Count);
        return _targets[chosenTargetIdx];
    }

    private IEnumerator LerpAcrossScreen(Transform bomb, Vector3 from, Vector3 to,
        float duration)
    {
        // Instantiating bomb and setting initial position
        bomb.position = from;

        // Lerp position
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            bomb.position = Vector3.Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bomb.position = to;

        yield break;
    }

    private IEnumerator DropBombs()
    {
        yield return new WaitForSeconds(_dropDelay);

        // Getting first target
        GameObject target1 = RandomTarget();
        // Removing target from target list
        _targets.Remove(target1);
        // Dropping both bombs
        Vector3 startPosition = transform.position;
        // Instantiating new bomb
        Bomb bomb1 = Instantiate(_enemyBomb);
        bomb1.gameObject.transform.position = startPosition;
        bomb1.Drop(startPosition, target1.transform.position);
        _bombsDropped.Add(bomb1);

        yield return new WaitForSeconds(_dropDelay);

        // Getting second target
        GameObject target2 = RandomTarget();
        // Removing target from target list
        _targets.Remove(target2);
        // Instantiating new bomb
        startPosition = transform.position;
        Bomb bomb2 = Instantiate(_enemyBomb);
        bomb2.gameObject.transform.position = startPosition;
        bomb2.Drop(startPosition, target2.transform.position);
        _bombsDropped.Add(bomb2);
    }

    public bool GetWaveFinished()
    {
        return _waveFinished;
    }
    */
}
