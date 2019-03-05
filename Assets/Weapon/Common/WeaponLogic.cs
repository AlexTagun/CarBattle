using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Values;

public class WeaponLogic : MonoBehaviour
{
    //Fields
    enum State {
        Idle,
        LoadingAmmoToClip,
        LoadingAmmoToBarrel,
        Shooting,
    }

    //-Settings
    public LimitedFloat.ZeroBasedState ClipSettings = new LimitedFloat.ZeroBasedState(10.0f, true);

    public float TimeToLoadAmmoToBarrel = 1.0f;
    public float TimeToReload = 3.0f;

    //-Runtime state
    bool _hasAmmoInBarrel = true;
    LimitedFloat _ammoInClip; //TODO: Use "int" instead of "float"

    SimpleStateMachine<State> _stateMachine = null;

    UIShootController uiShootController;

    //Methods
    public void doShoot() {
        uiShootController.setAmmoQuantity((int)_ammoInClip.getValue());
        internalDoShoot();
    }

    //-Child methods
    protected virtual void performShoot() { }

    //-Implementation
    protected void Start() {
        _ammoInClip = new LimitedFloat(ClipSettings);

        _stateMachine = new SimpleStateMachine<State>(State.Idle);
        uiShootController = GetComponent<UIShootController>();
        uiShootController.setClipStats((int)_ammoInClip.getValue(), (int)_ammoInClip.getValue());
    }

    protected void Update() {
        _stateMachine.update(Time.deltaTime);
    }

    private bool internalDoShoot() {
        if (_stateMachine.isTransiting()) return false;
        if (State.Idle != _stateMachine.getState()) return false;

        if (!_hasAmmoInBarrel) {
            internalPutAmmoToBarrel();
            return false;
        }

        performShoot();
        _hasAmmoInBarrel = false;
        _stateMachine.setState(State.Idle);

        return true;
    }

    private bool internalPutAmmoToBarrel() {
        if (_stateMachine.isTransiting()) return false;
        if (State.Idle != _stateMachine.getState()) return false;

        if (0 == _ammoInClip.getValue()) {
            internalReload();
            return false;
        }

        _stateMachine.setState(State.LoadingAmmoToBarrel);
        _stateMachine.transitToState(State.Idle, TimeToLoadAmmoToBarrel, (State inOldState, State inNewState) => {
            _ammoInClip.changeValue(-1);
            _hasAmmoInBarrel = true;
        });
        return true;
    }

    private bool internalReload() {
        if (_stateMachine.isTransiting()) return false;
        if (State.Idle != _stateMachine.getState()) return false;

        _stateMachine.setState(State.LoadingAmmoToClip);
        _stateMachine.transitToState(State.Idle, TimeToReload, (State inOldState, State inNewState) => {
            _ammoInClip.changeValue(_ammoInClip.getMaximum());
            _hasAmmoInBarrel = true;
        });
        return true;
    }

    //-Accessors
    protected LimitedFloat getAmmoInClip() { return _ammoInClip; }
}
