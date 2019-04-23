using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickTrackingComponent : MonoBehaviour
{
    public delegate void OnClick();
    public OnClick onClick;

    private void Update() {
        if (Input.GetMouseButton(0)) {
            onClick?.Invoke();
        }
    }
}
