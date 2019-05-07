using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    //Methods
    //-API
    public void init(CarCityObject inCarCity) {
        _carCity = XUtils.verify(inCarCity);
    }

    //-Child API
    protected CarCityObject getCarCity() {
        return _carCity;
    }

    //Fields
    public BuildingScheme _buildingScheme = null;

    private CarCityObject _carCity = null;
}
