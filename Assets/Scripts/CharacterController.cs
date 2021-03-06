using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class CharacterController : Actor2DBase
{
    private const string INTERACTABLE_TAG = "Interactable";
    
    public Rigidbody2D Rigidbody2D;

    public CharacterAnimator Animator { get; private set; }
    private AudioController AudioController { get; set; }


    public float moveSpeed;

    private bool[] _moves;

    private List<IInteractable> _interactables;

    private bool _locked;

    //Unity Functions
    //====================================================================================================================//
    
    // Start is called before the first frame update
    protected override void Start()
    {
        AudioController = FindObjectOfType<AudioController>();
        Animator = GetComponent<CharacterAnimator>();
        
        base.Start();
        
        _moves = new bool[2];
        _interactables = new List<IInteractable>();

        CharacterAnimator.OnStep += StepSound;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_locked)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && _interactables.Count > 0)
        {
            _interactables[_interactables.Count - 1].Interact(this);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _moves[0] = true;
            Animator.StartAnimation("Run");
            Animator.SetXOrientation(true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _moves[0] = false;
            Animator.StartAnimation("Idle");
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            _moves[1] = true;
            Animator.StartAnimation("Run");
            Animator.SetXOrientation(false);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            _moves[1] = false;
            Animator.StartAnimation("Idle");
        }
    }

    private void FixedUpdate()
    {
        if (_locked)
            return;
        
        if (_moves[0])
        {
            Rigidbody2D.position += Vector2.left * moveSpeed * Time.fixedDeltaTime;
        }
        else if (_moves[1])
        {
            Rigidbody2D.position += Vector2.right * moveSpeed * Time.fixedDeltaTime;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(INTERACTABLE_TAG))
            return;
        
        _interactables.Add(other.GetComponent<IInteractable>());
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(INTERACTABLE_TAG))
            return;
        
        _interactables.Remove(other.GetComponent<IInteractable>());
    }

    private void OnDestroy()
    {
        CharacterAnimator.OnStep -= StepSound;

    }

    //====================================================================================================================//

    private void StepSound()
    {
        AudioController.PlaySoundEffect(AudioController.EFFECT.STEP);
    }

    public void SetLocked(in bool state)
    {
        _locked = state;
        Rigidbody2D.isKinematic = state;

        if (state)
        {
            Rigidbody2D.velocity = Vector2.zero;

            _moves[0] = _moves[1] = false;
        }
        else
        {
            if(_moves[0] || _moves[1])
                Animator.StartAnimation("Run");
            else
                Animator.StartAnimation("Idle");
        }

    }

    //====================================================================================================================//
}
