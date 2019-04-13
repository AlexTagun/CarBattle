using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField]
    private GameObject _timerPattern;

    private BoxCollider2D _collider;
    private bool _isPicked = true;
    private Vector2 _screenPosition = Vector2.zero;

    private int _cntTriggerEnter = 0;

    private Color _baseColor;
    private Color _green = new Color(162F / 255F, 255F / 255F, 146F / 255F);
    private Color _red = new Color(255F / 255F, 45F / 255F, 69F / 255F);

    enum State {
        planning,
        processing,
        ready
    }
    private State _state = State.planning;

    void Start() {
        _baseColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = _green;
        
        _collider = GetComponent<BoxCollider2D>();
        gameObject.transform.position = MousePosToWorldPoint();
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(transform.position);
        _screenPosition.x = Input.mousePosition.x - cameraPosition.x;
        _screenPosition.y = Input.mousePosition.y - cameraPosition.y;
    }

    void Update() {
        switch (_state) {
            case State.planning:
                if (Input.GetMouseButtonDown(1)) Destroy(gameObject);
                OnMouseMove();
                colorUpdate();
                break;
            default:
                break;
        }

        
    }
    private void OnMouseDown() {
        if (_state == State.planning) {
            if(_cntTriggerEnter != 0) {
                ErrorEvent();
                return;
            }
            _state = State.processing;
            GetComponent<SpriteRenderer>().color = _baseColor;
            createTimer();
            Destroy(GetComponent<Rigidbody2D>());
        }
     
    }

    private void OnMouseMove() {
        if (!HasMouseMoved()) return;
        if (_state != State.planning) return;
        Vector3 position = transform.position;
        Vector3 curPos = new Vector3(Input.mousePosition.x - _screenPosition.x, Input.mousePosition.y - _screenPosition.y, 0);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        worldPos.z = 0;
        transform.position = worldPos;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _cntTriggerEnter++;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _cntTriggerEnter--;
    }

    private bool HasMouseMoved() {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    private Vector3 MousePosToWorldPoint() {
        Vector3 curPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        worldPos.z = 0;
        return worldPos;
    }

    //TODO
    private void ErrorEvent() {
        Debug.Log("Can't build here");
    }

    private void colorUpdate() {
        GetComponent<SpriteRenderer>().color = (_cntTriggerEnter == 0) ? _green : _red;
    }

    private void createTimer() {
        GameObject timer = Instantiate(_timerPattern, transform);
        timer.GetComponent<BuildTimer>().setTimer(15);
    }

    public void OnTimerDestroy() {
        _state = State.ready;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        gameObject.AddComponent<Building>();
        Destroy(this);
    }
}
