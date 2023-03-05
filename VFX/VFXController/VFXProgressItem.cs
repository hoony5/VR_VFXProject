using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VFXProgressItem
{
    public string index;
    public Transform area;
    public List<VFXInteractValue> interactValues;

    public VFXProgressItem(int valueCapacity = 128)
    {
        interactValues = new List<VFXInteractValue>(valueCapacity);
    }
    public void ApplyValeus(VFXTraits traits)
    {
        for (int i = 0; i < interactValues.Count; i++)
        {
            VFXInteractValue interactValue = interactValues[i];
            switch (interactValue.propertyType)
            {
                case VFXPropertyType.Float:
                    interactValue.ApplyFloat(traits,
                        ((VFXFloat)interactValue.vfxValue).value);
                    continue;
                case VFXPropertyType.Int:
                    interactValue.ApplyInt(traits,
                        ((VFXInt)interactValue.vfxValue).value > 0
                            ? ((VFXInt)interactValue.vfxValue).value
                            : 1);
                    continue;
                case VFXPropertyType.Bool:
                    interactValue.ApplyBool(traits, ((VFXBool)interactValue.vfxValue).value);
                    continue;
                case VFXPropertyType.Vector2:
                    interactValue.ApplyVector2(traits, interactValue.axis,
                        ((VFXVector2)interactValue.vfxValue).value);
                    continue;
                case VFXPropertyType.Vector3:
                    Vector3 value = ((VFXVector3)interactValue.vfxValue).value;
                    if (interactValue.isScale)
                        interactValue.ApplyScale(traits, interactValue.axis, value);
                    else interactValue.ApplyVector3(traits, interactValue.axis, value);
                    continue;
                case VFXPropertyType.String:
                    interactValue.ApplyString(traits, VFXStepType.Current);
                    continue;
                case VFXPropertyType.Curve:
                    interactValue.ApplyCurve(traits, VFXStepType.Current);
                    continue;
                case VFXPropertyType.Gradient:
                    interactValue.ApplyGradient(traits, VFXStepType.Current);
                    continue;
                default:
                case VFXPropertyType.None:
                    continue;
            }
        }
    }
}