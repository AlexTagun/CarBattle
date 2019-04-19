using UnityEngine;
using UnityEngine.UI;

public class CarCityUIObject : MonoBehaviour
{
    public CarCityObject carCity = null;

    public Button goToWorldButton = null;
    public CarCityResourcesUIObject carCityResourcesUI = null;

    public GameObject constructionUIPrefab = null; //TODO: Replace with full implemented construction UI

    void Start() {
        XUtils.verify(buildButton).onClick.AddListener(startBuildingPlan);
        XUtils.check(buildPlanPrefab);

        _worldObjectsAttachedUIManger = XUtils.verify(
            gameObject.GetComponent<WorldObjectsAttachedUIManger>()
        );

        XUtils.verify(goToWorldButton).onClick.AddListener(()=>{
            onClickedGoToWorld?.Invoke();
        });
    }

    void Update() {
        updateResources();

        updateInput();
        updateBuildingPlan();
    }

    private void updateResources() {
        carCityResourcesUI.set(
            carCity.getTotalCrewMembersCount(),
            carCity.getFreeCrewMemberCount()
        );
    }

    private void updateInput() {
        _isMouseButtonPressed = Input.GetMouseButton(0);
    }

    private void updateBuildingPlan() {
        if (!XUtils.isValid(_currentBuildingPlan)) return;

        _currentBuildingPlan.transform.position = XUtils.getMouseWorldPosition();

        if (!_isMouseButtonPressed) return;
        if (!_currentBuildingPlan.isPossibleToBuild()) return;

        ConstructionSiteObject theConstructionSite =
            carCity.startConstructionFromBuildingPlan(_currentBuildingPlan);

        GameObject theConstructionUI = Instantiate(constructionUIPrefab);
        theConstructionUI.transform.SetParent(gameObject.transform, false);

        theConstructionUI.GetComponent<ConstructionSiteUIObject>().init(carCity, theConstructionSite);
        RectTransform theTransform = theConstructionUI.GetComponent<RectTransform>();
        theTransform.sizeDelta = new Vector2(200.0f, 100.0f);

        _worldObjectsAttachedUIManger.attach(theConstructionUI, theConstructionSite.gameObject);
    }

    void startBuildingPlan() {
        GameObject theBuildingPlan = Instantiate(buildPlanPrefab);
        _currentBuildingPlan = XUtils.verify(
            theBuildingPlan.GetComponent<BuildingPlanObject>()
        );
    }

    //Fields
    public delegate void OnClickedGoToWorld();
    public OnClickedGoToWorld onClickedGoToWorld;

    private WorldObjectsAttachedUIManger _worldObjectsAttachedUIManger = null;

    //Test {
    public Button buildButton = null;
    public GameObject buildPlanPrefab = null;
    private BuildingPlanObject _currentBuildingPlan = null;
    //}

    //TODO: Find better solution (some central input manager Or component?) {
    bool _isMouseButtonPressed = false;
    //}
}
