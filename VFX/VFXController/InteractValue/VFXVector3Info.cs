using System.ComponentModel;
using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXVector3Info : VFXValueInfo
{
    public VFXVector3Info(ExposedProperty property, Vector2 value)
    {
        vfxVector3 = new VFXVector3(property, value);
    }
    [SerializeField] private VFXVector3 vfxVector3;
    
    public Vector3 start;
    public Vector3 min;
    public Vector3 max;

    public Vector3 Value => vfxVector3.value;
    public Vector3 MaxValue => max;
    
    public override VFXPropertyType GetPropertyType()
    {
        return vfxVector3.propertyType;
    }
    public override ExposedProperty GetExposedPropertyID()
    {
        return vfxVector3.exposedProperty;
    }

    public override void Reset()
    {
        vfxVector3.value = start;
    }

    public void SaveStartValue(Vector3 startValue)
    {
        start = startValue;
    }
    // AFTER ADD VALUE
    private bool IsMinValue(Axis axis)
    {
        bool result = axis switch
        {
            Axis.X => vfxVector3.value.x <= min.x,
            Axis.Y => vfxVector3.value.y <= min.y,
            Axis.Z => vfxVector3.value.z <= min.z,
            Axis.XY => vfxVector3.value.x <= min.x && vfxVector3.value.y <= min.y,
            Axis.XZ => vfxVector3.value.x <= min.x && vfxVector3.value.z <= min.z,
            Axis.YZ => vfxVector3.value.y <= min.y && vfxVector3.value.z <= min.z,
            Axis.XYZ => vfxVector3.value.x <= min.x && vfxVector3.value.y <= min.y && vfxVector3.value.z <= min.z,
            _ => throw new InvalidEnumArgumentException("Check Axis Enum")
        };
        return result;
    }

    // AFTER ADD VALUE
    private bool IsMaxValue(Axis axis)
    {
        bool result = axis switch
        {
            Axis.X => vfxVector3.value.x >= max.x,
            Axis.Y => vfxVector3.value.y >= max.y,
            Axis.Z => vfxVector3.value.z >= max.z,
            Axis.XY => vfxVector3.value.x >= max.x && vfxVector3.value.y >= max.y,
            Axis.XZ => vfxVector3.value.x >= max.x && vfxVector3.value.z >= max.z,
            Axis.YZ => vfxVector3.value.y >= max.y && vfxVector3.value.z >= max.z,
            Axis.XYZ => vfxVector3.value.x >= max.x && vfxVector3.value.y >= max.y && vfxVector3.value.z >= max.z,
            _ => throw new InvalidEnumArgumentException("Check Axis Enum")
        };
        return result;
    }

    public void AddXYZ(Vector3 value)
    {
        vfxVector3.value += value;
        
        if (IsMaxValue(Axis.XYZ))
            vfxVector3.value = max;
        if(IsMinValue(Axis.XYZ))
            vfxVector3.value = min;
    }
    public void AddX(Vector3 value)
    {
        vfxVector3.value += new Vector3(value.x, 0, 0);
        
        if (IsMaxValue(Axis.X))
            vfxVector3.value = new Vector3(max.x, vfxVector3.value.y, vfxVector3.value.z);
        if(IsMinValue(Axis.X))
            vfxVector3.value = new Vector3(min.x, vfxVector3.value.y, vfxVector3.value.z);
    }
    public void AddY(Vector3 value)
    {
        vfxVector3.value += new Vector3(0 , value.y, 0);
        
        if (IsMaxValue(Axis.Y))
            vfxVector3.value = new Vector3(vfxVector3.value.x, max.y, vfxVector3.value.z);
        if(IsMinValue(Axis.Y))
            vfxVector3.value =  new Vector3(vfxVector3.value.x, min.y, vfxVector3.value.z);
    }

    public void AddZ(Vector3 value)
    {
        vfxVector3.value += new Vector3(0,0,value.z);
        
        if (IsMaxValue(Axis.Z))
            vfxVector3.value = new Vector3(vfxVector3.value.x, vfxVector3.value.y, max.z);
        if(IsMinValue(Axis.Z))
            vfxVector3.value =  new Vector3(vfxVector3.value.x, vfxVector3.value.y, min.z);
    }

    public void AddXY(Vector3 value)
    {
        vfxVector3.value += new Vector3(value.x, value.y, 0);
        
        if (IsMaxValue(Axis.XY))
            vfxVector3.value = new Vector3(max.x, max.y, vfxVector3.value.z);
        if(IsMinValue(Axis.XY))
            vfxVector3.value =  new Vector3(min.x, min.y, vfxVector3.value.z);
    }
    public void AddXZ(Vector3 value)
    {
        vfxVector3.value += new Vector3(value.x, 0, value.z);
        
        if (IsMaxValue(Axis.XZ))
            vfxVector3.value = new Vector3(max.x, vfxVector3.value.y, max.z);
        if(IsMinValue(Axis.XZ))
            vfxVector3.value =  new Vector3(min.x, vfxVector3.value.y, min.z);
    }
    public void AddYZ(Vector3 value)
    {
        vfxVector3.value += new Vector3(0, value.y, value.z);
        
        if (IsMaxValue(Axis.YZ))
            vfxVector3.value = new Vector3(vfxVector3.value.x, max.y, max.z);
        if(IsMinValue(Axis.YZ))
            vfxVector3.value =  new Vector3(vfxVector3.value.x, min.y, min.z);
    }
}