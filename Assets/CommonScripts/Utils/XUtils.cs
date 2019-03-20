using UnityEngine;
using System.Collections.Generic;

public class XUtils : MonoBehaviour
{
    public static bool arrayContains<T_Type>(T_Type[] inArray, T_Type inValue)
    {
        return -1 != System.Array.IndexOf(inArray, inValue);
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
}
