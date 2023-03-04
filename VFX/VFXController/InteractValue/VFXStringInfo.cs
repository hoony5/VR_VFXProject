using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXStringInfo : VFXValueInfo
{
    public VFXStringInfo(ExposedProperty property)
    {
        vfxString = new VFXString(property);
    }
    [SerializeField] private VFXString vfxString;
    
    [SerializeField] private ExposedProperty start;
    [SerializeField] private ExposedProperty end;

    public override VFXPropertyType GetPropertyType()
    {
        return vfxString.propertyType;
    }
    public int Value => vfxString.exposedProperty;
    
    public void SaveStartValue(string startValue)
    {
        start = startValue;
    }

    public override ExposedProperty GetExposedPropertyID()
    {
        return vfxString.exposedProperty;
    }

    public override void Reset()
    {
        vfxString.exposedProperty = start;
    }
    public void ConvertToEnd()
    {
        vfxString.exposedProperty = end;
    }
}