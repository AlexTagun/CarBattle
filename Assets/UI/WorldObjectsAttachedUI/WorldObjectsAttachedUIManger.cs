using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectsAttachedUIManger : MonoBehaviour
{
    private Canvas _canvas = null;
    public Camera _camera = null;

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

    //-Implementation
    void Start() {
        _canvas = gameObject.GetComponent<Canvas>();
    }

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
        //Vector2 theViewportPosition = ;
        //
        //Debug.Log(inWorldPosition);
        //Debug.Log(theViewportPosition.x + " : " + _camera.pixelWidth + " : " +
        //    theViewportPosition.y + " : " + _camera.pixelHeight
        //);

        return _camera.WorldToViewportPoint(inWorldPosition);
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


    //TODO: Replace with own-maded "Deque" or "FastArray" method based on array with swaps
    FastArray<UIAttach_ToWorldObjectAttachPoint> _uiAttaches_toWorldObjectAttachPoint =
        new FastArray<UIAttach_ToWorldObjectAttachPoint>();
}
