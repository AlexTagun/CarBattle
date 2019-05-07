using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingScheme", menuName = "Building Scheme", order = 51)]
public class BuildingScheme : ScriptableObject
{
    public BuildingPlanObject buildingPlan = null;
    public ConstructionSiteObject constructionSite = null;
    public BuildingObject building = null;

    public float buildPointsToConstruct = 60.0f;
    public int maxWorkersToAssign = 6;

    public string buildingName = "<none>";

    public void OnEnable() {
        XUtils.verify(buildingPlan)._buildingScheme = this;
        XUtils.verify(constructionSite)._buildingScheme = this;
        XUtils.verify(building)._buildingScheme = this;
    }
}
