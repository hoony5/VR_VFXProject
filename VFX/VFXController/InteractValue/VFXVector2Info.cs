using System;
using UnityEngine;
using UnityEngine.VFX.Utility;

[System.Serializable]
public sealed class VFXVector2Info : VFXValueInfo
{
    public VFXVector2Info(ExposedProperty property, Vector2 value)
    {
        vfxVector2 = new VFXVector2(property, value);
    }
    [SerializeField] private VFXVector2 vfxVector2;
        
    [SerializeField] private Vector2 start;
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    public override VFXPropertyType GetPropertyType()
    {
        return vfxVector2.propertyType;
    }
    public Vector2 MaxValue => max;
    public Vector2 Value => vfxVector2.value;
    public void SaveStartValue(Vector2 startValue)
    {
        start = startValue;
    }

    public override ExposedProperty GetExposedPropertyID()
    {
        return vfxVector2.exposedProperty;
    }

    public override void Reset()
    {
        vfxVector2.value = start;
    }
    // AFTER ADD VALUE
    private bool IsMinValue(Axis axis)
    {
        bool result = axis switch
        {
            Axis.X => vfxVector2.value.x <= min.x,
            Axis.Y => vfxVector2.value.y <= min.y,
            Axis.XY => vfxVector2.value.x <= min.x && vfxVector2.value.y <= min.y,
            _ => throw new ArgumentException("Vector2 axis is x or y or xy.")
        };
        return result;
    }

    // AFTER ADD VALUE
    private bool IsMaxValue(Axis axis)
    {
        bool result = axis switch
        {
            Axis.X => vfxVector2.value.x >= max.x,
            Axis.Y => vfxVector2.value.y >= max.y,
            Axis.XY => vfxVector2.value.x >= max.x && vfxVector2.value.y >= max.y,
            _ => throw new ArgumentException("Vector2 axis is x or y or xy.")
        };
        return result;
    }

    public void AddXY(Vector2 value)
    {
        vfxVector2.value += value;
        
        if (IsMaxValue(Axis.XY))
            vfxVector2.value = max;
        if (IsMinValue(Axis.XY))
            vfxVector2.value = min;
    }

    public void AddX(Vector2 value)
    {
        vfxVector2.value += new Vector2(value.x, 0);

        if (IsMaxValue(Axis.X))
            vfxVector2.value = new Vector2(max.x, vfxVector2.value.y);
        if (IsMinValue(Axis.X))
            vfxVector2.value = new Vector2(min.x, vfxVector2.value.y);

    }
    public void AddY(Vector2 value)
    {
        vfxVector2.value += new Vector2(0, value.y);

        if (IsMaxValue(Axis.Y))
            vfxVector2.value = new Vector2(vfxVector2.value.x, max.y);
        if (IsMinValue(Axis.Y))
            vfxVector2.value = new Vector2(vfxVector2.value.x, min.y);
    }
}