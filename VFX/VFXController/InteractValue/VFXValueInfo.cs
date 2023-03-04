using UnityEngine.VFX.Utility;

[System.Serializable]
public abstract class VFXValueInfo
{
    public string displayName;

    public abstract VFXPropertyType GetPropertyType();
    public abstract ExposedProperty GetExposedPropertyID();
    public abstract void Reset();
}