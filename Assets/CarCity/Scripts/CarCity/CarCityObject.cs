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
        int theResult = 0;
        foreach (CrewMember theCrewMember in _crewMembers) {
            theResult += theCrewMember.isFree() ? 1 : 0;
        }
        return theResult;
    }

    public CrewMember getFirstFreeCrewMember() {
        foreach (CrewMember theCrewMember in _crewMembers) {
            if (theCrewMember.isFree()) return theCrewMember;
        }
        return null;
    }

    public void collectCrewMembers(
        ref FastArray<CrewMember> outCrewMemebers,
        FastArray<CrewMember>.ElementPredicate inPredicate)
    {
        _crewMembers.collectAll(ref outCrewMemebers, inPredicate);
    }

    //-Buildings
    //TODO: Wrap to the UI Interface ??? {
    public BuildingScheme[] getBuildingSchemes() {
        BuildingScheme[] theBuildingSchemes =
            new BuildingScheme[_buildingSchemes.getSize()];
        _buildingSchemes.collectAll(theBuildingSchemes);
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

    //-Buildings
    private FastArray<BuildingScheme> _buildingSchemes = new FastArray<BuildingScheme>();
    
    //TEST {
    public BuildingScheme[] testBuildingSchemes;
    //}
}
