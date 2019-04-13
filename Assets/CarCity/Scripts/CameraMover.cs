using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private PolygonCollider2D _collider;
    private bool _isPicked = false;
    private Vector3 _screenPosition = Vector3.zero;
    private Vector3 _oldPos = Vector3.zero;
    
    private float _panSpeedX = 17.55F;
    
    private float _panSpeedY = 10F;
    void Start() {
        _collider = GetComponent<PolygonCollider2D>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("DOWN");
            _isPicked = true;
            _oldPos = transform.position;
            _screenPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0)) {
            _isPicked = false;
        }
        Debug.Log(transform.position);
        OnMouseMove();
    }

    private void OnMouseUp() {
        _isPicked = false;
    }

    private void OnMouseMove() {
        if (!_isPicked) return;
        if (!HasMouseMoved()) return;
        
        Vector3 worldPos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - _screenPosition;

        worldPos.x *= _panSpeedX;
        worldPos.y *= _panSpeedY;
        transform.position = _oldPos + -worldPos;
    }

    private bool HasMouseMoved() {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }
}
