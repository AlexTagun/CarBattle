using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGunEffectInterface : MonoBehaviour
{
    public GameObject HitParticlesPrefab = null;
    public GameObject ShootLinePrefab = null;

    public virtual void onShoot(Vector2 inWorldStart, Vector2 inWorldEnd, bool inHitted) {
        GameObject theShootPrefab = Instantiate(ShootLinePrefab, new Vector3(0.0f, 0.0f, -0.3f), new Quaternion());
        theShootPrefab.GetComponent<ShootLineLogic>().init(inWorldStart, inWorldEnd, 1.0f);

        if (inHitted) {
            Vector3 theParitclesPosition = inWorldEnd;
            theParitclesPosition.z = -0.1f;
            Instantiate(HitParticlesPrefab, theParitclesPosition, new Quaternion());
        }
    }
}
