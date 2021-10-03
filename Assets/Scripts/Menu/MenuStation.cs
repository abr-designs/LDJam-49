using UnityEngine;

public class MenuStation : InteractableBase
{

    [SerializeField]
    private bool opensSettings;
    
    protected override CharacterController CharacterController { get; set; }
    protected override bool Interacting { get; set; }
    protected override bool InteractCooldown { get; set; }

    private static MenuController MenuController
    {
        get
        {
            if (_menuController == null)
                _menuController = FindObjectOfType<MenuController>();

            return _menuController;
        }
    }
    private static MenuController _menuController;
    

    // Update is called once per frame
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
        }
    }


    public override void Interact(CharacterController characterController)
    {
        if(opensSettings)
            MenuController.OpenSettingsMenu();
        else 
            MenuController.OpenStartMenu();
        
        CharacterController = characterController;

        CharacterController.SetLocked(true);

        Interacting = true;
        InteractCooldown = true;
    }

    protected override void StopInteract()
    {
        MenuController.CloseWindows();
        
        Interacting = false;
        CharacterController.SetLocked(false);
        CharacterController = null;
    }

    public void ForceStopInteraction()
    {
        if (CharacterController == null)
            return;

        StopInteract();
    }

}
