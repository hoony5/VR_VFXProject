using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    // Brain Model
    [SerializeField] private VFXModel model;

    private const int Capacity = 64;

    // VFX Exposed Properties
    private List<VFXExposedProperty> vfxExposedPropertyList;

    // VFX Controller Initializer
    private VFXControllerInitializer _initializer;

    // when hose hit fire or something hit
    [SerializeField] private VFXColliderEvent colliderEvent;

    // auto self action event something sequential vfx
    [SerializeField] private VFXTimer timer;

    // UI { vfx status or mission or something }
    [SerializeField] private VFXUIViewEvent uiViewEvent;

    // main VFX only one
    [Header("Editor Only, Not Affect Runtime")]
    [SerializeField] private VisualEffect vfx;

    private void OnEnable()
    {
        vfxExposedPropertyList = new List<VFXExposedProperty>(Capacity);
        _initializer = new VFXControllerInitializer();
    }
    
    public void MigrateVFXPropertiesDatas(VFXValueContainer valueInfo)
    {
        vfxExposedPropertyList = new List<VFXExposedProperty>(Capacity);
        _initializer = new VFXControllerInitializer();
        _initializer.MigrateVFXPropertiesData(vfx, vfxExposedPropertyList, valueInfo);
    }

    public bool IsPlaying() => model.isPlaying;

    public void StartPlay(VFXTraits traits)
    {
        traits.localVFX.Play();

        if (traits.isMainVFX)
        {
            model.StartTimer();
            model.isPlaying = true;   
        }
        if (timer)
        {
            timer.onStartVFX?.Invoke(model.GetCurrentTime().Seconds);
            timer.isStart = true;
        }

        _initializer.SaveStartVFXValues(traits.localVFX, traits.valueInfo);
    }

    public void StopPlay(VFXTraits traits)
    {
        vfx.Stop();
        
        if(traits.isMainVFX)
        {
            model.StopTimer();
            model.isPlaying = false;
        }

        if (timer)
        {
            timer.onStopVFX?.Invoke(model.GetCurrentTime().Seconds);
            timer.isStart = false;
        }
    }

    #region Collider Event

    public void SendToBrainHitEnter(bool setActive)
    {
        model.isHitEnter = setActive;
    }

    public void SendToBrainHitStay(bool setActive)
    {
        model.isHitStay = setActive;
    }

    public void SendToBrainHitExit(bool setActive)
    {
        model.isHitExit = setActive;
    }

    public void WakeUpColliderEvent()
    {
        colliderEvent.sleepOn = false;
    }

    public void SleepColliderEvent()
    {
        colliderEvent.sleepOn = true;
    }

    #endregion

    #region Time

    public TimeSpan GetProgressTime() => model.GetCurrentTime();

    #endregion

    #region UI View



    #endregion

    public float GetFloat(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXValueLoadType loadType)
    {
        if (loadType is VFXValueLoadType.Raw) return vfx.GetFloat(property);

        return valueInfo.TryGetFloat(property, out VFXFloatInfo result) ? result.Value : 0;
    }

    public int GetInt(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXValueLoadType loadType)
    {
        if (loadType is VFXValueLoadType.Raw) return vfx.GetInt(property);

        return valueInfo.TryGetInt(property, out VFXIntInfo result) ? result.Value : 0;
    }

    public bool GetBool(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXValueLoadType loadType)
    {
        if (loadType is VFXValueLoadType.Raw) return vfx.GetBool(property);

        return valueInfo.TryGetBool(property, out VFXBoolInfo result) && result.Value;
    }

    public AnimationCurve GetCurve(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXValueLoadType loadType)
    {
        if (loadType is VFXValueLoadType.Raw) return vfx.GetAnimationCurve(property);

        return valueInfo.TryGetCurve(property, out VFXCurveInfo result) ? result.Value : null;
    }

    public Vector2 GetVector2(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXValueLoadType loadType)
    {
        if (loadType is VFXValueLoadType.Raw) return vfx.GetVector2(property);

        return valueInfo.TryGetVector2(property, out VFXVector2Info result) ? result.Value : Vector2.zero;
    }

    public Vector3 GetVector3(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXValueLoadType loadType)
    {
        if (loadType is VFXValueLoadType.Raw) return vfx.GetVector3(property);

        return valueInfo.TryGetVector3(property, out VFXVector3Info result) ? result.Value : Vector3.zero;
    }

    public Gradient GetGradient(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXValueLoadType loadType)
    {
        if (loadType is VFXValueLoadType.Raw) return vfx.GetGradient(property);

        return valueInfo.TryGetGradient(property, out VFXGradientInfo result) ? result.Value : null;
    }

    public void SendString(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXStepType stepType)
    {
        if (!valueInfo.TryGetString(property, out VFXStringInfo result))
            return;

        switch (stepType)
        {
            case VFXStepType.Manually:
                break;
            case VFXStepType.Current:
                vfx.SendEvent(result.Value);
                break;
            case VFXStepType.Next:
                result.ConvertToEnd();
                vfx.SendEvent(result.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
        }
    }

    public void AddTransformScale(Transform transform, Axis axis, Vector3 value)
    {
        transform.localScale += axis switch
        {
            Axis.X => new Vector3(value.x, 0, 0),
            Axis.Y => new Vector3(0, value.y, 0),
            Axis.Z => new Vector3(0, 0, value.z),
            Axis.XY => new Vector3(value.x, value.y, 0),
            Axis.XZ => new Vector3(value.x, 0, value.z),
            Axis.YZ => new Vector3(0, value.y, value.z),
            Axis.XYZ => value,
            _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
        };
    }

    public bool AddFloat(VisualEffect vfx, VFXValueContainer valueInfo, string property, float value)
    {
        if (!valueInfo.TryGetFloat(property, out VFXFloatInfo result))
            return false;

        result.AddValue(value);
        vfx.SetFloat(property, result.Value);
        return true;
    }

    public bool AddInt(VisualEffect vfx, VFXValueContainer valueInfo, string property, int value)
    {
        if (!valueInfo.TryGetInt(property, out VFXIntInfo result))
            return false;

        result.AddValue(value);
        vfx.SetInt(property, result.Value);
        return true;
    }

    public bool SetBool(VisualEffect vfx, VFXValueContainer valueInfo, string property, bool value)
    {
        if (!valueInfo.TryGetBool(property, out VFXBoolInfo result))
            return false;

        result.SetValue(value);
        vfx.SetBool(property, result.Value);
        return true;
    }

    public bool AddVector2(VisualEffect vfx, VFXValueContainer valueInfo, string property, Axis axis,
        Vector2 value)
    {
        if (!valueInfo.TryGetVector2(property, out VFXVector2Info result))
            return false;

        switch (axis)
        {
            case Axis.X:
                result.AddX(value);
                break;
            case Axis.Y:
                result.AddY(value);
                break;
            case Axis.XY:
                result.AddXY(value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
        }

        vfx.SetVector2(property, result.Value);
        return true;
    }

    public bool AddVector3(VisualEffect vfx, VFXValueContainer valueInfo, string property, Axis axis,
        Vector3 value)
    {
        if (!valueInfo.TryGetVector3(property, out VFXVector3Info result))
            return false;

        switch (axis)
        {
            case Axis.X:
                result.AddX(value);
                break;
            case Axis.Y:
                result.AddY(value);
                break;
            case Axis.Z:
                result.AddZ(value);
                break;
            case Axis.XY:
                result.AddXY(value);
                break;
            case Axis.XZ:
                result.AddXZ(value);
                break;
            case Axis.YZ:
                result.AddYZ(value);
                break;
            case Axis.XYZ:
                result.AddXYZ(value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
        }

        vfx.SetVector3(property, result.Value);
        return true;
    }

    public bool SetCurveValue(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXStepType stepType)
    {
        if (!valueInfo.TryGetCurve(property, out VFXCurveInfo result))
            return false;

        switch (stepType)
        {
            case VFXStepType.Manually:
                break;
            case VFXStepType.Current:
                vfx.SetAnimationCurve(property, result.Value);
                break;
            case VFXStepType.Next:
                result.ConvertToEnd();
                vfx.SetAnimationCurve(property, result.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
        }

        return true;
    }

    public bool SetGradientValue(VisualEffect vfx, VFXValueContainer valueInfo, string property,
        VFXStepType stepType)
    {
        if (!valueInfo.TryGetGradient(property, out VFXGradientInfo result))
            return false;

        switch (stepType)
        {
            case VFXStepType.Manually:
                break;
            case VFXStepType.Current:
                vfx.SetGradient(property, result.Value);
                break;
            case VFXStepType.Next:
                result.ConvertToEnd();
                vfx.SetGradient(property, result.Value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
        }

        return true;
    }
}