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

        // Get sizes from mask.sprite,
        // Align the RectTransform accordingly

        int height, width;
        height = mask.sprite.texture.height;
        width = mask.sprite.texture.width;

        float aspectRatio = (float)width / height;

        RectTransform rt = IconImage.GetComponent<RectTransform>();
        if (aspectRatio >= 1.0f)
        {
            rt.sizeDelta = new Vector2(100.0f, 100.0f / aspectRatio);
        }
        else
        {
            rt.sizeDelta = new Vector2(100.0f * aspectRatio, 100.0f);
        }

    }
}