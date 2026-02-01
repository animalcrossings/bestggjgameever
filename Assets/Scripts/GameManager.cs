using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    public void HandleItemPickup(GameObject itemObject)
    {
        Debug.LogFormat("PlayerController: Picking up item {0}.", itemObject.name);
        if (!itemObject.TryGetComponent<Item>(out var itemComponent))
        {
            Debug.LogError("Item component missing on item object.");
            return;
        }
        InventoryManager.Instance.AddItem(itemComponent.itemData);
    }

    public void TryOpenDoor(GameObject doorObject)
    {
        // Does the player have the key?
        if (!doorObject.TryGetComponent<Door>(out var doorComponent))
        {
            Debug.LogError("Door component missing on door object.");
            return;
        }

        if (!doorComponent.isLocked)
        {
            Debug.LogFormat("PlayerController: Door with key {0} is already unlocked.", doorComponent.doorData.key);
            return;
        }

        if (!InventoryManager.Instance.HasItemByType(doorComponent.doorData.key))
        {
            Debug.LogFormat("PlayerController: Missing key {0} for door.", doorComponent.doorData.key);
            return;
        }

        Debug.LogFormat("PlayerController: Opening door with {0}.", doorComponent.doorData.key);
        doorComponent.OpenDoor();
    }
}