using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Values;

public class DurabilityComponent : MonoBehaviour
{
    //Events
    public delegate void OnAchievedMaxHitPoints();
    public delegate void OnAchievedNoHitPoints();

    //Fields
    //-Settings
    public float MaxHitPoints = 100.0f;

    //-Runtime
    private LimitedFloat _hitPoints;

    //Methods
    //-API
    public float getHitPoints() {
        return _hitPoints.getValue();
    }

    public float getDamage() {
        return _hitPoints.getValueFromMaximum();
    }

    public void setHitPoints(float inHitPoints) {
        _hitPoints.setValue(inHitPoints);
    }

    public float changeHitPoints(float inHitPoints) {
        return _hitPoints.changeValue(inHitPoints);
    }

    public float getMaxHitPoints() {
        return _hitPoints.getMaximum();
    }

    public void setMaxHitPoints(float inMaxHitPoints) {
        _hitPoints.setMaximum(inMaxHitPoints);
    }

    //-Events
    public OnAchievedMaxHitPoints onAchievedMaxHitPoints;
    public OnAchievedMaxHitPoints onAchievedNoHitPoints;

    //-Implementation
    private void Start() {
        _hitPoints = new LimitedFloat(0.0f, MaxHitPoints, MaxHitPoints);
        _hitPoints.onAchievedMaximum += () => onAchievedMaxHitPoints?.Invoke();
        _hitPoints.onAchievedMinimum += () => onAchievedNoHitPoints?.Invoke();
    }
}
