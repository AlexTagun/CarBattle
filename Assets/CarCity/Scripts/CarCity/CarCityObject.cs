using UnityEngine;
using Values;

public class CarCityObject : MonoBehaviour
{
    //Methods
    //-State getters
    public float getHitPoints() { return _hitPoints.getValue(); }
    public float getMaxHitPoints() { return _hitPoints.getMaximum(); }

    public float getEnergy() { return _energy.getValue(); }
    public float getMaxEnergy() { return _energy.getMaximum(); }

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

    public ElementMover<CrewMember> startFreeCrewMemberAssignment(
        CrewMember inCrewMember)
    {
        return _freeCrewMembers.startMovingOfElement(inCrewMember);
    }

    public CrewMember getFirstFreeCrewMember() {
        return _freeCrewMembers.getFirstElement().getValue(false);
    }

    //-Buildings
    //TODO: Wrap to the UI Interface ??? {
    public BuildingScheme[] getBuildingSchemes() {
        BuildingScheme[] theBuildingSchemes =
            new BuildingScheme[_buildingSchemes.getSize()];
        _buildingSchemes.collectElements(theBuildingSchemes);
        return theBuildingSchemes;
    }

    public delegate void OnBuildingSchemeAdded(
        BuildingScheme inBuildingScheme
    );
    public OnBuildingSchemeAdded onBuildingSchemeAdded;
    //}

    public void addBuildingScheme(BuildingScheme inBuildingScheme) {
        _buildingSchemes.add(inBuildingScheme);
        onBuildingSchemeAdded?.Invoke(inBuildingScheme);
    }

    //-Implementation
    private void Awake() {
        for (int theIndex = 0; theIndex < 10; ++theIndex) {
            _crewMembers.add(
                ScriptableObject.CreateInstance<CrewMember>()
            );
        }
        _freeCrewMembers.addAll(_crewMembers);

        //TEST {
        foreach (BuildingScheme theTestBuildingScheme in testBuildingSchemes) {
            addBuildingScheme(theTestBuildingScheme);
        }
        //}
    }

    private void Update() {
        foreach (CrewMember theCrewMember in _crewMembers) {
            theCrewMember.update(Time.deltaTime);
        }
    }

    //Fields
    //-Resources
    LimitedFloat _hitPoints = new LimitedFloat(0.0f, 100.0f, 100.0f);

    LimitedFloat _energy = new LimitedFloat(0.0f, 0.0f);
    LimitedFloat _metal = new LimitedFloat(0.0f, float.MaxValue);

    //-Crew
    FastArray<CrewMember> _crewMembers = new FastArray<CrewMember>();
    FastArray<CrewMember> _freeCrewMembers = new FastArray<CrewMember>();

    //-Buildings
    private FastArray<BuildingScheme> _buildingSchemes = new FastArray<BuildingScheme>();
    
    //TEST {
    public BuildingScheme[] testBuildingSchemes;
    //}
}
