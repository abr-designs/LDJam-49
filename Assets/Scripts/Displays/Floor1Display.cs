using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor1Display : MonoBehaviour, IShowWarning, ICanBeGarbled
{
    [SerializeField]
    private SingleSliderWindow horizontalSlider;
    [SerializeField]
    private SingleSliderWindow verticalSlider;

    [SerializeField]
    private SpriteRenderer displaySpaceSprite;

    private Bounds _bounds;
    private Vector2 _min, _max;

    [SerializeField]
    private Transform reticle;
    private SpriteRenderer _reticleRenderer;

    [SerializeField]
    private Color goodColor = Color.white; 
    [SerializeField]
    private Color badColor = Color.white;

    private bool _showWarning;

    // Start is called before the first frame update
    private void Start()
    {
        //FIXME Need to fix the space conversion to allow the recticle to remain visible
        _bounds = displaySpaceSprite.bounds;
        _min = _bounds.min;
        _max = _bounds.max;

        _reticleRenderer = reticle.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var value = new Vector2(horizontalSlider.CurrentValue, verticalSlider.CurrentValue);
        
        reticle.position = new Vector2(
            Mathf.Lerp(_min.x, _max.x, value.x),
            Mathf.Lerp(_max.y, _min.y, value.y)
            );

        var magnitude = (value - Vector2.one * 0.5f).magnitude;

        _showWarning = magnitude > 0.1f;

        displaySpaceSprite.color = Color.Lerp(goodColor, badColor, magnitude * 2f);
        _reticleRenderer.color = Color.Lerp(Color.green, Color.red, magnitude * 4f);
    }

    public bool ShouldDisplayWarning() => _showWarning;
    public void Garble()
    {
        horizontalSlider.RandomizeValues();
        verticalSlider.RandomizeValues();
    }
}
