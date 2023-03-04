using System;
using System.Diagnostics;
using UnityEngine;

public class VFXModel : MonoBehaviour
{
    public bool isPlaying;
    
    public bool isHitEnter;
    public bool isHitStay;
    public bool isHitExit;

    private long currentTime;
    private long startTime;
    private long endTime;

    public TimeSpan GetCurrentTime()
    {
        currentTime = Stopwatch.GetTimestamp();
        return new TimeSpan(currentTime);
    }

    public void StopTimer()
    {
        endTime = Stopwatch.GetTimestamp();
    }

    public void StartTimer()
    {
        if (!isPlaying) return;
        startTime = Stopwatch.GetTimestamp();
    }
}