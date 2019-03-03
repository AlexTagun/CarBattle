using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLineLogic : MonoBehaviour
{
    private float _timeToDesappear = 0.0f;
    private float _lifeTime = 0.0f;

    private LineRenderer _lineRender = null;
    float _startBaseAlpha = 0.0f;
    float _endBaseAlpha = 0.0f;

    public void init(Vector2 inStartPoint, Vector2 inEndPoint, float inTimeToDesappear) {
        _timeToDesappear = inTimeToDesappear;

        _lineRender = gameObject.GetComponent<LineRenderer>();
        _lineRender.SetPosition(0, new Vector3(inStartPoint.x, inStartPoint.y, -0.3f));
        _lineRender.SetPosition(1, new Vector3(inEndPoint.x, inEndPoint.y, -0.3f));

        _startBaseAlpha = _lineRender.startColor.a;
        _endBaseAlpha = _lineRender.endColor.a; 
    }

    void Update() {
        _lifeTime += Time.deltaTime;
        if (_lifeTime <= _timeToDesappear) {
            float theAlpha = 1.0f - _lifeTime/_timeToDesappear;

            Color theColor;

            theColor = _lineRender.startColor;
            theColor.a = _startBaseAlpha * theAlpha;
            _lineRender.startColor = theColor;

            theColor = _lineRender.endColor;
            theColor.a = _endBaseAlpha * theAlpha;
            _lineRender.endColor = theColor;

            return;
        }

        Destroy(gameObject);
    }
}
