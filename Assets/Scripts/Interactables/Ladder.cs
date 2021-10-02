using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : InteractableBase
{
    public float climbSpeed;
    private bool _flip;
    
    protected override CharacterController CharacterController { get; set; }
    protected override bool Interacting { get; set; }
    protected override bool InteractCooldown { get; set; }

    private void Update()
    {
        if (Interacting == false)
            return;

        if (InteractCooldown)
        {
            InteractCooldown = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
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
            CharacterController.SetColor(Color.red);
        }
        else
        {
            if (!Input.GetKeyDown(KeyCode.A)) 
                return;
            CharacterController.transform.position += Vector3.up * climbSpeed;
            _flip = !_flip;
            CharacterController.SetColor(Color.blue);
        }
    }

    public override void Interact(CharacterController characterController)
    {
        CharacterController = characterController;

        CharacterController.SetLocked(true);
        var pos = CharacterController.transform.position;
        pos.x = transform.position.x;
        CharacterController.transform.position = pos;
        
        CharacterController.SetColor(Color.red);
        CharacterController.SetSpriteOrder(GetSortingOrder() + 1);
        Interacting = true;
        InteractCooldown = true;
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
