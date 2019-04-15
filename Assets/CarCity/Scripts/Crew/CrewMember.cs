using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember
{
    public float getBuildPointsPerUpdate(float inDeltaTime) {
        return getBuildPointsPerSecond() * inDeltaTime;
    }

    public float getBuildPointsPerSecond() { return 1.0f; }
}
