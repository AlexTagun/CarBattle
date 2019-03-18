using UnityEngine;

using Values;

public class CarPhysicsLogic : RootSimulatableLogic
{
    //Fields
    [SerializeField] private Rigidbody2D _rigidBody = null;

    public GameObject[] _wheelTransforms;
    public GameObject[] _drivingWheelTransforms;
    public GameObject[] _controlWheelTransforms;

    public LimitedFloat.State GasValueSettings = new LimitedFloat.State(-10.0f, 30.0f);
    public float GasChangeSpeed = 0.3f;

    public LimitedFloatAngle.State SteeringWheelValueSettings = new LimitedFloatAngle.State(45.0f, true);
    public float SteeringWheelSpeed = 1.0f;
    public float SteeringWheelReturnSpeed = 2.0f;

    //-Runtime
    private WheelState[] _wheels = null;
    private WheelState[] _drivingWheels = null;
    private WheelState[] _controlWheels = null;

    private LimitedFloat _gasValue;
    private bool _isGasWasChangedOnUpdate = false;

    private LimitedFloatAngle _steeringWheelValue;
    private bool _isSteeringWheelWasRotatedOnUpdate = false;

    //Methods
    bool _isInitialized = false;

    public void init() {
        if (_isInitialized) return;
        _isInitialized = true;

        createWheels();

        _gasValue = new LimitedFloat(GasValueSettings);
        _steeringWheelValue = new LimitedFloatAngle(SteeringWheelValueSettings);
    }

    public void setFrom(CarPhysicsLogic inLogic) {
        _rigidBody.angularVelocity = inLogic._rigidBody.angularVelocity;
        _rigidBody.velocity = inLogic._rigidBody.velocity;

        _wheels = new WheelState[inLogic._wheels.Length];
        int theWheelIndex = 0;

        _drivingWheels = new WheelState[inLogic._drivingWheels.Length];
        int theDrivingWheelIndex = 0;

        _controlWheels = new WheelState[inLogic._controlWheels.Length];
        int theControlWheelIndex = 0;

        foreach (WheelState theWheelToCopy in inLogic._wheels) {
            WheelState theNewWheel = new WheelState(theWheelToCopy);
            _wheels[theWheelIndex++] = theNewWheel;

            if (-1 != System.Array.IndexOf(inLogic._drivingWheels, theWheelToCopy)) {
                _drivingWheels[theDrivingWheelIndex++] = theNewWheel;
            }

            if (-1 != System.Array.IndexOf(inLogic._controlWheels, theWheelToCopy)) {
                _controlWheels[theControlWheelIndex++] = theNewWheel;
            }
        }

        _gasValue = new LimitedFloat(inLogic._gasValue);
        _isGasWasChangedOnUpdate = inLogic._isGasWasChangedOnUpdate;

        _steeringWheelValue = new LimitedFloatAngle(inLogic._steeringWheelValue);
        _isSteeringWheelWasRotatedOnUpdate = inLogic._isSteeringWheelWasRotatedOnUpdate;
    }

    public void applyGas() {
        getGasValue().changeValue(GasChangeSpeed);
        _isGasWasChangedOnUpdate = true;
    }

    public void applyRevers() {
        getGasValue().changeValue(-GasChangeSpeed);
        _isGasWasChangedOnUpdate = true;
    }

    public void rotateSteeringWheelClockwise() {
        getSteeringWheelValue().changeAngle(SteeringWheelSpeed);
        _isSteeringWheelWasRotatedOnUpdate = true;
    }

    public void rotateSteeringWheelCounterClockwise() {
        getSteeringWheelValue().changeAngle(-SteeringWheelSpeed);
        _isSteeringWheelWasRotatedOnUpdate = true;
    }

    public override GameObject createSimulation() {
        GameObject theSimulation = GameObject.Instantiate(gameObject);

        theSimulation.GetComponent<CarPhysicsLogic>().setFrom(this);
        theSimulation.GetComponent<CarPhysicsLogic>()._rigidBody =
                theSimulation.GetComponent<Rigidbody2D>();

        Destroy(theSimulation.GetComponent<SimulatableObjectUpdater>());
        Destroy(theSimulation.GetComponent<CarFutureSimulationTest>());
        Destroy(theSimulation.GetComponent<InputCarController>());

        return theSimulation;
    }

    override public void simulate() {
        base.simulate();

        simulate_control();
        simulate_passive();
    }

    //-Implementation
    private void Start() { init(); }

    //--Control
    private void simulate_control() {
        simulate_control_dissipate();
        simulate_control_apply();
    }

