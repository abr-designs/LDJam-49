using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;

public class DialsWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private int maxValue;

    [SerializeField]
    private Vector2 randomRangeValues;
    [SerializeField]
    private UI_Knob[] dials;
    
    [SerializeField]
    private TMP_Text[] displays;

    
    public int[] CurrentValues => _values;
    private int[] _values;
    
    
    public void InitUI()
    {
        _values = new int[dials.Length];
        for (var i = 0; i < dials.Length; i++)
        {
            var dial = dials[i];
            dial.SetKnobValue(0);
            displays[i].text = "0";
            
            var index = i;
            dial.OnValueChanged.AddListener(value =>
            {
                int intValue = Mathf.RoundToInt(value * 10f);
                _values[index] = intValue;
                displays[index].text = $"{intValue:0}";
            });
        }
    }

    public void RefreshValues()
    {
        
    }

    public void ResetValues()
    {
        foreach (var dial in dials)
        {
            dial.SetKnobValue(0);
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
        throw new NotImplementedException();
    }
}
