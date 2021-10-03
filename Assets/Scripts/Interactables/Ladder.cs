using TMPro;
using UnityEngine;

public class Ladder : InteractableBase
{
    public float climbSpeed;
    private bool _flip;

    [SerializeField]
    public GameObject controlSquarePrefab;

    private Transform _controlSquareTransform;
    private TextMeshPro _controlSquareText;
    
    protected override CharacterController CharacterController { get; set; }
    protected override bool Interacting { get; set; }
    protected override bool InteractCooldown { get; set; }
    
    private float _maxHeight;
    
    protected override void Start()
    {
        base.Start();

        ForceUpdateMaxHeight();

        var tempControlSquare = Instantiate(controlSquarePrefab);
        tempControlSquare.name = "Ladder Control Square";
        _controlSquareTransform = tempControlSquare.transform;
        _controlSquareText = tempControlSquare.GetComponentInChildren<TextMeshPro>();
        tempControlSquare.SetActive(false);
    }

    private void Update()
    {
        void CheckPosition()
        {
            if (CharacterController.transform.position.y < _maxHeight)
                return;
            
            StopInteract();
        }
        if (Interacting == false)
            return;

        _controlSquareTransform.position =
            CharacterController.transform.position + (_flip ? Vector3.right : Vector3.left) * 1.5f;


        if (InteractCooldown)
        {
            InteractCooldown = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            StopInteract();
            return;
        }
        
        if (_flip)
        {
            if (!Input.GetKeyDown(KeyCode.D)) 
                return;
            CharacterController.transform.position += Vector3.up * climbSpeed;
            _flip = !_flip;
            
            CheckPosition();
            CharacterController?.Animator.IncrementFrame();

            _controlSquareText.text = "A";
        }
        else
        {
            if (!Input.GetKeyDown(KeyCode.A)) 
                return;
            
            CharacterController.transform.position += Vector3.up * climbSpeed;
            _flip = !_flip;

            CheckPosition();
            CharacterController?.Animator.IncrementFrame();
            
            _controlSquareText.text = "D";
        }
    }


    //====================================================================================================================//

    public void ForceUpdateMaxHeight()
    {
        _maxHeight = GetSpriteBounds().max.y;
    }

    public override void Interact(CharacterController characterController)
    {
        CharacterController = characterController;

        CharacterController.SetLocked(true);
        var pos = CharacterController.transform.position;
        pos.x = transform.position.x;
        CharacterController.transform.position = pos;

        _controlSquareTransform.gameObject.SetActive(true);
        _controlSquareText.text = "A";
        
        CharacterController.SetSpriteOrder(GetSortingOrder() + 1);
        Interacting = true;
        InteractCooldown = true;
        
        CharacterController.Animator.StartAnimation("Climb");
    }
    
    protected override void StopInteract()
    {
        _controlSquareTransform.gameObject.SetActive(false);
        
        Interacting = false;
        CharacterController.SetLocked(false);
        CharacterController.SetSpriteOrder(0);
        CharacterController = null;
        
        
    }
}
