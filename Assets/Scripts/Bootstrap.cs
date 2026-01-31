using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Bootstrap : MonoBehaviour
{

    [Header("Manager Classes")]
    [SerializeField] private GameObject _gameManagerPrefab;
    [SerializeField] private GameObject _levelManagerPrefab;
    [SerializeField] private GameObject _audioManagerPrefab;


    void Awake()
    {
        // Ensure GameManager exists
        if (GameManager.Instance == null)
        {
            GameObject gameManagerObj = Instantiate(_gameManagerPrefab);
            DontDestroyOnLoad(gameManagerObj);
            
        }

        // Ensure LevelManager exists
        if (LevelManager.Instance == null)
        {
            GameObject levelManagerObj = Instantiate(_levelManagerPrefab);
            DontDestroyOnLoad(levelManagerObj);
        }

        // Ensure AudioManager exists
        if (AudioManager.Instance == null)
        {
            GameObject audioManagerObj = Instantiate(_audioManagerPrefab);
            DontDestroyOnLoad(audioManagerObj);
        }

        Destroy(this.gameObject);
    }

    void Start()
    {
        Debug.LogFormat("[Bootstrap] Loading initial scene...");
        // Optionally load the initial scene here
        SceneManager.LoadScene("MainMenu");
    }

}