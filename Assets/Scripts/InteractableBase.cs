using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : Actor2DBase, IInteractable
{
    protected abstract CharacterController CharacterController { get; set; }
    protected abstract bool Interacting { get; set; }
    protected abstract bool InteractCooldown { get; set; }

    public abstract void Interact(CharacterController characterController);
    
    protected abstract void StopInteract();
}
