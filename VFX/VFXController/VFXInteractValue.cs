using UnityEngine;

[System.Serializable]
public class VFXInteractValue
{
    // for editor work flow
    public string displayName;
    public bool isMainParticle;
    public Transform vfxZone;
    [SerializeReference] public VFXValue vfxValue;
    [SerializeField] public VFXPropertyType propertyType;
    // Float/Int
    [SerializeField] public float deltaSpeed;
    // Vector2/Vector3 
    [SerializeField] public bool isScale;
    [SerializeField] public Axis axis;
    
    public void ApplyString(VFXTraits traits, VFXStepType stepType)
    {
        traits.controller.SendString(traits.localVFX, traits.valueInfo, vfxValue.displayName, stepType);
    }
    public void ApplyFloat(VFXTraits traits, float value)
    {
        VFXFloat scenarioItem = vfxValue as VFXFloat;
        traits.controller.AddFloat(traits.localVFX, traits.valueInfo, vfxValue.displayName, value * scenarioItem.value * deltaSpeed);
    }
    public void ApplyInt(VFXTraits traits, int value)
    {
        VFXInt scenarioItem = vfxValue as VFXInt;
        traits.controller.AddInt(traits.localVFX, traits.valueInfo, vfxValue.displayName, value * deltaSpeed > 0 ? (int)(value * scenarioItem.value * deltaSpeed) : 1);
    }
    public void ApplyBool(VFXTraits traits, bool value)
    {
        traits.controller.SetBool(traits.localVFX, traits.valueInfo, vfxValue.displayName, value);
    }
    public void ApplyGradient(VFXTraits traits, VFXStepType stepType)
    {
        traits.controller.SetGradientValue(traits.localVFX, traits.valueInfo, vfxValue.displayName, stepType);
    }
    public void ApplyCurve(VFXTraits traits, VFXStepType stepType)
    {
        traits.controller.SetCurveValue(traits.localVFX, traits.valueInfo, vfxValue.displayName, stepType);
    }
    public void ApplyVector2(VFXTraits traits , Axis axis, Vector2 value)
    {
        VFXVector2 scenarioItem = vfxValue as VFXVector2;
        traits.controller.AddVector2(traits.localVFX, traits.valueInfo, vfxValue.displayName,axis, scenarioItem.value * (value.magnitude * deltaSpeed));
    }
    public void ApplyVector3(VFXTraits traits, Axis axis, Vector3 value)
    {
        VFXVector3 scenarioItem = vfxValue as VFXVector3;
        traits.controller.AddVector3(traits.localVFX, traits.valueInfo, vfxValue.displayName,axis, scenarioItem.value * (value.magnitude * deltaSpeed));
    }
    public void ApplyScale(VFXTraits traits, Axis axis, Vector3 value )
    {
        VFXVector3 scenarioItem = vfxValue as VFXVector3;
        traits.controller.AddTransformScale(vfxZone, axis, scenarioItem.value * (value.magnitude * deltaSpeed));
    }
}