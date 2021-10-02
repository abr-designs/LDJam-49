using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SlidersWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private float expectedValue;
    [SerializeField]
    private Vector2 randomRangeValues;
    [SerializeField]
    private Slider[] sliders;
    private float[] _values;

    public void InitUI()
    {
        _values = new float[sliders.Length];
        for (var i = 0; i < sliders.Length; i++)
        {
            var slider = sliders[i];
            slider.value = expectedValue;
            
            var index = i;
            slider.onValueChanged.AddListener(value =>
            {
                _values[index] = value;
            });
        }
    }

    public void ResetValues()
    {
        foreach (var slider in sliders)
        {
            slider.value = expectedValue;
        }
    }

    public void RandomizeValues()
    {
        foreach (var slider in sliders)
        {
            slider.value = Random.Range(randomRangeValues.x, randomRangeValues.y);
        }
    }

    public bool CheckValues()
    {
        for (var i = 0; i < sliders.Length; i++)
        {
            if (Math.Abs(_values[i] - expectedValue) > 0.1f)
                return false;
        }

        return true;
    }
}