    //---Dissipate
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
            _gasValue.setValue(0.0f);
        }
        _isGasWasChangedOnUpdate = false;
    }

    private void simulate_control_dissipate_steeringWheel() {
        if (!_isSteeringWheelWasRotatedOnUpdate) {
            _steeringWheelValue.changeAngleToAchieveTargetAngleWithSpeed(0.0f, SteeringWheelReturnSpeed);
        }
        _isSteeringWheelWasRotatedOnUpdate = false;
    }

    //---Apply
    private void simulate_control_apply() {
        simulate_control_apply_gas();
        simulate_control_apply_steeringWheelRotation();
    }

    private void simulate_control_apply_gas() {
        float theGasValue = getGasValue().getValue();
        if (Mathf.Approximately(theGasValue, 0.0f)) return;

        foreach (WheelState theWheelState in getDrivingWheels()) {
            applyGasForWheel(theWheelState, theGasValue);
        }
    }

    private void simulate_control_apply_steeringWheelRotation() {
        float theSteeringWheelRotation = _steeringWheelValue.getAngle();
        foreach (WheelState theWheelState in getControlWheels()) {
            theWheelState.setRotation(theSteeringWheelRotation);
        }
    }

    //--Passive
    private void simulate_passive() {
        simulate_passive_wheelsResistance();
    }

    private void simulate_passive_wheelsResistance() {
        foreach (WheelState theWheel in getAllWheels()) {
            applyResistanceForWheel(theWheel);
        }
    }

    //-Utils
    //--Gas
    private void applyGasForWheel(WheelState inWheelState, float inGasValue) {
        _rigidBody.AddForceAtPosition(
            getWheelGasForce(inWheelState, inGasValue),
            getWheelWorldPosition(inWheelState),
            ForceMode2D.Force
        );
    }

    private Vector2 getWheelGasForce(WheelState inWheelState, float inGasValue) {
        return getDiractionVectorForAngle(getWheelWorldRotation(inWheelState)) * inGasValue;
    }

    //--Wheel resistance
    private void applyResistanceForWheel(WheelState inWheelState) {
        _rigidBody.AddForceAtPosition(
            getWheelResistanceForce(inWheelState),
            getWheelWorldPosition(inWheelState),
            ForceMode2D.Force
        );
    }

    private Vector2 getWheelResistanceForce(WheelState inWheelState) {
        float theDirectResistanceK = 0.3f;
        float theSideResistanceK = 0.9f;

        Vector2 theWheelPosition = getWheelWorldPosition(inWheelState);
        float theWheelsRotation = getWheelWorldRotation(inWheelState);

        Vector2 theVelocityInWheelPosition = _rigidBody.GetPointVelocity(theWheelPosition);

        Vector3 theWheelsDiraction = getDiractionVectorForAngle(theWheelsRotation);
        Vector2 theDirectProjectedAccumulateVelocity =
            Vector3.Project(theVelocityInWheelPosition, theWheelsDiraction);
        Vector2 theDirectResistanceForce = -theDirectProjectedAccumulateVelocity * theDirectResistanceK;

        Vector3 theWheelsNormalDiraction = getVectorRotatedBy90Degrees(theWheelsDiraction);
        Vector2 theSideProjectedAccumulateVelocity =
            Vector3.Project(theVelocityInWheelPosition, theWheelsNormalDiraction);
        Vector2 theSideResistanceForce = -theSideProjectedAccumulateVelocity * theSideResistanceK;

        return theDirectResistanceForce + theSideResistanceForce;
    }

    //--Wheel utils
    private Vector2 getWheelWorldPosition(WheelState inWheel) {
        return transform.TransformPoint(inWheel.getPosition());
    }

    private float getWheelWorldRotation(WheelState inWheel) {
        return transform.rotation.eulerAngles.z + inWheel.getRotation();
    }

    //--State access
    private WheelState[] getAllWheels() { return _wheels; }
    private WheelState[] getDrivingWheels() { return _drivingWheels; }
    private WheelState[] getControlWheels() { return _controlWheels; }

    Rigidbody2D getRigidbody() { return _rigidBody; }

    public ref LimitedFloat getGasValue() { return ref _gasValue; }
    public ref LimitedFloatAngle getSteeringWheelValue() { return ref _steeringWheelValue; }

    //---Utils
    private void createWheels() {
        //TODO: Put validations here

        _wheels = new WheelState[_wheelTransforms.Length];
        int theWheelIndex = 0;

        _drivingWheels = new WheelState[_drivingWheelTransforms.Length];
        int theDrivingWheelIndex = 0;

        _controlWheels = new WheelState[_controlWheelTransforms.Length];
        int theControlWheelIndex = 0;

        foreach (GameObject theWheelTransform in _wheelTransforms) {
            var theTransformHolder = theWheelTransform.GetComponent<TransformHolderLogic>();

            WheelState theNewWheel = new WheelState(theTransformHolder);
            _wheels[theWheelIndex++] = theNewWheel;

            if (-1 != System.Array.IndexOf(_drivingWheelTransforms, theWheelTransform)) {
                _drivingWheels[theDrivingWheelIndex++] = theNewWheel;
            }

            if (-1 != System.Array.IndexOf(_controlWheelTransforms, theWheelTransform)) {
                _controlWheels[theControlWheelIndex++] = theNewWheel;
            }
        }

        foreach (GameObject theWheelTransform in _wheelTransforms) {
            var theTransformHolder = theWheelTransform.GetComponent<TransformHolderLogic>();
            theTransformHolder.destroy();
        }

        _wheelTransforms = null;
        _drivingWheelTransforms = null;
        _controlWheelTransforms = null;
    }

    //---Types
    private class WheelState
    {
        public WheelState(TransformHolderLogic inHolder) {
            _point = inHolder.getTransform(false);
        }

        public WheelState(WheelState inWheelState) { _point.set(inWheelState._point); }

        public Vector2 getPosition() { return _point.position; }
        public float getRotation() { return _point.rotation; }

        public void setRotation(float inRotation) { _point.rotation = inRotation; }

        TransformHolderLogic.Point _point;
    }

    //GLOBAL UTILS: Move to separate file
    static Vector3 getDiractionVectorForAngle(float inAngle) {
        float theAngleRadians = inAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(theAngleRadians), Mathf.Sin(theAngleRadians), 0.0f);
    }

    static Vector3 getVectorRotatedBy90Degrees(Vector3 inVector) {
        return new Vector3(-inVector.y, inVector.x, 0.0f);
    }
}
