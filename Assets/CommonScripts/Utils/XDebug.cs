using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XDebug : MonoBehaviour
{

    // --------------------------- drawCross -----------------------------------

    void drawCross(Vector2 inPosition,
        Color inColor, float inDuration, bool inDepthTest)
    {
        const float theCrossSize = 0.3f;

        Debug.DrawLine(
            new Vector2(inPosition.x - theCrossSize, inPosition.y),
            new Vector2(inPosition.x + theCrossSize, inPosition.y),
            inColor, inDuration, inDepthTest
        );

        Debug.DrawLine(
            new Vector2(inPosition.x, inPosition.y - theCrossSize),
            new Vector2(inPosition.x, inPosition.y + theCrossSize),
            inColor, inDuration, inDepthTest
        );
    }

    // ------------------------- drawRectangle ---------------------------------

    public static void drawRectangle(
        Vector2 inPosition, Vector2 inSize, float inRotation,
        Color inColor, float inDuration, bool inDepthTest)
    {
        Vector2 theXSideVector = XMath.rotate(new Vector2(inSize.x, 0.0f), inRotation);
        Vector2 theYSideVector = XMath.rotate(new Vector2(0.0f, inSize.y), inRotation);

        Vector2 theRectCornerPosition = inPosition +
            XMath.rotate(new Vector2(-inSize.x/2, -inSize.y/2), inRotation);

        Vector2 theLastRectCornerPosition = theRectCornerPosition;
        theRectCornerPosition += theXSideVector;
        Debug.DrawLine(theLastRectCornerPosition, theRectCornerPosition,
            inColor, inDuration, inDepthTest
        );

        theLastRectCornerPosition = theRectCornerPosition;
        theRectCornerPosition += theYSideVector;
        Debug.DrawLine(theLastRectCornerPosition, theRectCornerPosition,
            inColor, inDuration, inDepthTest
        );

        theLastRectCornerPosition = theRectCornerPosition;
        theRectCornerPosition -= theXSideVector;
        Debug.DrawLine(theLastRectCornerPosition, theRectCornerPosition,
            inColor, inDuration, inDepthTest
        );

        theLastRectCornerPosition = theRectCornerPosition;
        theRectCornerPosition -= theYSideVector;
        Debug.DrawLine(theLastRectCornerPosition, theRectCornerPosition,
            inColor, inDuration, inDepthTest
        );
    }

    public static void drawRectangle(
        Vector2 inPosition, Vector2 inSize, float inRotation,
        Color inColor, float inDuration)
    {
        XDebug.drawRectangle(inPosition, inSize, inRotation,
            inColor, inDuration, true
        );
    }

    public static void drawRectangle(
        Vector2 inPosition, Vector2 inSize, float inRotation,
        Color inColor)
    {
        XDebug.drawRectangle(inPosition, inSize, inRotation,
            inColor, 0.0f
        );
    }

    public static void drawRectangle(
        Vector2 inPosition, Vector2 inSize, float inRotation)
    {
        XDebug.drawRectangle(inPosition, inSize, inRotation,
            Color.white
        );
    }
}
