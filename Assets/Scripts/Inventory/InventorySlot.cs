using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public Image borderImage;       // The frame (White/Yellow)
    public Image backgroundImage;   // The backing (Dark Grey)
    public GameObject activeGlow;   // Optional "Equipped" indicator

    private ItemData _myData;

    public void Setup(ItemData item)
    {
        _myData = item;
        
        if (iconImage != null)
        {
            iconImage.sprite = item.icon;
            iconImage.preserveAspect = true;
            iconImage.enabled = item.icon != null;
        }
        
        // Default state: Dimmed, no glow
        SetEquipped(false);
    }

    public void SetEquipped(bool isEquipped)
    {
        // CS:GO Style Logic:
        // Equipped: Full Opacity, Glow ON, Border Bright
        // Unequipped: Fade Opacity, Glow OFF, Border Dark
        
        float opacity = isEquipped ? 1f : 0.4f;

        if (iconImage != null) 
        {
            Color c = iconImage.color;
            c.a = opacity;
            iconImage.color = c;
        }

        if (activeGlow != null) 
            activeGlow.SetActive(isEquipped);

        // Optional: Scale punch
        transform.localScale = isEquipped ? Vector3.one * 1.15f : Vector3.one;
    }

    public ItemData GetData() => _myData;
}