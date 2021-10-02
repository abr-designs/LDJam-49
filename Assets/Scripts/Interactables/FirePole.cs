using UnityEngine;

public class FirePole : InteractableBase
{
    public float fallSpeed;
    public float topSpeed;
    
    protected override CharacterController CharacterController { get; set; }
    protected override bool Interacting { get; set; }
    protected override bool InteractCooldown { get; set; }
    
    private Vector2 _topPosition;
    private Vector2 _endPosition;

    private float _speed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        var spriteBounds = GetSpriteBounds();
        _topPosition = spriteBounds.max;
        _topPosition.x = spriteBounds.center.x;
        _endPosition = spriteBounds.min;
        _endPosition.x = spriteBounds.center.x;
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
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            StopInteract();
            return;
        }

        var characterPos = CharacterController.transform.position;

        if (characterPos.y <= _endPosition.y + 1)
        {
            StopInteract();
            return;
        }

        _speed += fallSpeed * Time.deltaTime;
        
        characterPos += Vector3.down * Mathf.Min(topSpeed, _speed);

        CharacterController.transform.position = characterPos;
    }

    public override void Interact(CharacterController characterController)
    {
        CharacterController = characterController;

        CharacterController.SetLocked(true);
        var pos = CharacterController.transform.position;
        pos.x = transform.position.x;
        CharacterController.transform.position = pos;
        
        CharacterController.SetSpriteOrder(GetSortingOrder() + 1);
        CharacterController.SetColor(Color.yellow);
        Interacting = true;
        InteractCooldown = true;
    }

    protected override void StopInteract()
    {
        _speed = 0f;
        Interacting = false;
        CharacterController.SetLocked(false);
        CharacterController.SetColor(Color.white);
        CharacterController.SetSpriteOrder(0);
        CharacterController = null;
    }
}
