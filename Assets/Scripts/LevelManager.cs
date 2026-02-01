using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

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

    public void LoadLevel(string levelName)
    {
        Debug.LogFormat("LevelManager: Loading level {0}.", levelName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        InventoryManager.Instance.ReloadLevelInventory(levelName);
    }

    public void RestartLevel()
    {
        string currentLevelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.LogFormat("LevelManager: Restarting level {0}.", currentLevelName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevelName);
        InventoryManager.Instance.ReloadLevelInventory(currentLevelName);
    }

    public void QuitGame()
    {
        Debug.Log("LevelManager: Quitting game.");
        Application.Quit();
    }
    
}
