using UnityEngine;

public struct Damage
{
    //TODO: Find how to organize Damage API Classes better
    public static void applyDamage(GameObject inGameObject, Damage inDamage) {
        XUtils.check(inGameObject);
        var theDamageProxy = XUtils.getComponent<DamageProxy>(
            inGameObject, XUtils.AccessPolicy.JustFind
        );
        if (theDamageProxy) {
            theDamageProxy.applyDamage(inDamage);
            return;
        }

        var theDurability = XUtils.getComponent<DurabilityComponent>(
            inGameObject, XUtils.AccessPolicy.JustFind
        );
        if (theDurability) {
            theDurability.changeHitPoints(-inDamage.damageAmount);
        }
    }

    public static void applyDamage(Component inComponent, Damage inDamage) {
        applyDamage(XUtils.verify(inComponent).gameObject, inDamage);
    }


    public float damageAmount;
}
