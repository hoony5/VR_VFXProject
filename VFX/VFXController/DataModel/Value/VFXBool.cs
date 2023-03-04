using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXBool : VFXValue
{
    public VFXBool(ExposedProperty property, bool value)
    {
        displayName = property.ToString();
        propertyType = VFXPropertyType.Bool;
        exposedProperty = property;
        this.value = value;
    }
    public bool value;
}