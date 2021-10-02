using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;

public class DialsWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private float expectedValue;
    [SerializeField]
    private Vector2 randomRangeValues;
    [SerializeField]
    private UI_Knob[] dials;
    private float[] _values;
    
    
    public void InitUI()
    {
        _values = new float[dials.Length];
        for (var i = 0; i < dials.Length; i++)
        {
            var dial = dials[i];
            dial.SetKnobValue(expectedValue);
            var index = i;
            dial.OnValueChanged.AddListener(value =>
            {
                _values[index] = value;
            });
        }
    }

    public void ResetValues()
    {
        foreach (var dial in dials)
        {
            dial.SetKnobValue(expectedValue);
        }
    }

    public void RandomizeValues()
    {
        foreach (var dial in dials)
        {
            dial.SetKnobValue(Random.Range(randomRangeValues.x, randomRangeValues.y));
        }
    }

    public bool CheckValues()
    {
        for (var i = 0; i < dials.Length; i++)
        {
            if (Math.Abs(_values[i] - expectedValue) > 0.1f)
                return false;
        }

        return true;
    }
}
