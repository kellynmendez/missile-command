using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int _cityCount = 6;
    private bool _waveFinished = false;

    private void Update()
    {
        if (_cityCount == 0)
        {
            
        }
        else if (_waveFinished)
        {

        }
    }

    public void DecrementCityCount()
    {
        _cityCount--;
    }

    public void WaveFinished()
    {
        _waveFinished = true;
    }
}
