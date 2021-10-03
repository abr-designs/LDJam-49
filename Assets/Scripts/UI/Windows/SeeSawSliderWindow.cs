using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SeeSawSliderWindow : MonoBehaviour, IWindowData
{
    public float CurrentValue => mainSlider.value;
    
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
    
    private static GameManager GameManager => GameManager.Instance;

    public void InitUI()
    {
        ResetValues();
        
        leftSlider.onValueChanged.AddListener(OnLeftChange);
        rightSlider.onValueChanged.AddListener(OnRightChange);
    }

    public void RefreshValues()
    {
        ResetValues();
        
        mainSlider.value = GameManager.GetStoredStationValue();
    }

    public void ResetValues()
    {
        _leftValue = leftSlider.value = 1f;
        _rightValue = rightSlider.value = 0f;

        leftSlider.interactable = true;
        rightSlider.interactable = false;

        mainSlider.value = 0f;
    }

    public void RandomizeValues()
    {
        throw new NotImplementedException();
    }
    
    public float GetRandomValue()
    {
        return Random.Range(randomRangeValue.x, randomRangeValue.y);
    }

    //====================================================================================================================//

    private void OnLeftChange(float value)
    {
        if (value >= _leftValue)
            return;

        mainSlider.value += (_leftValue - value) * fillSpeedMult;
        _leftValue = value;
        _rightValue = rightSlider.value = 1f - value;
        
        GameManager.StoreStationValue(mainSlider.value);

        if (_rightValue >= 1f)
        {
            leftSlider.interactable = false;
            rightSlider.interactable = true;
        }
        
    }
    private void OnRightChange(float value)
    {
        if (value >= _rightValue)
            return;

        mainSlider.value += (_rightValue - value) * fillSpeedMult;
        _rightValue = value;
        _leftValue = leftSlider.value = 1f - value;
        GameManager.StoreStationValue(mainSlider.value);
        
        if (_leftValue >= 1f)
        {
            leftSlider.interactable = true;
            rightSlider.interactable = false;
        }
        
    }
}
