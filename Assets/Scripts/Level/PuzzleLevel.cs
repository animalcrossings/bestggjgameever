using System.Collections.Generic;
using UnityEngine;


public class PuzzleLevel
{

    public PuzzleLevelData LevelData { get; private set; }
    private List<PuzzleEntity> ActiveEntities = new List<PuzzleEntity>();
    public PuzzleTileNode[,] Grid { get; private set; } 

    public PuzzleLevel(PuzzleLevelData data)
    {
        LevelData = data;
        // TODO: Additional initialization logic here
        LoadGridFromData();
    }

    public void LoadGridFromData()
{
        Grid = new PuzzleTileNode[LevelData.width, LevelData.height];

        for (int i = 0; i < LevelData.layout.Length; i++)
        {
            // Convert 1D Index -> 2D Coordinates
            int x = i % LevelData.width;
            int y = i / LevelData.width;

            PuzzleTileType type = LevelData.layout[i];

            // Create the live node
            Grid[x, y] = new PuzzleTileNode { x = x, y = y, type = type };
        }
    }

    private void InstantiateDynamicEntities()
    {
        foreach (var entity in LevelData.entities)
        {
            // TODO: Initialize or activate dynamic entities
        }
    }

}