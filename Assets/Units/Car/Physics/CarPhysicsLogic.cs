using UnityEngine;
using System;

using Values;

public class CarPhysicsLogic : ISimulatableLogic
{
    //Fields
    public LimitedFloat.State GasValueSettings = new LimitedFloat.State(-10.0f, 30.0f);
    public float GasChangeSpeed = 0.3f;

    public LimitedFloatAngle.State SteeringWheelValueSettings = new LimitedFloatAngle.State(45.0f, true);
    public float SteeringWheelSpeed = 1.0f;
    public float SteeringWheelReturnSpeed = 2.0f;

    //-Runtime
    private WheelState[] _wheels = null;
    private int[] _drivingWheelIndices = null;
    private int[] _controlWheelIndices = null;

    private LimitedFloat _gasValue;
    private bool _isGasWasChangedOnUpdate = false;

    private LimitedFloatAngle _steeringWheelValue;
    private bool _isSteeringWheelWasRotatedOnUpdate = false;

    //--Cache
    public Rigidbody2D _rigidBody = null;
    private BoxCollider2D _collider = null;

    //Methods
    public void applyGas() {
        _gasValue.changeValue(GasChangeSpeed);
        _isGasWasChangedOnUpdate = true;
    }

    public void applyRevers() {
        _gasValue.changeValue(-GasChangeSpeed);
        _isGasWasChangedOnUpdate = true;
    }

    public void rotateSteeringWheelClockwise() {
        _steeringWheelValue.changeAngle(SteeringWheelSpeed);
        _isSteeringWheelWasRotatedOnUpdate = true;
    }

    public void rotateSteeringWheelCounterClockwise() {
        _steeringWheelValue.changeAngle(-SteeringWheelSpeed);
        _isSteeringWheelWasRotatedOnUpdate = true;
    }

    //AI helping Accessors
    public bool isRotatingByClockwice() {
        return (_rigidBody.angularVelocity > 0.0f);
    }

    public LimitedFloat getGasValue() {
        return _gasValue;
    }

    //-Implementation
    //[ISimulatableLogic]
    protected override void implementation_initialize() {
        implementation_initialize_wheels();
        implementation_initialize_state();
        implementation_initialize_componentReferences();
    }

    protected void implementation_initialize_wheels() {
        _wheels = new WheelState[4]{
            new WheelState(new Vector2(-0.8f,  0.3f)), new WheelState(new Vector2(0.8f,  0.3f)),
            new WheelState(new Vector2(-0.8f, -0.3f)), new WheelState(new Vector2(0.8f, -0.3f)),
        };

        _drivingWheelIndices = new int[2] { 0, 2 };
        _controlWheelIndices = new int[2] { 1, 3 };
    }

    protected void implementation_initialize_state() {
        _gasValue = new LimitedFloat(GasValueSettings);
        _steeringWheelValue = new LimitedFloatAngle(SteeringWheelValueSettings);
    }

    protected void implementation_initialize_componentReferences() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    protected override GameObject implementation_createSimulation() {
        GameObject theSimulationObject = GameObject.Instantiate(gameObject);
        Destroy(theSimulationObject.GetComponent<SimulatableObjectUpdater>());
        Destroy(theSimulationObject.GetComponent<InputCarController>());
        //Destroy(theSimulationObject.GetComponent<BoxCollider2D>());

        var theSimulationLogic = findSimulatableInGameObject<CarPhysicsLogic>(theSimulationObject);
        theSimulationLogic.implementation_initialize_wheels();
        theSimulationLogic.implementation_initialize_state();
        theSimulationLogic.implementation_initialize_componentReferences();
        theSimulationLogic.setSimulationFrom(this);

        return theSimulationObject;
    }

    protected override void implementation_setSimulationFromLogic(
        ISimulatableLogic inLogicToSetFrom)
    {
        CarPhysicsLogic theLogicToSetFrom = inLogicToSetFrom as CarPhysicsLogic;
        if (!theLogicToSetFrom) {
            //TODO: Make universal though for incorrect type casts
            throw new Exception("Simulation object has no compitable simulatable logic");
        }
        GameObject theGameObjectToSetFrom = theLogicToSetFrom.gameObject;

        //Set rigid body
        var theRigidBodyToSetFrom = theGameObjectToSetFrom.GetComponent<Rigidbody2D>();

        _rigidBody.angularVelocity = theRigidBodyToSetFrom.angularVelocity;
        _rigidBody.velocity = theRigidBodyToSetFrom.velocity;

        //Set logic state
        _gasValue = theLogicToSetFrom._gasValue;
        _isGasWasChangedOnUpdate = theLogicToSetFrom._isGasWasChangedOnUpdate;

        _steeringWheelValue = theLogicToSetFrom._steeringWheelValue;
        _isSteeringWheelWasRotatedOnUpdate = theLogicToSetFrom._isSteeringWheelWasRotatedOnUpdate;
    }

    protected override void implementation_simulate() {
        simulate_control();
        simulate_passive();
    }

    protected override void implementation_debugDraw(XDebug.DebugDrawSettings inDrawSettings) {
        var theBoxCollider = gameObject.GetComponent<BoxCollider2D>();
        Transform theTransform = gameObject.transform;

        XDebug.drawRectangle(
            theTransform.position, theBoxCollider.size, theTransform.rotation.eulerAngles.z,
            inDrawSettings
        );
    }

