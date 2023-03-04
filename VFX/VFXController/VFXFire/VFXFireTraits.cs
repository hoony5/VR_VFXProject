using System;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
#endif

public sealed class VFXFireTraits : VFXTraits
{
    [SerializeField] private FireType fireType;
    [SerializeField] private FireScaleType scaleType;
    [Space(10)]
    [SerializeField, Range(0, 1.0f)] private float largeFireThreshold = 1f;
    [SerializeField, Range(0, 1.0f)] private float mediumFireThreshold = 0.7f;
    [SerializeField, Range(0, 1.0f)] private float smallFireThreshold = 0.3f;
    [Space(10)]
    [SerializeField, Range(0, 1.0f)] private float hardDifficulty = 0.25f;
    [SerializeField, Range(0, 1.0f)] private float normalDifficulty = 0.6f;
    [SerializeField, Range(0, 1.0f)] private float easyDifficulty = 1f;
    [Space(10)]
    [SerializeField] private VFXFloatInfo mainParticleInfo;

#if UNITY_EDITOR
    [Button]
#endif
    private void SetMainParticle()
    {
        foreach (VFXInteractValue item in interactValueContainer.interactItems)
        {
            if (item.isMainParticle)
            {
              //  mainParticleInfo = valueInfo.TryGetFloat(item.vfxValue.exposedProperty, out VFXFloatInfo info) ? info : null;
                break;
            }
        }
    }
    private float _receiveValue;

    public bool IsValidate(FireType other)
    {
        return other == fireType;
    }
    private float GetDifficulty()
    {
        return scaleType switch
        {
            FireScaleType.None => easyDifficulty,
            FireScaleType.Small => easyDifficulty,
            FireScaleType.Midium => normalDifficulty,
            FireScaleType.Large => hardDifficulty,
            _ => throw new ArgumentOutOfRangeException("scaleType")
        };
    }

    // for main VFXControllerEvent (onStayTrigger)
    public override bool IsReceivedFloat(float value)
    {
        if (IsReset)
        {
            _receiveValue = 0;
            return false;
        }
        _receiveValue = value;
        CheckFireScaleType();
        UpdateScenarios();
        return true;
    }
    // for VFXColliderProxy Event (onStayTrigger)
    public void TryProxyTraitsRecvFloat(VFXInteractor interactor)
    {
        if (IsReset)
        {
            _receiveValue = 0;
            return;
        }

        _receiveValue = ((VFXNozzleProxy)interactor)?.damage ?? 0;
        CheckFireScaleType();
        UpdateScenarios();
    }

    private void CheckFireScaleType()
    {
        float particles = controller.GetFloat(localVFX,valueInfo, mainParticleInfo.displayName, VFXValueLoadType.Normalize);
        float max = mainParticleInfo.MaxValue;
        
        if (particles >= max * largeFireThreshold) scaleType = FireScaleType.Large;   
        if (particles >= max * mediumFireThreshold) scaleType = FireScaleType.Midium;   
        if (particles >= max * smallFireThreshold) scaleType = FireScaleType.Small;   
    }

    private VFXStepType CheckVFXStep()
    {
        return scaleType switch
        {
            FireScaleType.None => VFXStepType.Manually,
            FireScaleType.Midium => VFXStepType.Current,
            FireScaleType.Large => VFXStepType.Next,
            _ => throw new ArgumentOutOfRangeException("scaleType")
        };
    }

    private void PostProcessScenario()
    {
        foreach (string key in interactValueContainer.InteractValueKeys)
        {
            bool exist = interactValueContainer.TryGetValue(key, out VFXInteractValue result);
            if(!exist) continue;
            
            switch (result.propertyType)
            {
                case VFXPropertyType.String:
                    result.ApplyString(this, CheckVFXStep());
                    continue;
                case VFXPropertyType.Curve:
                    result.ApplyCurve(this,CheckVFXStep());
                    continue;
                case VFXPropertyType.Gradient:
                    result.ApplyGradient(this, CheckVFXStep());
                    continue;   
                default:
                    continue;
            }
        }
    }
    private void UpdateScenarios()
    {
        foreach (string key in interactValueContainer.InteractValueKeys)
        {
            bool exist = interactValueContainer.TryGetValue(key, out VFXInteractValue result);
            if(!exist) continue;
            
            switch (result.propertyType)
            {
                case VFXPropertyType.Float:
                    result.ApplyFloat(this, GetDifficulty() * _receiveValue );
                    continue;
                case VFXPropertyType.Int:
                    result.ApplyInt(this, (int)_receiveValue );
                    continue;
                case VFXPropertyType.Bool:
                    bool weakFire = scaleType is FireScaleType.Small or FireScaleType.None;
                    result.ApplyBool(this, !weakFire);
                    continue;
                case VFXPropertyType.Vector2:
                    result.ApplyVector2(this, result.axis, Vector2.one * (GetDifficulty() * _receiveValue));
                    continue;
                case VFXPropertyType.Vector3:
                    if (result.isScale) result.ApplyScale(this, transform ,result.axis,  Vector3.one * (GetDifficulty() * _receiveValue));
                    else result.ApplyVector3(this,result.axis, Vector3.one * (GetDifficulty() * _receiveValue));
                    continue;
                default:
                case VFXPropertyType.None:
                    continue;
            }

        }
    }
}