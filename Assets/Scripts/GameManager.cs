using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    //Structs
    //====================================================================================================================//
    
    [Serializable]
    private struct WarningData
    {
        public bool HasSystemWarning { get; private set; }

        [SerializeField]
        private GameObject floorDisplayObject;
        private IShowWarning _showWarning;
        private ICanBeGarbled _canBeGarbled;
        [SerializeField]
        private GameObject warningLightObject;

        public void CheckWarning()
        {
            _showWarning ??= floorDisplayObject.GetComponent<IShowWarning>();
            HasSystemWarning = _showWarning.ShouldDisplayWarning();
            warningLightObject.SetActive(HasSystemWarning);
        }
        public void TryGarbleFloor()
        {
            if (_canBeGarbled is null)
                _canBeGarbled = floorDisplayObject.GetComponent<ICanBeGarbled>();
            
            _canBeGarbled.Garble();
        }
    }

    [Serializable]
    private struct FloorLockData
    {
        [SerializeField]
        private GameObject floorShadeObject;
        [SerializeField]
        private SpriteRenderer leftHatchRenderer;
        [SerializeField]
        private SpriteRenderer rightHatchRenderer;
        [SerializeField]
        private Sprite targetSprite;

        public void OpenFloor()
        {
            /*const string TARGET_LAYER = "Background";
            const int TARGET_SORTING_ORDER = 50;*/

            floorShadeObject.SetActive(false);
            leftHatchRenderer.gameObject.SetActive(false);
            rightHatchRenderer.gameObject.SetActive(false);
            /*rightHatchRenderer.sortingLayerName = leftHatchRenderer.sortingLayerName = TARGET_LAYER;
            rightHatchRenderer.sortingOrder = leftHatchRenderer.sortingOrder = TARGET_SORTING_ORDER;
            rightHatchRenderer.sprite = leftHatchRenderer.sprite = targetSprite;
            rightHatchRenderer.color = leftHatchRenderer.color = Color.gray;*/
        }
    }
    
    //Properties
    //====================================================================================================================//

    [SerializeField, Header("Damage")] 
    private float damagePerSecond;
    [SerializeField]
    private SoundRepeater alarmRepeater;
    
    [SerializeField, Range(0,4), Header("Floors")] 
    private int startingFloor;
    [SerializeField]
    private float[] floorHeights;

    [SerializeField] 
    private WarningData[] warningDatas;

    [SerializeField]
    private FloorLockData[] floorLockDatas;


    //--------------------------------------------------------------------------------------------------------//
    
    [SerializeField, Header("Events")]
    private AnimationCurve eventTimeCurve;
    [SerializeField]
    private AnimationCurve eventGarbleChanceCurve;
    [SerializeField]
    private Vector2 eventTimeRange;
    [SerializeField]
    private float timeFrequencyIncrementation;
    [SerializeField]
    private float[] floorUnlockPercentage;

    private float _progressionT;
    private float _eventTimer;

    private int _currentFloor;

    //--------------------------------------------------------------------------------------------------------//

    private CinemachineImpulseSource _cinemachineImpulseSource;
    
    private FirePole _firePole;
    private Ladder _ladder;
    private Starter _starter;
    private ShipCore _shipCore;

    public float[] coolantTowerValues { get; private set; }
    public int[] breakerBoxValues { get; private set; }
    

    public static GameManager Instance { get; private set; }
    
    private Station.TYPE _activeStation = Station.TYPE.NONE;
    private int _currentSubStationIndex;
    
    private bool _dead;


    //====================================================================================================================//
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _shipCore = GetComponent<ShipCore>();
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        
        _firePole = FindObjectOfType<FirePole>();
        _ladder = FindObjectOfType<Ladder>();
        _starter = FindObjectOfType<Starter>();

        coolantTowerValues = new float[3];
        breakerBoxValues = Enumerable.Repeat(2, 4).ToArray();

        for (int i = 0; i < warningDatas.Length; i++)
        {
            warningDatas[i].CheckWarning();
        }

        UnlockFloor(startingFloor);
        _eventTimer = Random.Range(2f,6f);

        ShipCore.OnDied += OnDied;
    }

    private void Update()
    {
        if (_dead)
            return;
        
        EventUpdate();
        CheckForWarnings();
    }

    private void OnDestroy()
    {
        ShipCore.OnDied -= OnDied;
    }

    private void OnDied()
    {
        _dead = true;
        alarmRepeater.enabled = false;
    }

    //====================================================================================================================//

    private void CheckForWarnings()
    {
        var warningCount = 0;
        for (int i = 0; i <= _currentFloor; i++)
        {
            warningDatas[i].CheckWarning();
            if (warningDatas[i].HasSystemWarning)
                warningCount++;
        }

        alarmRepeater.enabled = warningCount > 0;
        
        if (warningCount <= 0)
            return;
        
        _shipCore.DamageCore(damagePerSecond * warningCount * Time.deltaTime);
    }

    public void UnlockFloor(int floorIndex)
    {
        _currentFloor = floorIndex;
        
        _firePole.gameObject.SetActive(floorIndex > 0);
        _ladder.gameObject.SetActive(floorIndex > 0);
        
        if (floorIndex == 0)
            return;
        
        //TODO Open Hatch Doors
        _firePole.SetEndHeight(floorHeights[floorIndex]);
        floorLockDatas[floorIndex - 1].OpenFloor();
    }

    //Event Update Functions
    //====================================================================================================================//

    private void EventUpdate()
    {
        if (_eventTimer > 0f)
        {
            _eventTimer -= Time.deltaTime;
            return;
        }

        _progressionT += timeFrequencyIncrementation;

        if (_currentFloor + 1 < floorUnlockPercentage.Length &&
            _progressionT >= floorUnlockPercentage[_currentFloor + 1])
        {
            UnlockFloor(_currentFloor + 1);
            //Force Garble New Floors
            warningDatas[_currentFloor].TryGarbleFloor();
        }
        
        _eventTimer = Mathf.Lerp(eventTimeRange.x, eventTimeRange.y, eventTimeCurve.Evaluate(_progressionT));

        if (_currentFloor == 0)
        {
            warningDatas[0].TryGarbleFloor();
        }
        else
        {
            for (int i = 0; i < _currentFloor; i++)
            {
                var hasGarble = Random.value <= eventGarbleChanceCurve.Evaluate(_progressionT);
                
                if(!hasGarble)
                    continue;
                
                warningDatas[i].TryGarbleFloor();
            }

            //At least one floor should disrupt each explosion
            if (warningDatas.All(x => x.HasSystemWarning == false))
            {
                warningDatas.PickRandomElement().TryGarbleFloor();
            }
        }

        _cinemachineImpulseSource.GenerateImpulse(5);
        AudioController.Instance.PlaySoundEffect(AudioController.EFFECT.EXPLOSION);
    }

    //Station Data Functions
    //====================================================================================================================//

    #region Station Data Functions

    public void SetActiveStation(in Station.TYPE stationType, int subStationIndex)
    {
        _activeStation = stationType;
        _currentSubStationIndex = subStationIndex;
    }

    public void StoreStationValue(in float value)
    {
        StoreStationValue(_activeStation, _currentSubStationIndex, value);
    }
    public void StoreStationValue(in Station.TYPE stationType, in int subStationIndex, in float value)
    {
        switch (stationType)
        {
            case Station.TYPE.NONE:
                return;
            case Station.TYPE.SEE_SAW:
                coolantTowerValues[subStationIndex] = value;
                break;
            case Station.TYPE.BREAKER:
                breakerBoxValues[subStationIndex] = Mathf.RoundToInt(value);
                break;
        }
    }

    public float GetStoredStationValue()
    {
        switch (_activeStation)
        {
            case Station.TYPE.NONE:
                return default;
            case Station.TYPE.SEE_SAW:
                return coolantTowerValues[_currentSubStationIndex];
            case Station.TYPE.BREAKER:
                return breakerBoxValues[_currentSubStationIndex];
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    #endregion //Station Data Functions
    
    //Unity Editor Functions
    //====================================================================================================================//

    #region Unity Editor Functions

#if UNITY_EDITOR

    [ContextMenu("Test Shake Impulse")]
    private void TestImpulse()
    {
        _cinemachineImpulseSource.GenerateImpulse(10);
    }

    private void OnDrawGizmos()
    {
        if (floorHeights == null || floorHeights.Length == 0)
            return;
        
        Gizmos.color = Color.yellow;
        var position = transform.position;
        
        for (int i = 0; i < floorHeights.Length; i++)
        {
            var floorPos = position;
            floorPos.y = floorHeights[i];
            Gizmos.DrawWireSphere(floorPos, 0.5f);
        }
    }

#endif

    #endregion //Unity Editor Functions

    //====================================================================================================================//
    
}
