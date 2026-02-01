using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

	public InventoryItemData tribalMask;

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
    }

    public void HandleBreakBlock(GameObject blockObject)
    {
        Debug.LogFormat("GameManager: Breaking block {0}.", blockObject.name);
        if (!blockObject.TryGetComponent<BreakableBlock>(out var breakableBlock))
        {
            Debug.LogError("BreakableBlock component missing on block object.");
            return;
        }
        // Check if the player has the required mask equipped
        bool hasRequiredMask = InventoryManager.Instance.IsEquippedByItemId(breakableBlock.requiredMask.id);
        if (!hasRequiredMask)
        {
            Debug.LogError("Player does not have the required mask equipped to break this block.");
            return;
        }
        bool isSuccess = breakableBlock.BreakBlock();

        if (!isSuccess)
        {
            return;
        }

        AudioManager.Instance.PlaySound(AudioManager.Instance.breakBlockSound);
    }

    public void HandleItemPickup(GameObject itemObject)
    {
        Debug.LogFormat("PlayerController: Picking up item {0}.", itemObject.name);
        if (!itemObject.TryGetComponent<InventoryItem>(out var itemComponent))
        {
            Debug.LogError("InventoryItem component missing on item object.");
            return;
        }
        InventoryManager.Instance.AddItem(itemComponent.inventoryItemData);
        UIManager.Instance.RefreshInventory();
        AudioManager.Instance.PlaySound(AudioManager.Instance.keyCollectSound);
        Destroy(itemObject);
    }

    public void HandlePortalEntry(GameObject portalObject, Transform entity)
    {
        if (!portalObject.TryGetComponent<Portal>(out var portalComponent))
        {
            Debug.LogError("Portal component missing on portal object.");
            return;
        }

        Debug.Log($"GameManager: Entity '{entity.name} Entering portal {portalObject.name}.");
        portalComponent.Teleport(entity);
    }

    public void TryOpenDoor(GameObject doorObject)
    {
        // Does the player have the key?
        if (!doorObject.TryGetComponent<Door>(out var doorComponent))
        {
            Debug.LogError("Door component missing on door object.");
            return;
        }

        // if (!doorComponent.isLocked)
        // {
        //     Debug.LogFormat("PlayerController: Door with key {0} is already unlocked.", doorComponent.doorData.key);
        //     return;
        // }

        if (!InventoryManager.Instance.IsEquippedByItemId(doorComponent.doorData.key.id))
        {
            Debug.LogFormat("PlayerController: Missing key {0} for door.", doorComponent.doorData.key);
            return;
        }
        
        bool useSuccess = InventoryManager.Instance.UseItemById(doorComponent.doorData.key.id);
        if (!useSuccess)
        {
            Debug.LogError("Failed to use the key from inventory.");
            return;
        }
        Debug.LogFormat("PlayerController: Opening door with {0}.", doorComponent.doorData.key);
        doorComponent.OpenDoor();
    }




}