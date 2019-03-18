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

    private void Update() {
        Update_Input();
        Update_CarControl();
    }

    //--Input
    private void Update_Input() {
        Update_Input_Mouse();
        Update_Input_Keyboard();
    }

    private void Update_Input_Mouse() {
        if (Input.GetMouseButtonDown(0)) _isMouseButtonPressed = true;
        if (Input.GetMouseButtonUp(0)) _isMouseButtonPressed = false;

        Vector2 thePosition = transform.position;
        _carRelatedMousePosition = getMouseWorldPosition() - thePosition;
    }

    private void Update_Input_Keyboard() {
        if (Input.GetKeyDown(KeyCode.S)) _isReversIsPressed = true;
        if (Input.GetKeyUp(KeyCode.S)) _isReversIsPressed = false;

        if (Input.GetKeyDown(KeyCode.W)) _isGasIsPressed = true;
        if (Input.GetKeyUp(KeyCode.W)) _isGasIsPressed = false;

        if (Input.GetKeyDown(KeyCode.A)) _isClockwiseRotatePressed = true;
        if (Input.GetKeyUp(KeyCode.A)) _isClockwiseRotatePressed = false;

        if (Input.GetKeyDown(KeyCode.D)) _isCounterClockwiseRotatePressed = true;
        if (Input.GetKeyUp(KeyCode.D)) _isCounterClockwiseRotatePressed = false;
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

    //-Utils
    private Vector2 getMouseWorldPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
