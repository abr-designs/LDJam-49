using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : InteractableBase
{
    public Vector2Int pullCountRange;
    
    public static bool Active { get; set; }
    
    protected override CharacterController CharacterController { get; set; }
    protected override bool Interacting { get; set; }
    protected override bool InteractCooldown { get; set; }

    private int _pullCount;
    
    
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {

        if (Interacting == false)
            return;

        if (InteractCooldown)
        {
            InteractCooldown = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopInteract();
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        _pullCount--;

        if (_pullCount <= 0)
        {
            Active = true;
            StopInteract();
        }
    }

    public override void Interact(CharacterController characterController)
    {
        CharacterController = characterController;

        CharacterController.SetLocked(true);
        var pos = CharacterController.transform.position;
        pos.x = transform.position.x;
        CharacterController.transform.position = pos;
        
        CharacterController.SetColor(Color.yellow + Color.red);
        CharacterController.SetSpriteOrder(GetSortingOrder() + 1);
        Interacting = true;
        InteractCooldown = true;

        _pullCount = Random.Range(pullCountRange.x, pullCountRange.y + 1);
    }
    
    protected override void StopInteract()
    {
        Interacting = false;
        CharacterController.SetLocked(false);
        CharacterController.SetColor(Color.white);
        CharacterController.SetSpriteOrder(0);
        CharacterController = null;
    }
}
