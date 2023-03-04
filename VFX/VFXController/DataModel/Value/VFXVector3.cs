using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXVector3 : VFXValue
{
    public VFXVector3(ExposedProperty property, Vector3 value)
    {
        displayName = property.ToString();
        propertyType = VFXPropertyType.Vector3;
        exposedProperty = property;
        this.value = value;
    }
    public Vector3 value;
}