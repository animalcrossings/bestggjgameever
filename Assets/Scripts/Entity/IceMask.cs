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