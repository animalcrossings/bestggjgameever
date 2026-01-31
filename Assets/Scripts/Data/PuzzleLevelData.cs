using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzleLevel", menuName = "Puzzle/Level Data")]
public class PuzzleLevelData : ScriptableObject
{
    public int index;
    public string sceneName;

    [Header("Grid Dimensions")]
    public int width;
    public int height;

    [HideInInspector]
    public PuzzleTileType[] layout; 

    public List<EntitySpawnDef> entities; 
}

[System.Serializable]
public struct EntitySpawnDef
{
    public PuzzleEntity prefab; 
    public Vector2Int startPosition;
}