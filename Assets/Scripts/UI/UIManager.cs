using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Texture2D _crosshair;
    [SerializeField] City[] _cities;

    private void Start()
    {
        // Setting the cursor to be a crosshair
        Cursor.SetCursor(
            _crosshair, 
            new Vector2(_crosshair.width/2, _crosshair.width / 2), 
            CursorMode.Auto);
    }

    
}
