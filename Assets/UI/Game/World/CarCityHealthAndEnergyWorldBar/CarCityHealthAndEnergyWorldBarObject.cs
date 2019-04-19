using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCityHealthAndEnergyWorldBarObject : MonoBehaviour
{
    public LimitedValueStripeIndicatorObject energyIndicator = null;
    public LimitedValueStripeIndicatorObject hitPointsIndicator = null;

    public void set(float inMaxHitPoints, float inHitPoints,
        float inMaxEnergy, float inEnergy)
    {
        energyIndicator.set(0.0f, inMaxEnergy, inEnergy);
        hitPointsIndicator.set(0.0f, inMaxHitPoints, inHitPoints);
    }
}
