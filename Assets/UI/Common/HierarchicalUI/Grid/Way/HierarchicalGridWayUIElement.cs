using UnityEngine;

public class HierarchicalGridWayUIElement : MonoBehaviour
{
    //Methods
    //-Child API
    protected void select() {
        XUtils.check(_element);
        XUtils.verify(_hierarchicalGridUIObject).goInto(_element);
    }

    //Fields
    internal HierarchicalGridUIObject _hierarchicalGridUIObject = null;
    internal HierarchicalUIElementObject _element = null;
}
