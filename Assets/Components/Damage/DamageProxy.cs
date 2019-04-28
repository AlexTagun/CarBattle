using UnityEngine;

public class DamageProxy : MonoBehaviour
{
    //Methods
    //-API
    virtual public void applyDamage(Damage inDamage) {
        float theDamageAmount = getDamageAmount(inDamage);
        _durabilityComponent.changeHitPoints(-theDamageAmount);
    }

    //-Child API
    virtual protected float getDamageAmount(Damage inDamage) {
        return inDamage.damageAmount;
    }

    protected DurabilityComponent getDurabilityComponent() {
        return _durabilityComponent;
    }

    //-Implementation
    private void Awake() {
        _durabilityComponent = XUtils.getComponent<DurabilityComponent>(
            this, XUtils.AccessPolicy.ShouldExist
        );
    }

    //Fields
    private DurabilityComponent _durabilityComponent = null;
}
