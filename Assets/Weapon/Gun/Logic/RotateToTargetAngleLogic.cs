using UnityEngine;
using Values;

public class RotateToTargetAngleLogic : MonoBehaviour
{
    //Fields
    [SerializeField] private float _rotationSpeed = 5.0f;

    [SerializeField] private float _targetAngle = 90.0f;

    //-Runtime
    private LimitedFloatAngle _rotation = new LimitedFloatAngle(-180.0f, 180.0f);

    //Methods
    //-API
    public void setTargetAngle(float inTargetAngle) {
        _targetAngle = LimitedFloatAngle.getNormalizedAngle(inTargetAngle);
    }

    //-Implementation
    private void Start() {
        _rotationSpeed = Mathf.Abs(_rotationSpeed);
    }

    private void Update() {
        Update_Rotation();
        Update_ApplyRotation();
    }

    private void Update_Rotation() {
        _rotation.changeAngleToAchieveTargetAngleWithSpeed(_targetAngle, _rotationSpeed);
    }

    void Update_ApplyRotation() {
        Vector3 theEulerAngles = transform.eulerAngles;
        theEulerAngles.z = _rotation.getAngle();
        transform.eulerAngles = theEulerAngles;
    }
}
