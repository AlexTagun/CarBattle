using UnityEngine;

public class HierarchicalUIElementObject : MonoBehaviour
{
    //Methods
    //-API
    public void addElement(HierarchicalUIElementObject inElement) {
        XUtils.check(null != inElement && null != _hierarchicalUIobject);
        _hierarchicalUIobject.addElementToElement(this, inElement);
    }

    public void removeElement(HierarchicalUIElementObject inElement) {
        XUtils.check(null != inElement && null != _hierarchicalUIobject);
        _hierarchicalUIobject.removeElementFromElement(this, inElement);
    }

    //-Child API
    protected void goInto() {
        XUtils.verify(_hierarchicalUIobject).goInto(this);
    }

    //-Child for implementation (optional)
    internal protected virtual HierarchicalGridWayUIElement createWayElement() {
        return null;
    }

    //Fields
    internal HierarchicalUIObjectBase _hierarchicalUIobject = null;
    internal FastArray<HierarchicalUIElementObject> _elements =
        new FastArray<HierarchicalUIElementObject>();
}
