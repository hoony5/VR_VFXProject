using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXBoolInfo : VFXValueInfo
{
    public VFXBoolInfo(ExposedProperty property, bool value)
    {
        vfxBool = new VFXBool(property, value);
    }
    [SerializeField] private VFXBool vfxBool;
    
    [SerializeField] private bool start;
    [SerializeField] private bool end;
    
    public bool Value => vfxBool.value;
    public void SaveStartValue(bool startValue)
    {
        start = startValue;
    }

    public override VFXPropertyType GetPropertyType()
    {
        return vfxBool.propertyType;
    }

    public override ExposedProperty GetExposedPropertyID()
    {
        return vfxBool.exposedProperty;
    }

    public override void Reset()
    {
        vfxBool.value = start;
    }

    public void ConvertToEnd()
    {
        vfxBool.value = end;
    }

    public void SetValue(bool value)
    {
        vfxBool.value = value;
    }
}