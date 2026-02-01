using UnityEngine;

public class InventoryItem : MonoBehaviour
{

    public InventoryItemData inventoryItemData;

    public void Initialize(InventoryItemData data)
    {
        inventoryItemData = data;
    }

    public InventoryItemType? GetItemType()
    {
        return inventoryItemData != null ? inventoryItemData.inventoryItemType : (InventoryItemType?)null;
    }

    public bool CheckId(string id)
    {
        return inventoryItemData != null && inventoryItemData.id == id;
    }

    public bool DoKeepBetweenLevels()
    {
        return inventoryItemData != null && inventoryItemData.keepBetweenLevels;
    }
}