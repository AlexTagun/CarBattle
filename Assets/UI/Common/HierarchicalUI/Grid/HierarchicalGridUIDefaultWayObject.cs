using UnityEngine;
using UnityEngine.UI;

public class HierarchicalGridUIDefaultWayObject : HierarchicalGridWayUIElement
{
    private void Awake() {
        _selectButton.onClick.AddListener(()=>{
            select();
        });

        var theTransform = XUtils.getComponent<RectTransform>(
            this, XUtils.AccessPolicy.ShouldExist
        );
        Vector2 theSize = theTransform.sizeDelta;
        theSize.x = 100;
        theTransform.sizeDelta = theSize;
    }

    [SerializeField] private Button _selectButton = null;
}
