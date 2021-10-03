using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowData
{
    void InitUI();
    void RefreshValues();
    void ResetValues();
    void RandomizeValues();
}