using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SingleSliderWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private float expectedValue;
    [SerializeField]
    private Vector2 randomValueRange;
    [SerializeField]
    private Slider horizontalSlider;

    private float _currentValue;

    //====================================================================================================================//
    
    public void InitUI()
    {
        horizontalSlider.minValue = randomValueRange.x;
        horizontalSlider.maxValue = randomValueRange.y;
        ResetValues();
        horizontalSlider.onValueChanged.AddListener(ValueListener);
    }

    public void ResetValues()
    {
        horizontalSlider.value = expectedValue;
    }

    public void RandomizeValues()
    {
         horizontalSlider.value = Random.Range(randomValueRange.x, randomValueRange.y);
    }

    public bool CheckValues()
    {
        return Math.Abs(_currentValue - expectedValue) < 0.1f;
    }

    //====================================================================================================================//

    private void ValueListener(float value)
    {
        _currentValue = value;
    }
}
