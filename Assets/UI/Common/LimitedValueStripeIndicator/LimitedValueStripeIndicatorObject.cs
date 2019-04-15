using UnityEngine;
using UnityEngine.UI;

using Values;

public class LimitedValueStripeIndicatorObject : MonoBehaviour
{
    public RectTransform _healthStripeTransform = null;
    public Text _healthText = null;

    public void set(float inMinValue, float inMaxValue, float inValue) {
        _value.set(inMinValue, inMaxValue, inValue);
        _healthText.text = inValue.ToString("0");

        Vector2 theAnchorMax = _healthStripeTransform.anchorMax;
        theAnchorMax.x = _value.getValuePercentFromMinimum();
        _healthStripeTransform.anchorMax = theAnchorMax;
    }

    private LimitedFloat _value;
}
