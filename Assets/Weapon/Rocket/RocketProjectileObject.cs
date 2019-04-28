using UnityEngine;

public class RocketProjectileObject : MonoBehaviour
{
    //Methods
    //-API
    public void init(GameObject inInstigator) {
        _instigator = inInstigator;
    }

    //-Implementation
    private void Awake() {
        _startingTime = Time.fixedTime;
    }

    private void FixedUpdate() {
        Vector2 theVelocityDiraction = calculateVelocityDiraction();
        float theFrameSpeed = Time.fixedDeltaTime *
            _speedCurve.Evaluate(Time.fixedTime - _startingTime) * _maxSpeedValue;

        update_damageScan(theVelocityDiraction, theFrameSpeed);
        update_position(theVelocityDiraction, theFrameSpeed);
    }

    private void update_damageScan(Vector2 inVelocityDiraction, float inFrameSpeed) {
        Vector2 thePosition = transform.position;

        RaycastHit2D theHit = XCollisions2D.raycast(
            thePosition, inVelocityDiraction, inFrameSpeed,
            (Collider2D inCollider)=>
        {
            //TODO: Additional ignore allies
            if (!_instigator) return false;
            return XUtils.isComponentRelatedToObject(_instigator, inCollider);
        });
        if (!theHit.collider) return;

        Damage theDamage = new Damage();
        theDamage.damageAmount = _damageAmount;
        Damage.applyDamage(theHit.collider, theDamage);

        Destroy(gameObject);
    }

    private void update_position(Vector2 inVelocityDiraction, float inFrameSpeed) {
        Vector2 thePosition = transform.position;
        thePosition += inVelocityDiraction * inFrameSpeed;
        transform.position = thePosition;
    }

    private Vector2 calculateVelocityDiraction() {
        float theRotation = transform.rotation.eulerAngles.z;
        return XMath.getDiractionVectorForAngle(theRotation);
    }

    //Fields
    [SerializeField] private AnimationCurve _speedCurve = null;
    [SerializeField] private float _maxSpeedValue = 10.0f;
    private float _startingTime = 0.0f;

    [SerializeField] private float _damageAmount = 1.0f;

    private GameObject _instigator = null;
}
