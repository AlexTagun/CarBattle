using System.Collections;
using System.Collections.Generic;

public struct Optional<T_ValueType>
{
    public Optional(T_ValueType inValue, bool inIsSet = true) {
        _value = inValue;
        _isSet = inIsSet;
    }

    public bool isSet() { return _isSet; }

    public T_ValueType getValue(bool inWithCheck = true) {
        if (!_isSet && inWithCheck) {
            new System.InvalidOperationException();
        }
        return _isSet ? _value : default(T_ValueType);
    }

    //public static implicit operator T_ValueType(Optional<T_ValueType> inOptional) {
    //    return inOptional.getValue();
    //}

    private bool _isSet;
    private T_ValueType _value;
}
