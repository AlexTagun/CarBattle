using UnityEngine;

public class CarCityRepObject : MonoBehaviour
{
    void Start() {
        
    }

    void Update() {
        
    }


    internal void createRepForElement(CarCityRepElementObject inElement) {
        //TODO: Create Rep for element
        //TODO: Add rep to City Rep
        
        //Rep should be removed automaticly if Object removed (by using IsValid)
        
        //Reps may be created by request & removed at any Time

        //TODO: Synchronize position of Rep & of Object (notify Rep about Object moves)
        //TODO: Notify Rep about any needed changes of Object state

        //TODO: Create API to request Objects spacial info in Rep (for example, create mouse
        // Rep in world city Object to request )
    }

    //TODO: Make scale of Rep be like direct scale of 
}
