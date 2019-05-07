using UnityEngine;
using System.Collections.Generic;

public class XUtils : MonoBehaviour
{
    // --- ALGORITHMS --- 
    // - - Basics - -

    public static void swap<T_Type>(ref T_Type inValueRefA, ref T_Type inValueRefB) {
        T_Type theSwapValue = inValueRefA;
        inValueRefA = inValueRefB;
        inValueRefB = theSwapValue;
    }

    public static bool isValueInRange(int inValue, int inMin, int inMax) {
        check(inMin <= inMax);
        return inValue >= inMin && inValue <= inMax;
    }

    public enum Comparation {
        ALessThenB,
        AEqualsB,
        AMoreThenB
    }

    public static Comparation compare<T_Type>(T_Type inA, T_Type inB)
        where T_Type : System.IComparable<T_Type>
    {
        int theComparationResult = inA.CompareTo(inB);
        return 0 == theComparationResult ? Comparation.AEqualsB :
            (theComparationResult > 0 ?
                Comparation.AMoreThenB : Comparation.ALessThenB);
    }

    // - - Array - -

    public static bool arrayContains<T_Type>(T_Type[] inArray, T_Type inValue) {
        return -1 != System.Array.IndexOf(inArray, inValue);
    }

    public static void sort<T_Type>(
        T_Type[] inoutArray, int inStartIndex, int inLength,
        System.Func<T_Type, T_Type, Comparation> inLambda)
    {
        XUtils.check(null != inLambda);
        System.Array.Sort(inoutArray, inStartIndex, inLength,
            LambdaComparer<T_Type>.getForLambda(inLambda)
        );
    }

    // --- VALIDATION --- 

    public static bool isValid(Object inObject) {
        return (inObject && null != inObject &&
            !inObject.name.Equals(kDestroyedName)
        );
    }

    public static bool isValid(Component inComponent) {
        return isValid((Object)inComponent) && isValid(inComponent.gameObject);
    }

    public static bool isValid(object inObject) {
        return null != inObject;
    }

    public static void check(
        bool inCondition, string inErrorMessage = "<No message>")
    {
        if (inCondition) return;
        throw new System.Exception(inErrorMessage);
    }

    public static void check(
        Object inUnityObject, string inErrorMessage = "<No message>")
    {
        if (isValid(inUnityObject)) return;
        throw new System.Exception(inErrorMessage);
    }

    public static void check(
        object inObject, string inErrorMessage = "<No message>")
    {
        if (isValid(inObject)) return;
        throw new System.Exception(inErrorMessage);
    }


    public static T_Type verify<T_Type>(
        T_Type inReference, string inErrorMessage = "<No message>")
            where T_Type : class
    {
        if (null != inReference) return inReference;
        throw new System.Exception(inErrorMessage);
    }

    public struct ChangesTracker {
        public void increaseChanges() { ++_changesCounter; }
        public bool Equals(ChangesTracker inOtherTracker) {
            return _changesCounter == inOtherTracker._changesCounter;
        }

        private int _changesCounter;
    }

    // --- COMPONENTS ACCESS --- 

    public enum AccessPolicy {
        JustFind,
        ShouldExist,
        CreateIfNo,
        ShouldBeCreated
    }

    public static T_Type getComponent<T_Type>(GameObject inGameObject,
        AccessPolicy inComponentAccessPolicy = AccessPolicy.JustFind)
        where T_Type : Component
    {
        T_Type theComponent = inGameObject.GetComponent<T_Type>();
        if (theComponent) {
            check(AccessPolicy.ShouldBeCreated != inComponentAccessPolicy);
        } else {
            switch(inComponentAccessPolicy) {
                case AccessPolicy.CreateIfNo:
                case AccessPolicy.ShouldBeCreated:
                    theComponent = inGameObject.AddComponent<T_Type>();
                    break;

                case AccessPolicy.ShouldExist:
                    check(false);
                    break;
            }
        }

        return theComponent;
    }

    public static T_Type getComponent<T_Type>(Component inGameObjectComponent,
        AccessPolicy inComponentAccessPolicy = AccessPolicy.JustFind)
        where T_Type : Component
    {
        return getComponent<T_Type>(inGameObjectComponent.gameObject, inComponentAccessPolicy);
    }

    // --- OBJECTS LIFECYCLE

    public static T_Type createObject<T_Type>(T_Type inObject)
        where T_Type : Component
    {
        return getComponent<T_Type>(
            Instantiate(inObject.gameObject), AccessPolicy.ShouldExist
        );
    }

    public static T_Type createObject<T_Type>(T_Type inObject, Transform inTransform)
        where T_Type : Component
    {
        T_Type theObject = createObject<T_Type>(inObject);
        XMath.assignTransform(theObject.transform, inTransform);
        return theObject;
    }

    public static void Destroy<T_Type>(T_Type inObject)
        where T_Type : Object
    {
        inObject.name = kDestroyedName;
        Object.Destroy(inObject);
    }

// --- OBJECT RELATIONS ---

public static bool isObjectHasComponent<T_Type>(GameObject inObject, T_Type inComponent)
        where T_Type : Component
    {
        check(inObject);
        check(inComponent);

        return (-1 != System.Array.IndexOf(inObject.GetComponents<T_Type>(), inComponent));
    }

    public static bool isComponentRelatedToObject<T_Type>(GameObject inObject, T_Type inComponent)
        where T_Type : Component
    {
        check(inObject);
        check(inComponent);

        if (isObjectHasComponent(inObject, inComponent)) return true;
        return isObjectRelatedToObject(inObject, inComponent.gameObject);
    }

    private static bool isObjectRelatedToObject(GameObject inObject, GameObject inTestingObject) {
        check(inObject);
        check(inTestingObject);

        if (inObject == inTestingObject) return true;

        Transform theParentTransform = inTestingObject.transform.parent;
        if (!theParentTransform) return false;
        return isObjectRelatedToObject(inObject, theParentTransform.gameObject);
    }

    // --- MISC ---

    public static Vector2 getMouseWorldPosition() {
        if (!Camera.main) return new Vector2();
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // --- UTILITY TYPES ---

    private class LambdaComparer<T_Type> : IComparer<T_Type>
    {
        public int Compare(T_Type inA, T_Type inB) {
            XUtils.check(null != lambda);
            switch(lambda(inA, inB)) {
                case Comparation.ALessThenB: return -1;
                case Comparation.AEqualsB:   return 0;
                case Comparation.AMoreThenB: return 1;
            }

            XUtils.check(false);
            return 0;
        }

        public static LambdaComparer<T_Type> getForLambda(
            System.Func<T_Type, T_Type, Comparation> inLambda)
        {
            __lambdaComparer.lambda = inLambda;
            return __lambdaComparer;
        }

        public System.Func<T_Type, T_Type, Comparation> lambda;
        private static LambdaComparer<T_Type> __lambdaComparer =
            new LambdaComparer<T_Type>();
    }

    private static string kDestroyedName = "$";
}
