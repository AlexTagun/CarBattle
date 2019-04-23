using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAttachComponent : MonoBehaviour
{
    void Update() {
        Vector3 theMousePosition = XUtils.getMouseWorldPosition();
        gameObject.transform.position = theMousePosition;
        onMouseMove?.Invoke(theMousePosition);
    }

    public delegate void OnMouseMove(Vector3 inMousePosition);
    public OnMouseMove onMouseMove;
}
