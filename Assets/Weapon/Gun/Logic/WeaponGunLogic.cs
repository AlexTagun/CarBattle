using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGunLogic : WeaponLogic
{
    //Settings
    public GameObject owner = null;

    //Methods
    //-API
    public void setTargetAngle(float inTargetAngle) {
        _rotateLogic.setTargetAngle(inTargetAngle);
    }

    //-Implementation
    //--Overriding
    protected new void Start() {
        base.Start();

        _effectInterfaces = gameObject.GetComponents<WeaponGunEffectInterface>();

        foreach (GameObject theShootingPoint in _shootingPoints) {
            theShootingPoint.SetActive(false);
        }
    }

    protected override void performShoot() {
        foreach (GameObject theShootingPoint in _shootingPoints) {
            performShootFromPoint(theShootingPoint);
        }
    }

    //--Shoot implementation
    private void performShootFromPoint(GameObject inShootingPoint) {
        Vector2 thePointWorld = new Vector2();
        float theRotation = 0.0f;
        XMath.getFromTransform(
            ref thePointWorld, ref theRotation, inShootingPoint
        );
        Vector2 theDiraction = XMath.getDiractionVectorForAngle(theRotation);

        float theShootingDistance = 10.0f;
        RaycastHit2D[] theHits = Physics2D.RaycastAll(
            thePointWorld, theDiraction, theShootingDistance
        );

        Vector2 theHitPoint = new Vector2();
        Vector2 theHitNormal = new Vector2();
        Collider2D theHittedCollider = null;
        foreach (RaycastHit2D theHit in theHits) {
            if (shouldIgnoreColliderOnHit(theHit.collider)) continue;

            theHittedCollider = theHit.collider;
            theHitPoint = theHit.point;
            theHitNormal = theHit.normal;
            break;
        }

        notifyEffectsAboutShoot(thePointWorld, theDiraction, theShootingDistance,
            theHitPoint, !!theHittedCollider
        );

        if (!theHittedCollider) return;
        Rigidbody2D theShootedRigidBody = theHittedCollider.gameObject.GetComponent<Rigidbody2D>();
        if (!theShootedRigidBody) return;

        float theForce = 30.0f;
        theShootedRigidBody.AddForceAtPosition(theDiraction * theForce, theHitPoint);

        var theDurability = theShootedRigidBody.gameObject.GetComponent<DurabilityComponent>();
        if (theDurability) {
            theDurability.changeHitPoints(-5.0f);
        }
    }

    //--Effects
    void notifyEffectsAboutShoot(Vector2 inStart, Vector2 inDiraction, float inDistance,
        Vector2 inHitPoint, bool inHitted)
    {
        Vector2 theEndPoint = inHitted ? inHitPoint : inStart + inDiraction * inDistance;
        foreach (WeaponGunEffectInterface theInterface in _effectInterfaces) {
            theInterface.onShoot(inStart, theEndPoint, inHitted);
        }
    }

    //--Utils
    GameObject getOwner() { return transform.parent.gameObject; }

    bool shouldIgnoreColliderOnHit(Collider2D inCollider) {
        Collider2D[] theOwnerColliders = owner.GetComponentsInChildren<Collider2D>();
        return (-1 != System.Array.IndexOf(theOwnerColliders, inCollider));
    }

    //-Fields
    [SerializeField] private RotateToTargetAngleLogic _rotateLogic = null;
    [SerializeField] private GameObject[] _shootingPoints = null;

    private WeaponGunEffectInterface[] _effectInterfaces = null;
}
