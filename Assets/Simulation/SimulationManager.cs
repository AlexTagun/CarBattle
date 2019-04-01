using UnityEngine;

using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{
    public delegate bool AfterSimulationStepDelegate(ISimulatableLogic inSimulationLogic);

    //Methods
    //TODO: Inject logic for each step
    public void simulate(GameObject inGameObject, int inStepsCount,
        AfterSimulationStepDelegate inDelegate, float inStepDelta = 0.02f)
    {
        var theLogicToSimulate = inGameObject.GetComponent<ISimulatableLogic>();
        if (!theLogicToSimulate) {
            Debug.Log("INVALID OBJECT FOR SIMULATION");
            return;
        }

        GameObject theSimulationObject = theLogicToSimulate.createSimulation();
        if (!theSimulationObject) {
            Debug.Log("CANNOT CREATE SIMULATION");
            return;
        }

        var theLogicSimulation =
                theSimulationObject.GetComponent<ISimulatableLogic>() as CarPhysicsLogic;

        Vector2 theVelocity = theLogicSimulation._rigidBody.velocity;
        float theAngularVelocity = theLogicSimulation._rigidBody.angularVelocity;
        SceneManager.MoveGameObjectToScene(theSimulationObject, _simulationScene);
        theLogicSimulation._rigidBody.velocity = theVelocity;
        theLogicSimulation._rigidBody.angularVelocity = theAngularVelocity;

        for (int theStepIndex = 0; theStepIndex < inStepsCount; ++theStepIndex) {
            if (!inDelegate(theLogicSimulation)) break;

            theLogicSimulation.simulate();
            _simulationScenePhysics.Simulate(inStepDelta);
        }

        Destroy(theSimulationObject);
    }

    //-Implementation
    void Awake() {
        _mainScenePhysics = SceneManager.GetActiveScene().GetPhysicsScene2D();

        _simulationScene = SceneManager.CreateScene("PhysicsSimulation",
            new CreateSceneParameters(LocalPhysicsMode.Physics2D)
        );
        _simulationScenePhysics = _simulationScene.GetPhysicsScene2D();

        Physics2D.autoSimulation = false;
    }

    private void FixedUpdate() {
        _mainScenePhysics.Simulate(Time.fixedDeltaTime);
    }

    Scene _simulationScene;
    PhysicsScene2D _simulationScenePhysics;

    PhysicsScene2D _mainScenePhysics;
}
