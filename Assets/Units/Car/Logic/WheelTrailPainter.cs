using UnityEngine;

public class WheelTrailPainter : MonoBehaviour
{
    [SerializeField] private GameObject _wheelTrailAsset = null;
    [SerializeField] private Material[] _materials = null;

    private BoxCollider2D _collider = null;
    private LineRenderer _lineRenderer = null;

    void Start(){
        _collider = GetComponent<BoxCollider2D>();
    }

    void Update(){
        UpdateTrail();
        checkLand();
    }

    private void UpdateTrail() {
        if (!_lineRenderer) return;
        drawVerticy();
    }

    private void checkLand() {
        RaycastHit2D hit = isOnCollider();
        if (hit) {
            //TODO indentification of such objects
            OnTriggerEnter2D(hit.collider.gameObject.name);
        } else {
            OnTriggerExit2D();
        }
    }

    private RaycastHit2D isOnCollider() {
        Ray ray = new Ray(transform.position, Vector3.back);
        return Physics2D.Raycast(ray.origin, ray.direction, 0, 1 << 8);
    }

    private LineRenderer startPaintTrail(Material material) {
        GameObject wheelTrailObj = Instantiate(_wheelTrailAsset);
        LineRenderer lineRenderer = wheelTrailObj.GetComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        return wheelTrailObj.GetComponent<LineRenderer>();
    }

    private void drawVerticy() {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, transform.position);
    }

    private void stopPaintTrail() {
        _lineRenderer = null;
    }

    private void OnTriggerEnter2D(string land) {
        switch (land) {
            case "sharpLand":
                _lineRenderer = startPaintTrail(_materials[0]);
                return;
            case "flame":
                _lineRenderer = startPaintTrail(_materials[1]);
                return;
        }
    }

    private void OnTriggerExit2D() {
        stopPaintTrail();
    }
}
