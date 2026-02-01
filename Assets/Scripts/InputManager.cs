using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{

    [Header("References")]
    public PlayerController playerController;
    
    public static InputManager Instance;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Hotbar.performed += OnHotbar;
        playerInputActions.Player.Cycle.performed += OnCycle;
        playerInputActions.Player.Interact.performed += OnInteract;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.LogFormat("InputManager: OnInteract triggered.");
        if (!context.performed) return;

        playerController.TryInteract();
    }


    public void OnHotbar(InputAction.CallbackContext context)
    {
        Debug.LogFormat("InputManager: OnHotbar triggered with control {0}.", context.control);
        if (!context.performed) return;
        
        // Cast the control to a KeyControl to get the specific Key enum
        var keyControl = context.control as KeyControl;
        
        if (keyControl != null)
        {
            // Key.Digit1 is the enum start. 
            // If you press '1', logic is: (Key) - (Key.Digit1) = 0.
            // Add 1 if your inventory is 1-based, or keep 0 for arrays.
            int index = keyControl.keyCode - Key.Digit1;

            // Optional: Clamp to ensure 0-8 range just in case
            if (index >= 0 && index <= 8)
            {
                Debug.LogFormat("InputManager: Equipping hotbar index {0}.", index);
                InventoryManager.Instance.EquipItem(index);
            }
        }
    }

    public void OnCycle(InputAction.CallbackContext context)
    {
        Debug.LogFormat("InputManager: OnCycle triggered with value {0}.", context.ReadValue<float>());
        // Only run on 'performed' to avoid double firing
        if (!context.performed) return;

        // Get the value (-1 or 1)
        float value = context.ReadValue<float>();

        // Convert to integer direction (1 for Next, -1 for Previous)
        int direction = value > 0 ? 1 : -1;

        Debug.LogFormat("InputManager: Cycling inventory in direction {0}.", direction);

        switch (direction)
        {
            case 1:
                InventoryManager.Instance.EquipNext();
                break;
            case -1:
                InventoryManager.Instance.EquipPrevious();
                break;
            default:
                Debug.LogErrorFormat("InputManager: Invalid cycle direction {0}.", direction);
                break;
        }
    }






}

