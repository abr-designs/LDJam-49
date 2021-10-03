using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BreakerWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private int expectedValue;
    private Vector2Int randomValueRange;
    [SerializeField]
    private Slider verticalSlider;

    [SerializeField]
    private Image breakerLightImage;
    
    private static GameManager GameManager => GameManager.Instance;

    public int CurrentValue
    {
        get => _currentValue;
        private set
        {
            _currentValue = value;
            UpdateColor();

            if (Starter.Active) Starter.Active = _currentValue == 2;
            
            GameManager.StoreStationValue(_currentValue);
        }
    }

    private int _currentValue;
    
    

    //====================================================================================================================//
    
    public void InitUI()
    {
        verticalSlider.minValue = 0f;
        verticalSlider.maxValue = 1f;
        ResetValues();
        verticalSlider.onValueChanged.AddListener(ValueListener);
    }

    public void RefreshValues()
    {
        CurrentValue = (int)GameManager.GetStoredStationValue();
        switch (CurrentValue)
        {
            case 0:
                verticalSlider.value = 0f;
                break;
            case 1:
            case 2:
                verticalSlider.value = 1f;
                break;
        }
        
       
    }

    public void ResetValues()
    {
        verticalSlider.value = expectedValue;
    }

    public void RandomizeValues()
    {
        throw new NotImplementedException();
    }

    public float GetRandomValue()
    {
        return Random.Range(randomValueRange.x, randomValueRange.y);
    }

    //====================================================================================================================//

    private void ValueListener(float value)
    {
        if (value == 0f && CurrentValue == 1 || CurrentValue == 2)
        {
            CurrentValue = 0;
        }
        else if (value == 1f && CurrentValue == 0)
        {
            CurrentValue = 2;
        }
    }

    private void UpdateColor()
    {
        switch (CurrentValue)
        {
            case 0:
                breakerLightImage.color = Color.red;
                break;
            case 1:
                breakerLightImage.color = Color.yellow;
                break;
            case 2:
                breakerLightImage.color = Color.green;
                break;
        }
    }
}
