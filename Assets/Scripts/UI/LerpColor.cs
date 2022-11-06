using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpColor : MonoBehaviour
{
    // color array
    [SerializeField] Color[] _colors;
    // interval before changing a color
    [SerializeField] float _interval = 0.05f;

    private Image _image;
    private float _duration = 1f;
    private int _colorIndex = 0;
    private float _timer = 0.0f;


    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    void Start()
    {
        _image.color = _colors[0];
    }

    void Update()
    {
        _timer += Time.deltaTime / _duration;

        if (_timer > _interval)
        {
            // Set color
            _image.color = _colors[_colorIndex];
            // Reset timer
            _timer = 0;
            // Set index to next color
            _colorIndex++;
            if (_colorIndex >= _colors.Length)
            {
                _colorIndex = 0;
            }
        }
    }
}
