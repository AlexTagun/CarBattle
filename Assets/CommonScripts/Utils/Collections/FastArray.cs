using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastArray<T_ElementType> : IEnumerable<T_ElementType>
{
    public delegate bool ElementPredicate(T_ElementType inValue);

    public FastArray() { }

    public void add(T_ElementType inElement) {
        int theSize = getSize();
        int theCapacity = getCapacity();
        if (theSize == theCapacity) {
            reallocate(theCapacity + kCapacityIncrement);
        }

        _elements[theSize] = inElement;
        ++_size;
    }

    public void iterateWithRemove(ElementPredicate inPredicate, bool inUseSwapRemove = true) {
        int theIndex = 0;
        while (theIndex < getSize()) {
            bool theDoRemove = inPredicate.Invoke(_elements[theIndex]);
            if (theDoRemove) {
                removeElement(theIndex, inUseSwapRemove);
            } else {
                ++theIndex;
            }
        }
    }

    public void removeElement(int inIndex, bool inUseSwapRemove = true) {
        //TODO: Add additional checks for index
        if (inIndex >= getSize()) {
            Debug.Log("Trying to remove element that is out of array: " + inIndex);
            return;
        }

        int theLastIndex = getSize() - 1;
        if (inUseSwapRemove) {
            XUtils.swap(ref _elements[inIndex], ref _elements[theLastIndex]);
        } else {
            for (int theIndex = inIndex; theIndex < theLastIndex; ++theIndex) {
                _elements[theIndex] = _elements[theIndex + 1];
            }
        }
        --_size;
    }

    public int getSize() { return _size; }
    public int getCapacity() { return (null != _elements) ? _elements.Length : 0; }

    public IEnumerator<T_ElementType> GetEnumerator() {
        foreach (T_ElementType theElement in _elements) {
            yield return theElement;
        }
    }

    //-Implementation
    void reallocate(int inNewSize) {
        T_ElementType[] theOldElements = _elements;
        _elements = new T_ElementType[inNewSize];
        int theSize = getSize();
        for (int theIndex = 0; theIndex < theSize; ++theIndex) {
            _elements[theIndex] = theOldElements[theIndex];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() { yield return GetEnumerator(); }

    private int _size = 0;
    private T_ElementType[] _elements = null;

    private const int kCapacityIncrement = 4;
}
