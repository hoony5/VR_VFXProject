using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXInt : VFXValue
{
    public VFXInt(ExposedProperty property, int value)
    {
        displayName = property.ToString();
        propertyType = VFXPropertyType.Int;
        exposedProperty = property;
        this.value = value;
    }
    public int value;
}