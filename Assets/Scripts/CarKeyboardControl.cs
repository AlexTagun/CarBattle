using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarKeyboardControl : MonoBehaviour
{
    //Fields
    private TestCarMovement _carMovement = null;

    private bool _isGasIsPressed = false;
    private bool _isReversIsPressed = false;
    private bool _isClockwiseRotatePressed = false;
    private bool _isCounterClockwiseRotatePressed = false;

    //Methods
    //-Lifecycle
    void Start()
    {
        _carMovement = gameObject.GetComponent<TestCarMovement>();
    }

    void Update() {
        Update_PressedKeys();

        if (_isGasIsPressed) _carMovement.applyGas();
        if (_isReversIsPressed) _carMovement.applyRevers();

        if (_isClockwiseRotatePressed) _carMovement.rotateSteeringWheelClockwise();
        if (_isCounterClockwiseRotatePressed) _carMovement.rotateSteeringWheelCounterClockwise();
    }

    private void Update_PressedKeys()
    {
        if (Input.GetKeyDown(KeyCode.S)) _isReversIsPressed = true;
        if (Input.GetKeyUp(KeyCode.S)) _isReversIsPressed = false;

        if (Input.GetKeyDown(KeyCode.W)) _isGasIsPressed = true;
        if (Input.GetKeyUp(KeyCode.W)) _isGasIsPressed = false;

        if (Input.GetKeyDown(KeyCode.D)) _isClockwiseRotatePressed = true;
        if (Input.GetKeyUp(KeyCode.D)) _isClockwiseRotatePressed = false;

        if (Input.GetKeyDown(KeyCode.A)) _isCounterClockwiseRotatePressed = true;
        if (Input.GetKeyUp(KeyCode.A)) _isCounterClockwiseRotatePressed = false;
    }
}
