using UnityEngine;

//TODO: Find better solution
public class CameraSettingsHolder : MonoBehaviour
{
    //Methods
    //-API
    public CameraTypes.CameraSettings getSettings() {
        return _cameraSettings;
    }

    //-Implementation
    private void Awake() {
        _camera = XUtils.getComponent<Camera>(
            this, XUtils.AccessPolicy.ShouldExist
        );
    }

    private void FixedUpdate() {
        _cameraSettings.position = _camera.transform.position;
        _cameraSettings.rotation = _camera.transform.rotation;

        _cameraSettings.orthographicSize = _camera.orthographicSize;
    }

    //Fields
    private Camera _camera = null;
    public CameraTypes.CameraSettings _cameraSettings =
        new CameraTypes.CameraSettings();
}
