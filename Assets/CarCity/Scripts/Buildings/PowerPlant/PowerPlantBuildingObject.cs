using UnityEngine;

public class PowerPlantBuildingObject : BuildingObject {

    private void FixedUpdate() {
        float theProductionPerTick =
            _energyProduction * Time.fixedDeltaTime;
        getCarCity().changeEnergy(theProductionPerTick);
    }

    //Fields
    //-Settings
    //TODO: Move to building scheme?
    [SerializeField] private float _energyProduction = 0.1f;
}
