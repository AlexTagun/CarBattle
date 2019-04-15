using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Values;

public class ConstructionSiteObject : MonoBehaviour
{
    //Settings
    public float buildPointsToConstruct = 100.0f;
    public int maxWorkersToAssign = 6;

    public GameObject buildPrefab = null;

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
    //TODO: Make accessable only by CarCity
    internal BuildingObject updateConstruction(float inDeltaTime) {
        foreach (CrewMember theCrewMember in _workers)
        {
            _buildPoints.changeValue(
                theCrewMember.getBuildPointsPerUpdate(inDeltaTime)
            );
        }

        if (!_buildPoints.isValueMaximum()) return null;

        Destroy(gameObject);
        return spawnBuilding();
    }

    void Awake() {
        _buildPoints = new LimitedFloat(0.0f, buildPointsToConstruct);
        XUtils.check(buildPrefab);
    }

    private BuildingObject spawnBuilding() {
        GameObject theBuilding = Instantiate(buildPrefab);
        XMath.assignTransform(theBuilding.transform, transform);
        return XUtils.verify(theBuilding.GetComponent<BuildingObject>());
    }

    //Fields
    LimitedFloat _buildPoints;
    FastArray<CrewMember> _workers = new FastArray<CrewMember>();
}
