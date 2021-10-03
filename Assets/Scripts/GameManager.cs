using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    [Serializable]
    private struct WarningData
    {
        [SerializeField]
        private GameObject floorDisplayObject;
        private IShowWarning _showWarning;
        [SerializeField]
        private GameObject warningLightObject;

        public void CheckWarning()
        {
            if (_showWarning is null)
                _showWarning = floorDisplayObject.GetComponent<IShowWarning>();
            
            warningLightObject.SetActive(_showWarning.ShouldDisplayWarning());
        }
    }
    //Properties
    //====================================================================================================================//
    
    [SerializeField, Range(0,4)] 
    private int startingFloor;
    [SerializeField]
    private float[] floorHeights;

    [SerializeField] 
    private WarningData[] warningDatas;

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

    private float _timeT;
    private float _eventTimer;

    private int _currentFloor;
    
    private FirePole _firePole;
    private Ladder _ladder;
    private Starter _starter;

    public float[] coolantTowerValues { get; private set; }
    public int[] breakerBoxValues { get; private set; }
    

    public static GameManager Instance { get; private set; }
    
    private Station.TYPE _activeStation = Station.TYPE.NONE;
    private int _currentSubStationIndex;

    //====================================================================================================================//
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _firePole = FindObjectOfType<FirePole>();
        _ladder = FindObjectOfType<Ladder>();
        _starter = FindObjectOfType<Starter>();

        coolantTowerValues = new float[4];
        breakerBoxValues = new int[4];

        for (int i = 0; i < 4; i++)
        {
            breakerBoxValues[i] = Random.Range(0, 3);
        }

        UnlockFloor(startingFloor);
    }

    private void Update()
    {
        CheckForWarnings();
    }

    //====================================================================================================================//

    private void CheckForWarnings()
    {
        for (int i = 0; i <= _currentFloor; i++)
        {
            warningDatas[i].CheckWarning();
        }
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
        switch (_activeStation)
        {
            case Station.TYPE.NONE:
                return;
            case Station.TYPE.SEE_SAW:
                coolantTowerValues[_currentSubStationIndex] = value;
                break;
            case Station.TYPE.BREAKER:
                breakerBoxValues[_currentSubStationIndex] = Mathf.RoundToInt(value);
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
