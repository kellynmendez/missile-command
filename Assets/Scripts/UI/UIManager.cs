using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Texture2D _crosshair;
    [SerializeField] GameObject _pointsPopUp;
    [SerializeField] Text _score;
    [SerializeField] Text _missileBonus;
    [SerializeField] Text _cityBonus;
    [SerializeField] GameObject[] _missileImages;
    [SerializeField] GameObject[] _cityImages;

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

    public void UpdateTotalScore()
    {

    }

    public void LevelFinished()
    {
        _pointsPopUp.SetActive(true);
        StartCoroutine(StartPointsPopUpRoutine());
    }

    private IEnumerator StartPointsPopUpRoutine()
    {
        yield return null;
        
    }



    
}
