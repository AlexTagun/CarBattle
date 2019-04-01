using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    private SimulationManager _simulationManager = null;
    private CarPhysicsLogic _carPhysics = null;

    public GameObject _targetObject = null;

    public float _euristicDistanceToMakeSpeedLessIfFar = 20.0f;
    public float _minimumGas = 3.0f;

    void Start() {
        _simulationManager = Object.FindObjectOfType<SimulationManager>();
        _carPhysics = gameObject.GetComponent<CarPhysicsLogic>();
    }

    void FixedUpdate() {
        if (!_targetObject) return;

        float theTargetAngle = XMath.getNearestAngleBetweenPoints(
            transform.position, _targetObject.transform.position
        );

        float theCurrentAngle = transform.rotation.eulerAngles.z;
        float theNearestDelta = XMath.getNormalizedAngle(theTargetAngle - theCurrentAngle);

        if (XMath.equalsWithPrecision(theNearestDelta, 0.0f, 30.0f)) {
            _carPhysics.applyGas();
        } else {
            float theDistanceSquare =
                _euristicDistanceToMakeSpeedLessIfFar * _euristicDistanceToMakeSpeedLessIfFar;
            Vector2 theDelta = _targetObject.transform.position - transform.position;

            if (theDelta.sqrMagnitude < theDistanceSquare) {
                if (_carPhysics.getGasValue().getValue() > _minimumGas) {
                    _carPhysics.applyRevers();
                } else {
                    _carPhysics.applyGas();
                }
            } else {
                _carPhysics.applyGas();
            }
        }

        bool theIsNeedMoveByClockwiseToAchieveTargetAngle = (theNearestDelta > 0.0f);

        bool theItsTimeToCorrectWheels = false;

        _simulationManager.simulate(gameObject, 50, (ISimulatableLogic inLogic)=>{
            var theLogic = inLogic as CarPhysicsLogic;

            if (theIsNeedMoveByClockwiseToAchieveTargetAngle) {
                theLogic.rotateSteeringWheelCounterClockwise();
                if (!theLogic.isRotatingByClockwice()) return false;
            } else {
                theLogic.rotateSteeringWheelClockwise();
                if (theLogic.isRotatingByClockwice()) return false;
            }

            float theSimulationCurrentAngle = theLogic.transform.rotation.eulerAngles.z;
            float theSimulationNearestDelta = XMath.getNormalizedAngle(theTargetAngle - theSimulationCurrentAngle);

            if (!XMath.hasSameSigns(theNearestDelta, theSimulationNearestDelta)) {
                theItsTimeToCorrectWheels = true;
                return false;
            }

            //theLogic.debugDraw();

            return true;
        });

        if (theIsNeedMoveByClockwiseToAchieveTargetAngle) {
            if (theItsTimeToCorrectWheels) {
                _carPhysics.rotateSteeringWheelCounterClockwise();
            } else {
                _carPhysics.rotateSteeringWheelClockwise();
            }
        } else {
            if (theItsTimeToCorrectWheels) {
                _carPhysics.rotateSteeringWheelClockwise();
            } else {
                _carPhysics.rotateSteeringWheelCounterClockwise();
            }
        }
    }
}
