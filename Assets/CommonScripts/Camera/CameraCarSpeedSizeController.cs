using UnityEngine;

public class CameraCarSpeedSizeController : MonoBehaviour
{
    void Awake() {
        _carRigidBody = XUtils.getComponent<Rigidbody2D>(
            _car, XUtils.AccessPolicy.ShouldExist
        );

        _camera = XUtils.getComponent<Camera>(
            gameObject, XUtils.AccessPolicy.ShouldExist
        );
    }

    void Update() {
        float theVelocityMagnitude =
            _carRigidBody.velocity.magnitude;

        float theVelocityForSizeParam = Mathf.Clamp(
            (theVelocityMagnitude - _minSizeCarSpeed) /
                (_maxSizeCarSpeed - _minSizeCarSpeed),
            0.0f, 1.0f
        );

        Debug.Log(theVelocityForSizeParam + "  :  " + theVelocityMagnitude);

        _camera.orthographicSize =
            _minSize + (_maxSize - _minSize) * theVelocityForSizeParam;
    }

    [SerializeField] CarObject _car = null;
    Rigidbody2D _carRigidBody = null;

    [SerializeField] float _minSize = 3.0f;
    [SerializeField] float _maxSize = 10.0f;
    [SerializeField] float _minSizeCarSpeed = 5.0f;
    [SerializeField] float _maxSizeCarSpeed = 20.0f;

    Camera _camera = null;
}
