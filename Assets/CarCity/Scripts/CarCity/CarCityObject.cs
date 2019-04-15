using UnityEngine;
using Values;

public class CarCityObject : MonoBehaviour
{
    //Methods
    //-API
    public ConstructionSiteObject startConstructionFromBuildingPlan(
        BuildingPlanObject inBuildingPlan)
    {
        ConstructionSiteObject theConstructionSiteObject =
            inBuildingPlan.startConstruction();
        _constructionSites.add(theConstructionSiteObject);
        return theConstructionSiteObject;
    }

    //-Crew management
    public int getTotalCrewMembersCount() {
        return _crewMembers.getSize();
    }
    public int getFreeCrewMemberCount() {
        return _freeCrewMembers.getSize();
    }

    public void withdrawWorkerAsFree(ElementMover<CrewMember> inMover) {
        _freeCrewMembers.placeMovingElement(inMover);
    }

    public ElementMover<CrewMember> startFreeCrewMemberAssignment(CrewMember inCrewMember) {
        return _freeCrewMembers.startMovingOfElement(inCrewMember);
    }

    public CrewMember getFirstFreeCrewMember() {
        return _freeCrewMembers.getFirstElement().getValue(false);
    }

    //-Implementation
    private void Awake() {
        for (int theIndex = 0; theIndex < 10; ++theIndex) {
            _crewMembers.add(new CrewMember());
        }
        _freeCrewMembers.addAll(_crewMembers);
    }

    void Update() {
        float theDeltaTime = Time.deltaTime;
        _constructionSites.iterateWithRemove(
            (ConstructionSiteObject inConstructionSite)=>
        {
            if (!XUtils.isValid(inConstructionSite)) return true;

            BuildingObject theBuilding =
                inConstructionSite.updateConstruction(theDeltaTime);
            if (!theBuilding) return false;

            _freeCrewMembers.placeMovingRange(
                inConstructionSite.startWithdrawAllWorkers()
            );

            _buildings.add(theBuilding);
            return true;
        });

        _buildings.iterateWithRemove(
            (BuildingObject inBuilding) =>
        {
            if (!XUtils.isValid(inBuilding)) return true;

            //TODO: Do building update
            return false;
        });
    }

    //Fields
    LimitedFloat _energy = new LimitedFloat(0.0f, 0.0f);
    LimitedFloat _metal = new LimitedFloat(0.0f, float.MaxValue);

    FastArray<CrewMember> _crewMembers = new FastArray<CrewMember>();
    FastArray<CrewMember> _freeCrewMembers = new FastArray<CrewMember>();

    FastArray<ConstructionSiteObject> _constructionSites = new FastArray<ConstructionSiteObject>();
    FastArray<BuildingObject> _buildings = new FastArray<BuildingObject>();
}
