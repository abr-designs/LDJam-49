using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private Button[] buttons;

    private bool[] _toggles;

    public void InitUI()
    {
        _toggles = new bool[buttons.Length];
        for (var i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];

            var index = i;
            button.onClick.AddListener(() =>
            {
                _toggles[index] = !_toggles[index];
            });
        }
    }

    public void ResetValues()
    {
        for (var i = 0; i < _toggles.Length; i++)
        {
            _toggles[i] = false;
        }
    }

    public void RandomizeValues()
    {
        for (var i = 0; i < _toggles.Length; i++)
        {
            _toggles[i] = Random.value >= 0.5f;
        }
    }

    public bool CheckValues()
    {
        throw new System.NotImplementedException();
    }
}
