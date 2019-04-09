using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObject : MonoBehaviour
{
    //Methods
    DurabilityComponent _durability = null;

    //-Implementation
    void Start() {
        _durability = GetComponent<DurabilityComponent>();
        _durability.onAchievedNoHitPoints += () => Destroy(gameObject);
    }

    void Update() {
    }
}
