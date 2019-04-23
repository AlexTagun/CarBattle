using UnityEngine;

public abstract class HierarchicalUIObjectBase : MonoBehaviour
{
    //Methods
    //-API
    public void addRootElement(HierarchicalUIElementObject inElement) {
        _rootElement.addElement(inElement);
    }

    public void removeRootElement(HierarchicalUIElementObject inElement) {
        _rootElement.removeElement(inElement);
    }

    //-Internal API
    internal void addElementToElement(
        HierarchicalUIElementObject inElement,
        HierarchicalUIElementObject inSubelement)
    {
        XUtils.check(inElement && inSubelement);

        inSubelement._hierarchicalUIobject = this;
        inElement._elements.add(inSubelement);

        implementation_addElementToElement(inElement, inSubelement);
    }

    internal void removeElementFromElement(
        HierarchicalUIElementObject inElement,
        HierarchicalUIElementObject inSubelement)
    {
        XUtils.check(inElement && inSubelement);
        XUtils.check(inElement._elements.contains(inSubelement));

        implementation_removeElementFromElement(inElement, inSubelement);

        inElement._elements.removeElement(inSubelement);
        inSubelement._hierarchicalUIobject = null;
    }

    internal void goInto(HierarchicalUIElementObject inElement) {
        XUtils.check(inElement);
        implementation_goInto(inElement);
    }

    //-Child API to implement
    abstract protected void implementation_addElementToElement(
        HierarchicalUIElementObject inElement,
        HierarchicalUIElementObject inSubelement
    );

    abstract protected void implementation_removeElementFromElement(
        HierarchicalUIElementObject inElement,
        HierarchicalUIElementObject inSubelement
    );

    abstract protected void implementation_goInto(
        HierarchicalUIElementObject inElement
    );

    //-Implementation
    protected void Awake() {
        _rootElement = gameObject.AddComponent<HierarchicalUIElementObject>();
        _rootElement._hierarchicalUIobject = this;
        goInto(_rootElement);
    }

    //Fields
    protected HierarchicalUIElementObject _rootElement = null;
}
