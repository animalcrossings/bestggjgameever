using UnityEngine;

public class Mask : MonoBehaviour
{
    public MaskData maskData;

    [Header("References")]
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (maskData == null || spriteRenderer == null)
        {
            Debug.LogError("Mask: Missing references for initialization.");
            return;
        }

        spriteRenderer.sprite = maskData.sprite;
    }
}
