using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAttachComponent : MonoBehaviour
{
    void Update() {
        Vector2 theMousePosition = XUtils.getMouseWorldPosition();
        gameObject.transform.position = new Vector3(
            theMousePosition.x, theMousePosition.y,
            gameObject.transform.position.z
        );
        onMouseMove?.Invoke(theMousePosition);
    }

    public delegate void OnMouseMove(Vector2 inMousePosition);
    public OnMouseMove onMouseMove;
}
