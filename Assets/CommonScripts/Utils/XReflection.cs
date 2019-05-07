using UnityEngine;

public class XReflection
{
    public static System.Reflection.FieldInfo getField<T_ObjectType>(
        string inFieldName, bool inDoCheck = true)
    {
        System.Type theType = typeof(T_ObjectType);
        System.Reflection.FieldInfo theFieldInfo = theType.GetField(inFieldName,
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
        );
        if (inDoCheck) {
            XUtils.check(null != theFieldInfo, "Cannot find provided field");
        }

        return theFieldInfo;
    }

    static public void setFieldValue<T_ObjectType, T_FieldType>(T_ObjectType inObject,
        string inFieldName, T_FieldType inFieldValue)
            where T_ObjectType : class
    {
        XUtils.check(null != inObject);
        getField<T_ObjectType>(inFieldName).SetValue(inObject, inFieldValue);
    }

#if false
    // --- ANIMATIONS ---

    abstract class FieldAnimation {

        public abstract void changeForTargetValue(object inTargetValue, float inSpeed);
        public abstract void
    }

    class FieldAnimation_Vector3 : FieldAnimation
    {
        public AnimatedField_Vector3(System.Reflection.FieldInfo inField) {
            XUtils.check(inField.GetType() == typeof(Vector3));

            _object = null;
            _field = inField;
        }

        public void setObject(object inObject) {
            _object = inObject;
        }

        internal object _object;
        internal System.Reflection.FieldInfo _field;
    }


    public struct OneFieldAnimationSettings<T_ObjectType> {
        public OneFieldAnimationSettings(
            string inMainFieldName, string[] inAttachedFieldsName)
        {
            mainField = getField<T_ObjectType>(inMainFieldName);

            attachedFields = new System.Reflection.FieldInfo[inAttachedFieldsName.Length];
            for (int theIndex = 0; theIndex < inAttachedFieldsName.Length; ++theIndex) {
                attachedFields[theIndex] = getField<T_ObjectType>(inAttachedFieldsName[theIndex]);
            }
        }

        //For animation system only
        internal System.Reflection.FieldInfo mainField;
        internal System.Reflection.FieldInfo[] attachedFields;
    }

    static public void changeObjectForTargetObject<T_ObjectType, T_FieldType>(
        T_ObjectType inObject, T_ObjectType inTargetObject,
        ref OneFieldAnimationSettings<T_ObjectType> inAnimationSettings, float inSpeed)
    {
        NumbericValue theValue;

        theValue.init();

        mainField.set

        //inAnimation.mainField.GetValue();
        //inObject.
    }

#endif
}
