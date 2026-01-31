using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance { get; private set; }

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

    // --- INVENTORY DATA STRUCTURES ---
    private List<KeyColor> _keys = new List<KeyColor>();

    // 2. Complex Items (Masks, Artifacts, Story Items)
    private List<ItemData> _items = new List<ItemData>();

    // 3. Current Equipment
    private List<MaskData> _collectedMasks = new List<MaskData>();

    public MaskData CurrentMask {get; private set;}
    private int _currentMaskIndex = -1;

    // --- ITEM LOGIC ---

    public void AddItem(ItemData newItem)
    {
        if (newItem == null) return;

        if (newItem is MaskData maskData)
        {
            Debug.Log($"[Inventory] Equipping mask: {maskData.displayName}");
            _collectedMasks.Add(maskData);

            if (CurrentMask == null)
            {
                CurrentMask = maskData;
            }
            // TODO: UI Update Signal
            // UIManager.Instance.RefreshInventory(_items);
            return;
        }

        // Optional: Prevent duplicates
        if (!_items.Contains(newItem))
        {
            _items.Add(newItem);
            Debug.Log($"[Inventory] Picked up: {newItem.displayName}");
            // TODO: UI Update Signal
            // UIManager.Instance.RefreshInventory(_items);
            return;

        }
    }

    public void CycleMask(int direction) // +1 or -1
    {
        if (_collectedMasks.Count <= 1) return; // Nothing to swap to

        // Update Index
        _currentMaskIndex += direction;

        // Loop around (Wrap)
        if (_currentMaskIndex >= _collectedMasks.Count) _currentMaskIndex = 0;
        if (_currentMaskIndex < 0) _currentMaskIndex = _collectedMasks.Count - 1;

        // Equip the new one
        EquipMask(_collectedMasks[_currentMaskIndex]);
    }

    public void EquipMask(MaskData mask)
    {
        // Logic remains the same (Unequip old, Equip new)
        if (CurrentMask != null) CurrentMask.OnUnequip();
        
        CurrentMask = mask;
        _currentMaskIndex = _collectedMasks.IndexOf(mask);

        if (CurrentMask != null) CurrentMask.OnEquip();
        
        // TODO: UI Update Signal
        // UIManager.Instance.UpdateMaskUI();
    }

    public bool HasItem(ItemData itemToCheck)
    {
        return _items.Contains(itemToCheck);
    }

    // --- KEY LOGIC (Simple) ---

    public void AddKey(KeyColor key)
    {
        _keys.Add(key);
        Debug.Log($"[Inventory] Key Added: {key}");
    }

    public bool HasKey(KeyColor key)
    {
        return _keys.Contains(key);
    }

    // --- LEVEL CLEANUP ---

    /// <summary>
    /// Called by GameManager when a level ends.
    /// Removes temporary keys and "Vanishable" items.
    /// </summary>
    public void ClearLevelSpecificItems()
    {
        // 1. Clear all simple keys (assuming keys don't carry over)
        _keys.Clear();

        // Create a copy to avoid modifying the collection while iterating
        List<ItemData> listOfItems = new List<ItemData>(_items);

        foreach (var item in listOfItems)
        {
            Debug.Log($"[Inventory] Item before cleanup: {item.displayName} (Vanishable: {item.vanishable})");
            if (item.vanishable)
            {
                Debug.Log($"[Inventory] Marked for removal: {item.displayName}");
                _items.Remove(item);
            }
        }
        // TODO: UI Update Signal
        // UIManager.Instance.RefreshInventory(_items);
    }
}