#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour
{
    [Header("Configuration")]
    public string levelName = "Level_01";
    public Tilemap sourceTilemap;
    public Transform entitiesParent; // Drag a GameObject holding all enemies here

    [Header("Mappings (Visual -> Logic)")]
    // Teammates drag the "Water Tile" asset here and select "Water" enum
    public List<TileMapping> mappings; 

    [System.Serializable]
    public struct TileMapping {
        public TileBase visualTile;
        public PuzzleTileType logicalType;
    }

    // This creates a button in the Inspector
    [ContextMenu("BAKE LEVEL")]
    public void BakeLevel()
    {
        // 1. Create or Find the Asset
        string path = $"Assets/_Game/Data/Levels/{levelName}.asset";
        PuzzleLevelData levelData = AssetDatabase.LoadAssetAtPath<PuzzleLevelData>(path);
        
        if (levelData == null)
        {
            levelData = ScriptableObject.CreateInstance<PuzzleLevelData>();
            AssetDatabase.CreateAsset(levelData, path);
        }

        // 2. Scan the Tilemap Bounds
        sourceTilemap.CompressBounds(); // Tighten to content
        BoundsInt bounds = sourceTilemap.cellBounds;
        
        levelData.width = bounds.size.x;
        levelData.height = bounds.size.y;
        levelData.layout = new PuzzleTileType[levelData.width * levelData.height];

        // 3. Iterate and Map
        for (int x = 0; x < levelData.width; x++)
        {
            for (int y = 0; y < levelData.height; y++)
            {
                // Tilemaps are centered, so we offset the position
                Vector3Int pos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);
                TileBase tile = sourceTilemap.GetTile(pos);

                PuzzleTileType type = PuzzleTileType.Floor; // Default
                
                // Find matching Enum for this Tile
                foreach (var m in mappings)
                {
                    if (m.visualTile == tile)
                    {
                        type = m.logicalType;
                        break;
                    }
                }

                // Convert 2D coord to 1D index
                int index = y * levelData.width + x; 
                levelData.layout[index] = type;
            }
        }

        // 4. Bake Entities (Optional)
        levelData.entities = new List<EntitySpawnDef>();
        foreach(Transform child in entitiesParent)
        {
            // Calculate grid position relative to bounds
            Vector3Int gridPos = sourceTilemap.WorldToCell(child.position);
            Vector2Int logicalPos = new Vector2Int(gridPos.x - bounds.xMin, gridPos.y - bounds.yMin);

            // Create Definition
            EntitySpawnDef def = new EntitySpawnDef();
            // Assuming the object name matches your Prefab names, or use a component
            // For a jam, just matching via a component "PuzzleEntity" is safer
            var entityScript = child.GetComponent<PuzzleEntity>();
            if(entityScript != null && entityScript.data != null) 
            {
                // In a real editor script, you'd link the prefab reference. 
                // For now, let's just save the position if you are linking prefabs manually later
                // OR better: Drag the source prefab into a "BakerComponent" on the object
                def.startPosition = logicalPos;
                // def.prefab = ... (This part requires you to reference the original prefab, not the scene instance)
            }
            levelData.entities.Add(def);
        }

        // 5. Save
        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();
        Debug.Log($"Level '{levelName}' Baked Successfully! Size: {levelData.width}x{levelData.height}");
    }
}
#endif