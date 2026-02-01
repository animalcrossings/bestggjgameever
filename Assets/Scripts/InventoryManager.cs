using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Jobs;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private const int INVENTORY_SIZE = 9;
    private const int MASK_SLOTS = 4;
    private const int KEY_SLOTS = 5;

    [SerializeField] public GameObject inventoryItemPrefab;
    public InventoryItem[] InventoryItems {get; private set;} = new InventoryItem[INVENTORY_SIZE];

    public int EquippedIndex {get; private set;} = 0;

    enum SlotType
    {
        MASK,
        KEY
    }

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

    private int FindEmptySlot(SlotType slotType)
    {
        int startIndex = slotType == SlotType.MASK ? 0 : MASK_SLOTS;
        int endIndex = slotType == SlotType.MASK ? MASK_SLOTS : INVENTORY_SIZE;

        for (int i = startIndex; i < endIndex; i++)
        {
            if (InventoryItems[i] == null)
            {
                return i;
            }
        }
        return -1; 
    }

    public void AddItem(InventoryItemData item)
    {
        int emptySlot = -1;
        if(item.inventoryItemType == InventoryItemType.MASK)
        {
            emptySlot = FindEmptySlot(SlotType.MASK);
        }
        else
        {
            emptySlot = FindEmptySlot(SlotType.KEY);
        }

        if (emptySlot == -1)
        {
            Debug.LogError("InventoryManager: No empty slots available.");
            return;
        }

        InventoryItem newItem = Instantiate(inventoryItemPrefab, transform).GetComponent<InventoryItem>();
        newItem.Initialize(item);
        InventoryItems[emptySlot] = newItem;

        Debug.LogFormat("InventoryManager: Added item {0} to inventory slot {1}, InventoryItems: {2}.", item.name, emptySlot, InventoryItems[emptySlot]);
        UIManager.Instance.RefreshInventory();
    }

    public void EquipItem(int index)
    {
        EquippedIndex = index;
        UIManager.Instance.RefreshInventory();
    }

    public void EquipNext()
    {
        EquippedIndex = (EquippedIndex + 1) % INVENTORY_SIZE;
        UIManager.Instance.RefreshInventory();
    }

    public void EquipPrevious()
    {
        EquippedIndex = (EquippedIndex - 1 + INVENTORY_SIZE) % INVENTORY_SIZE;
        UIManager.Instance.RefreshInventory();
    }

    public bool IsEquippedByItemData(InventoryItemData itemData)
    {
        InventoryItem? item = InventoryItems[EquippedIndex];
        return item != null && item.CheckId(itemData.id);
    }

    public bool IsEquippedByItemId(string id)
    {
        InventoryItem? item = InventoryItems[EquippedIndex];
        return item != null && item.CheckId(id);
    }

    public bool UseItemById(string id)
    {
        InventoryItem? item = InventoryItems[EquippedIndex];
        if (!IsEquippedByItemId(id))
        {
            Debug.LogErrorFormat("InventoryManager: Equipped item does not match the requested id {0}.", id);
            return false;
        }

        if (item.inventoryItemData.inventoryItemType == InventoryItemType.MASK)
        {
            Debug.LogErrorFormat("InventoryManager: Cannot use a mask item {0}.", item.inventoryItemData.name);
            return false;
        }

        InventoryItems[EquippedIndex] = null;
        Debug.LogFormat("InventoryManager: Used item {0} from inventory.", id);
        UIManager.Instance.RefreshInventory();
        return true;
    }


    public void RemoveVanishableItems()
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            InventoryItem item = InventoryItems[i];
            if (item != null && item.DoKeepBetweenLevels() == false)
            {
                InventoryItems[i] = null;
            }
        }
        Debug.Log("[Inventory] Removed vanishable items at level end.");
        UIManager.Instance.RefreshInventory();
    }



    public void OnCycle(InputAction.CallbackContext context)
    {
        Debug.LogFormat("InventoryManager: OnCycle triggered with value {0}.", context.ReadValue<float>());
        // Only run on 'performed' to avoid double firing
        if (!context.performed) return;

        // Get the value (-1 or 1)
        float value = context.ReadValue<float>();

        // Convert to integer direction (1 for Next, -1 for Previous)
        int direction = value > 0 ? 1 : -1;

        Debug.LogFormat("InventoryManager: Cycling inventory in direction {0}.", direction);

        switch (direction)
        {
            case 1:
                EquipNext();
                break;
            case -1:
                EquipPrevious();
                break;
            default:
                Debug.LogErrorFormat("InventoryManager: Invalid cycle direction {0}.", direction);
                break;
        }
    }




}