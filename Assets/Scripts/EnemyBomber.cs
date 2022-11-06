using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : MonoBehaviour
{
    // Targets are the three missile bases and the six cities
    [SerializeField] GameObject[] _targetsArray;
    [SerializeField] Bomb _enemyBomb;
    [SerializeField] GameObject _bombExplosion;
    [SerializeField] int _normalBombNum = 2;
    [SerializeField] float _interval = 15f;
    [SerializeField] float _startDelay = 5f;

    private List<GameObject> _targets;

    private void Awake()
    {
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
        
    }

    private IEnumerator StartBombWave()
    {
        yield return new WaitForSeconds(_startDelay);

        for (int i = 0; i < _normalBombNum; i++)
        {
            // Instantiating the bomb
            Bomb bomb = Instantiate(_enemyBomb);
            bomb.gameObject.transform.position = transform.position;
            // Getting the target
            GameObject target = RandomTarget();
            // Removing target from target list
            _targets.Remove(target);
            // Dropping the bomb
            bomb.DropBomb(transform.position, target.transform.position);
            yield return new WaitForSeconds(_interval);
        }
    }

    private GameObject RandomTarget()
    {
        int chosenTargetIdx = Random.Range(0, _targets.Count);
        return _targets[chosenTargetIdx];
    }
}
