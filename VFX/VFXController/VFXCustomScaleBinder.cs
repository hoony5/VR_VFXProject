using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[AddComponentMenu("VFX/Property Binders/Scale Binder")]
[VFXBinder("Transform/Scale")]
public class VFXCustomScaleBinder : VFXBinderBase
{
    public string Property { get { return (string)m_Property; } set { m_Property = value; UpdateSubProperties(); } }

    [VFXPropertyBinding( "UnityEditor.VFX.Position", "UnityEngine.Vector3" ), SerializeField, UnityEngine.Serialization.FormerlySerializedAs("m_Parameter")]
    
    protected ExposedProperty m_Property = "Transform";
    public Transform Target = null;

    private ExposedProperty Scale;
    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateSubProperties();
    }

    void OnValidate()
    {
        UpdateSubProperties();
    }

    void UpdateSubProperties()
    {
        Scale = m_Property;
    }

    public override bool IsValid(VisualEffect component)
    {
        return Target != null && component.HasVector3((int)Scale);
    }

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetVector3((int)Scale, Target.localScale);
    }

    public override string ToString()
    {
        return $"Scale : '{m_Property}' -> {(Target == null ? "(null)" : Target.name)}";
    }
}