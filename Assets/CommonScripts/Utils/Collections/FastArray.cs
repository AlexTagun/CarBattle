using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Make debug code more organized!

public class FastArray<T_ElementType> : IEnumerable<T_ElementType>
    where T_ElementType : UnityEngine.Object
{
    public delegate void ElementProcessingDelegate(T_ElementType inValue);
    public delegate bool ElementPredicate(T_ElementType inValue);

    //Methods
    //-Construction
    public FastArray(int inInitialCapacity = kCapacityIncrement) {
        reallocate(inInitialCapacity);
    }

    //-Accessors
    public int getSize() { return _size; }
    public int getLastIndex() { return getSize() - 1; }
    public bool isEmpty() { return 0 == getSize(); }

    public int getCapacity() { return (null != _elements) ? _elements.Length : 0; }

    public void collectElements(T_ElementType[] outElements) {
        int theLastIndex = Mathf.Min(outElements.Length - 1, getLastIndex());
        for (int theIndex = 0; theIndex <= theLastIndex; ++theIndex) {
            outElements[theIndex] = _elements[theIndex];
        }
    }

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
        int theIndex = 0;
        foreach (T_ElementType theElement in this) {
            if (theElement == inElement) break;
            ++theIndex;
        }
        return new Optional<int>(theIndex, isValidIndex(theIndex));
    }

    public bool contains(T_ElementType inElement) {
        return indexOf(inElement).isSet();
    }

    public Optional<T_ElementType> getFirstElement() {
        T_ElementType theValue = !isEmpty() ? _elements[0] : default(T_ElementType);
        return new Optional<T_ElementType>(theValue, !isEmpty());
    }

    public Optional<T_ElementType> getLastElement() {
        T_ElementType theValue = !isEmpty() ?
            _elements[getLastIndex()] : default(T_ElementType);
        return new Optional<T_ElementType>(theValue, !isEmpty());
    }

    public T_ElementType getLastElementChecked() {
        validateIndex(getLastIndex());
        return _elements[getLastIndex()];
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
    public void removeElementAt(int inIndex, bool inUseSwapRemove = false) {
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
        int inElementIndex, bool inUseSwapRemove = false)
    {
        validateIndex(inElementIndex);
        return new ElementMover<T_ElementType>(
            this, inElementIndex, inUseSwapRemove
        );
    }

    public ElementMover<T_ElementType> startMovingOfElement(
        T_ElementType inElement, bool inUseSwapRemove = false)
    {
        return startMovingOfElementAt(
            indexOf(inElement).getValue(), inUseSwapRemove
        );
    }

    public void removeElementsInRange(int inBeginIndex, int inEndIndex,
        ElementProcessingDelegate inProcessingDelegate)
    {
        validateIndex(inBeginIndex);
        validateIndex(inEndIndex);
        XUtils.check(inBeginIndex <= inEndIndex);

        int theLastIndex = getLastIndex();
        int theElementsNumToRemove = inEndIndex - inBeginIndex;
        int theRemovingElementsOffset = theElementsNumToRemove + 1;

        if (null != inProcessingDelegate) {
            for (int theIndex = inBeginIndex; theIndex < inEndIndex; ++theIndex) {
                inProcessingDelegate.Invoke(_elements[theIndex]);
            }
        }

        if (inEndIndex != theLastIndex) {
            int theLastMovingElementIndex = theLastIndex - theRemovingElementsOffset;
            for (int theIndex = inBeginIndex; theIndex <= theLastMovingElementIndex; ++theIndex) {
                _elements[theIndex] = _elements[theIndex + theRemovingElementsOffset];
            }
        }

        _size -= theElementsNumToRemove;
    }

    public void removeElementsUpToEnd(int inBeginIndex) {
        removeElementsInRange(inBeginIndex, getSize() - 1, null);
    }

    public void removeElementsUpToEnd(int inBeginIndex,
        ElementProcessingDelegate inProcessingDelegate)
    {
        removeElementsInRange(inBeginIndex, getSize() - 1, inProcessingDelegate);
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
        //NB: We cannot use foreach by elements here,
        // we iterate over filled elements only!
        int theSize = getSize();
        for (int theIndex = 0; theIndex < theSize; ++theIndex) {
            yield return _elements[theIndex];
        }
    }

    public void iterateWithRemove(ElementPredicate inPredicate, bool inUseSwapRemove = false) {
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

    //-Find
    public Optional<int> findIndex(ElementPredicate inPredicate) {
        if (null == inPredicate) return new Optional<int>();
        XUtils.check(null != inPredicate);

        int theIndex = 0;
        foreach (T_ElementType theElement in this) {
            if (inPredicate.Invoke(theElement)) {
                return new Optional<int>(theIndex);
            }
            ++theIndex;
        }

        return new Optional<int>();
    }

    public Optional<T_ElementType> findElement(ElementPredicate inPredicate) {
        Optional<int> theIndex = findIndex(inPredicate);
        return !theIndex.isSet() ? new Optional<T_ElementType>() :
            new Optional<T_ElementType>(this[theIndex.getValue()]);
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
        XUtils.check(isValidIndex(inIndex));
        //Debug.Log("Trying to remove element that is out of array: " + inIndex);
    }

    private bool isValidIndex(int inIndex) {
        return isEmpty() ? false : XUtils.isValueInRange(inIndex, 0, getLastIndex());
    }

    private int _size = 0;
    private T_ElementType[] _elements = null;

    private const int kCapacityIncrement = 4;

    //TODO: Make this API visibe only for Mover (currently is public for all classes in assembly)
    internal XUtils.ChangesTracker _validation_changesTracker;
}
