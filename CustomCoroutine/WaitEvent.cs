using System;
using UnityEngine;

public class WaitEvent : CustomYieldInstruction
{
    private Func<bool> _predicate;
    private bool _wake;
    private bool _stay;
    private bool _useUntil;
    private bool _useWhile;

    public WaitEvent()
    {
        
    }

    public WaitEvent(Func<bool> predicate)
    {
        this._predicate = predicate;
    }

    // if you used this it is no alloc, when using predicate which not captured. Try using static instead of captured or lamda.
    public WaitEvent WaitWhile(Func<bool> predicate)
    {
        UseWaitWhileType();
        this._predicate = predicate;
        return this;
    }

    // if you used this it is no alloc, when using predicate which not captured. Try using static instead of captured or lamda.
    public WaitEvent WaitUntil(Func<bool> predicate)
    {
        UseWaitUntilType();
        this._predicate = predicate;
        return this;
    }

    // reset flags
    void UseWaitWhileType()
    {
        _useUntil = false;
        _useWhile = true;
    }

    // reset flags
    void UseWaitUntilType()
    {
        _useUntil = true;
        _useWhile = false;
    }

    public void Reset()
    {
        _wake = false;
        _stay = false;
        _useUntil = false;
        _useWhile = false;
        _predicate = null;
    }

    public override bool keepWaiting
    {
        get
        {
            // until is same WaitUntil and while is same WaitWhile.
            _wake = _useUntil ? _predicate.Invoke() : _useWhile ? !_predicate.Invoke() : !_wake;

            if (_wake)
            {
                _wake = false;
                    
                if(!_stay) 
                    return _stay = true;
                    
                return _stay = false;
            }

            return _stay = true;
        }
    }
}