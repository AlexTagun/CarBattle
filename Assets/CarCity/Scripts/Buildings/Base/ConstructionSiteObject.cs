using UnityEngine;

using Values;

public class ConstructionSiteObject : MonoBehaviour
{
    //Methods
    //-API
    public void init(CarCityObject inCarCity) {
        _carCity = XUtils.verify(inCarCity);
    }

    //--Progress info
    public float getBuildPointsToConstruct() {
        return _buildPoints.getMaximum();
    }
    public float getCurrentBuildPoints() {
        return _buildPoints.getValue();
    }

    //--Workers control
    public int getMaxWorkersPossibleToAssign() {
        return getBuildingScheme().maxWorkersToAssign;
    }

    //-CrewMember API
    internal void constructionStep(float inBuildPoints) {
        if (!XUtils.isValid(gameObject)) return;

        _buildPoints.changeValue(inBuildPoints);

        if (!_buildPoints.isValueMaximum()) return;

        createBuilding();

        XUtils.Destroy(gameObject);
    }

    //-Implementation
    void Awake() {
        _buildPoints = new LimitedFloat(
            0.0f, getBuildingScheme().buildPointsToConstruct
        );
    }

    private BuildingObject createBuilding() {
        BuildingObject theBuilding = XUtils.createObject(
            getBuildingScheme().building, transform
        );
        theBuilding.init(_carCity);

        theBuilding.transform.SetParent(transform.parent, true);
        theBuilding.gameObject.layer = gameObject.layer;

        return theBuilding;
    }

    private BuildingScheme getBuildingScheme() {
        return XUtils.verify(_buildingScheme);
    }

    //Fields
    public BuildingScheme _buildingScheme = null;

    private CarCityObject _carCity = null;
    LimitedFloat _buildPoints;
}
