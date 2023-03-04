using System;
using UnityEngine;
using UnityEngine.VFX.Utility;

[Serializable]
public sealed class VFXVector2 : VFXValue
{
    public VFXVector2(ExposedProperty property, Vector2 value)
    {
        displayName = property.ToString();
        propertyType = VFXPropertyType.Vector2;
        exposedProperty = property;
        this.value = value;
    }
    public Vector2 value;
}