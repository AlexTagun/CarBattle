using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatableObjectUpdater : MonoBehaviour
{
    private ISimulatableLogic _simulatableLogic = null;

    void Start() {
        _simulatableLogic = gameObject.GetComponent<ISimulatableLogic>();
        _simulatableLogic.initialize();
    }

    void FixedUpdate() {
        _simulatableLogic.simulate();
    }
}
