using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Bootstrap : MonoBehaviour
{

    [Header("Manager Classes")]
    [SerializeField] private GameObject _systemsPrefab;

    void Awake()
    {
        // Check if the "Brain" exists. If not, we are in a fresh start.
        if (GameManager.Instance == null)
        {
            Debug.Log("[Bootstrap] Initializing Systems...");
            
            GameObject systems = Instantiate(_systemsPrefab);
            
            systems.name = "[SYSTEMS]";
            
            DontDestroyOnLoad(systems);
        }
        else
        {
             Debug.Log("[Bootstrap] Systems already exist. Skipping.");
        }

        Destroy(gameObject);
    }

    void Start()
    {
        // Only load the main menu if we started in the "_Startup" scene.
        if (SceneManager.GetActiveScene().name == "_Startup")
        {
             SceneManager.LoadScene("MainMenu");
        }
    }

}