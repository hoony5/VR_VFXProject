using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class VFXControllerInitializer
{
    private List<VFXValueInfo> tempValueContainer = new List<VFXValueInfo>(128);
    #region Init Property & Values
    
    public void MigrateVFXPropertiesData(VisualEffect vfx, List<VFXExposedProperty> vfxExposedPropertyList, VFXValueContainer valueInfo)
    {
        if (vfxExposedPropertyList.Count != 0) return;
        
        vfxExposedPropertyList.Clear();
        tempValueContainer.Clear();
        vfx.visualEffectAsset.GetExposedProperties(vfxExposedPropertyList);
        
        if (vfxExposedPropertyList.Count == 0)
        {
            Debug.LogError($"There are no VFX Properties");
            return;
        }
        
        for (int i = 0; i < vfxExposedPropertyList.Count; i++)
        {
            VFXExposedProperty current = vfxExposedPropertyList[i];
            
            if(current.type != typeof(int) && current.type != typeof(float) && current.type != typeof(string)  && current.type != typeof(bool)
               && current.type != typeof(Vector2) && current.type != typeof(Vector3) && current.type != typeof(AnimationCurve)
               && current.type != typeof(Gradient)) continue;
            
            ExposedProperty exposedProperty = current.name;
            VFXValueInfo vfxValue = null;

            if (current.type == typeof(int))
                vfxValue = new VFXIntInfo(exposedProperty, 0);
            if(current.type == typeof(float))
                vfxValue = new VFXFloatInfo(exposedProperty, 0);
            if(current.type == typeof(AnimationCurve))
                vfxValue = new VFXCurveInfo(exposedProperty, null);
            if(current.type == typeof(Vector2))
                vfxValue = new VFXVector2Info(exposedProperty, Vector2.zero);
            if(current.type == typeof(Vector3))
                vfxValue = new VFXVector3Info(exposedProperty, Vector3.zero);
            if(current.type == typeof(bool))
                vfxValue = new VFXBoolInfo(exposedProperty, false);
            if(current.type == typeof(Gradient))
                vfxValue = new VFXGradientInfo(exposedProperty, null);
            
            if (vfxValue is null) continue;
            vfxValue.displayName = current.name;
            tempValueContainer.Add(vfxValue);
        }

         valueInfo.propertyValues = tempValueContainer.ToArray();
    }
    public void SaveStartVFXValues(VisualEffect vfx,VFXValueContainer valueInfo)
    {
        foreach (VFXValueInfo info in valueInfo.propertyValues)
        {
            SetValueByType(vfx, info);
        }
    }

    private void SetValueByType(VisualEffect vfx, VFXValueInfo value)
    {
        switch (value.GetPropertyType())
        {
            default:
            case VFXPropertyType.Bool:
            case VFXPropertyType.None:
            case VFXPropertyType.String:
                break;
            case VFXPropertyType.Float:
                SaveFloat(vfx, (VFXFloatInfo)value);
                break;
            case VFXPropertyType.Int:
                SaveInt(vfx, (VFXIntInfo)value);
                break;
            case VFXPropertyType.Vector2:
                SaveVector2(vfx, (VFXVector2Info)value);
                break;
            case VFXPropertyType.Vector3:
                SaveVector3(vfx, (VFXVector3Info)value);
                break;
            case VFXPropertyType.Curve:
                SaveCurve(vfx, (VFXCurveInfo)value);
                break;
            case VFXPropertyType.Gradient:
                SaveGradient(vfx, (VFXGradientInfo)value);
                break;
        }
        value.Reset();
    }
    private void SaveInt(VisualEffect vfx, VFXIntInfo value)
    {
        value.SaveStartValue(vfx.GetInt(value.GetExposedPropertyID()));
    }
    private void SaveFloat(VisualEffect vfx,VFXFloatInfo value)
    {
        value.SaveStartValue(vfx.GetFloat(value.GetExposedPropertyID()));
    }
    
    private void SaveVector2(VisualEffect vfx, VFXVector2Info value)
    {
        value.SaveStartValue(vfx.GetVector2(value.GetExposedPropertyID()));
    }
    private void SaveVector3(VisualEffect vfx, VFXVector3Info value)
    {
        value.SaveStartValue(vfx.GetVector3(value.GetExposedPropertyID()));
    }
    private void SaveCurve(VisualEffect vfx, VFXCurveInfo value)
    {
        value.SaveStartValue(vfx.GetAnimationCurve(value.GetExposedPropertyID()));
    }
    private void SaveGradient(VisualEffect vfx, VFXGradientInfo value)
    {
        value.SaveStartValue(vfx.GetGradient(value.GetExposedPropertyID()));
    }
    #endregion

}