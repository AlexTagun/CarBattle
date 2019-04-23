using UnityEngine;
using UnityEngine.UI;

public class WorldUIObject : MonoBehaviour
{
    //Methods
    //-Internal API [should be accesssable only from Game UI]
    internal void initFromGameUI(CarCityObject inCarCity) {
        _carCity = inCarCity;
    }

    //-Implementation
    private void Start() {
        XUtils.verify(_goToCarCityButton).onClick.AddListener(()=> {
            onClickedGoToCarCity?.Invoke();
        });
    }

    private void Update() {
        _healthAndEnergyBar.set(
            _carCity.getMaxHitPoints(), _carCity.getHitPoints(),
            _carCity.getMaxEnergy(), _carCity.getEnergy()
        );
    }

    //Events
    public delegate void OnClickedGoToCarCity();
    public OnClickedGoToCarCity onClickedGoToCarCity;

    //Fields
    [SerializeField] private CarCityObject _carCity = null;

    [SerializeField] private Button _goToCarCityButton = null;
    [SerializeField] private CarCityHealthAndEnergyWorldBarObject _healthAndEnergyBar = null;
}
