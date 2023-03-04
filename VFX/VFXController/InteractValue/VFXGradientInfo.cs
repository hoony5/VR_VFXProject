using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXGradientInfo : VFXValueInfo
{
    public VFXGradientInfo(ExposedProperty property, Gradient value)
    {
        vfxGradient = new VFXGradient(property, value);
    }
    [SerializeField] private VFXGradient vfxGradient;

    [SerializeField] private Gradient start;
    [SerializeField] private Gradient end;

    public override VFXPropertyType GetPropertyType()
    {
        return vfxGradient.propertyType;
    }

    public Gradient Value => vfxGradient.value;
    public override ExposedProperty GetExposedPropertyID()
    {
        return vfxGradient.exposedProperty;
    }

    public override void Reset()
    {
        vfxGradient.value = start;
    }

    public void SaveStartValue(Gradient startValue)
    {
        start = startValue;
    }

    public void ConvertToEnd()
    {
        vfxGradient.value = end;
    }
}