using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Objects/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Item Settings")]
    public string displayName;
    public Sprite sprite;
    public ItemType type;
    public bool keepBetweenLevels; 
}

public enum ItemType
{
    OTHER,
    RED_KEY,
    GREEN_KEY,
    BLUE_KEY,
    YELLOW_KEY
}