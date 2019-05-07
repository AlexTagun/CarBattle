using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Methods
    //-API
    public void setCamera(
        CameraTypes.CameraSettings inCameraSettings)
    {
        XUtils.check(inCameraSettings);

        internalStopTransition();
        internalSetCameraSettings(inCameraSettings);
    }

    //NB: Speed is used for ortho size change
    //TODO: Find some better approach for transition time setup
    public void setCamera(
        CameraTypes.CameraSettings inCameraSettings, float inSpeed = 0.0f)
    {
        XUtils.check(inCameraSettings);
        XUtils.check(inSpeed >= 0);

        internalStopTransition();

        if (0.0f == inSpeed) {
            internalSetCameraSettings(inCameraSettings);
        } else {
            internalStartTransition(inCameraSettings, inSpeed);
        }
    }

    //-Implementation
    private void Awake() {
        XUtils.check(_camera);

        _cameraSettings =
            XUtils.verify(_initialSettingsHolder).getSettings();
    }

    private void FixedUpdate() {
        validateState();

        if (isTransiting()) {
            internalUpdateTransition();
        } else {
            internalUpdateCameraFromSettings();
        }
    }

    //--Camera settings
    private void internalSetCameraSettings(
        CameraTypes.CameraSettings inCameraSettings)
    {
        _cameraSettings = inCameraSettings;
    }

    private void internalUpdateCameraFromSettings() {
        _camera.transform.position = _cameraSettings.position;
        _camera.transform.rotation = _cameraSettings.rotation;
        _camera.orthographicSize = _cameraSettings.orthographicSize;
    }

    //--Transition
    private void internalStartTransition(
        CameraTypes.CameraSettings inTargetCameraSettings, float inSpeed)
    {
        _transitionState = new Optional<TransitionState>(
            new TransitionState(inTargetCameraSettings, inSpeed)
        );
        _cameraSettings = null;
    }

    private void internalUpdateTransition() {

        //Compute progress based on main param value & speed
        float theTransitionSpeed = _transitionState.getValue().speed;
        CameraTypes.CameraSettings theTargetCameraSettings =
            _transitionState.getValue().targetCameraSettings;

        float theTransitionSpeedPerUpdate = theTransitionSpeed * Time.fixedUnscaledDeltaTime;

        float theProgressForSpeedPerUpdate = theTransitionSpeedPerUpdate /
            Mathf.Abs(theTargetCameraSettings.orthographicSize - _camera.orthographicSize);

        theProgressForSpeedPerUpdate = Mathf.Clamp(theProgressForSpeedPerUpdate, 0.0f, 1.0f);

        //Stop transition if params will be achieved on this update or...
        if (1.0f == theProgressForSpeedPerUpdate) {
            internalStopTransition();
            internalSetCameraSettings(theTargetCameraSettings);
        } else {

            //...evaluate params otherwise
            _camera.transform.position = Vector3.Lerp(
                _camera.transform.position, theTargetCameraSettings.position,
                theProgressForSpeedPerUpdate
            );

            _camera.transform.rotation = Quaternion.Lerp(
                _camera.transform.rotation, theTargetCameraSettings.rotation,
                theProgressForSpeedPerUpdate
            );

            _camera.orthographicSize = Mathf.Lerp(
                _camera.orthographicSize, theTargetCameraSettings.orthographicSize,
                theProgressForSpeedPerUpdate
            );
        }
    }

    private void internalStopTransition() {
        _transitionState = new Optional<TransitionState>();
    }

    //--Utility accessors
    private bool isTransiting() {
        return null == _cameraSettings;
    }

    //--Debug
    private void validateState() {
        XUtils.check(_camera);
        XUtils.check(Vector3.one == _camera.transform.localScale);

        XUtils.check(null != _cameraSettings || _transitionState.isSet());
    }

    //Fields
    [SerializeField] private Camera _camera = null;

    //-Transition
    private CameraTypes.CameraSettings _cameraSettings = null;
    private Optional<TransitionState> _transitionState;

    //TODO: Find better solution {
    [SerializeField] private CameraSettingsHolder _initialSettingsHolder = null;
    //}

    //Private types
    private struct TransitionState {
        public TransitionState(
            CameraTypes.CameraSettings inTargetCameraSettings, float inSpeed)
        {
            targetCameraSettings = inTargetCameraSettings;
            speed = inSpeed;
        }

        public CameraTypes.CameraSettings targetCameraSettings;
        public float speed;
    }
}
