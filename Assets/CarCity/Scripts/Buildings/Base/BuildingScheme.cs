using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingScheme", menuName = "Building Scheme", order = 51)]
public class BuildingScheme : ScriptableObject
{
    public BuildingPlanObject buildingPlan = null;
    public ConstructionSiteObject constructionSite = null;
    public BuildingObject building = null;

    public string buildingName = "<none>";

    public void OnEnable() {
        buildingPlan._buildingScheme = this;
        constructionSite._buildingScheme = this;
    }
}
