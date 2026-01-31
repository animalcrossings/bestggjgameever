using System;
using UnityEngine;


public class PuzzleTileNode
{
    public int x, y;
    public PuzzleTileType type;

    #nullable enable
    public PuzzleEntity? currentOccupant;
}

