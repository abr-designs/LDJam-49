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

    public float CurrentValue { get; private set; }

    //====================================================================================================================//
    
    public void InitUI()
    {
        horizontalSlider.minValue = randomValueRange.x;
        horizontalSlider.maxValue = randomValueRange.y;
        ResetValues();
        horizontalSlider.onValueChanged.AddListener(ValueListener);
    }

    public void RefreshValues()
    {
    }

    public void ResetValues()
    {
        CurrentValue = horizontalSlider.value = expectedValue;
    }

    public void RandomizeValues()
    {
        CurrentValue = horizontalSlider.value = Random.Range(randomValueRange.x, randomValueRange.y);
    }

    public bool CheckValues()
    {
        return Math.Abs(CurrentValue - expectedValue) < 0.1f;
    }

    //====================================================================================================================//

    private void ValueListener(float value)
    {
        CurrentValue = value;
    }

    
}
