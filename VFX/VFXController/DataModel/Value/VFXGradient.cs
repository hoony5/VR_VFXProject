using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXGradient : VFXValue
{
    public VFXGradient(ExposedProperty property, Gradient value)
    {
        displayName = property.ToString();
        propertyType = VFXPropertyType.Gradient;
        exposedProperty = property;
        this.value = value;
    }
    public Gradient value;   
}