using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Floor2Display : MonoBehaviour, IShowWarning
{
    [SerializeField]
    private DialsWindow dialsWindow;
    [SerializeField]
    private ButtonsWindow buttonsWindow;

    [SerializeField]
    private TMP_Text displayText;

    private int[] _expectedValues = new int[3];

    private bool _showWarning;

    //====================================================================================================================//
    
    // Start is called before the first frame update
    void Start()
    {
        var numbers = Enumerable.Range(1, 9).PickRandomElements(3);
        SetExpectedValues(numbers);
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
                displayList[i] = $"<color=\"white\">{_expectedValues[i]}</color>";
            }
            else if (dialValues.Contains(_expectedValues[i]) || buttonsWindow.ToggleActive(_expectedValues[i] - 1))
            {
                displayList[i] = $"<color=\"yellow\">{_expectedValues[i]}</color>";
                _showWarning = true;
            }
            else
            {
                displayList[i] = $"<color=\"red\">{_expectedValues[i]}</color>";
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
}
