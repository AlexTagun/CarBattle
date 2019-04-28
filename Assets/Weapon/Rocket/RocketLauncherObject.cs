using UnityEngine;

public class RocketLauncherObject : WeaponLogic
{
    //Methods
    //Methods
    //-API
    public void setTargetAngle(float inTargetAngle) {
        _rotateLogic.setTargetAngle(inTargetAngle);
    }

    //-Implementation
    //--Overriding
    protected new void Start() {
        base.Start();

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
        Vector2 theRocketPosition = new Vector2();
        float theRocketRotation = 0.0f;
        XMath.getFromTransform(
            ref theRocketPosition, ref theRocketRotation, inShootingPoint
        );

        RocketProjectileObject theRocket =
            XUtils.createObject(_projectilePrefab);
        theRocket.init(XUtils.verify(_owner));

        theRocket.transform.position = theRocketPosition;
        Vector3 theRotation = theRocket.transform.rotation.eulerAngles;
        theRotation.z = theRocketRotation;
        theRocket.transform.eulerAngles = theRotation;
    }

    //Fields
    [SerializeField] private GameObject _owner = null;
    [SerializeField] private RotateToTargetAngleLogic _rotateLogic = null;
    [SerializeField] private GameObject[] _shootingPoints = null;
    [SerializeField] private RocketProjectileObject _projectilePrefab = null;
}
