using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXFloatInfo : VFXValueInfo
{
    public VFXFloatInfo(ExposedProperty property, float value)
    {
        vfxFloat = new VFXFloat(property, value);
    }
    [SerializeField] private VFXFloat vfxFloat;
    
    [SerializeField] private float start;
    [SerializeField] private float min;
    [SerializeField] private float max;

    public float Value => vfxFloat.value;

    public override VFXPropertyType GetPropertyType()
    {
        return vfxFloat.propertyType;
    }
    public float MaxValue => max;
    public override ExposedProperty GetExposedPropertyID()
    {
        return vfxFloat.exposedProperty;
    }

    public override void Reset()
    {
        vfxFloat.value = start;
    }
    public void SaveStartValue(float startValue)
    {
        start = startValue;
    }
    // AFTER ADD VALUE
    private bool IsMinValue()
    {
        return vfxFloat.value <= min;
    }

    // AFTER ADD VALUE
    private bool IsMaxValue()
    {
        return vfxFloat.value >= max;
    }
    
    public void AddValue(float value)
    {
        vfxFloat.value += value;
        if (IsMaxValue())
            vfxFloat.value = max;
        if (IsMinValue())
            vfxFloat.value = min;
    }
}