    //--Simulation
    //---Control
    private void simulate_control() {
        simulate_control_dissipate();
        simulate_control_apply();
    }

    //----Dissipate
    private void simulate_control_dissipate() {
        simulate_control_dissipate_gas();
        simulate_control_dissipate_steeringWheel();
    }

    private void simulate_control_dissipate_gas() {
        if (!_isGasWasChangedOnUpdate) {
            float theValue = _gasValue.getValue();
            if (Mathf.Abs(theValue) > GasChangeSpeed) {
                _gasValue.changeValue(theValue > 0.0f ? -GasChangeSpeed : GasChangeSpeed);
            } else {
                _gasValue.setValue(0.0f);
            }
            //_gasValue.setValue(0.0f);
        }
        _isGasWasChangedOnUpdate = false;
    }

    private void simulate_control_dissipate_steeringWheel() {
        if (!_isSteeringWheelWasRotatedOnUpdate) {
            _steeringWheelValue.changeAngleToAchieveTargetAngleWithSpeed(0.0f, SteeringWheelReturnSpeed);
        }
        _isSteeringWheelWasRotatedOnUpdate = false;
    }

    //----Apply
    private void simulate_control_apply() {
        simulate_control_apply_gas();
        simulate_control_apply_steeringWheelRotation();
    }

    private void simulate_control_apply_gas() {
        float theGasValue = _gasValue.getValue();
        if (Mathf.Approximately(theGasValue, 0.0f)) return;

        foreach (int theIndex in _drivingWheelIndices) {
            applyGasForWheel(_wheels[theIndex], theGasValue);
        }
    }

    private void simulate_control_apply_steeringWheelRotation() {
        float theSteeringWheelRotation = _steeringWheelValue.getAngle();
        foreach (int theIndex in _controlWheelIndices) {
            _wheels[theIndex].setRotation(theSteeringWheelRotation);
        }
    }

    //---Passive
    private void simulate_passive() {
        simulate_passive_wheelsResistance();
    }

    private void simulate_passive_wheelsResistance() {
        foreach (WheelState theWheel in _wheels) {
            applyResistanceForWheel(theWheel);
        }
    }

    //--Utils
    //---Gas
    private void applyGasForWheel(WheelState inWheelState, float inGasValue) {
        Vector2 theGetForceInUnits = XMetrics.forceToUnits(
            getWheelGasForce(inWheelState, inGasValue)
        );

        _rigidBody.AddForceAtPosition(
            theGetForceInUnits, getWheelWorldPosition(inWheelState),
            ForceMode2D.Force
        );
    }

    private Vector2 getWheelGasForce(WheelState inWheelState, float inGasValue) {
        Vector2 theDiraction = XMath.getDiractionVectorForAngle(getWheelWorldRotation(inWheelState));
        return theDiraction * inGasValue;
    }

    //---Wheel resistance
    private void applyResistanceForWheel(WheelState inWheelState) {
        _rigidBody.AddForceAtPosition(
            getWheelResistanceForce(inWheelState),
            getWheelWorldPosition(inWheelState),
            ForceMode2D.Force
        );
    }

    private Vector2 getWheelResistanceForce(WheelState inWheelState) {
        Vector2 theWheelPosition = getWheelWorldPosition(inWheelState);
        float theWheelsRotation = getWheelWorldRotation(inWheelState);

        Vector2 theVelocityInWheelPosition = _rigidBody.GetPointVelocity(theWheelPosition);

        Vector3 theWheelsDiraction = XMath.getDiractionVectorForAngle(theWheelsRotation);
        Vector2 theDirectProjectedAccumulateVelocity =
            Vector3.Project(theVelocityInWheelPosition, theWheelsDiraction);
        Vector2 theDirectResistanceForce = -theDirectProjectedAccumulateVelocity * _directResistanceK;

        Vector3 theWheelsNormalDiraction = XMath.getVectorRotatedBy90Degrees(theWheelsDiraction);
        Vector2 theSideProjectedAccumulateVelocity =
            Vector3.Project(theVelocityInWheelPosition, theWheelsNormalDiraction);
        Vector2 theSideResistanceForce = -theSideProjectedAccumulateVelocity * _sideResistanceK;

        return theDirectResistanceForce + theSideResistanceForce;
    }

    //--Wheel utils
    private Vector2 getWheelWorldPosition(WheelState inWheel) {
        return transform.TransformPoint(inWheel.getPosition());
    }

    private float getWheelWorldRotation(WheelState inWheel) {
        return transform.rotation.eulerAngles.z + inWheel.getRotation();
    }

    //---Types
    private struct WheelState
    {
        public WheelState(Vector2 inPosition, float inRotation = 0.0f) {
            _position = inPosition;
            _rotation = inRotation;
        }

        public Vector2 getPosition() { return _position; }
        public float getRotation() { return _rotation; }

        public void setRotation(float inRotation) { _rotation = inRotation; }

        Vector2 _position;
        float _rotation;
    }

    //TODO: Make this value more controlled
    [SerializeField] float _directResistanceK = 0.3f;
    [SerializeField] float _sideResistanceK = 0.9f;
}
