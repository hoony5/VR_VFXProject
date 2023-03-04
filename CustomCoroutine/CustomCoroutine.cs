using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class CustomCoroutine
{
    public CustomCoroutine()
    {
        _behaviours = new List<CustomCoroutineInternal>(_capacity);
    }

    public int index;
    private readonly int _capacity = 16;
    private List<CustomCoroutineInternal> _behaviours;
    public bool keepWatiting;
    private bool _isReturnNull;

    public IEnumerator StartCoroutine(IEnumerator rotine, CustomCoroutineToken token)
    {
        CustomCoroutineInternal item = new CustomCoroutineInternal(rotine, token);
        token.behaviourIndex = _behaviours.IndexOf(item);
        _behaviours.Add(item);
        return rotine;
    }

    public void DebugBehavioursToken()
    {
        if (_behaviours.Count == 0) return;

        foreach (CustomCoroutineInternal behaviour in _behaviours)
        {
            Debug.Log($"==========\nBehaviourIndex ? {behaviour.token.behaviourIndex}\n | Pause ? {behaviour.token.pause}\n | Stop ? {behaviour.token.stop}\n | AwaitTask ? {behaviour.token.awaitTask}\n | KeepGoing ? {behaviour.token.keepGoing}\n==========");
        }
    }
    public void SetToken(int index, CustomCoroutineToken token) => _behaviours[index].SetToken(token); 

    public void StopCoroutine(CustomCoroutineToken token)
    {
        if (_behaviours.Count == 0) return;
        if (token.behaviourIndex >= _behaviours.Count) return;
        
        _behaviours.RemoveAt(token.behaviourIndex);
    }

    public void StopAllCoroutine()
    {
        _behaviours.Clear();
    }
    
    public void Update(int index)
    {
        if (_behaviours.Count <= index || ReferenceEquals(_behaviours, null)) return;
            
        Process(_behaviours[index]);
    }
    public void Update()
    {
        if (_behaviours.Count == 0 || ReferenceEquals(_behaviours, null)) return;
            
        for (var i = 0; i < _behaviours?.Count; i++)
        {
            CustomCoroutineInternal behaviour = _behaviours[i];
            Process(behaviour);    
        }
    }
    private bool Process(in CustomCoroutineInternal behaviour)
    {
        do
        {
            if (ReferenceEquals(behaviour.Routine, null))
                break;

            if (behaviour.token.stop) break;
            
            if (behaviour.token.pause) return false;

            object current = behaviour.Routine.Current;
                
            switch (current)
            {
                case WaitWhile{keepWaiting: true}:
                    return false;
                case WaitWhile{keepWaiting: false}:
                    continue;
                case WaitUntil{keepWaiting: true}:
                    return false;
                case WaitUntil{keepWaiting: false}:
                    continue;
                case CustomYieldInstruction { keepWaiting: true }:
                    return false;
                case CustomYieldInstruction { keepWaiting: false }:
                    continue;
                case WaitForEndOfFrame :
                case WaitForFixedUpdate :
                case null:
                    if(keepWatiting)
                        return false;
                    
                    _isReturnNull = !_isReturnNull;
                    if (_isReturnNull) return false;
                    continue;
            }

            if (current is Task task)
            {
                if(behaviour.token.awaitTask)
                {
                    if (task.IsCanceled || task.IsCompleted || task.IsCompletedSuccessfully || task.IsFaulted)
                        continue;

                    return false;
                }
                    
                continue;
            }
                
            if (current is not IEnumerator other) continue;

            if (behaviour.token.keepGoing)
            {
                if (!Process(other, behaviour.token)) continue;

                return true;    
            }
            if (Process(other, behaviour.token)) continue;

            return true;

        } while (behaviour.Routine.MoveNext());

        /*
        if (behaviour.Routine != null && !behaviour.Routine.MoveNext())
        {
            StopCoroutine(behaviour.token);
        }*/

        return true;
    }

    private bool Process(in IEnumerator routine, in CustomCoroutineToken token)
    {
        do
        {
            if (ReferenceEquals(routine, null))
                break;

            if (token.stop) break;

            if (token.pause) return false;

            object current = routine.Current;

            switch (current)
            {
                case WaitWhile{keepWaiting: true}:
                    return false;
                case WaitWhile{keepWaiting: false}:
                    continue;
                case WaitUntil{keepWaiting: true}:
                    return false;
                case WaitUntil{keepWaiting: false}:
                    continue;
                case CustomYieldInstruction { keepWaiting: true }:
                    return false;
                case CustomYieldInstruction { keepWaiting: false }:
                    continue;
                case WaitForEndOfFrame :
                case WaitForFixedUpdate :
                case null:
                    _isReturnNull = !_isReturnNull;
                    if (_isReturnNull) return false;
                    continue;
            }

            if (current is Task task)
            {
                if (task.IsCanceled || task.IsCompleted || task.IsCompletedSuccessfully || task.IsFaulted)
                    continue;

                return false;
            }

            if (current is not IEnumerator other) continue;

            if (token.keepGoing)
            {
                if (!Process(other, token)) continue;

                return true;
            }

            if (Process(other, token)) continue;

            return true;

        } while (routine.MoveNext());

        /*
        if (routine != null && !routine.MoveNext())
            StopCoroutine(token);*/
        return true;
    }
}