using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemData itemData;
    public MaskData maskData;

    [Header("References")]
    public Image IconImage;    
    public bool isMask;

    public void Setup(ItemData item)
    {
        itemData = item;
        isMask = false;
        IconImage.sprite = item.sprite;
    }

    public void Setup(MaskData mask)
    {
        maskData = mask;
        isMask = true;
        IconImage.sprite = mask.sprite;
    }
}