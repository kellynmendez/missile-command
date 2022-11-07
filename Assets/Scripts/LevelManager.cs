using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] NormalBombDropper[] _normalDroppers;
    [SerializeField] SplitBombDropper[] _splitDroppers;
    [SerializeField] BomberDropper[] _bomberDroppers;

    private UIManager _uiManager;
    private int _cityCount = 6;
    private bool _waveFinished = false;
    private bool _levelFinished = false;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        Time.timeScale = 0;
        _uiManager.ShowStartText(true);
    }

    private void Update()
    {
        if (!_levelFinished)
        {
            // Checking if all bomb waves are done
            bool checkFinished = true;
            int i = 0;
            // Checking normal bombs first
            while (i < _normalDroppers.Length && checkFinished)
            {
                if (!_normalDroppers[i].GetWaveFinished())
                {
                    checkFinished = false;
                }
                i++;
            }
            // Checking split bombs second
            i = 0;
            while (i < _splitDroppers.Length && checkFinished)
            {
                if (!_splitDroppers[i].GetWaveFinished())
                {
                    checkFinished = false;
                }
                i++;
            }
            // Checking bombers third
            i = 0;
            while (i < _bomberDroppers.Length && checkFinished)
            {
                if (!_bomberDroppers[i].GetWaveFinished())
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
                _uiManager.LevelLost();
                _levelFinished = true;
            }
            else if (_waveFinished)
            {
                _uiManager.LevelFinished();
                _levelFinished = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // Reloading the level
            int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(activeSceneIndex);
            // Unfreezing screen
            if (Time.timeScale == 0)
                Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            _uiManager.ShowStartText(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
