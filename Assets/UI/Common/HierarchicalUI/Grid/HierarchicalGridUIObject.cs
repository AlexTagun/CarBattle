using UnityEngine;

public class HierarchicalGridUIObject : HierarchicalUIObjectBase
{
    //Methods
    //-Child API implementation
    override protected void implementation_addElementToElement(
        HierarchicalUIElementObject inElement,
        HierarchicalUIElementObject inSubelementToAdd)
    {
        if (_elementShownInGrid == inElement) {
            refreshShowingElement();
        }
    }

    override protected void implementation_removeElementFromElement(
        HierarchicalUIElementObject inElement,
        HierarchicalUIElementObject inSubelementToRemove)
    {
        Optional<int> theWayIndexOfRemovingElement = _wayUI.getIndexOfElement(inSubelementToRemove);
        if (theWayIndexOfRemovingElement.isSet()) {
            goInto(_wayUI.getElementByIndex(theWayIndexOfRemovingElement.getValue() - 1));
        } else if (inElement == _elementShownInGrid) {
            refreshShowingElement();
        }
    }

    override protected void implementation_goInto(
        HierarchicalUIElementObject inElement)
    {
        XUtils.check(inElement);
        if (inElement == _elementShownInGrid) return;

        Optional<int> theWayIndexOfElementToGoInto = _wayUI.getIndexOfElement(inElement);
        if (theWayIndexOfElementToGoInto.isSet()) {
            _wayUI.removeWayElementsUpToEnd(theWayIndexOfElementToGoInto.getValue() + 1);
        } else {
            _wayUI.addWayElement(createWayElementForElement(inElement));
        }

        showElementInGrid(inElement);
    }

    //-Utils
    private void showElementInGrid(HierarchicalUIElementObject inElement) {
        _elementShownInGrid = XUtils.verify(inElement);
        refreshShowingElement();
    }

    private void refreshShowingElement() {
        XUtils.check(_elementShownInGrid);

        _gridRectTransform.DetachChildren();

        foreach (HierarchicalUIElementObject theChildElement in _elementShownInGrid._elements) {
            XUtils.getComponent<RectTransform>(
                theChildElement, XUtils.AccessPolicy.ShouldExist
            ).SetParent(_gridRectTransform, false);
        }
    }

    private HierarchicalGridWayUIElement createWayElementForElement(
        HierarchicalUIElementObject inElement)
    {
        HierarchicalGridWayUIElement theWayElement = inElement.createWayElement();
        if (!theWayElement) {
            GameObject theWayElementGameObject = Instantiate(_defaultWayElementPrefab);
            theWayElement = XUtils.verify(
                theWayElementGameObject.GetComponent<HierarchicalGridWayUIElement>()
            );

        }
        theWayElement._hierarchicalGridUIObject = this;
        theWayElement._element = inElement;

        return theWayElement;
    }

    //Fields
    [SerializeField] private HierarchicalGridWayUIObject _wayUI = null;
    [SerializeField] private GameObject _defaultWayElementPrefab = null;

    [SerializeField] private RectTransform _gridRectTransform = null;

    private HierarchicalUIElementObject _elementShownInGrid = null;
}
