using UnityEngine;

[CreateAssetMenu(fileName = "New Mask Data", menuName = "Scriptable Objects/Mask Data")]
public class MaskData : ScriptableObject
{
    [Header("Mask Settings")]
    public string displayName;
    public Sprite sprite;
}