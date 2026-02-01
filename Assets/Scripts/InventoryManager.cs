using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Jobs;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public Dictionary<Object, List<InventoryItemData>> levelInventories = new Dictionary<Object, List<InventoryItemData>>();


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

    public void ReloadLevelInventory(string levelName)
    {
        ClearInventory();
        foreach (var key in levelInventories.Keys.ToList())
        {
            if (key.name == levelName)
            {
                List<InventoryItemData> itemsToLoad = levelInventories[key];
                InventoryItems = new InventoryItem[INVENTORY_SIZE];
                foreach (var itemData in itemsToLoad)
                {
                    AddItem(itemData);
                }
                Debug.LogFormat("InventoryManager: Reloaded inventory for level {0} with {1} items.", levelName, itemsToLoad.Count);
                return;
            }
        }
        Debug.LogFormat("InventoryManager: No saved inventory for level {0}.", levelName);
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

    public void ClearInventory()
    {
        InventoryItems = new InventoryItem[INVENTORY_SIZE];
        EquippedIndex = 0;
        Debug.Log("InventoryManager: Cleared inventory.");
        UIManager.Instance.RefreshInventory();
    }

    public void AddItem(InventoryItemData item)
    {
        int emptySlot = -1;
        if(item.inventoryItemType == InventoryItemType.MASK)
        {
            emptySlot = FindEmptySlot(SlotType.MASK);
            AudioManager.Instance.PlaySound(AudioManager.Instance.gainMaskSound);
        }
        else
        {
            emptySlot = FindEmptySlot(SlotType.KEY);
            AudioManager.Instance.PlaySound(AudioManager.Instance.keyCollectSound);
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

}