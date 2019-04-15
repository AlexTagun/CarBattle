using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Make debug code more organized!

public class FastArray<T_ElementType> : IEnumerable<T_ElementType>
{
    public delegate bool ElementPredicate(T_ElementType inValue);

    //Methods
    //-Construction
    public FastArray(int inInitialCapacity = kCapacityIncrement) {
        reallocate(inInitialCapacity);
    }

    //-Accessors
    public int getSize() { return _size; }
    public bool isEmpty() { return 0 == getSize(); }

    public int getCapacity() { return (null != _elements) ? _elements.Length : 0; }

    //-Elements accessors
    public T_ElementType this[int inKey] {
        get {
            validateIndex(inKey);
            return _elements[inKey];
        }
        set {
            validateIndex(inKey);
            _elements[inKey] = value;
            _validation_changesTracker.increaseChanges();
        }
    }

    public Optional<int> indexOf(T_ElementType inElement) {
        int theIndex = System.Array.IndexOf(_elements, inElement);
        return new Optional<int>(theIndex, theIndex >= 0);
    }

    public Optional<T_ElementType> getFirstElement() {
        T_ElementType theValue = !isEmpty() ? _elements[0] : default(T_ElementType);
        return new Optional<T_ElementType>(theValue, !isEmpty());
    }

    public Optional<T_ElementType> getLastElement() {
        int theLastIndex = getSize() - 1;
        T_ElementType theValue = !isEmpty() ? _elements[theLastIndex] : default(T_ElementType);
        return new Optional<T_ElementType>(theValue, !isEmpty());
    }

    //-Adding
    public void add(T_ElementType inElement) {
        int theSize = getSize();
        int theCapacity = getCapacity();
        if (theSize == theCapacity) {
            reallocate(theCapacity + kCapacityIncrement);
        }

        _elements[theSize] = inElement;
        ++_size;

        _validation_changesTracker.increaseChanges();
    }

    public void placeMovingElement(ElementMover<T_ElementType> inMover) {
        T_ElementType theElement = inMover.extract();
        add(theElement);
    }

    public void addAll(FastArray<T_ElementType> inOtherArray) {
        foreach (T_ElementType theElementToAdd in inOtherArray) {
            add(theElementToAdd);
        }
    }

    public void placeMovingRange(RangeMover<T_ElementType> inMover) {
        foreach(T_ElementType theElementToMove in inMover.extractAll()) {
            add(theElementToMove);
        }
    }

    //-Removing
    public void removeElementAt(int inIndex, bool inUseSwapRemove = true) {
        validateIndex(inIndex);

        int theLastIndex = getSize() - 1;
        if (inUseSwapRemove) {
            XUtils.swap(ref _elements[inIndex], ref _elements[theLastIndex]);
        } else {
            for (int theIndex = inIndex; theIndex < theLastIndex; ++theIndex) {
                _elements[theIndex] = _elements[theIndex + 1];
            }
        }
        --_size;

        _validation_changesTracker.increaseChanges();
    }

    public void removeElement(T_ElementType inElement) {
        removeElementAt(indexOf(inElement).getValue());
    }

    public ElementMover<T_ElementType> startMovingOfElementAt(
        int inElementIndex, bool inUseSwapRemove = true)
    {
        validateIndex(inElementIndex);
        return new ElementMover<T_ElementType>(
            this, inElementIndex, inUseSwapRemove
        );
    }

    public ElementMover<T_ElementType> startMovingOfElement(
        T_ElementType inElement, bool inUseSwapRemove = true)
    {
        return startMovingOfElementAt(
            indexOf(inElement).getValue(), inUseSwapRemove
        );
    }

    public RangeMover<T_ElementType> startMovingAll(
        bool inFreeMemoryAfterMove = false)
    {
        return new RangeMover<T_ElementType>(
            this, 0, getSize(), inFreeMemoryAfterMove
        );
    }

    public void clear(bool inFreeMemory = false) {
        _size = 0;
        if (inFreeMemory) {
            _elements = null;
        }
    }

    //-Iteration
    public IEnumerator<T_ElementType> GetEnumerator() {
        int theSize = getSize();
        for (int theIndex = 0; theIndex < theSize; ++theIndex) {
            yield return _elements[theIndex];
        }
    }

    public void iterateWithRemove(ElementPredicate inPredicate, bool inUseSwapRemove = true) {
        int theIndex = 0;
        while (theIndex < getSize()) {
            bool theDoRemove = inPredicate.Invoke(_elements[theIndex]);
            if (theDoRemove) {
                removeElementAt(theIndex, inUseSwapRemove);
            } else {
                ++theIndex;
            }
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

    private void validateIndex(int inIndex) {
        XUtils.check(XUtils.isValueInRange(inIndex, 0, _size - 1));
        //Debug.Log("Trying to remove element that is out of array: " + inIndex);
    }

    private int _size = 0;
    private T_ElementType[] _elements = null;

    private const int kCapacityIncrement = 4;

    //TODO: Make this API visibe only for Mover (currently is public for all classes in assembly)
    internal XUtils.ChangesTracker _validation_changesTracker;
}
