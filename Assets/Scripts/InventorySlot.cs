using System;
using UnityEngine;
using UnityEngine.UI;

#nullable enable
public class InventorySlot : MonoBehaviour
{
    public InventoryItemData? inventoryItemData;

    [Header("References")]
    [SerializeField] public Image IconImage;    
    [SerializeField] public Image? BgImage;
    [SerializeField] public Sprite? BgSprite;
    [SerializeField] public Sprite? SelectedBgSprite;

    private bool isSelected = false;

    public void Awake()
    {
        if (BgImage != null && BgSprite != null)
        {
            BgImage.sprite = BgSprite;
        }
    }

    public void Start()
    {
        
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (BgImage != null)
        {
            BgImage.sprite = isSelected && SelectedBgSprite != null ? SelectedBgSprite : BgSprite;
        }
    }

    public void SetItem(InventoryItemData? item)
    {
        inventoryItemData = item;
        IconImage.sprite = item != null ? item.sprite : null;
        if (BgImage != null && BgSprite != null)
        {
            BgImage.sprite = BgSprite;
        }
    }

    public void ClearItem()
    {
        inventoryItemData = null;
        IconImage.sprite = null;
    }
}