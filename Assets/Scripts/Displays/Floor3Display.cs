using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor3Display : MonoBehaviour, IShowWarning, ICanBeGarbled
{
    private static GameManager GameManager => GameManager.Instance;

    [SerializeField]
    private SeeSawSliderWindow SeeSawSliderWindow;
    
    [SerializeField]
    private SpriteRenderer[] targets;
    private Transform[] _targetTransforms;
    [SerializeField]
    private float maxScale;
    
    [SerializeField]
    private Color emptyColor = Color.white;
    [SerializeField]
    private Color fullColor = Color.white;

    private bool _showWarning;
    
    // Start is called before the first frame update
    private void Start()
    {
        _targetTransforms = new Transform[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            _targetTransforms[i] = targets[i].transform;
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var storedValues = GameManager.coolantTowerValues;

        _showWarning = false;
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].color = Color.Lerp(emptyColor, fullColor, storedValues[i]);

            if (storedValues[i] < 0.75f)
                _showWarning = true;
        }
        //TODO Add resizing too
    }

    public bool ShouldDisplayWarning() => _showWarning;
    public void Garble()
    {
        var count = GameManager.coolantTowerValues.Length;

        for (int i = 0; i < count; i++)
        {
            GameManager.StoreStationValue(Station.TYPE.SEE_SAW, i, SeeSawSliderWindow.GetRandomValue());
        }
    }
}
