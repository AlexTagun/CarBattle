using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGunLogic : WeaponLogic
{
    //Fields
    //-Settings
    private RotateToTargetAngleLogic _rotateLogic = null;

    WeaponGunEffectInterface[] _effectInterfaces = null;

    //Methods
    public void setTargetAngle(float inTargetAngle) {
        _rotateLogic.setTargetAngle(inTargetAngle);
    }

    //-Implementation
    //--Overriding
    protected override void performShoot() {
        float theRotation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 theDiraction = new Vector2(Mathf.Cos(theRotation), Mathf.Sin(theRotation));

        float theShootingDistance = 10.0f;
        RaycastHit2D[] theHits = Physics2D.RaycastAll(transform.position, theDiraction, theShootingDistance);

        Vector2 theHitPoint = new Vector2();
        Vector2 theHitNormal = new Vector2();
        Collider2D theHittedCollider = null;
        foreach (RaycastHit2D theHit in theHits) {
            Collider2D theHitCollider = theHit.collider;
            Collider2D theOwnerCollider = getOwner().GetComponent<Collider2D>();

            if (theHitCollider != theOwnerCollider) {
                theHittedCollider = theHitCollider;
                theHitPoint = theHit.point;
                theHitNormal = theHit.normal;
                break;
            }
        }

        notifyEffectsAboutShoot(transform.position, theDiraction, theShootingDistance,
            theHitPoint, !!theHittedCollider
        );

        if (!theHittedCollider) return;
        Rigidbody2D theShootedRigidBody = theHittedCollider.gameObject.GetComponent<Rigidbody2D>();
        if (!theShootedRigidBody) return;

        float theForce = 30.0f;
        theShootedRigidBody.AddForceAtPosition(theDiraction * theForce, theHitPoint);
    }

    //-Implementation
    protected new void Start() {
        base.Start();

        _rotateLogic = gameObject.GetComponent<RotateToTargetAngleLogic>();

        _effectInterfaces = gameObject.GetComponents<WeaponGunEffectInterface>();        
    }

    //--Effects
    void notifyEffectsAboutShoot(Vector2 inStart, Vector2 inDiraction, float inDistance, Vector2 inHitPoint, bool inHitted) {
        Vector2 theEndPoint = inHitted ? inHitPoint : inStart + inDiraction * inDistance;
        foreach (WeaponGunEffectInterface theInterface in _effectInterfaces)
        {
            theInterface.onShoot(transform.position, theEndPoint, inHitted);
        }
    }

    //--Utils
    GameObject getOwner() { return transform.parent.gameObject; }
}
