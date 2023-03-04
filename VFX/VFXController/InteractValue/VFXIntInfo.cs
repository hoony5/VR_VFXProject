using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXIntInfo : VFXValueInfo
{
    public VFXIntInfo(ExposedProperty property, int value)
    {
        vfxInt = new VFXInt(property, value);
    }
    
    [SerializeField] private VFXInt vfxInt;
    
    [SerializeField] private int start;
    [SerializeField] private int min;
    [SerializeField] private int max;

    public override VFXPropertyType GetPropertyType()
    {
        return vfxInt.propertyType;
    }
    public int Value => vfxInt.value;
    public int MaxValue => max;
    public override ExposedProperty GetExposedPropertyID()
    {
        return vfxInt.exposedProperty;
    }

    public override void Reset()
    {
        vfxInt.value = start;
    }

    public void SaveStartValue(int startValue)
    {
        start = startValue;
    }

    // AFTER ADD VALUE
    private bool IsMinValue()
    {
        bool result = vfxInt.value <= min;
        
        if (result)
            vfxInt.value = min;

        return result;
    }

    // AFTER ADD VALUE
    private  bool IsMaxValue()
    {
        bool result = vfxInt.value >= max;
        
        if (result)
            vfxInt.value = max;

        return result;
    }
    public void AddValue(int value)
    {
        vfxInt.value += value;
        
        if (IsMaxValue())
            vfxInt.value = max;
        if (IsMinValue())
            vfxInt.value = min;
    }
}