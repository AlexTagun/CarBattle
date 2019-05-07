using UnityEngine;

public class XMetrics : MonoBehaviour
{
    // --- Force ---

    public static float forceToKilonewtons(float inForceInUnits) {
        return inForceInUnits * kMetersInUnit;
    }

    public static Vector2 forceToKilonewtons(Vector2 inForceInUnits) {
        return inForceInUnits * kMetersInUnit;
    }

    public static float forceToUnits(float inForceInKilonewtons) {
        return inForceInKilonewtons / kMetersInUnit;
    }

    public static Vector2 forceToUnits(Vector2 inForceInKilonewtons) {
        return inForceInKilonewtons / kMetersInUnit;
    }

    // --- Constants

    public static float kGravityForceMagnitude = 0.01f;

    //Details
    private static float kMetersInUnit = 10.0f;
}
