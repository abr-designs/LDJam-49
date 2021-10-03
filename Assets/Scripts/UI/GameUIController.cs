using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Serializable]
    public class Window
    {
        public string Name;
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

    [SerializeField]
    private GameObject diedWindow;
    [SerializeField]
    private Button retryButton;


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

        ShipCore.OnDied += OnDied;
        
        retryButton.onClick.AddListener(() =>
        {
            Destroy(AudioController.Instance.gameObject);
            SceneManager.LoadScene(0);
        });
        diedWindow.SetActive(false); 
    }

    private void OnDestroy()
    {
        ShipCore.OnDied -= OnDied;
    }

    private void OnDied()
    {
        diedWindow.SetActive(true);
    }

    //====================================================================================================================//
    
    public void ShowWindow(in Station.TYPE stationType)
    {
        var index = (int)stationType;
        
        backgroundImage.SetActive(index >= 0);
        
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(i == index);
            
            if(i == index)
                windows[i].WindowData.RefreshValues();
        }
    }

    //====================================================================================================================//

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var window in windows)
        {
            window.window.name = $"STATION_{window.Name}";
        }
    }

    [ContextMenu("Open Window")]
    private void TestWindow()
    {
        ShowWindow(0);
    }
#endif
}
