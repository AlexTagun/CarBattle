using UnityEngine;
using UnityEngine.UI;

public class BuildingsGroupSelectionUIElement : HierarchicalUIElementObject
{
    //Methods
    //-Implementation
    private void Awake() {
        _selectionBuildingsGroupButton.onClick.AddListener(()=>{
            //TODO: Add buildings here
            goInto();
        });
    }

    //Fields
    [SerializeField] private Button _selectionBuildingsGroupButton = null;
}
