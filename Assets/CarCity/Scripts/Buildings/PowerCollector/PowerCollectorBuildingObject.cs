using UnityEngine;

public class PowerCollectorBuildingObject : BuildingObject
{
    private void Start() {
        getCarCity().changeMaxEnergy(_energyPerCollector);
    }

    private void OnDestroy() {
        getCarCity().changeMaxEnergy(-_energyPerCollector);
    }

    [SerializeField] private float _energyPerCollector = 10.0f;
}
