using UnityEngine;

public class BuildingSelectionUIObject : MonoBehaviour
{
    //Methods
    //-API
    public void init(CarCityObject inCarCity) {
        _carCity = inCarCity;

        BuildingScheme[] theBuildingSchemes = _carCity.getBuildingSchemes();
        foreach (BuildingScheme theBuildingScheme in theBuildingSchemes) {
            _buildingsSelectionHierarchicalGrid.addRootElement(
                createBuildingSelectionUI(theBuildingScheme)
            );
        }

        _carCity.onBuildingSchemeAdded += (BuildingScheme inBuildingScheme) => {
            _buildingsSelectionHierarchicalGrid.addRootElement(
                createBuildingSelectionUI(inBuildingScheme)
            );
        };
    }

    //-Implementation
    private void Awake() {
        XUtils.check(_buildingsSelectionHierarchicalGrid);

        XUtils.check(_buildingSelectionUIElementPrefab);
        XUtils.check(_buildingsGroupSelectionUIElementPrefab);
    }

    BuildingSelectionUIElementObject createBuildingSelectionUI(
        BuildingScheme inBuildingSceme)
    {
        GameObject theBuildingSelectionUIGameObject =
            Instantiate(_buildingSelectionUIElementPrefab);
        var theBuildingSelectionUI =
            XUtils.getComponent<BuildingSelectionUIElementObject>(
                theBuildingSelectionUIGameObject, XUtils.AccessPolicy.ShouldExist
            );

        theBuildingSelectionUI.init(inBuildingSceme,
            (BuildingScheme inBuildingScheme) =>
        {
            BuildingPlanObject theBuildingPlan = XUtils.createObject(
                inBuildingScheme.buildingPlan
            );
            theBuildingPlan.init(_carCity);
            theBuildingPlan.getUIInterface().init(
                _carCity, _constructionSiteUIPrefab
            );
        });

        return theBuildingSelectionUI;
    }

    //Fields
    //-Car city
    private CarCityObject _carCity = null;

    //-UI elements
    [SerializeField]
    private HierarchicalGridUIObject _buildingsSelectionHierarchicalGrid = null;

    //-Element prefabs
    [SerializeField] private GameObject _buildingSelectionUIElementPrefab = null;
    [SerializeField] private GameObject _buildingsGroupSelectionUIElementPrefab = null;


    //-WORKAROUND: TODO: Find better way to pass this info to BuildingPlatUIInterface {
    public ConstructionSiteUIObject _constructionSiteUIPrefab = null;
    //}
}
