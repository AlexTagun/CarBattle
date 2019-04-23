public struct ElementMover<T_ElementType>
    where T_ElementType : UnityEngine.Object
{
    public T_ElementType getMovingElement() {
        checkChanges();
        return _arrayFrom[_indexToMove];
    }

    //TODO: Make this API visibe only for array (currently is public for all classes in assembly)
    internal ElementMover(FastArray<T_ElementType> inArrayFrom, int inIndexToMove, bool inUseSwapRemove) {
        _arrayFrom = inArrayFrom;
        _indexToMove = inIndexToMove;
        _useSwapRemove = inUseSwapRemove;
        _validation_changesTracker = inArrayFrom._validation_changesTracker;
    }

    internal T_ElementType extract() {
        checkChanges();

        T_ElementType theElement = getMovingElement();
        _arrayFrom.removeElementAt(_indexToMove, _useSwapRemove);
        return theElement;
    }

    private void checkChanges() {
        XUtils.check(_arrayFrom._validation_changesTracker.Equals(_validation_changesTracker));
    }

    private FastArray<T_ElementType> _arrayFrom;
    private int _indexToMove;
    bool _useSwapRemove;
    private XUtils.ChangesTracker _validation_changesTracker;
}
