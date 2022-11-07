using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager Instance = null;

    // Variables
    [SerializeField] Texture2D _crosshair;
    [SerializeField] GameObject _bonusPointsPopUp;
    [SerializeField] GameObject _lostLevelPopUp;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _missileBonusText;
    [SerializeField] TMP_Text _cityBonusText;
    [SerializeField] TMP_Text _lostTotalPointsText;
    [SerializeField] GameObject[] _missileImages;
    [SerializeField] GameObject[] _cityImages;
    [Header("Cities and Missiles")]
    [SerializeField] MissileBase[] _missileBaseArray;
    [SerializeField] City[] _cityArray;
    [Header("Time delays")]
    [SerializeField] float _delayBetweenMissiles = 0.2f;
    [SerializeField] float _delayBetweenCities = 0.6f;
    [Header("Set Point Amounts")]
    [SerializeField] int _bombPoints = 25;
    [SerializeField] int _satellitePoints = 100;
    [SerializeField] int _bomberPoints = 100;
    [SerializeField] int _smartBombPoints = 125;
    [Header("Bonus Points")]
    [SerializeField] int _unusedMissilesPoints = 5;
    [SerializeField] int _savedCitiesPoints = 100;

    private int _totalPoints = 0;
    private int _missileBonus = 0;
    private int _cityBonus = 0;
    private float _popUpDelay = 3f;

    private void Awake()
    {
        if (Instance == null)
        {
            // doesn't exist yet, this is now our singleton
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _scoreText.text = "0";
        _missileBonusText.text = "0";
        _cityBonusText.text = "0";
    }

    private void Start()
    {
        // Setting the cursor to be a crosshair
        Cursor.SetCursor(
            _crosshair, 
            new Vector2(_crosshair.width/2, _crosshair.width / 2), 
            CursorMode.Auto);
        // Confining cursor
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public void LevelFinished()
    {
        Debug.Log("LEVEL FINISHED");
        StartCoroutine(StartPointsPopUpRoutine());
    }

    public void LevelLost()
    {
        Debug.Log("LEVEL FINISHED");
        StartCoroutine(LostLevelPopupRoutine());
    }

    private IEnumerator StartPointsPopUpRoutine()
    {
        yield return new WaitForSeconds(_popUpDelay);

        Time.timeScale = 0;
        _bonusPointsPopUp.SetActive(true);

        int i = 0;
        // Adding all missile bonus points
        foreach (MissileBase mBase in _missileBaseArray)
        {
            int missilesLeft = mBase.GetMissilesLeft();
            if (mBase.BaseActive())
            {
                int mCount = 0;
                while (mCount < missilesLeft)
                {
                    _missileImages[i].SetActive(true);
                    _missileBonus += _unusedMissilesPoints;
                    _missileBonusText.text = _missileBonus.ToString();
                    mBase.RemoveMissile();
                    i++;
                    mCount++;
                    yield return new WaitForSecondsRealtime(_delayBetweenMissiles);
                }
            }
            yield return null;
        }

        yield return new WaitForSecondsRealtime(_delayBetweenMissiles);

        i = 0;
        // Adding all city bonus points
        for (int cCount = 0; cCount < _cityArray.Length; cCount++)
        {
            City city = _cityArray[cCount];
            if (city.CityActive())
            {
                _cityImages[i].SetActive(true);
                _cityBonus += _savedCitiesPoints;
                _cityBonusText.text = _cityBonus.ToString();
                city.RemoveCity();
                i++;
                yield return new WaitForSecondsRealtime(_delayBetweenCities);
            }
            yield return null;
        }

        // Setting final score
        _totalPoints += _cityBonus + _missileBonus;
        SetTotalScoreText();
    }

    private IEnumerator LostLevelPopupRoutine()
    {
        yield return new WaitForSeconds(_popUpDelay);

        Time.timeScale = 0;
        _lostTotalPointsText.text = _totalPoints.ToString();
        _lostLevelPopUp.SetActive(true);
    }

    private void SetTotalScoreText()
    {
        _scoreText.text = _totalPoints.ToString();
    }

    public void BombHitIncrementScore()
    {
        _totalPoints += _bombPoints;
        SetTotalScoreText();
    }

    public void BomberHitIncrementScore()
    {
        _totalPoints += _bomberPoints;
        SetTotalScoreText();
    }

    public void SmartBombHitIncrementScore()
    {
        _totalPoints += _smartBombPoints;
        SetTotalScoreText();
    }
}
