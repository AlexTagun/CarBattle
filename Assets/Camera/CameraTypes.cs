using UnityEngine;

public class CameraTypes
{
    //NB: This is class (not structure) for possibility to
    // reference value from several place
    public class CameraSettings {
        public Vector3 position;
        public Quaternion rotation;

        public float orthographicSize;
    }
}
