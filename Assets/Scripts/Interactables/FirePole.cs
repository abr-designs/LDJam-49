using UnityEngine;

public class FirePole : InteractableBase
{
    public float fallSpeed;
    public float topSpeed;
    
    protected override CharacterController CharacterController { get; set; }
    protected override bool Interacting { get; set; }
    protected override bool InteractCooldown { get; set; }
    
    private float _endHeight;

    private float _speed;

    //====================================================================================================================//
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Interacting)
            return;

        if (InteractCooldown)
        {
            InteractCooldown = false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            StopInteract();
            return;
        }

        var characterPos = CharacterController.transform.position;

        if (characterPos.y <= _endHeight + 1)
        {
            StopInteract();
            return;
        }

        _speed += fallSpeed * Time.deltaTime;
        
        characterPos += Vector3.down * Mathf.Min(topSpeed, _speed);

        CharacterController.transform.position = characterPos;
    }

    //====================================================================================================================//
    

    public override void Interact(CharacterController characterController)
    {
        CharacterController = characterController;

        CharacterController.SetLocked(true);
        var pos = CharacterController.transform.position;
        pos.x = transform.position.x;
        CharacterController.transform.position = pos;
        
        CharacterController.SetSpriteOrder(GetSortingOrder() + 1);
        Interacting = true;
        InteractCooldown = true;
        
        CharacterController.Animator.StartAnimation("Slide");
    }

    protected override void StopInteract()
    {
        _speed = 0f;
        Interacting = false;
        CharacterController.SetLocked(false);
        CharacterController.SetSpriteOrder(0);
        CharacterController = null;
    }

    //====================================================================================================================//
    public void SetEndHeight(float endHeight)
    {
        _endHeight = endHeight;
    }
}
