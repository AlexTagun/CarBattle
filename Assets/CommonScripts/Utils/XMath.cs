using UnityEngine;

public class XMath
{
    public static Vector3 getDiractionVectorForAngle(float inAngle) {
        float theAngleRadians = inAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(theAngleRadians), Mathf.Sin(theAngleRadians), 0.0f);
    }

    public static Vector3 getVectorRotatedBy90Degrees(Vector3 inVector) {
        return new Vector3(-inVector.y, inVector.x, 0.0f);
    }

    public static Vector2 rotate(Vector2 inVector, float inDegrees) {
        float theDegrees = inDegrees * Mathf.Deg2Rad;
        float theSin = Mathf.Sin(theDegrees);
        float theCos = Mathf.Cos(theDegrees);

        return new Vector2(
            (theCos * inVector.x) - (theSin * inVector.y),
            (theSin * inVector.x) + (theCos * inVector.y)
        );
    }
}
