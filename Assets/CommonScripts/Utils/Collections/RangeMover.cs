using System.Collections;
using System.Collections.Generic;

public struct RangeMover<T_ElementType> : IEnumerable<T_ElementType>
    where T_ElementType : UnityEngine.Object
{
    //Methods
    //-API
    public IEnumerator<T_ElementType> GetEnumerator() {
        checkChanges();

        for (int theIndex = 0; theIndex < _elementsToMove; ++theIndex) {
            int theElementIndex = _indexToMoveFrom + theIndex;
            yield return _arrayFrom[theElementIndex];
        }
    }

    //-Implementation
    //TODO: Make this API visibe only for array (currently is public for all classes in assembly)
    internal RangeMover(FastArray<T_ElementType> inArrayFrom, int inIndexToMoveFrom, int inElementsToMove, bool inUseSwapRemove)
    {
        _arrayFrom = inArrayFrom;
        _indexToMoveFrom = inIndexToMoveFrom;
        _elementsToMove = inElementsToMove;
        _useSwapRemove = inUseSwapRemove;
        _validation_changesTracker = inArrayFrom._validation_changesTracker;
    }

    internal IEnumerable<T_ElementType> extractAll() {
        checkChanges();

        if (isMovingWholeArray()) {
            for (int theIndex = 0; theIndex < _elementsToMove; ++theIndex) {
                T_ElementType theElementToExtract = _arrayFrom[_indexToMoveFrom];
                _arrayFrom.removeElementAt(_indexToMoveFrom);
                yield return theElementToExtract;
            }
            _arrayFrom.clear(_useSwapRemove);
        } else {
            for (int theIndex = 0; theIndex < _elementsToMove; ++theIndex) {
                T_ElementType theElementToExtract = _arrayFrom[_indexToMoveFrom];
                _arrayFrom.removeElementAt(_indexToMoveFrom);
                yield return theElementToExtract;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() { yield return GetEnumerator(); }

    private void checkChanges() {
        XUtils.check(_validation_changesTracker.Equals(_arrayFrom._validation_changesTracker));
    }

    private bool isMovingWholeArray() {
        return (0 == _indexToMoveFrom && _elementsToMove == _arrayFrom.getSize());
    }

    private FastArray<T_ElementType> _arrayFrom;
    private int _indexToMoveFrom;
    private int _elementsToMove;
    bool _useSwapRemove;
    private XUtils.ChangesTracker _validation_changesTracker;
}
