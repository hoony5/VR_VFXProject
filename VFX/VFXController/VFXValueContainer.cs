using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class VFXValueContainer : MonoBehaviour
{
    [SerializeReference] public VFXValueInfo[] propertyValues = Array.Empty<VFXValueInfo>();
    private const int Capacity = 64;

    private Dictionary<string, VFXValueInfo>
        propertyValueDictionary;

    private void OnEnable()
    {
        propertyValueDictionary = new Dictionary<string, VFXValueInfo>(Capacity);
        Init();
    }

    private void Init()
    {
        propertyValueDictionary.Clear();
        propertyValueDictionary = propertyValues?.ToDictionary(key => key.GetExposedPropertyID().ToString(), value => value);   
    }

    private void OnDisable()
    {
        propertyValueDictionary?.Clear();
    }

    #region TryGetValues
    public bool TryGetString(string exposedProperty, out VFXStringInfo value)
    {
        bool exist = propertyValueDictionary.TryGetValue(exposedProperty, out VFXValueInfo property);
        if (!exist)
        {
            value = null;
            return false;
        }
        
        value = (VFXStringInfo)property;
        return true;
    }
    public bool TryGetFloat(string exposedProperty, out VFXFloatInfo value)
    {
        bool exist = propertyValueDictionary.TryGetValue(exposedProperty, out VFXValueInfo property);
        
        if (!exist)
        {
            value = null;
            return false;
        }
        
        value = (VFXFloatInfo)property;
        return true;
    }
    public bool TryGetInt(string exposedProperty, out VFXIntInfo value)
    {
        bool exist = propertyValueDictionary.TryGetValue(exposedProperty, out VFXValueInfo property);
        if (!exist)
        {
            value = null;
            return false;
        }
        
        value = (VFXIntInfo)property;
        return true;
    }
    public bool TryGetVector2(string exposedProperty, out VFXVector2Info value)
    {
        bool exist = propertyValueDictionary.TryGetValue(exposedProperty, out VFXValueInfo property);
        if (!exist)
        {
            value = null;
            return false;
        }
        
        value = (VFXVector2Info)property;
        return true;
    }
    public bool TryGetVector3(string exposedProperty, out VFXVector3Info value)
    {
        bool exist = propertyValueDictionary.TryGetValue(exposedProperty, out VFXValueInfo property);
        if (!exist)
        {
            value = null;
            return false;
        }
        
        value = (VFXVector3Info)property;
        return true;
    }
    public bool TryGetCurve(string exposedProperty, out VFXCurveInfo value)
    {
        bool exist = propertyValueDictionary.TryGetValue(exposedProperty, out VFXValueInfo property);
        if (!exist)
        {
            value = null;
            return false;
        }
        
        value = (VFXCurveInfo)property;
        return true;
    }
    public bool TryGetBool(string exposedProperty, out VFXBoolInfo value)
    {
        bool exist = propertyValueDictionary.TryGetValue(exposedProperty, out VFXValueInfo property);
        if (!exist)
        {
            value = null;
            return false;
        }
        
        value = (VFXBoolInfo)property;
        return true;
    }
    public bool TryGetGradient(string exposedProperty, out VFXGradientInfo value)
    {
        bool exist = propertyValueDictionary.TryGetValue(exposedProperty, out VFXValueInfo property);
        if (!exist)
        {
            value = null;
            return false;
        }
        
        value = (VFXGradientInfo)property;
        return true;
    }
    #endregion
}