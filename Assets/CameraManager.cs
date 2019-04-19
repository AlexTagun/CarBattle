using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera cameraA = null;
    public Camera cameraB = null;

    bool _isKeyPressed = true;
    bool _isKeyReleased = true;

    void Start() {
        cameraA.enabled = true;
        cameraB.enabled = false;
    }

    void Update() {
        _isKeyPressed = Input.GetKey(KeyCode.Q);
        if (!_isKeyPressed) {
            _isKeyReleased = true;
        }

        if (_isKeyPressed) {
            if (_isKeyReleased) {
                _isKeyReleased = false;
                switchCamera();
            }
        }
    }

    public void switchCamera() {
        cameraA.enabled = !cameraA.enabled;
        cameraB.enabled = !cameraB.enabled;
    }
}
