using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItemData", menuName = "Scriptable Objects/New Inventory Item (Key&Mask)")]
public class InventoryItemData : ScriptableObject
{
    [Header("Item Settings")]
    public string id;
    public string displayName;
    public Sprite sprite;
    public bool keepBetweenLevels;
    public InventoryItemType inventoryItemType;

}

