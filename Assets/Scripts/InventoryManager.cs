using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<ItemData> InventoryItems { get; private set; } = new List<ItemData>();
    public List<MaskData> InventoryMasks { get; private set; } = new List<MaskData>();

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

    public bool HasItemByType(ItemType itemType)
    {
        foreach (var item in InventoryItems)
        {
            if (item.type == itemType)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasMask(MaskData mask)
    {
        return InventoryMasks.Contains(mask);
    }

    public void AddItem(ItemData newItem)
    {
        if (newItem == null) return;

        if (!InventoryItems.Contains(newItem))
        {
            InventoryItems.Add(newItem);
            Debug.Log($"[Inventory] Added Item: {newItem.displayName}");
            return;
        }
        UIManager.Instance.RefreshInventory();
    }

    public void RemoveVanishableItems()
    {
        InventoryItems.RemoveAll(item => !item.keepBetweenLevels);
        Debug.Log("[Inventory] Removed vanishable items at level end.");

        // Notify UI Manager to refresh item inventory if needed
        UIManager.Instance.RefreshInventory();
    }

    public bool UseItemByType(ItemType itemType)
    {
        for (int i = 0; i < InventoryItems.Count; i++)
        {
            if (InventoryItems[i].type == itemType)
            {
                ItemData itemToUse = InventoryItems[i];
                InventoryItems.RemoveAt(i);
                Debug.Log($"[Inventory] Using Item: {itemToUse.displayName}");
                Debug.Log($"[Inventory] Item {itemToUse.displayName} has been consumed after use.");

                // Notify UI Manager to refresh item inventory if needed
                UIManager.Instance.RefreshInventory();
                return true;
            }
        }

        Debug.LogWarning($"[Inventory] Cannot use item of type {itemType} as it is not in inventory.");
        UIManager.Instance.RefreshInventory();
        return false;
    }

    public bool UseItem(ItemData item)
    {
        if (item == null) return false;

        if (!InventoryItems.Contains(item))
        {
            Debug.LogWarning($"[Inventory] Cannot use item {item.displayName} as it is not in inventory.");
            return false;
        }

        Debug.Log($"[Inventory] Using Item: {item.displayName}");
        InventoryItems.Remove(item);
        Debug.Log($"[Inventory] Item {item.displayName} has been consumed after use.");

        UIManager.Instance.RefreshInventory();
        return true;
    }

    public void AddMask(MaskData newMask)
    {
        if (newMask == null) return;

        if (!InventoryMasks.Contains(newMask))
        {
            InventoryMasks.Add(newMask);
            Debug.Log($"[Inventory] Added Mask: {newMask.displayName}");
            return;
        }
        UIManager.Instance.RefreshInventory();
    }

}