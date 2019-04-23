using UnityEngine;
using UnityEngine.UI;

internal class HierarchicalGridWayUIObject : MonoBehaviour {

    //-Internal API
    internal void addWayElement(HierarchicalGridWayUIElement inWayElement) {
        XUtils.getComponent<LayoutElement>(
            inWayElement, XUtils.AccessPolicy.ShouldBeCreated
        ).flexibleHeight = 1;

        XUtils.getComponent<RectTransform>(
            inWayElement, XUtils.AccessPolicy.ShouldExist
        ).SetParent(_rectTransformForPlacingWayElements, false);

        _wayElements.add(inWayElement);
    }

    internal void removeWayElementsUpToEnd(int inBeginIndex) {
        _wayElements.removeElementsUpToEnd(inBeginIndex,
            (HierarchicalGridWayUIElement inWayElement)=>
        {
            XUtils.getComponent<RectTransform>(
                inWayElement.gameObject, XUtils.AccessPolicy.ShouldExist
            ).SetParent(null);
        });
    }

    internal HierarchicalUIElementObject getElementForLastWayElement() {
         return _wayElements.getLastElementChecked()._element;
    }

    internal Optional<int> getIndexOfElement(HierarchicalUIElementObject inElement) {
        return _wayElements.findIndex((HierarchicalGridWayUIElement inWayElement)=>{
            return inElement == inWayElement._element;
        });
    }

    internal HierarchicalUIElementObject getElementByIndex(int inIndex) {
        return _wayElements[inIndex]._element;
    }

    //-Implementation
    private void Awake() {
        _rectTransformForPlacingWayElements =
            XUtils.verify(GetComponent<RectTransform>());
    }

    //Fields
    private RectTransform _rectTransformForPlacingWayElements = null;

    private FastArray<HierarchicalGridWayUIElement> _wayElements =
        new FastArray<HierarchicalGridWayUIElement>();
}
