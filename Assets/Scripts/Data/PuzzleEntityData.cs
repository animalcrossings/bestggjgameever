using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzleEntityData", menuName = "Scriptable Objects/Puzzle Entity Data")]
public class PuzzleEntityData : ScriptableObject
{
    public string entityName;
    public Sprite defaultSprite;
    public bool isPushable;
    public bool isDestructible;
    public string moveSoundEffect;
}