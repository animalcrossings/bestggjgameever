using UnityEngine;

[CreateAssetMenu(fileName = "NewMaskData", menuName = "Scriptable Objects/Mask Data")]
public class MaskData : ItemData {

    public MaskData()
    {
        vanishable = false;
    }

    public virtual void OnPassiveUpdate() {}
    public virtual void OnActiveUse() {}
    public virtual void OnEquip() {}

    public virtual void OnUnequip() {}


}