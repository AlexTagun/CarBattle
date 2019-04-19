using UnityEngine;
using UnityEngine.UI;

using Values;

public class LimitedValueStripeIndicatorObject : MonoBehaviour
{
    public RectTransform _valueStripeTransform = null;
    public Text _valueText = null;

    public void set(float inMinValue, float inMaxValue, float inValue) {
        _valueText.text = inValue.ToString("0");

        Vector2 theAnchorMax = _valueStripeTransform.anchorMax;
        theAnchorMax.x = XMath.getValueRatioInRange(inMinValue, inMaxValue, inValue);
        _valueStripeTransform.anchorMax = theAnchorMax;
    }
}
