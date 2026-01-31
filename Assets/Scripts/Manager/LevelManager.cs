using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{

    private Dictionary<int, PuzzleLevelData> levels;

    public static LevelManager Instance { get; private set; }

    public PuzzleLevel CurrentLevel { get; private set; }

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

    public void Start()
    {
        LoadAllLevelsFromFile();
    }

    public void StartLevel(int levelIndex)
    {
        Debug.LogFormat("[LevelManager] Starting level with index: {0}", levelIndex);
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("[LevelManager] No levels loaded. Cannot start level.");
            return;
        }
        
        if (!levels.ContainsKey(levelIndex))
        {
            Debug.LogFormat("[LevelManager] Invalid level index: {0}. Cannot start level.", levelIndex);
            return;
        }

        PuzzleLevelData levelData = levels[levelIndex];
        LoadLevel(levelData);
    }

    
    private void LoadLevel(PuzzleLevelData levelData)
    {
        Debug.LogFormat("[LevelManager] Loading level: {0} (Index: {1})", levelData.name, levelData.index);
        PuzzleLevel puzzleLevel = new PuzzleLevel(levelData);
        CurrentLevel = puzzleLevel;
        SceneManager.LoadScene(levelData.sceneName);
    }

    /// <summary>
    /// Loads all levels in the Resources/Levels folder.
    /// </summary>
    public void LoadAllLevelsFromFile()
    {
        Debug.Log("[LevelManager] Loading all levels from file...");
        // Load all levels from Resources/Levels folder
        PuzzleLevelData[] loadedLevels = Resources.LoadAll<PuzzleLevelData>("Levels");
        if (loadedLevels.Length == 0)
        {
            Debug.LogError("[LevelManager] No levels found in Resources/Levels folder.");
        }
        levels = new Dictionary<int, PuzzleLevelData>();
        foreach (var level in loadedLevels)
        {
            Debug.LogFormat("[LevelManager] Loading level: {0} (Index: {1})", level.name, level.index);

            levels[level.index] = level;
        }
        Debug.LogFormat("[LevelManager] Total levels loaded: {0}", levels.Count);
    }

    private bool ValidateLevelData(PuzzleLevelData levelData)
    {
        // TODO: Validate the current level data

        // Player start location exists
        // Exit location exists
        // All tiles are valid types

        return true;
    }

    public void ReloadCurrentLevel()
    {
        if (CurrentLevel == null)
        {
            Debug.LogError("[LevelManager] No current level to reload.");
            return;
        }

        Debug.LogFormat("[LevelManager] Reloading current level: {0} (Index: {1})", CurrentLevel.LevelData.name, CurrentLevel.LevelData.index);
        LoadLevel(CurrentLevel.LevelData);
    }


    /// <summary>
    ///  Replaces all instances of a specific tile type with another in the current level.
    /// </summary>
    /// <param name="oldType">Old tile type to be replaced.</param>
    /// <param name="newType">New tile type to replace with.</param>
    public void ReplaceTileType(PuzzleTileType oldType, PuzzleTileType newType)
    {
        throw new NotImplementedException("ReplaceTileType method is not implemented yet.");
    }
}