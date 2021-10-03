using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsWindow : MonoBehaviour, IWindowData
{
    [SerializeField]
    private Button[] buttons;

    
    public int[] CurrentValues => _toggles
        .Where((x, i) => x)
        .Select((x, i)  => i)
        .ToArray();
    private bool[] _toggles;

    [SerializeField]
    private Color toggledColor = Color.white;
    [SerializeField]
    private Color untoggledColor = Color.white;

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


                button.image.color = _toggles[index] ? toggledColor : untoggledColor;
                AudioController.Instance.PlaySoundEffect(AudioController.EFFECT.BUTTON_PRESS);
            });

            button.GetComponentInChildren<TMP_Text>().text = $"{i + 1}";
        }
    }

    public void RefreshValues()
    {
        
    }

    public void ResetValues()
    {
        for (var i = 0; i < _toggles.Length; i++)
        {
            _toggles[i] = false;
            buttons[i].image.color = _toggles[i] ? toggledColor : untoggledColor;
        }
    }

    public void RandomizeValues()
    {
        for (var i = 0; i < _toggles.Length; i++)
        {
            _toggles[i] = Random.value >= 0.5f;
            buttons[i].image.color = _toggles[i] ? toggledColor : untoggledColor;
        }
    }

    public bool CheckValues()
    {
        throw new System.NotImplementedException();
    }

    //====================================================================================================================//

    public bool ToggleActive(in int index)
    {
        return _toggles[index];
    }
}
