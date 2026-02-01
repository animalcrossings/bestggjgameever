using UnityEngine;

[CreateAssetMenu(fileName = "New Portal Data", menuName = "Scriptable Objects/Portal Data")]
public class PortalData : ScriptableObject
{
    [Header("Portal Settings")]
    public Sprite portalSprite;
    
    [Header("Color")]
    public Color portalColor = Color.cyan;
    
    [Header("Optional Audio")]
    public AudioClip teleportSound;
}