using UnityEngine;

abstract public class RootSimulatableLogic : ISimulatableLogic
{
    public ISimulatableLogic[] _children = null;

    public abstract GameObject createSimulation();

    override public void simulate() {
        foreach (ISimulatableLogic theChild in _children) {
            theChild.simulate();
        }
    }
}
