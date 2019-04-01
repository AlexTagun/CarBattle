using UnityEngine;
using System;

abstract public class ISimulatableLogic : MonoBehaviour
{
    //Methods
    //-API
    //--Initialization
    public void initialize() { implementation_initialize(); }

    public GameObject createSimulation() {
        GameObject theSimulationObject = implementation_createSimulation();
        if (!theSimulationObject) return null;

        var theSimulatable = findSimulatableInGameObject<ISimulatableLogic>(
            theSimulationObject, true
        );
        theSimulatable._isSimulation = true;

        return theSimulationObject;
    }

    public void setSimulationFrom(ISimulatableLogic inLogicToSetFrom) {
        setSimulationFromLogic_internal(inLogicToSetFrom);
    }

    public void setSimulationFrom(GameObject inObjectToSimulate) {
        var theSimulatable = findSimulatableInGameObject<ISimulatableLogic>(
            inObjectToSimulate
        );
        setSimulationFromLogic_internal(theSimulatable);
    }

    //--Update
    public void simulate() { implementation_simulate(); }

    //--Accessors
    public bool isSimulation() { return _isSimulation; }

    //--Debug
    public void debugDraw(XDebug.DebugDrawSettings inDrawSettings) {
        implementation_debugDraw(inDrawSettings);
    }

    public void debugDraw() {
        implementation_debugDraw(XDebug.DebugDrawSettings.defaultValue);
    }

    //-Implementation
    protected abstract void implementation_initialize();

    protected abstract GameObject implementation_createSimulation();
    protected abstract void implementation_setSimulationFromLogic(ISimulatableLogic inLogicToSetFrom);

    protected abstract void implementation_simulate();

    protected virtual void implementation_debugDraw(XDebug.DebugDrawSettings inDrawSettings) {
        XDebug.drawCross(transform.position, inDrawSettings);
    }

    //--Internal
    private void setSimulationFromLogic_internal(ISimulatableLogic inLogicToSetFrom) {
        //if (!isSimulation()) {
        //    throw new Exception("Try to setup object as simulation");
        //}
        implementation_setSimulationFromLogic(inLogicToSetFrom);
    }

    //--Utils
    protected DerivedLogicTypeToCast findSimulatableInGameObject<DerivedLogicTypeToCast>(
        GameObject inObjectToSimulate, bool inDoCheck = false) where DerivedLogicTypeToCast : ISimulatableLogic
    {
        DerivedLogicTypeToCast theLogic =
                inObjectToSimulate.GetComponent(GetType()) as DerivedLogicTypeToCast;
        if (inDoCheck && !theLogic)
        {
            //TODO: Add here more details
            throw new Exception("Simulation object has no compitable simulatable logic");
        }
        return theLogic;
    }

    //Fields
    private bool _isSimulation = false;
}
