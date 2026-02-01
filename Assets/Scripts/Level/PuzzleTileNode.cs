using System;
using UnityEngine;


public class PuzzleTileNode
{
    public int x, y;
    public PuzzleTileType type;

    // #nullable enable
    // public PuzzleEntity? currentOccupant; // TODO: Implement PuzzleEntity class
}

public class PortalTileNode : PuzzleTileNode
{
    public PortalTileNode linkedPortal;
}