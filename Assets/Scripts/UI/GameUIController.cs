using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Serializable]
    public class Window
    {
        [SerializeField]
        private string Name;
        public GameObject window;
        public TMP_Text title;

        public IWindowData WindowData => _windowData ??= window.GetComponent<IWindowData>();
        private IWindowData _windowData;

        public void SetActive(in bool state)
        {
            window.SetActive(state);
        }
    }

    //====================================================================================================================//
    
    [SerializeField]
    private Window[] windows;
    [SerializeField]
    private GameObject backgroundImage;

    //====================================================================================================================//
    
    // Start is called before the first frame update
    private void Start()
    {
        backgroundImage.SetActive(false);
        foreach (var t in windows)
        {
            t.WindowData.InitUI();
            t.SetActive(false);
        }
    }

    //====================================================================================================================//
    
    public void ShowWindow(in int index)
    {
        backgroundImage.SetActive(index >= 0);
        
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(i == index);
        }
    }

    //====================================================================================================================//

    [ContextMenu("Open Window 7")]
    private void TestWindow()
    {
        ShowWindow(6);
    }
}
