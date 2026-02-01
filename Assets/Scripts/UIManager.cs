using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Inventory Panels")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Prefabs")]
    [SerializeField] private GameObject inventorySlotPrefab;

    private Dictionary<int, GameObject> inventorySlotObjects = new Dictionary<int, GameObject>();


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

    public void Start()
    {
        CreateInventorySlots();
    }

    private void CreateInventorySlots()
    {
        for (int i = 0; i < InventoryManager.Instance.InventoryItems.Length; i++)
        {
            GameObject slotObj = Instantiate(inventorySlotPrefab, inventoryPanel.transform);
            inventorySlotObjects[i] = slotObj;
        }
    }


    public void RefreshInventory()
    {
        UpdatePanel(InventoryManager.Instance.InventoryItems);
    }

    private void UpdatePanel(InventoryItem[] items)
    {
        // Logic to update the masks panel UI
        Debug.Log("[UIManager] Updating Inventory Panel.");

        for (int i = 0; i < items.Length; i++)
        {
            InventoryItem item = items[i];
            GameObject slotObj = inventorySlotObjects[i];
            InventorySlot slotComponent = slotObj.GetComponent<InventorySlot>();
            if (item != null)
            {
                Debug.LogFormat("UIManager: Setting slot {0} to item {1}.", i, item.inventoryItemData.displayName);
                slotComponent.SetItem(item.inventoryItemData);
            }
            else
            {
                Debug.LogFormat("UIManager: Clearing slot {0}.", i);
                slotComponent.ClearItem();
            }

            // Highlight the equipped item
            if (i == InventoryManager.Instance.EquippedIndex)
            {
                slotComponent.SetSelected(true);
            }
            else
            {
                slotComponent.SetSelected(false);
            }
        }

    }

}