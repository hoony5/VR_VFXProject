using UnityEngine;
using UnityEngine.Events;

public class VFXTimer : MonoBehaviour
{
    [SerializeField] private VFXController vfxController;
    [SerializeField] private VFXProgressScenario[] _vfxProgressScenarios;
    [SerializeField] private VFXTimeTable _vfxTimeTable = new VFXTimeTable();
    [SerializeField] private int currentSeconds;

    private int _timeOffset;
    public bool isStart;
    public UnityEvent<int> onStartVFX;
    public UnityEvent<int> onStopVFX;

    private void Update()
    {
        ReadScenarios();
    }

    private void ReadScenarios()
    {
        if (isStart)
        {
            isStart = false;
            _timeOffset = vfxController.GetProgressTime().Seconds;
        }
        if (!vfxController.IsPlaying()) return;
        UpdateScenario();
        currentSeconds = vfxController.GetProgressTime().Seconds - _timeOffset;
    }

    private void UpdateScenario()
    {
        foreach (VFXProgressScenario scenario in _vfxProgressScenarios)
        {
            if(!scenario.gameObject.activeInHierarchy) continue;
            if (scenario.currentIndex == scenario.progressItems.Count - 1)
            {
                scenario.isStart = false;
                scenario.isEnd = true;
                return;
            }

            bool tryGetValue = _vfxTimeTable.TryGetValue(currentSeconds, out VFXTimeTableItem @event);

            if (!tryGetValue) return;

            if (currentSeconds < @event.thresholdsSeconds) return;

            if (!scenario.isStart)
            {
                scenario.isStart = true;
                scenario.isEnd = false;
            }

            scenario.currentIndex++;
        }
    }
    private void OnDisable()
    {
        currentSeconds = 0;
        isStart = false;
    }
}