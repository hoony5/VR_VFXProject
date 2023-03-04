using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXString : VFXValue
{
    public VFXString(ExposedProperty property)
    {
        displayName = property.ToString();
        propertyType = VFXPropertyType.String;
        exposedProperty = property;
    }
}