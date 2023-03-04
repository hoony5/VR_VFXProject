using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXFloat : VFXValue
{
    public VFXFloat(ExposedProperty property, float value)
    {
        displayName = property.ToString();
        propertyType = VFXPropertyType.Float;
        exposedProperty = property;
        this.value = value;
    }
    public float value;
}