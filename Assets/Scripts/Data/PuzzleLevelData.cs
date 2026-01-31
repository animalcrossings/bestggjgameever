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

