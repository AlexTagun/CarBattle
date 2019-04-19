using UnityEngine;
using UnityEngine.UI;

public class WorldUIObject : MonoBehaviour
{
    public CarCityObject carCity = null;

    public Button goToCarCityButton = null;
    public CarCityHealthAndEnergyWorldBarObject healthAndEnergyBar = null;

    void Start() {
        XUtils.verify(goToCarCityButton).onClick.AddListener(()=> {
            onClickedGoToCarCity?.Invoke();
        });
    }

    void Update() {
        healthAndEnergyBar.set(
            carCity.getMaxHitPoints(), carCity.getHitPoints(),
            carCity.getMaxEnergy(), carCity.getEnergy()
        );
    }

    public delegate void OnClickedGoToCarCity();
    public OnClickedGoToCarCity onClickedGoToCarCity;
}
