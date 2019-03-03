using System;
using UnityEngine;
using Values;

public class SimpleStateMachine<T_EventType>
{
    //Methods
    public SimpleStateMachine(T_EventType inInitialState) {
        _currentState = inInitialState;
        _targetState = inInitialState;
    }

    public T_EventType getState() { return _currentState; }

    public bool isFloatsEquals(float inFloatA, float inFloatB, float inPrecision) {
        return (Mathf.Abs(inFloatA - inFloatB) <= inPrecision);
    }

    public void update(float inDeltaTime) {
        if (inDeltaTime <= 0.0f) return;
        if (-1.0f == _timer.getMaximum()) return;

        float theActualDelta = _timer.changeValue(inDeltaTime);
        if (isFloatsEquals(theActualDelta, inDeltaTime, 0.001f)) return;
        _timer.setMaximum(-1.0f);

        T_EventType theOldCurrentState = _currentState;
        _currentState = _targetState;
        _callbackOnAchievedState.Invoke(theOldCurrentState, _targetState);
    }

    public void setState(T_EventType inTargetState) {
        _timer.setMaximum(-1.0f);

        _currentState = inTargetState;
    }

    public void transitToState(T_EventType inTargetState,
        float inTimeToAchieve, Action<T_EventType, T_EventType> inCallbackOnAcieve)
    {
        _targetState = inTargetState;

        float theTimeToAchieve = Mathf.Clamp(inTimeToAchieve, 0.0f, float.MaxValue);
        _timer.setValue(0.0f);
        _timer.setMaximum(theTimeToAchieve);

        _callbackOnAchievedState = inCallbackOnAcieve;
    }

    //-Accessors
    public bool isTransiting() { return (_timer.getMaximum() != -1.0f); }

    //Fields
    private T_EventType _currentState;
    private T_EventType _targetState;

    private LimitedFloat _timer = new LimitedFloat(0.0f, -1.0f); //TODO: Implement timer as separate object
    private Action<T_EventType, T_EventType> _callbackOnAchievedState;
}
