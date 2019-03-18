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

            theLogic.rotateSteeringWheelClockwise();
            theLogic.applyGas();

            drawCross(inSimultaion.transform.position);
        });

        //isFirstLocal = false;
    }

    void drawCross(Vector2 inPosition) {
        const float theCrossSize = 0.3f;

        Debug.DrawLine(
            new Vector2(inPosition.x - theCrossSize, inPosition.y),
            new Vector2(inPosition.x + theCrossSize, inPosition.y),
            Color.green
        );

        Debug.DrawLine(
            new Vector2(inPosition.x, inPosition.y - theCrossSize),
            new Vector2(inPosition.x, inPosition.y + theCrossSize),
            Color.green
        );
    }
}
