using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //Properties
    //====================================================================================================================//

    [SerializeField, Header("Settings Menu")]
    private GameObject settingsWindow;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider sfxVolumeSlider;
    [SerializeField]
    private Button closeButton;
    
    [SerializeField, Header("Start Menu")]
    private GameObject gameStartWindow;
    [SerializeField]
    private Slider breakerSlider;
    [SerializeField]
    private Image breakerLightImage;

    [SerializeField, Header("Ladder")]
    private Ladder ladder;

    [SerializeField]
    private float ladderMoveSpeed;

    private Transform _ladderTransform;
    [SerializeField]
    private Vector3 ladderTargetPosition;
    private Vector3 _ladderOffsetPosition;

    private bool _playerReady;
    

    [SerializeField, Header("Extras")]
    private GameObject warningLight;
    [SerializeField]
    private TMP_Text displayText;

    [SerializeField]
    private OpenTrigger gameDoorTrigger;

    [SerializeField]
    private float floorHeight;

    private MenuStation[] _menuStations;
    private FirePole _firePole;

    //Unity Functions
    //====================================================================================================================//
    
    // Start is called before the first frame update
    private void Start()
    {
        _ladderTransform = ladder.transform;
        _ladderOffsetPosition = ladderTargetPosition + (Vector3.down * 10);
        _ladderTransform.position = _ladderOffsetPosition;

        _firePole = FindObjectOfType<FirePole>();
        _firePole.SetEndHeight(floorHeight);
        
        _menuStations = FindObjectsOfType<MenuStation>();
        InitUIElements();

        gameDoorTrigger.OnTriggerEnter += value =>
        {
            LoadGameScene();
        };
        
        
        CloseWindows();
        
        breakerLightImage.color = Color.red;
        displayText.text = "LOCKED";
        warningLight.SetActive(false);
    }

    private void Update()
    {
        var currentPos = _ladderTransform.position;

        currentPos = Vector3.MoveTowards(currentPos, _playerReady ? ladderTargetPosition : _ladderOffsetPosition,
            ladderMoveSpeed * Time.deltaTime);

        _ladderTransform.position = currentPos;
        ladder.ForceUpdateMaxHeight();
    }

    private void InitUIElements()
    {
        //Settings Menu
        //--------------------------------------------------------------------------------------------------------//
        
        musicVolumeSlider.onValueChanged.AddListener(value =>
        {
            //TODO Set the game Volume
        }); 
        sfxVolumeSlider.onValueChanged.AddListener(value =>
        {
            //TODO Set the game Volume
        }); 
        
        closeButton.onClick.AddListener(ForceStopInteractions);
        
        //Game Start Menu
        //--------------------------------------------------------------------------------------------------------//
        
        breakerSlider.onValueChanged.AddListener(value =>
        {
            //TODO Set the game Volume

            var newStatus = Math.Abs(value - 1f) < 0.01f;

            if (newStatus == _playerReady)
                return;
            
            breakerLightImage.color = newStatus ? Color.green : Color.red;
            displayText.text = newStatus ? "READY" : "LOCKED";
            warningLight.SetActive(newStatus);

            _playerReady = newStatus;

            if (_playerReady)
            {
                AudioController.Instance.PlaySoundEffect(AudioController.EFFECT.BREAKER_OFF);
                //TODO Force close the window after a moment
                StartCoroutine(DelayedCallCoroutine(0.3f, ForceStopInteractions));
            }
        }); 
    }

    //Menu Controller Functions
    //====================================================================================================================//
    
    public void OpenSettingsMenu()
    {
        settingsWindow.SetActive(true);
    }

    public void OpenStartMenu()
    {
        gameStartWindow.SetActive(true);
    }

    public void CloseWindows()
    {
        settingsWindow.SetActive(false);
        gameStartWindow.SetActive(false);
    }

    private void ForceStopInteractions()
    {
        foreach (var menuStation in _menuStations)
        {
            menuStation.ForceStopInteraction();
        }
    }

    //====================================================================================================================//

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    private IEnumerator DelayedCallCoroutine(float time, Action onComplete)
    {
        yield return new WaitForSeconds(time);
        
        onComplete?.Invoke();
    }

    //Unity Editor Functions
    //====================================================================================================================//

    #region Unity Editor Functions

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.yellow;
        var position = transform.position;
        
        var floorPos = position;
        floorPos.y = floorHeight;
        Gizmos.DrawWireSphere(floorPos, 0.5f);
    }

#endif

    #endregion //Unity Editor Functions

    //====================================================================================================================//
}
