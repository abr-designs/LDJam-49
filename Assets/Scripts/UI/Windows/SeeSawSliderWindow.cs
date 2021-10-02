using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SeeSawSliderWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private float expectedValue;
    
    [SerializeField]
    private float defaultValue;
    [SerializeField]
    private Vector2 randomRangeValue;
    
    [SerializeField]
    private Slider mainSlider;

    [SerializeField]
    private float fillSpeedMult;
    
    [SerializeField]
    private Slider leftSlider;
    [SerializeField]
    private Slider rightSlider;

    private float _leftValue;
    private float _rightValue;

    public void InitUI()
    {
        ResetValues();
        
        leftSlider.onValueChanged.AddListener(OnLeftChange);
        rightSlider.onValueChanged.AddListener(OnRightChange);
    }

    public void ResetValues()
    {
        _leftValue = leftSlider.value = 1f;
        _rightValue = rightSlider.value = 0f;

        mainSlider.value = defaultValue;
    }

    public void RandomizeValues()
    {
        mainSlider.value = Random.Range(randomRangeValue.x, randomRangeValue.y);
    }

    public bool CheckValues()
    {
        return Math.Abs(mainSlider.value - expectedValue) < 0.1f;
    }

    //====================================================================================================================//

    private void OnLeftChange(float value)
    {
        if (value >= _leftValue)
            return;

        mainSlider.value += (_leftValue - value) * fillSpeedMult;
        _leftValue = value;
        _rightValue = rightSlider.value = 1f - value;
        
    }
    private void OnRightChange(float value)
    {
        if (value >= _rightValue)
            return;

        mainSlider.value += (_rightValue - value) * fillSpeedMult;
        _rightValue = value;
        _leftValue = leftSlider.value = 1f - value;
    }
}
