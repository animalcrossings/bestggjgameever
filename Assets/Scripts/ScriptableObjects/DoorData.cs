using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Door Data", menuName = "Scriptable Objects/Door Data")]
public class DoorData : ScriptableObject
{
    [Header("Door Settings")]
    public InventoryItemData key;
    

}