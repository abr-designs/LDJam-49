using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Floor2Display : MonoBehaviour, IShowWarning, ICanBeGarbled
{
    [SerializeField]
    private DialsWindow dialsWindow;
    [SerializeField]
    private ButtonsWindow buttonsWindow;

    [SerializeField]
    private TMP_Text displayText;

    private int[] _expectedValues = new int[3];
    
    [SerializeField]
    private Color wrongColor = Color.white;
    [SerializeField]
    private Color warningColor = Color.white;
    [SerializeField]
    private Color correctColor = Color.white;

    private string[] _colors;


    private bool _showWarning;

    //====================================================================================================================//
    
    // Start is called before the first frame update
    private void Start()
    {
        var numbers = Enumerable.Range(1, 9).PickRandomElements(3);
        SetExpectedValues(numbers);

        _colors = new[]
        {
            ColorUtility.ToHtmlStringRGBA(correctColor),
            ColorUtility.ToHtmlStringRGBA(warningColor),
            ColorUtility.ToHtmlStringRGBA(wrongColor),
        };
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var dialValues = dialsWindow.CurrentValues;
        //var buttonValues = buttonsWindow.CurrentValues.Select(x => x + 1).ToArray();

        string[] displayList = new string[3];
        _showWarning = false;
        for (int i = 0; i < displayList.Length; i++)
        {
            if (dialValues.Contains(_expectedValues[i]) && buttonsWindow.ToggleActive(_expectedValues[i] - 1))
            {
                displayList[i] = $"<color=#{_colors[0]}>{_expectedValues[i]}</color>";
            }
            else if (dialValues.Contains(_expectedValues[i]) || buttonsWindow.ToggleActive(_expectedValues[i] - 1))
            {
                displayList[i] = $"<color=#{_colors[1]}>{_expectedValues[i]}</color>";
                _showWarning = true;
            }
            else
            {
                displayList[i] = $"<color=#{_colors[2]}>{_expectedValues[i]}</color>";
                _showWarning = true;
            }
        }
        
        displayText.text = string.Join("", displayList);
    }

    //====================================================================================================================//

    public void SetExpectedValues(in int[] expectedValues)
    {
        for (int i = 0; i < _expectedValues.Length; i++)
        {
            _expectedValues[i] = expectedValues[i];
        }

        //displayText.text = string.Join("", _expectedValues);
    }

    //====================================================================================================================//

    public bool ShouldDisplayWarning() => _showWarning;
    public void Garble()
    {
        dialsWindow.RandomizeValues();
        buttonsWindow.RandomizeValues();

        var numbers = Enumerable.Range(1, 9).PickRandomElements(3);
        SetExpectedValues(numbers);
    }
}
