// using System;
// using NUnit.Framework.Constraints;
// using UnityEngine;

// [CreateAssetMenu(menuName = "Masks/IceMask")]
// public class SanXinDuiMask : MaskData 
// {

//     public PuzzleTileType sanXinDuiTileHidden = PuzzleTileType.sanXinDuiTile;
//     public PuzzleTileType sanXinDuiTileRevealed = PuzzleTileType.sanXinDuiTileRevealed;

//     public override void OnEquip()
//     {
//         LevelManager.Instance.ReplaceTileType(sanXinDuiTileHidden, sanXinDuiTileRevealed);
//     }

//     public override void OnUnequip()
//     {
//         LevelManager.Instance.ReplaceTileType(sanXinDuiTileRevealed, sanXinDuiTileHidden);
//     }

//     public override void OnPassiveUpdate()
//     {
//         throw new NotImplementedException("IceMask OnPassiveUpdate is not implemented yet.");
//     }

//     public override void OnActiveUse()
//     {
//         throw new NotImplementedException("IceMask OnActiveUse is not implemented yet.");
//     }

// }