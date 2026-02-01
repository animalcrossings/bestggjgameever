using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorData doorData;

    public void OpenDoor()
    {
        Debug.LogFormat("Door with key {0} opened.", doorData.key.id);
        Destroy(gameObject);
    }
    
}