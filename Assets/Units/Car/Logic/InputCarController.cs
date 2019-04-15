using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCarController : MonoBehaviour
{
    //Fields
    private CarPhysicsLogic _carPhysics = null;
    private WeaponGunLogic[] _weaponGunsLogic = null;

    //-Input state
    //--Mouse
    private bool _isMouseButtonPressed = false;

    private Vector2 _carRelatedMousePosition;

    //--Keyboard
    private bool _isGasIsPressed = false;
    private bool _isReversIsPressed = false;
    private bool _isClockwiseRotatePressed = false;
    private bool _isCounterClockwiseRotatePressed = false;

    //Methods
    //-Implementation
    private void Start() {
        _carPhysics = gameObject.GetComponentInChildren<CarPhysicsLogic>();
        _weaponGunsLogic = gameObject.GetComponentsInChildren<WeaponGunLogic>();
    }

    private void FixedUpdate() {
        Update_Input();
        Update_CarControl();
    }

    //--Input
    private void Update_Input() {
        Update_Input_Mouse();
        Update_Input_Keyboard();
    }

    private void Update_Input_Mouse() {
        _isMouseButtonPressed = Input.GetMouseButton(0);
        _carRelatedMousePosition =
            XUtils.getMouseWorldPosition() - (Vector2)transform.position;
    }

    private void Update_Input_Keyboard() {
        _isReversIsPressed = Input.GetKey(KeyCode.S);
        _isGasIsPressed = Input.GetKey(KeyCode.W);
        _isClockwiseRotatePressed = Input.GetKey(KeyCode.A);
        _isCounterClockwiseRotatePressed = Input.GetKey(KeyCode.D);
    }

    //-Car control
    private void Update_CarControl() {
        //Car movement update
        if (_isGasIsPressed) _carPhysics.applyGas();
        if (_isReversIsPressed) _carPhysics.applyRevers();

        if (_isClockwiseRotatePressed) _carPhysics.rotateSteeringWheelClockwise();
        if (_isCounterClockwiseRotatePressed) _carPhysics.rotateSteeringWheelCounterClockwise();

        //Car shooting update
        if (_isMouseButtonPressed) {
            foreach(WeaponGunLogic theGunLogic in _weaponGunsLogic) {
                theGunLogic.doShoot();
            }
        }

        foreach (WeaponGunLogic theGunLogic in _weaponGunsLogic) {
            theGunLogic.setTargetAngle(
                Mathf.Atan2(_carRelatedMousePosition.y, _carRelatedMousePosition.x) * Mathf.Rad2Deg
            );
        }
    }
}
