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

        public Vector2 position;
        public float rotation;
    }

     public Point getTransformAndDestroy() {
        Point theResult = new Point(gameObject.transform);
        Destroy(this);
        return theResult;
    }
}
