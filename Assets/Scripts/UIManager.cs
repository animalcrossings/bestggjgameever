using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Inventory Panels")]
    [SerializeField] private GameObject masksPanel;
    [SerializeField] private GameObject itemsPanel;

    [Header("Prefabs")]
    [SerializeField] private GameObject inventorySlotPrefab;


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


    public void RefreshInventory()
    {
        // Logic to refresh the inventory UI panels
        Debug.Log("[UIManager] Refreshing Inventory UI Panels.");

        UpdateMasksPanel(InventoryManager.Instance.InventoryMasks);
        UpdateItemsPanel(InventoryManager.Instance.InventoryItems);
    }

    private void UpdateMasksPanel(List<MaskData> masks)
    {
        // Logic to update the masks panel UI
        Debug.Log("[UIManager] Updating Masks Panel.");

        // Clear existing UI elements
        foreach (Transform child in masksPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new UI elements for each mask
        foreach (var mask in masks)
        {
            GameObject inventorySlot = Instantiate(inventorySlotPrefab, masksPanel.transform);   
            inventorySlot.GetComponent<InventorySlot>().Setup(mask);
        }

    }

    private void UpdateItemsPanel(List<ItemData> items)
    {
        // Logic to update the items panel UI
        Debug.Log("[UIManager] Updating Items Panel.");

        // Clear existing UI elements
        foreach (Transform child in itemsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new UI elements for each item
        foreach (var item in items)
        {
            GameObject inventorySlot = Instantiate(inventorySlotPrefab, itemsPanel.transform);   
            inventorySlot.GetComponent<InventorySlot>().Setup(item);
        }
    }

}