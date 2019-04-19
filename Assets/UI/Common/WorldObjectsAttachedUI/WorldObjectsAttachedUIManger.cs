using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectsAttachedUIManger : MonoBehaviour
{
    //Methods
    //-API
    public void attach(
        GameObject inUI, WorldObjectAttachPoint inAttachPoint, bool inDestroyUIWithObject = true)
    {
        var theNewAttach = new UIAttach_ToWorldObjectAttachPoint(
            inUI.GetComponent<RectTransform>(), inAttachPoint, inDestroyUIWithObject
        );
        _uiAttaches_toWorldObjectAttachPoint.add(theNewAttach);
    }

    public void attach(
        GameObject inUI, GameObject inGameObject, bool inDestroyUIWithObject = true)
    {
        var theAttachPoint = XUtils.verify(
            inGameObject.GetComponent<WorldObjectAttachPoint>()
        );
        attach(inUI, theAttachPoint, inDestroyUIWithObject);
    }

    //-Implementation
    void Update() {
        _uiAttaches_toWorldObjectAttachPoint.iterateWithRemove(
            (UIAttach_ToWorldObjectAttachPoint inAttach) =>
        {
            if (!inAttach.isValid()) {
                if (XUtils.isValid(inAttach.UITransform)) {
                    Destroy(inAttach.UITransform.gameObject);
                }
                return true;
            }

            updateToWorldObjectAttachPoint(ref inAttach);
            return false;
        });
    }

    void updateToWorldObjectAttachPoint(ref UIAttach_ToWorldObjectAttachPoint inAttach) {
        Vector2 theViewportNormalizedPosition = getViewportNormalizedPositionForWorldPosition(
            inAttach.attachPoint.getAttachPointWorld()
        );

        inAttach.UITransform.anchorMax = theViewportNormalizedPosition;
        inAttach.UITransform.anchorMin = theViewportNormalizedPosition;
    }


    private Vector2 getViewportNormalizedPositionForWorldPosition(Vector3 inWorldPosition) {
        if (!Camera.main) return new Vector2();
        return Camera.main.WorldToViewportPoint(inWorldPosition);
    }

    private struct UIAttach_ToWorldObjectAttachPoint
    {
        public UIAttach_ToWorldObjectAttachPoint(
            RectTransform inUITransform, WorldObjectAttachPoint inAttachPoint, bool inDestroyUIWithObject)
        {
            UITransform = inUITransform;
            attachPoint = inAttachPoint;
            destroyUIWithObject = inDestroyUIWithObject;
        }

        public bool isValid() {
            return XUtils.isValid(UITransform) && XUtils.isValid(attachPoint);
        }

        public RectTransform UITransform;
        public WorldObjectAttachPoint attachPoint;
        public bool destroyUIWithObject;
    }

    FastArray<UIAttach_ToWorldObjectAttachPoint> _uiAttaches_toWorldObjectAttachPoint =
        new FastArray<UIAttach_ToWorldObjectAttachPoint>();
}
