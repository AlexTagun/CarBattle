using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarObject : MonoBehaviour
{
    public RectTransform _healthStripeTransform = null;
    public Text _healthText = null;

    public void set(float inHitPoints, float inMaxHitPoints) {
        _healthText.text = inHitPoints + "/" + inMaxHitPoints;

        Vector2 theAnchorMax = _healthStripeTransform.anchorMax;
        float theLifeRatio = Mathf.Clamp(inHitPoints / inMaxHitPoints, 0.0f, 1.0f);
        theAnchorMax.x = theLifeRatio;
        _healthStripeTransform.anchorMax = theAnchorMax;
    }
}
