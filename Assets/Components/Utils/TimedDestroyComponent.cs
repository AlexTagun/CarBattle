using UnityEngine;

public class TimedDestroyComponent : MonoBehaviour
{
    private void Awake() {
        _startingTime = Time.fixedTime;
    }

    private void FixedUpdate() {
        if (Time.fixedTime - _startingTime < _timeRangeForDestroy) return;
        Destroy(gameObject);
    }

    [SerializeField] float _timeRangeForDestroy = 1.0f;
    private float _startingTime = 0.0f;
}
