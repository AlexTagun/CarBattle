using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIObject : MonoBehaviour
{
    //Methods
    //-API
    public void switchToWorldUI(
        float inCameraTransitionTime = 0.0f)
    {
        hideUI(carCityUI.gameObject);
        showUI(worldUI.gameObject);

        _cameraManager.setCamera(
            worldCamera.getSettings(), inCameraTransitionTime
        );

        _pauseManager.setPauseEnabled(false);
    }

    public void switchToCarCityUI(
        float inCameraTransitionTime = 0.0f)
    {
        hideUI(worldUI.gameObject);
        showUI(carCityUI.gameObject);

        _cameraManager.setCamera(
            carCityCamera.getSettings(), inCameraTransitionTime
        );

        _pauseManager.setPauseEnabled(true);
    }

    //-Implementation
    void Awake() {
        //TODO: Remove editor pannels here

        XUtils.verify(worldUI).initFromGameUI(carCity);
        XUtils.verify(carCityUI).initFromGameUI(carCity);

        worldUI.onClickedGoToCarCity += ()=> { switchToCarCityUI(300.0f); };
        carCityUI.onClickedGoToWorld += ()=> { switchToWorldUI(300.0f); };

        _cameraManager = XUtils.verify(
            FindObjectOfType<CameraManager>()
        );

        _pauseManager = XUtils.verify(
            FindObjectOfType<PauseManager>()
        );

        XUtils.check(worldCamera);
        XUtils.check(carCityCamera);

        switchToWorldUI();
    }

    void showUI(GameObject inUI) {
        inUI.SetActive(true);
        var theUiTransform = inUI.GetComponent<RectTransform>();
        theUiTransform.anchorMin = new Vector2(0.0f, 0.0f);
        theUiTransform.anchorMax = new Vector2(1.0f, 1.0f);
        theUiTransform.SetParent(transform, false);
    }

    void hideUI(GameObject inUI) {
        inUI.SetActive(false);
    }

    //Fields
    //-Settings
    public CarCityObject carCity = null;

    public WorldUIObject worldUI = null;
    public CarCityUIObject carCityUI = null;

    private CameraManager _cameraManager = null;
    private PauseManager _pauseManager = null;

    //TODO: Make this system more organized
    //{
    public CameraSettingsHolder worldCamera = null;
    public CameraSettingsHolder carCityCamera = null;
    //}
}
