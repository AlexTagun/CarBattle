using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TransformHolderLogic : MonoBehaviour
{
    public struct Point {
        public Point(Transform inTransform) {
            position = inTransform.localPosition;
            rotation = inTransform.localRotation.eulerAngles.z;
        }

        public void set(Point inPoint) {
            position = inPoint.position;
            rotation = inPoint.rotation;
        }

        public Vector2 position;
        public float rotation;
    }

    public Point getTransform(bool inDestroyAfterGet = true) {
        Point theResult = new Point(gameObject.transform);
        if (inDestroyAfterGet) destroy();
        return theResult;
    }

    public void destroy() {
        Destroy(gameObject);
    }
}
