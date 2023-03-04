using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXCurveInfo : VFXValueInfo
{
    public VFXCurveInfo(ExposedProperty property, AnimationCurve value)
    {
        vfxCurve = new VFXCurve(property, value);
    }
    [SerializeField] private VFXCurve vfxCurve;
        
    [SerializeField] private AnimationCurve start; 
    [SerializeField] private AnimationCurve end;

    public override VFXPropertyType GetPropertyType()
    {
        return vfxCurve.propertyType;
    }

    public AnimationCurve Value => vfxCurve.value;
    public void SaveStartValue(AnimationCurve startValue)
    {
        start = startValue;
    }

    public override ExposedProperty GetExposedPropertyID()
    {
        return vfxCurve.exposedProperty;
    }

    public override void Reset()
    {
        vfxCurve.value = start;
    }

    public void ConvertToEnd()
    {
        vfxCurve.value = end;
    }
}