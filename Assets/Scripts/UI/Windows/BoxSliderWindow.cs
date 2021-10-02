using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;

public class BoxSliderWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private Vector2 expectedValue;
    [SerializeField]
    private Vector2 horizontalRandomValues;
    [SerializeField]
    private Vector2 verticalRandomValues;

    [SerializeField]
    private BoxSlider boxSlider;

    private Vector2 _currentValue;

    //====================================================================================================================//
    
    public void InitUI()
    {
        boxSlider.OnValueChanged.AddListener(OnValueChanged);
    }

    public void ResetValues()
    {
        boxSlider.ValueX = expectedValue.x;
        boxSlider.ValueY = expectedValue.y;
    }

    public void RandomizeValues()
    {
        boxSlider.ValueX = Random.Range(horizontalRandomValues.x, horizontalRandomValues.y);
        boxSlider.ValueY = Random.Range(verticalRandomValues.x, verticalRandomValues.y);
    }

    public bool CheckValues()
    {
        return Math.Abs(_currentValue.x - expectedValue.x) < 0.1f && Math.Abs(expectedValue.y - _currentValue.y) < 0.1f;
    }

    //====================================================================================================================//

    private void OnValueChanged(float x, float y)
    {
        _currentValue = new Vector2(x, y);
    }
}
