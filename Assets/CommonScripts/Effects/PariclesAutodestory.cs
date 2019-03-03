using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PariclesAutodestory : MonoBehaviour
{
    ParticleSystem _particleSystem = null;

    void Start() {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void Update() {
        if (!_particleSystem.IsAlive()) {
            Destroy(gameObject);
        }
    }
}
