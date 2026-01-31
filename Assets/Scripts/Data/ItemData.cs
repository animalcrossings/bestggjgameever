using UnityEngine;

// Base class for anything that goes into the Inventory
public abstract class ItemData : ScriptableObject
{
    [Header("Item Settings")]
    [Tooltip("Unique identifier for the item.")]
    public string id; 
    public string displayName;
    public Sprite icon;


    
    [Tooltip("If true, this item is removed from inventory when the level ends.")]
    public bool vanishable; 
}