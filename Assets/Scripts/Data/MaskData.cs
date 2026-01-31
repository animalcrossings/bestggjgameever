using UnityEngine;

[CreateAssetMenu(fileName = "NewMaskData", menuName = "Scriptable Objects/Mask Data")]
public class MaskData : ScriptableObject {
    public string maskName;
    public Sprite maskSprite;

    public virtual void OnPassiveUpdate() {}

    public virtual void OnActiveUse() {}

    public virtual void OnEquip() {}


}