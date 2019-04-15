using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCityResourcesUIObject : MonoBehaviour
{
    public LimitedValueStripeIndicatorObject _freeWorkersIndicator;

    public void set(int inTotalWorkers, int inFreeWorkers) {
        _freeWorkersIndicator.set(0, inTotalWorkers, inFreeWorkers);
    }
}
