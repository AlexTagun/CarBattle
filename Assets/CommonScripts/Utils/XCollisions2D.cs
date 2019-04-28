using UnityEngine;

public class XCollisions2D
{
    //------------------------- Raycast ------------------------------

    public static RaycastHit2D raycast(
        Vector2 inOrigin, Vector2 inDiraction, float inDistance,
        System.Func<Collider2D, bool> inContinueLambda)
    {
        XUtils.check(null != inContinueLambda);

        int theActualResultsCount = Physics2D.RaycastNonAlloc(
            inOrigin, inDiraction, __hitResults, inDistance
        );

        XUtils.sort(__hitResults, 0, theActualResultsCount,
            (RaycastHit2D inHitA, RaycastHit2D inHitB)=>
        {
            return XUtils.compare(inHitA.distance, inHitB.distance);
        });

        for (int theIndex = 0; theIndex < theActualResultsCount; ++theIndex) {
            Collider2D theCollider = __hitResults[theIndex].collider;
            if (inContinueLambda(theCollider)) continue;
            return __hitResults[theIndex];
        }

        return new RaycastHit2D();
    }

    //----------------------------- Linecast -------------------------------------

    public static RaycastHit2D linecast(
        Vector2 inStart, Vector2 inEnd,
        System.Func<Collider2D, bool> inContinueLambda)
    {
        XUtils.check(null != inContinueLambda);

        int theActualResultsCount = Physics2D.LinecastNonAlloc(
            inStart, inEnd, __hitResults
        );
        XUtils.sort(__hitResults, 0, theActualResultsCount,
            (RaycastHit2D inHitA, RaycastHit2D inHitB)=>
        {
            return XUtils.compare(inHitA.distance, inHitB.distance);
        });

        for (int theIndex = 0; theIndex < theActualResultsCount; ++theIndex) {
            Collider2D theCollider = __hitResults[theIndex].collider;
            if (inContinueLambda(theCollider)) continue;
            return __hitResults[theIndex];
        }

        return new RaycastHit2D();
    }

    static private RaycastHit2D[] __hitResults = new RaycastHit2D[50];
}
