using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGunLogic : WeaponLogic
{
    //Fields
    //-Settings
    public GameObject owner = null;

    private RotateToTargetAngleLogic _rotateLogic = null;

    private TransformHolderLogic.Point[] _shootingPoints = null;

    private WeaponGunEffectInterface[] _effectInterfaces = null;

    //Methods
    public void setTargetAngle(float inTargetAngle) {
        _rotateLogic.setTargetAngle(inTargetAngle);
    }

    //-Implementation
    //--Overriding
    protected new void Start() {
        base.Start();

        TransformHolderLogic[] theShootingPointHolders = gameObject.GetComponentsInChildren<TransformHolderLogic>();
        _shootingPoints = new TransformHolderLogic.Point[theShootingPointHolders.Length];
        for (int theIndex = 0; theIndex < theShootingPointHolders.Length; ++theIndex) {
            _shootingPoints[theIndex] = theShootingPointHolders[theIndex].getTransform();
        }

        _rotateLogic = gameObject.GetComponent<RotateToTargetAngleLogic>();

        _effectInterfaces = gameObject.GetComponents<WeaponGunEffectInterface>();
    }

    protected override void performShoot() {
        foreach (TransformHolderLogic.Point theShootingPoint in _shootingPoints) {
            performShootFromPoint(theShootingPoint);
        }
    }


    //--Shoot implementation
    private void performShootFromPoint(TransformHolderLogic.Point inShootingPoint) {
        Vector2 thePointWorld = transform.TransformPoint(inShootingPoint.position);

        Quaternion theLocalRotation = Quaternion.Euler(0.0f, 0.0f, inShootingPoint.rotation);
        Quaternion theWorldRotation = transform.rotation * theLocalRotation;

        float theWorldAngle = theWorldRotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 theDiraction = new Vector2(Mathf.Cos(theWorldAngle), Mathf.Sin(theWorldAngle));

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
            Debug.Log(theDurability.getHitPoints());
        }
    }

    //--Effects
    void notifyEffectsAboutShoot(Vector2 inStart, Vector2 inDiraction, float inDistance, Vector2 inHitPoint, bool inHitted) {
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
}
