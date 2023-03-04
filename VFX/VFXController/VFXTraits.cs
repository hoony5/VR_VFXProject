using UnityEngine;
using UnityEngine.VFX;

#if UNITY_EDITOR
using NaughtyAttributes;
#endif

public abstract class VFXTraits : MonoBehaviour
{
    public bool isMainVFX;
    public VisualEffect localVFX;
    public VFXValueContainer valueInfo;
    public VFXController controller;
    [SerializeField] protected VFXInteractValueContainer interactValueContainer;

    protected bool IsReset;

    private void Start()
    {
       controller.StartPlay(this);
    }
#if UNITY_EDITOR
   [Button("Caching Property's Values")]
#endif
   private void CachePropertyValues()
   {
      controller.MigrateVFXPropertiesDatas(valueInfo);
   }
#if UNITY_EDITOR
   [Button("Start")]
#endif
   public void StartPlay()
   {
      controller.StartPlay(this);
   }
#if UNITY_EDITOR
   [Button("Stop")]
#endif
   public void StopPlay()
   {
      controller.StopPlay(this);
   }
    public virtual void ResetValue(bool setActive) => IsReset = setActive;

    public virtual bool IsReceivedBool(bool value)
    {
        return !IsReset;
    }

    public virtual bool IsReceivedInt(int value)
    {
       return !IsReset;
    }

    public virtual bool IsReceivedFloat(float value)
    {
       return !IsReset;
    }

    public virtual bool IsReceivedVector2(Vector2 value)
    {
       return !IsReset;
    }

    public virtual bool IsReceivedVector3(Vector3 value)
    {
       return !IsReset;
    }

    public virtual bool IsReceivedCurve(AnimationCurve value)
    {
       return !IsReset;
    }

    public virtual bool IsReceivedGradient(Gradient value)
    {
       return !IsReset;
    }

    public virtual bool IsReceivedEvent(string value)
    {
       return !IsReset;
    }
}