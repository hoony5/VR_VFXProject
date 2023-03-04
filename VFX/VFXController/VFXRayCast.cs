using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class VFXRayCast : MonoBehaviour
{
    public bool debugOn;
    public Transform start;
    public float distance = 10;
    private NativeArray<RaycastCommand> _commands;
    private NativeArray<RaycastHit> _result;
    private JobHandle _handle;
    public LayerMask interactionlayer;

    private bool _isRayEnter;
    private bool _isRayStay;
    private bool _isRayExit;
    
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        RayCastJob();
    }

    private void OnDestroy()
    {
        Release();
    }

    private void Init()
    {
        _commands = new NativeArray<RaycastCommand>(2, Allocator.Persistent);
        _result = new NativeArray<RaycastHit>(2, Allocator.Persistent);
    }
    
    public void RayCastJob()
    {
        Vector3 origin = start.position;
        Vector3 direction = start.forward;
        _commands[0] = new RaycastCommand(
            origin
            , direction
            , new QueryParameters(interactionlayer, true, QueryTriggerInteraction.UseGlobal)
            , distance);
        _handle = RaycastCommand.ScheduleBatch(_commands, _result, 1, 1);
        _handle.Complete();
        
        if(_handle.IsCompleted)
        {
            if(debugOn)
                Debug.DrawRay(origin, direction * distance, Color.red);

            
            if (_result.Length == 0 ) return;
            RaycastHit hit = _result[0];
            if (hit.collider is null) return;

            if (!hit.transform.TryGetComponent(out VFXColliderProxy receiver))
            {
                if (_isRayExit) return;
                
                SetFlagsRayExit();
                receiver.RayExit(this);
                return;
            }
            
            if(!_isRayEnter)
            {
                SetFlagsRayEnter();
                receiver.RayEnter(this);
            }

            if (!_isRayStay) return;
            
            SetFlagsRayStay();
            receiver.RayStay(this);
            //... something logic
        }
    }

    private void Release()
    {
        _handle.Complete();
        _commands.Dispose();
        _result.Dispose();
        ResetFlags();
    }

    private void ResetFlags()
    {
        _isRayEnter = false;
        _isRayStay = false;
        _isRayExit = false;
    }

    private void SetFlagsRayEnter()
    {
        _isRayEnter = true;
        _isRayStay = true;
        _isRayExit = false;
    }
    private void SetFlagsRayStay()
    {
        _isRayEnter = false;
        _isRayStay = true;
        _isRayExit = false;
    }
    private void SetFlagsRayExit()
    {
        _isRayEnter = false;
        _isRayStay = false;
        _isRayExit = true;
    }
}
