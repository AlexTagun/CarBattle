using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XDebug : MonoBehaviour
{
    public struct DebugDrawSettings {

        public DebugDrawSettings(Color inColor,
            float inDuration = 0.0f,
            bool inDoDepthTest = false)
        {
            color = inColor;
            duration = inDuration;
            doDepthTest = inDoDepthTest;
        }

        public Color color;
        public float duration;
        public bool doDepthTest;

        public static DebugDrawSettings defaultValue =
                new DebugDrawSettings(Color.white, 0.0f, false);
    }

    // --------------------------- drawCross -----------------------------------

    public static void drawCross(Vector2 inPosition, DebugDrawSettings inDrawSettings) {
        const float theCrossSize = 0.3f;

        Debug.DrawLine(
            new Vector2(inPosition.x - theCrossSize, inPosition.y),
            new Vector2(inPosition.x + theCrossSize, inPosition.y),
            inDrawSettings.color, inDrawSettings.duration, inDrawSettings.doDepthTest
        );

        Debug.DrawLine(
            new Vector2(inPosition.x, inPosition.y - theCrossSize),
            new Vector2(inPosition.x, inPosition.y + theCrossSize),
            inDrawSettings.color, inDrawSettings.duration, inDrawSettings.doDepthTest
        );
    }

    public static void drawCross(Vector2 inPosition) {
        drawCross(inPosition, DebugDrawSettings.defaultValue);
    }

    // ------------------------- drawRectangle ---------------------------------

    public static void drawRectangle(Vector2 inPosition, Vector2 inSize, float inRotation,
        DebugDrawSettings inDrawSettings)
    {
        Vector2 theXSideVector = XMath.rotate(new Vector2(inSize.x, 0.0f), inRotation);
        Vector2 theYSideVector = XMath.rotate(new Vector2(0.0f, inSize.y), inRotation);

        Vector2 theRectCornerPosition = inPosition +
            XMath.rotate(new Vector2(-inSize.x/2, -inSize.y/2), inRotation);

        Vector2 theLastRectCornerPosition = theRectCornerPosition;
        theRectCornerPosition += theXSideVector;
        Debug.DrawLine(theLastRectCornerPosition, theRectCornerPosition,
            inDrawSettings.color, inDrawSettings.duration, inDrawSettings.doDepthTest
        );

        theLastRectCornerPosition = theRectCornerPosition;
        theRectCornerPosition += theYSideVector;
        Debug.DrawLine(theLastRectCornerPosition, theRectCornerPosition,
            inDrawSettings.color, inDrawSettings.duration, inDrawSettings.doDepthTest
        );

        theLastRectCornerPosition = theRectCornerPosition;
        theRectCornerPosition -= theXSideVector;
        Debug.DrawLine(theLastRectCornerPosition, theRectCornerPosition,
            inDrawSettings.color, inDrawSettings.duration, inDrawSettings.doDepthTest
        );

        theLastRectCornerPosition = theRectCornerPosition;
        theRectCornerPosition -= theYSideVector;
        Debug.DrawLine(theLastRectCornerPosition, theRectCornerPosition,
            inDrawSettings.color, inDrawSettings.duration, inDrawSettings.doDepthTest
        );
    }

    public static void drawCross(Vector2 inPosition, Vector2 inSize, float inRotation) {
        drawRectangle(inPosition, inSize, inRotation, DebugDrawSettings.defaultValue);
    }
}
