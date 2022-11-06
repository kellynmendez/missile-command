using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Point Amounts")]
    [SerializeField] int _missile = 25;
    [SerializeField] int _satellite = 100;
    [SerializeField] int _bomber = 100;
    [SerializeField] int _smartBomb = 125;
    [Header("Bonus Points")]
    [SerializeField] int _unusedMissiles = 5;
    [SerializeField] int _savedCities = 100;

    private UIManager _uiManager;
    private int _totalPoints = 0;
    private int _missileBonus = 0;
    private int _cityBonus = 0;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void MissileHitIncrementScore()
    {
        _totalPoints += _missile;
    }

    public void BomberHitIncrementScore()
    {
        _totalPoints += _bomber;
    }

    public void SatelliteHitIncrementScore()
    {
        _totalPoints += _satellite;
    }

    public void SmartBombHitIncrementScore()
    {
        _totalPoints += _smartBomb;
    }

    public int CalculateMissileBonus(int missiles)
    {
        _missileBonus = missiles * _unusedMissiles;
        return _missileBonus;
    }

    public int CalculateCityBonus(int numCities)
    {
        _cityBonus = numCities * _savedCities;
        return _cityBonus;
    }

    public int GetTotalScore()
    {
        return _totalPoints;
    }
}
