using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFutureSimulationTest : MonoBehaviour
{
    static bool isFirst = true;
    bool isFirstLocal = false;

    //int frameToSimulte = 0;
    //int frameToSimulteIndex = 0;

    void FixedUpdate() {
        //if (isFirst) {
        //    isFirst = false;
        //    isFirstLocal = true;
        //}
        //if (!isFirstLocal) return;
        //
        //if (frameToSimulteIndex < frameToSimulte) {
        //    ++frameToSimulteIndex;
        //    return;
        //} else {
        //    frameToSimulteIndex = 0;
        //}

        var theSimulationManager = Object.FindObjectOfType<SimulationManager>();

        theSimulationManager.simulate(gameObject, 50, (GameObject inSimultaion)=>{
            CarPhysicsLogic theLogic = inSimultaion.GetComponent<CarPhysicsLogic>();
            theLogic.debugDraw();

            theLogic.rotateSteeringWheelClockwise();
            theLogic.applyGas();
        });

        //isFirstLocal = false;
    }
}
