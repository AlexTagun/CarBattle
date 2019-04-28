using UnityEngine;
using System.Collections.Generic;

public class XUtils : MonoBehaviour
{
    public static bool arrayContains<T_Type>(T_Type[] inArray, T_Type inValue) {
        return -1 != System.Array.IndexOf(inArray, inValue);
    }

    public enum Comparation {
        ALessThenB,
        AEqualsB,
        AMoreThenB
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

    public static Comparation compare<T_Type>(T_Type inA, T_Type inB)
        where T_Type : System.IComparable<T_Type>
    {
        int theComparationResult = inA.CompareTo(inB);
        return 0 == theComparationResult ? Comparation.AEqualsB :
            (theComparationResult > 0 ?
                Comparation.AMoreThenB : Comparation.ALessThenB);
    }

    public struct ArraysTransformPair<T_TypeFrom, T_TypeTo>
    {
        public ArraysTransformPair(T_TypeFrom[] inFrom, System.Action<T_TypeTo[]> inToAssigning) {
            from = inFrom;
            toAssigning = inToAssigning;
        }

        public T_TypeFrom[] from;
        public System.Action<T_TypeTo[]> toAssigning;
    }

    public struct PushableArrayBuilder<T_Type>
    {
        public PushableArrayBuilder(int inSize) {
            array = new T_Type[inSize];
            pushingIndex = 0;
        }

        public void push(T_Type inNewValue) {
            array[pushingIndex++] = inNewValue;
        }

        public T_Type[] getArray() { return array; }

        T_Type[] array;
        int pushingIndex;
    };

    public static void transformArray<T_TypeFrom, T_TypeTo>(
        ArraysTransformPair<T_TypeFrom, T_TypeTo> inMainTransform,
        System.Func<T_TypeFrom, T_TypeTo> inMappingLogic,
        ArraysTransformPair<T_TypeFrom, T_TypeTo>[] inRelatedTransforms = null
    ) where T_TypeFrom : class   where T_TypeTo : class
    {
        //Initialize result builders
        var theMainResultBuilder = new PushableArrayBuilder<T_TypeTo>(
            inMainTransform.from.Length
        );

        var theRelatedResultBuilders = new PushableArrayBuilder<T_TypeTo>[
            inRelatedTransforms.Length
        ];
        for (int theIndex = 0; theIndex < inRelatedTransforms.Length; ++theIndex) {
            theRelatedResultBuilders[theIndex] = new PushableArrayBuilder<T_TypeTo>(
                inRelatedTransforms[theIndex].from.Length
            );
        }

        foreach (T_TypeFrom theValueFrom in inMainTransform.from) {
            T_TypeTo theValueTo = inMappingLogic(theValueFrom);
            theMainResultBuilder.push(theValueTo);

            for (int theIndex = 0; theIndex < inRelatedTransforms.Length; ++theIndex) {
                if (arrayContains(inRelatedTransforms[theIndex].from, theValueFrom)) {
                    theRelatedResultBuilders[theIndex].push(theValueTo);
                }
            }
        }

        inMainTransform.toAssigning(theMainResultBuilder.getArray());
        for (int theIndex = 0; theIndex < inRelatedTransforms.Length; ++theIndex) {
            inRelatedTransforms[theIndex].toAssigning(theRelatedResultBuilders[theIndex].getArray());
        }
    }

    public static void swap<T_Type>(ref T_Type inValueRefA, ref T_Type inValueRefB) {
        T_Type theSwapValue = inValueRefA;
        inValueRefA = inValueRefB;
        inValueRefB = theSwapValue;
    }

    public static bool isValid(GameObject inObject) {
        return !!inObject;
    }

    public static bool isValid(Component inComponent) {
        return !!inComponent && isValid(inComponent.gameObject);
    }

    public static bool isValueInRange(int inValue, int inMin, int inMax) {
        check(inMin <= inMax);
        return inValue >= inMin && inValue <= inMax;
    }

    public struct ChangesTracker {
        public void increaseChanges() { ++_changesCounter; }
        public bool Equals(ChangesTracker inOtherTracker) {
            return _changesCounter == inOtherTracker._changesCounter;
        }

        private int _changesCounter;
    }

    public static void check(
        bool inCondition, string inErrorMessage = "<No message>")
    {
        if (inCondition) return;
        throw new System.Exception(inErrorMessage);
    }
 
    public static T_Type verify<T_Type>(T_Type inReference, string inErrorMessage)
        where T_Type : Object
    {
        if (null != inReference) return inReference;
        throw new System.Exception(inErrorMessage);
    }
    public static T_Type verify<T_Type>(T_Type inReference) where T_Type : Object {
        return verify<T_Type>(inReference, "<No message>");
    }

    public static Vector2 getMouseWorldPosition() {
        if (!Camera.main) return new Vector2();
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

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

    // - - - - - - - - - - - - - - - - - - - - - - - - -

    class LambdaComparer<T_Type> : IComparer<T_Type>
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
}