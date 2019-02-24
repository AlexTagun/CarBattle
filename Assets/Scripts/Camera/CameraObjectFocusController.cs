using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObjectFocusController : MonoBehaviour
{
    public GameObject Object = null;
    public bool GetRotationFromObject = false;

    void Start() { }

    void Update() {
        Vector3 theObjectPosition = Object.transform.position;
        theObjectPosition.z = transform.position.z;
        transform.position = theObjectPosition;

        if (GetRotationFromObject) {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, Object.transform.rotation.eulerAngles.z);
        }
    }
}
