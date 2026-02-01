using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorData doorData;

    public bool isLocked { get; private set; } = true;

    [Header("References")]
    public SpriteRenderer spriteRenderer;

    public void Awake()
    {
        if (spriteRenderer == null || doorData == null)
        {
            Debug.LogError("Door: Missing references for initialization.");
            return;
        }
        
        if (isLocked)
        {
            spriteRenderer.sprite = doorData.lockedSprite;
        }
        else
        {
            spriteRenderer.sprite = doorData.unlockedSprite;
        }
    }

    public void OpenDoor()
    {
        if (isLocked)
        {
            isLocked = false;
            spriteRenderer.sprite = doorData.unlockedSprite;
            Debug.LogFormat("Door: door with key {0} unlocked.", doorData.key);
        }
        else
        {
            Debug.LogFormat("Door: door with key {0} is already unlocked.", doorData.key);
        }
    }
    
}