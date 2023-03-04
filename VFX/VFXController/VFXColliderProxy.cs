using System.Collections;
using UnityEngine;

public class VFXColliderProxy : MonoBehaviour
{
    [SerializeField] private VFXColliderEvent _colliderEvent;
    [SerializeField] private VFXTraits _traits;
    [SerializeField] private string interactionTag;
    [SerializeField] private BoxCollider interactCollider;
    [SerializeField] private Vector3 sizeOffset;
    [SerializeField] private TriggerReceiveType triggerReceiveType;
    private VFXInteractor _interactor;
    
    private void FixedUpdate()
    {
        if (!interactCollider || triggerReceiveType is TriggerReceiveType.Ignore) return;
        interactCollider.size = transform.localScale + sizeOffset;
    }

    #region RayCast Trigger Enter Stay Exit
    public void RayEnter(VFXRayCast rayCast)
    {
        if (_colliderEvent.sleepOn || triggerReceiveType is TriggerReceiveType.Ignore or TriggerReceiveType.NormalTrigger) return;
        if (!rayCast.CompareTag(interactionTag)) return;
        _colliderEvent.RecognizeHitEnter();

            
        if (_interactor is not null) return;
        if (!rayCast.TryGetComponent(out VFXInteractor interactor)) return;
            
        _colliderEvent.triggerEnterEvent?.Invoke(_interactor);
        _traits.ResetValue(false);
        _interactor = interactor;
    }

    public void RayStay(VFXRayCast rayCast)
    {
        if (_colliderEvent.sleepOn || triggerReceiveType is TriggerReceiveType.Ignore or TriggerReceiveType.NormalTrigger) return;
        if (!rayCast.CompareTag(interactionTag)) return;
        _colliderEvent.RecognizeHitStay();
        
        if (!_interactor) return;
        _interactor.ApplyValueTo(_traits);
        _colliderEvent.triggerStayEvent?.Invoke(_interactor);
    }

    public void RayExit(VFXRayCast rayCast)
    {
        if (_colliderEvent.sleepOn || triggerReceiveType is TriggerReceiveType.Ignore or TriggerReceiveType.NormalTrigger) return;
        if (!rayCast.CompareTag(interactionTag)) return;
        _colliderEvent.triggerExitEvent?.Invoke(_interactor);
        _colliderEvent.RecognizeHitExit();

        if (!_interactor) return;

        _traits.ResetValue(true);
        _interactor = null;
    }
    #endregion
    
    private void OnTriggerEnter(Collider other)
    {
        if (_colliderEvent.sleepOn || triggerReceiveType is TriggerReceiveType.Ignore or TriggerReceiveType.Raycast) return;
        if (!other.CompareTag(interactionTag)) return;
        _colliderEvent.RecognizeHitEnter();

        if (_interactor)
        {
            _colliderEvent.triggerEnterEvent?.Invoke(_interactor);
            return;
        }

        bool exist = other.TryGetComponent(out VFXInteractor interactor);
        _traits.ResetValue(!exist);
            
        if (!exist) return;
        _interactor = interactor;
    }
    private IEnumerator OnTriggerStay(Collider other)
    {
        if (_colliderEvent.sleepOn || triggerReceiveType is TriggerReceiveType.Ignore or TriggerReceiveType.Raycast) yield break;
        if (!other.CompareTag(interactionTag)) yield break;
        
        _colliderEvent.RecognizeHitStay();

        if (!_interactor) yield break;
        _interactor.ApplyValueTo(_traits);
        _colliderEvent.triggerStayEvent?.Invoke(_interactor);
            
        yield return CoroutineYield.waitForAPointSeconds;

        // yield 0.1s
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_colliderEvent.sleepOn || triggerReceiveType is TriggerReceiveType.Ignore or TriggerReceiveType.Raycast) return;
        if (!other.CompareTag(interactionTag)) return;
        _colliderEvent.triggerExitEvent?.Invoke(_interactor);
        _colliderEvent.RecognizeHitExit();

        if (!_interactor) return;

        _traits.ResetValue(true);
        _interactor = null;
    }
}