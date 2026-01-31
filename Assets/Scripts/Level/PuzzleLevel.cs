using System;
using System.Collections.Generic;
using UnityEngine;


public class PuzzleLevel
{

    public PuzzleLevelData levelData { get; private set; }
    private List<PuzzleEntity> activeEntities = new List<PuzzleEntity>();

    public PuzzleLevel(PuzzleLevelData data)
    {
        levelData = data;
        // TODO: Additional initialization logic here
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        throw new NotImplementedException("InitializeGrid method not implemented yet.");

        // InstantiateDynamicEntities();
    }

    private void InstantiateDynamicEntities()
    {
        foreach (var entity in levelData.dynamicEntities)
        {
            // TODO: Initialize or activate dynamic entities
        }
    }

}