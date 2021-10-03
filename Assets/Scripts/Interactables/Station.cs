using System;
using UnityEngine;

public class Station : InteractableBase
{
    
    public enum TYPE
    {
        NONE = -1,
        HORIZONTAL,
        VERTICAL,
        DIALS,
        BUTTONS,
        SEE_SAW,
        BREAKER
    }

    public int StationSecondaryIndex
    {
        get => subStationIndex;
        set => subStationIndex = value;
    }

    [SerializeField]
    private TYPE stationType;

    [SerializeField]
    private int subStationIndex;

    protected override CharacterController CharacterController { get; set; }
    protected override bool Interacting { get; set; }
    protected override bool InteractCooldown { get; set; }

    private static GameUIController GameUIController
    {
        get
        {
            if (_gameUIController == null)
                _gameUIController = FindObjectOfType<GameUIController>();

            return _gameUIController;
        }
    }
    private static GameUIController _gameUIController;
    
    private static GameManager GameManager => GameManager.Instance;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Interacting == false)
            return;

        if (InteractCooldown)
        {
            InteractCooldown = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            StopInteract();
            return;
        }
    }


    public override void Interact(CharacterController characterController)
    {
        GameManager.SetActiveStation(stationType, subStationIndex);
        GameUIController.ShowWindow(stationType);
        
        CharacterController = characterController;

        CharacterController.SetLocked(true);

        Interacting = true;
        InteractCooldown = true;
        
        CharacterController.Animator.StartAnimation("Interact");
    }

    protected override void StopInteract()
    {
        GameUIController.ShowWindow(TYPE.NONE);
        
        Interacting = false;
        CharacterController.SetLocked(false);
        CharacterController = null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        gameObject.name = $"{stationType}_[{subStationIndex}]";
    }
#endif
}
