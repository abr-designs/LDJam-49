using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public static Action OnStep;

    [Serializable]
    private struct Animation
    {
        public string name;
        public bool loop;
        public bool manualIncrement;
        public Sprite[] sprites;

        public float frameDelay;
        
    }

    private SpriteRenderer _spriteRenderer;

    [NonReorderable]
    [SerializeField] 
    private Animation[] animations;

    private int _currentAnimationIndex;
    private Animation _currentAnimation;
    private float _frameTimer;
    private int _frameIndex;

    private Dictionary<string, int> _animationNames;

    private bool HasAnimation => string.IsNullOrWhiteSpace(_currentAnimation.name) == false;

    // Start is called before the first frame update
   private  void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animationNames = new Dictionary<string, int>();
        for (int i = 0; i < animations.Length; i++)
        {
            _animationNames.Add(animations[i].name, i);
        }
        
        StartAnimation(0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (HasAnimation == false)
            return;
        
        if (_currentAnimation.manualIncrement) 
            return;
        
        _frameTimer += Time.deltaTime;

        if (_frameTimer >= _currentAnimation.frameDelay)
        {
            IncrementFrame();
        }
    }

    //====================================================================================================================//
    

    public void StartAnimation(string animationName)
    {
        if(!_animationNames.TryGetValue(animationName, out var index))
            throw new Exception();
        
        StartAnimation(index);
    }
    public void StartAnimation(in int index)
    {
       
        
        _currentAnimationIndex = index;
        _frameTimer = 0f;
        _frameIndex = 0;
        
        _currentAnimation = animations[index];
        _spriteRenderer.sprite = _currentAnimation.sprites[_frameIndex];
    }

    public void IncrementFrame()
    {
        if (HasAnimation == false)
            return;

        if (_currentAnimationIndex == 3 && (_frameIndex == 0 || _frameIndex == 2))
        {
            OnStep?.Invoke();
        }
        
        if (_frameIndex + 1 >= _currentAnimation.sprites.Length && _currentAnimation.loop)
        {
            _frameIndex = 0;
        }
        else if (_frameIndex + 1 >= _currentAnimation.sprites.Length && _currentAnimation.loop == false)
        {
            return;
        }
        else
        {
            _frameIndex++;
        }

        _frameTimer = 0f;
        _spriteRenderer.sprite = _currentAnimation.sprites[_frameIndex];
    }

    public void SetXOrientation(bool flipped)
    {
        _spriteRenderer.flipX = flipped;
    }

    //====================================================================================================================//
    
}
