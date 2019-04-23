using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectionUIElementObject : HierarchicalUIElementObject
{
    //Methods
    //-API
    public delegate void OnSelectBuilding(BuildingScheme inBuildingScheme);

    public void init(
        BuildingScheme inBuildingScheme, OnSelectBuilding inOnSelectBuilding)
    {
        _buildingScheme = inBuildingScheme;
        _onSelectBuilding = inOnSelectBuilding;
        refreshUIElements();
    }

    //-Implementation
    private void refreshUIElements() {
        XUtils.verify(_selectBuildingButton.GetComponentInChildren<Text>()).
            text = _buildingScheme.buildingName;
    }

    private void Awake() {
        XUtils.verify(_selectBuildingButton).onClick.AddListener(()=>{
            _onSelectBuilding?.Invoke(_buildingScheme);
        });
    }

    //Fields
    [SerializeField] private Button _selectBuildingButton = null;
    private BuildingScheme _buildingScheme = null;

    private OnSelectBuilding _onSelectBuilding;
}
