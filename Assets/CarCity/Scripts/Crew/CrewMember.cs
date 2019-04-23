using UnityEngine;

public class CrewMember : ScriptableObject
{
    public float getBuildPointsPerSecond() { return 1.0f; }

    public void setConstruction(ConstructionSiteObject inConstruction) {
        _construction = inConstruction;
    }

    public void update(float inDeltaTime) {
        if (!_construction) return;

        float theBuildPointsPerUpdate =
            getBuildPointsPerSecond() * inDeltaTime;
        _construction.constructionStep(theBuildPointsPerUpdate);
    }

    ConstructionSiteObject _construction;
}
