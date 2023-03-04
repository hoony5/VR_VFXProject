using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXCurve : VFXValue
{
    public VFXCurve(ExposedProperty property, AnimationCurve value)
    {
        displayName = property.ToString();
        propertyType = VFXPropertyType.Curve;
        exposedProperty = property;
        this.value = value;
    }
    public AnimationCurve value;
}