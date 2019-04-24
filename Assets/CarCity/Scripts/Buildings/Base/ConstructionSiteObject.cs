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
    public int getMaxWorkersPossibleToAssign() {
        return maxWorkersToAssign;
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
}
