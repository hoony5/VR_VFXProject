using System;
using System.Collections.Generic;
using UnityEngine;

public enum UpdateTiming
{
    UnityEventCallBack,
    FixedUpdate,
    Update,
    LateUpdate
}
public class CustomCoroutineUpdater : MonoBehaviour
{
    private const int Capacity = 32;
    [SerializeField] private List<CustomCoroutine> coroutines;
    public UpdateTiming updateTiming;
    public bool debugOn;
    private void Start()
    {
        coroutines = new List<CustomCoroutine>(Capacity);
    }

    private void OnDisable()
    {
        ClearCoroutines();
    }

    private void UpdateCoroutines()
    {
        foreach (CustomCoroutine c in coroutines)
        {
            c.Update();
            
            if(debugOn)
                c.DebugBehavioursToken();
        }
    }

    public int IndexOf(CustomCoroutine coroutine)
    {
        return coroutines.IndexOf(coroutine);
    }
    public void Add(CustomCoroutine coroutine)
    {
        if (coroutines.Contains(coroutine)) return;
        
        coroutines.Add(coroutine);
    }

    public void Remove(CustomCoroutine coroutine)
    {
        if (!coroutines.Contains(coroutine)) return;
        
        coroutines.Remove(coroutine);
    }

    public void ClearCoroutines()
    {
        coroutines.Clear();
    }
    
    
    private void FixedUpdate()
    {
        if (updateTiming is not UpdateTiming.FixedUpdate or UpdateTiming.UnityEventCallBack) return;
        if (coroutines.Count == 0) return;
        UpdateCoroutines();
    }
    private void Update()
    {
        if (updateTiming is not UpdateTiming.Update or UpdateTiming.UnityEventCallBack) return;
        if (coroutines.Count == 0) return;
        UpdateCoroutines();
    }
    private void LateUpdate()
    {
        if (updateTiming is not UpdateTiming.LateUpdate or UpdateTiming.UnityEventCallBack) return;
        if (coroutines.Count == 0) return;
        UpdateCoroutines();
    }
}