using UnityEngine;
using UnityEngine.UI;

public class CarCityUIObject : MonoBehaviour
{
    //Methods
    //-Internal API [should be accesssable only from Game UI]
    internal void initFromGameUI(CarCityObject inCarCity) {
        _carCity = inCarCity;

        _buildSelectionUI.init(inCarCity);
    }

    //-Implementation
    void Start() {
        XUtils.verify(_goToWorldButton).onClick.AddListener(()=>{
            onClickedGoToWorld?.Invoke();
        });
    }

    void Update() {
        updateResources();
    }

    private void updateResources() {
        _carCityResourcesUI.set(
            _carCity.getTotalCrewMembersCount(),
            _carCity.getFreeCrewMemberCount()
        );
    }

    //Events
    public delegate void OnClickedGoToWorld();
    public OnClickedGoToWorld onClickedGoToWorld;

    //Fields
    [SerializeField] private CarCityObject _carCity = null;

    [SerializeField] private Button _goToWorldButton = null;
    [SerializeField] private CarCityResourcesUIObject _carCityResourcesUI = null;

    [SerializeField] private BuildingSelectionUIObject _buildSelectionUI = null;

    [SerializeField] private WorldObjectsAttachedUIManger _worldObjectsAttachedUIManger = null;
}
