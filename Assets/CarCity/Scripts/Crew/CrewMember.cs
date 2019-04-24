using UnityEngine;

public class CrewMember : ScriptableObject
{
    public float getBuildPointsPerSecond() { return 1.0f; }

    public void setConstruction(ConstructionSiteObject inConstruction) {
        _construction = inConstruction;
    }

    public ConstructionSiteObject getConstruction() {
        return _construction;
    }


    public bool isFree() {
        return !XUtils.isValid(_construction);
    }

    public void update(float inDeltaTime) {
        if (isFree()) return;

        float theBuildPointsPerUpdate =
            getBuildPointsPerSecond() * inDeltaTime;
        _construction.constructionStep(theBuildPointsPerUpdate);
    }

    ConstructionSiteObject _construction = null;
}
