using UnityEngine;

using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{
    public delegate void AfterSimulationStepDelegate(GameObject inSimulation);

    //Methods
    //TODO: Inject logic for each step
    public void simulate(GameObject inGameObject, int inStepsCount,
        AfterSimulationStepDelegate inDelegate, float inStepDelta = 0.02f)
    {
        //TODO: Validate if object can be simulated

        var theRootSimulationLogics = inGameObject.GetComponent<RootSimulatableLogic>();
        if (!theRootSimulationLogics) return;

        GameObject theSimulation = theRootSimulationLogics.createSimulation();
        SceneManager.MoveGameObjectToScene(theSimulation, _simulationScene);

        theRootSimulationLogics = theSimulation.GetComponent<RootSimulatableLogic>();
        for (int theStepIndex = 0; theStepIndex < inStepsCount; ++theStepIndex) {
            theRootSimulationLogics.simulate();
            _simulationScenePhysics.Simulate(inStepDelta);
        
            inDelegate(theSimulation);
        }

        Destroy(theSimulation);
    }

    //-Implementation
    void Awake() {
        _simulationScene = SceneManager.CreateScene("PhysicsSimulation",
            new CreateSceneParameters(LocalPhysicsMode.Physics2D)
        );
        _simulationScenePhysics = _simulationScene.GetPhysicsScene2D();
    }

    Scene _simulationScene;
    PhysicsScene2D _simulationScenePhysics;
}
