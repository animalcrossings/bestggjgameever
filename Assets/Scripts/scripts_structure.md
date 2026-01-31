# Scripts Folder Structure and Contents

## Root Scripts

### AudioManager.cs
```csharp
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

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
}
```

### Bootstrap.cs
```csharp
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
```

### GameManager.cs
```csharp
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
```

## Data Folder

### MaskData.cs
```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaskData", menuName = "Scriptable Objects/Mask Data")]
public class MaskData : ScriptableObject {
    public string maskName;
    public Sprite maskSprite;

    public virtual void OnPassiveUpdate() {}

    public virtual void OnActiveUse() {}

    public virtual void OnEquip() {}


}
```

### PuzzleEntityData.cs
```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzleEntityData", menuName = "Scriptable Objects/Puzzle Entity Data")]
public class PuzzleEntityData : ScriptableObject
{
    public string entityName;
    public Sprite defaultSprite;
    public bool isPushable;
    public bool isDestructible;
    public string moveSoundEffect;
}
```

### PuzzleLevelData.cs
```csharp
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PuzzleLevelData : ScriptableObject
{
    public int index;



    public int width;
    public int height;
    public PuzzleTileNode[,] grid;
    public List<PuzzleEntity> dynamicEntities;
    public string sceneName;
}

```

## Entity Folder

### EndLevelBlock.cs
```csharp
using UnityEngine;

public class EndLevelBlock : MonoBehaviour, IInteractable
{
    public void OnInteract(PuzzleEntity interactor)
    {
        Debug.Log("Level Completed!");
        GameManager.Instance.CompleteLevel();
    }

    public bool OnBump(PuzzleEntity bumper)
    {
        // No special bump behavior
        return false;
    }
}
```

### IInteractable.cs
```csharp
using UnityEngine;

public interface IInteractable
{
    void OnInteract(PuzzleEntity interactor);
    bool OnBump(PuzzleEntity bumper);
    
}

```

### IceMask.cs
```csharp
using System;
using NUnit.Framework.Constraints;
using UnityEngine;


[CreateAssetMenu(menuName = "Masks/IceMask")]
public class IceMask : MaskData {

    public PuzzleTileType waterTile = PuzzleTileType.Water;
    public PuzzleTileType iceTile = PuzzleTileType.Ice;

    public override void OnEquip()
    {
        LevelManager.Instance.ReplaceTileType(waterTile, iceTile);
    }



    public override void OnPassiveUpdate()
    {
        throw new NotImplementedException("IceMask OnPassiveUpdate is not implemented yet.");
    }

    public override void OnActiveUse()
    {
        throw new NotImplementedException("IceMask OnActiveUse is not implemented yet.");
    }


}
```

### PuzzleEntity.cs
```csharp
using UnityEngine;
using System.Collections;

public abstract class PuzzleEntity : MonoBehaviour
{
    [Header("State")]
    public Vector2Int GridPosition;
    public bool IsMoving { get; private set; }

    [Header("Config")]
    public PuzzleEntityData data; 

    public virtual void SnapToGrid(Vector2Int pos)
    {
        GridPosition = pos;
        transform.position = new Vector3(pos.x, pos.y, 0);
    }

    public virtual IEnumerator MoveRoutine(Vector2Int targetPos, float duration)
    {
        IsMoving = true;
        Vector3 start = transform.position;
        Vector3 end = new Vector3(targetPos.x, targetPos.y, 0);
        float elapsed = 0;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        GridPosition = targetPos;
        IsMoving = false;
        
        OnMoveComplete();
    }

    protected virtual void OnMoveComplete() { }
}
```

## Level Folder

### LevelManager.cs
```csharp
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
            Debug.LogError("No levels loaded. Cannot start level.");
            return;
        }
        
        if (!levels.ContainsKey(levelIndex))
        {
            Debug.LogError($"Invalid level index: {levelIndex}. Cannot start level.");
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
            Debug.LogError("No levels found in Resources/Levels folder.");
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
```

### PuzzleLevel.cs
```csharp
using System.Collections.Generic;
using UnityEngine;


public class PuzzleLevel : MonoBehaviour
{

    private PuzzleLevelData levelData;
    private List<PuzzleEntity> activeEntities = new List<PuzzleEntity>();


    public PuzzleLevel(PuzzleLevelData data)
    {
        levelData = data;
        // TODO: Additional initialization logic here

        InstantiateDynamicEntities();
    }

    private void InstantiateDynamicEntities()
    {
        foreach (var entity in levelData.dynamicEntities)
        {
            // TODO: Initialize or activate dynamic entities
        }
    }

}
```

### PuzzleTileNode.cs
```csharp
using UnityEngine;


public class PuzzleTileNode
{
    public int x, y;
    public PuzzleTileType type;
    public PuzzleEntity currentOccupant;
}

```

### PuzzleTileType.cs
```csharp
using UnityEngine;


public enum PuzzleTileType
{
    Floor,
    Wall,
    Water,
    Lava,
    Ice
}

```