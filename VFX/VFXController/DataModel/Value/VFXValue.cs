using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public abstract class VFXValue
{
    [SerializeField] public string displayName;
    [SerializeField] public ExposedProperty exposedProperty;
    [SerializeField] public VFXPropertyType propertyType;
}