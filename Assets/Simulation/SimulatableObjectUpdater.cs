using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatableObjectUpdater : MonoBehaviour
{
    private RootSimulatableLogic _simulatableLogic = null;

    void Start() {
        _simulatableLogic = gameObject.GetComponent<RootSimulatableLogic>();
    }

    void FixedUpdate() {
        _simulatableLogic.simulate();
    }
}
