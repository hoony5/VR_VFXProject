using UnityEngine;

public abstract class VFXInteractor : MonoBehaviour
{
    public VFXTraits target;
    public VFXPropertyType PropertyType;
    public bool interactOnStart;

    public virtual void ResetValue(VFXTraits traits, bool setActive)
    {
        traits.ResetValue(setActive);
    }

    public abstract void ApplyValueTo(VFXTraits traits);

    protected virtual void ApplyBool(VFXTraits traits, bool value)
    {
        interactOnStart = traits.IsReceivedBool(value);
    }

    protected virtual void ApplyInt(VFXTraits traits, int value)
    {
        interactOnStart = traits.IsReceivedInt(value);
    }

    protected virtual void ApplyFloat(VFXTraits traits, float value)
    {
        interactOnStart = traits.IsReceivedFloat(value);
    }

    protected virtual void ApplyVector2(VFXTraits traits, Vector2 value)
    {
        interactOnStart = traits.IsReceivedVector2(value);
    }

    protected virtual void ApplyVector3(VFXTraits traits, Vector3 value)
    {
        interactOnStart = traits.IsReceivedVector3(value);
    }

    protected virtual void ApplyGradient(VFXTraits traits, Gradient value)
    {
        interactOnStart = traits.IsReceivedGradient(value);
    }

    protected virtual void ApplyCurve(VFXTraits traits, AnimationCurve curve)
    {
        interactOnStart = traits.IsReceivedCurve(curve);
    }

    protected virtual void SendEvent(VFXTraits traits, string @eventName)
    {
        interactOnStart = traits.IsReceivedEvent(@eventName);
    }
}