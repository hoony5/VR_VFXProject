using UnityEngine;
using UnityEngine.Events;

public class VFXColliderEvent : MonoBehaviour
{
    [SerializeField] private VFXController vfxController;
    public UnityEvent<VFXInteractor> triggerEnterEvent;
    public UnityEvent<VFXInteractor> triggerStayEvent;
    public UnityEvent<VFXInteractor> triggerExitEvent;
    public bool sleepOn;
    
    public void RecognizeHitEnter()
    {
        vfxController.SendToBrainHitEnter(true);
        vfxController.SendToBrainHitStay(true);           
        vfxController.SendToBrainHitExit(false);
    }
    public void RecognizeHitStay()
    {
        vfxController.SendToBrainHitEnter(false);
        vfxController.SendToBrainHitStay(true);           
        vfxController.SendToBrainHitExit(false);
    }
    public void RecognizeHitExit()
    {
        vfxController.SendToBrainHitEnter(false);
        vfxController.SendToBrainHitStay(false);           
        vfxController.SendToBrainHitExit(true);
    }
}