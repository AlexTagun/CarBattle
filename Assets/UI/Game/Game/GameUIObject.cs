using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIObject : MonoBehaviour
{
    //Settings
    public CarCityObject carCity = null;

    public WorldUIObject worldUI = null;
    public CarCityUIObject carCityUI = null;

    //TODO: Organize cameras in some Camera Manager
    // Find integral solution for all player-related functionality (input+cameras)
    //{
    public Camera worldCamera = null;
    public Camera carCityCamera = null;
    //}

    //Methods
    //-API
    public void switchToWorldUI() {
        hideUI(carCityUI.gameObject);
        showUI(worldUI.gameObject);

        worldCamera.enabled = true;
        carCityCamera.enabled = false;
    }

    public void switchToCarCityUI() {
        hideUI(worldUI.gameObject);
        showUI(carCityUI.gameObject);

        worldCamera.enabled = false;
        carCityCamera.enabled = true;
    }

    //-Implementation
    void Start() {
        //TODO: Remove editor pannels here

        XUtils.verify(worldUI).carCity = carCity;
        XUtils.verify(carCityUI).carCity = carCity;

        worldUI.onClickedGoToCarCity += switchToCarCityUI;
        carCityUI.onClickedGoToWorld += switchToWorldUI;

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
}
