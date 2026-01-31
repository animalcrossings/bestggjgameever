using UnityEngine;


public class GameManager: MonoBehaviour
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
    

    public void CompleteLevel()
    {
        Debug.Log("GameManager: Level Completed!");

        // Clear vanishable items
        InventoryManager.Instance.ClearLevelSpecificItems();
    }

}