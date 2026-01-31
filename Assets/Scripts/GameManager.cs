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
        // Add level completion logic here (e.g., load next level, show UI, etc.)
    }

}