using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Values;

public class ConstructionSiteObject : MonoBehaviour
{
    //Settings
    public float buildPointsToConstruct = 60.0f;
    public int maxWorkersToAssign = 6;

    //Methods
    //-API
    //--Progress info
    public float getBuildPointsToConstruct() {
        return _buildPoints.getMaximum();
    }
    public float getCurrentBuildPoints() {
        return _buildPoints.getValue();
    }

    //--Workers control
    public int getCurrenAssignedWorkersCount() {
        return _workers.getSize();
    }
    public int getMaxWorkersPossibleToAssign() {
        return maxWorkersToAssign;
    }

    public void assignWorker(ElementMover<CrewMember> inWorkerMover) {
        if (_workers.getSize() == maxWorkersToAssign) return;
        _workers.placeMovingElement(inWorkerMover);
    }
    public ElementMover<CrewMember> startWithdrawWorker(CrewMember inWorker) {
        return _workers.startMovingOfElement(inWorker);
    }

    public CrewMember getFirstWorker() {
        Optional<CrewMember> theCrewMember = _workers.getFirstElement();
        return theCrewMember.isSet() ? theCrewMember.getValue() : null;
    }

    public RangeMover<CrewMember> startWithdrawAllWorkers() {
        return _workers.startMovingAll();
    }

    //-Implementation
    //TODO: Make accessable only by CrewMember
    internal BuildingObject constructionStep(
        float inBuildPoints)
    {
        _buildPoints.changeValue(inBuildPoints);

        if (!_buildPoints.isValueMaximum()) return null;

        Destroy(gameObject);
        return XUtils.createObject(
            getBuildingScheme().building, transform
        );
    }

    void Awake() {
        _buildPoints = new LimitedFloat(0.0f, buildPointsToConstruct);
    }

    private BuildingScheme getBuildingScheme() {
        return XUtils.verify(_buildingScheme);
    }

    //Fields
    public BuildingScheme _buildingScheme = null;

    LimitedFloat _buildPoints;
    FastArray<CrewMember> _workers = new FastArray<CrewMember>();
}
