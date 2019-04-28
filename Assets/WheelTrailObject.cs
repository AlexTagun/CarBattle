using UnityEngine;

public class WheelTrailObject : MonoBehaviour
{
    private void Awake() {
        _lineRender = XUtils.getComponent<LineRenderer>(
            this, XUtils.AccessPolicy.ShouldExist
        );
    }

    void Update() {
        _lifeTime += Time.deltaTime;
        if (_lifeTime >= _timeToDesappear) {
            Destroy(gameObject);
            return;
        }

        float theAlpha = 1.0f - _lifeTime / _timeToDesappear;

        Color theColor;

        theColor = _lineRender.startColor;
        theColor.a = theAlpha;
        _lineRender.startColor = theColor;

        theColor = _lineRender.endColor;
        theColor.a = theAlpha;
        _lineRender.endColor = theColor;
    }

    [SerializeField] private float _timeToDesappear = 0.0f;
    private float _lifeTime = 0.0f;

    private LineRenderer _lineRender = null;
}
