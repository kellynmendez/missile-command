using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] NormalBombDropper[] _bombers;
    [SerializeField] SplitBombDropper[] _splitBombers;

    private int _cityCount = 6;
    private bool _waveFinished = false;
    private bool _levelFinished = false;

    private void Update()
    {
        if (!_levelFinished)
        {
            // Checking if all bomb waves are done
            bool checkFinished = true;
            int i = 0;
            // Checking normal bombs first
            while (i < _bombers.Length && checkFinished)
            {
                if (!_bombers[i].GetWaveFinished())
                {
                    checkFinished = false;
                }
                i++;
            }
            // Checking split bombs second
            i = 0;
            while (i < _splitBombers.Length && checkFinished)
            {
                if (!_splitBombers[i].GetWaveFinished())
                {
                    checkFinished = false;
                }
                i++;
            }
            // If the flag was never set to false, then all bomb waves are done
            if (checkFinished)
            {
                _waveFinished = checkFinished;
            }

            // Checking if cities have all been destroyed
            if (_cityCount == 0)
            {
                UIManager.Instance.LevelLost();
                _levelFinished = true;
            }
            else if (_waveFinished)
            {
                UIManager.Instance.LevelFinished();
                _levelFinished = true;
            }
        }
    }

    public void DecrementCityCount()
    {
        _cityCount--;
    }

    public bool LevelFinished()
    {
        return _levelFinished;
    }
}
