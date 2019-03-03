using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCarMovement : MonoBehaviour
{
    //Fields
    private Rigidbody2D _rigidBody = null;

    //-Settings
    public bool IsControlled = true;

    public float GasChangeSpeed = 0.3f;
    public float MaxGasValue = 10.0f;
    public float MaxReversValue = 10.0f;

    public float SteeringWheelSpeed = 1.0f;
    public float SteeringWheelReturnSpeed = 2.0f;
    public float SteeringWheelMaxRotation = 45.0f;

    //--Weels
    private Vector2 _rearLeftWheelPosition = new Vector2(-0.8f, -0.3f);
    private Vector2 _rearRightWheelPosition = new Vector2(-0.8f, 0.3f);

    private Vector2 _frontLeftWheelPosition = new Vector2(0.8f, -0.3f);
    private Vector2 _frontRightWheelPosition = new Vector2(0.8f, 0.3f);

    //-Runtime
    //--Movement state
    private float _gasValue = 0.0f;
    private bool _isSteeringGasWasChangedOnUpdate = false;

    private float _steeringWheelRotation = 0.0f;
    private bool _isSteeringWheelWasRotatedOnUpdate = false;

    private float _rearWeelsRotation = 0.0f;
    private float _frontWeelsRotation = 0.0f;

    //Methods
    //-API
    public void applyGas() {
        _gasValue = Mathf.Clamp(
            _gasValue + GasChangeSpeed, -MaxReversValue, MaxGasValue
        );
        _isSteeringGasWasChangedOnUpdate = true;
    }

    public void applyRevers() {
        _gasValue = Mathf.Clamp(
            _gasValue - GasChangeSpeed, -MaxReversValue, MaxGasValue
        );
        _isSteeringGasWasChangedOnUpdate = true;
    }

    public void rotateSteeringWheelClockwise() {
        _steeringWheelRotation = Mathf.Clamp(
            _steeringWheelRotation - SteeringWheelSpeed, -SteeringWheelMaxRotation, SteeringWheelMaxRotation
        );
        _isSteeringWheelWasRotatedOnUpdate = true;
    }

    public void rotateSteeringWheelCounterClockwise() {
        _steeringWheelRotation = Mathf.Clamp(
            _steeringWheelRotation + SteeringWheelSpeed, -SteeringWheelMaxRotation, SteeringWheelMaxRotation
        );
        _isSteeringWheelWasRotatedOnUpdate = true;
    }

    //-Lifecycle
    void Start() {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update() {
        Update_Gas();
        Update_SteeringWheel();

        Update_GasForces();
        Update_WeelsResistance();
    }

    private void Update_Gas() {
        if (!_isSteeringGasWasChangedOnUpdate) {
            _gasValue = 0.0f;
        }

        _isSteeringGasWasChangedOnUpdate = false;
    }

    private void Update_SteeringWheel() {
        if (!_isSteeringWheelWasRotatedOnUpdate) {
            if (_steeringWheelRotation > SteeringWheelReturnSpeed) {
                _steeringWheelRotation -= SteeringWheelReturnSpeed;
            } else if (_steeringWheelRotation < -SteeringWheelReturnSpeed) {
                _steeringWheelRotation += SteeringWheelReturnSpeed;
            } else {
                _steeringWheelRotation = 0.0f;
            }
        }

        _frontWeelsRotation = _steeringWheelRotation;

        _isSteeringWheelWasRotatedOnUpdate = false;
    }

    private void Update_GasForces() {
        float theWorldWeelsRotationRadians =
            transformRotationFromLocalToWorld(_rearWeelsRotation) * Mathf.Deg2Rad;
        Vector2 theForceDiraction = new Vector2(
            Mathf.Cos(theWorldWeelsRotationRadians), Mathf.Sin(theWorldWeelsRotationRadians)
        );
        Vector2 theForce = theForceDiraction * _gasValue;

        applyForceAtPosition(theForce, _rearLeftWheelPosition, ForceMode2D.Force);
        applyForceAtPosition(theForce, _rearRightWheelPosition, ForceMode2D.Force);
    }

    private void Update_WeelsResistance() {
        applyWeelResistance(_rearLeftWheelPosition, _rearWeelsRotation);
        applyWeelResistance(_rearRightWheelPosition, _rearWeelsRotation);
        
        applyWeelResistance(_frontLeftWheelPosition, _frontWeelsRotation);
        applyWeelResistance(_frontRightWheelPosition, _frontWeelsRotation);
    }

    //-Movement utils
    private void applyWeelResistance(Vector2 inLocalWeelPosition, float inWeelRotation) {
        float theDirectResistanceK = 0.3f;
        float theSideResistanceK = 0.9f;

        //Calculate full speed: linear + angular
        Vector2 theWorldWeelPosition = transformPositionFromLocalToWorld(inLocalWeelPosition);
        Vector2 theAngularVelocityLinear = _rigidBody.GetPointVelocity(theWorldWeelPosition);

        Vector2 theAccumulateWorldVelocity = _rigidBody.velocity + theAngularVelocityLinear;

        //Calculate weel diraction & normal diraction
        float theWorldWeelsRotationRadians = transformRotationFromLocalToWorld(inWeelRotation) * Mathf.Deg2Rad;
        Vector3 theWorldWeelsDiraction = new Vector3(
            Mathf.Cos(theWorldWeelsRotationRadians), Mathf.Sin(theWorldWeelsRotationRadians), 0.0f
        );
        Vector3 theWorldWeelsNormalDiraction = new Vector3(
            -theWorldWeelsDiraction.y, theWorldWeelsDiraction.x, 0.0f
        );

        //Apply direct resistance
        Vector2 theDirectProjectedAccumulateWorldVelocity = Vector3.Project(
            theAccumulateWorldVelocity, theWorldWeelsDiraction
        );
        Vector2 theDirectResistanceForce = -theDirectProjectedAccumulateWorldVelocity * theDirectResistanceK;
        applyForceAtPosition(theDirectResistanceForce, inLocalWeelPosition, ForceMode2D.Force);

        //Apply side resistance
        Vector2 theSideProjectedAccumulateWorldVelocity = Vector3.Project(
            theAccumulateWorldVelocity, theWorldWeelsNormalDiraction
        );
        Vector2 theSideResistanceForce = -theSideProjectedAccumulateWorldVelocity * theSideResistanceK;
        applyForceAtPosition(theSideResistanceForce, inLocalWeelPosition, ForceMode2D.Force);
    }

    //-Utils
    private void applyForceAtPosition(Vector2 inForce, Vector2 inLocalPosition, ForceMode2D inForceMode) {
        Vector2 theWorldPoint = transformPositionFromLocalToWorld(inLocalPosition);
        _rigidBody.AddForceAtPosition(inForce, theWorldPoint, inForceMode);
    }

    private Vector2 transformPositionFromLocalToWorld(Vector2 inLocalPosition) {
        return transform.TransformPoint(inLocalPosition);
    }

    private float transformRotationFromLocalToWorld(float inLocalRotation) {
        return transform.rotation.eulerAngles.z + inLocalRotation;
    }
}
