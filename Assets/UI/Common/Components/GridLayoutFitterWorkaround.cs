using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class GridLayoutFitterWorkaround : UIBehaviour
{
    //Methods
    //-API
    public void setColumnsNum(int inValue) {
        if (0 == inValue) return;
        if (_columnsNum == inValue) return;

        _columnsNum = inValue;
        updateLayoutFromSettings();
    }

    public void setRowsNum(int inValue) {
        if (0 == inValue) return;
        if (_rowsNum == inValue) return;

        _rowsNum = inValue;
        updateLayoutFromSettings();
    }

    private void set(int inColumnsNum, int inRowsNum) {
        if (0 == inColumnsNum || 0 == inRowsNum) return;
#       if !UNITY_EDITOR
        if (_columnsNum == inColumnsNum && _rowsNum == inRowsNum) return;
#       endif

        _columnsNum = inColumnsNum;
        _rowsNum = inRowsNum;
        updateLayoutFromSettings();
    }

    //-Implementation
    private new void Awake() {
        base.Awake();

        _layout = XUtils.getComponent<GridLayoutGroup>(
            this, XUtils.AccessPolicy.ShouldExist
        );

        _rectTransform = XUtils.getComponent<RectTransform>(
            this, XUtils.AccessPolicy.ShouldExist
        );

        set(_columnsNum, _rowsNum);
    }

    protected override void OnRectTransformDimensionsChange() {
        updateLayoutFromSettings();
    }

    private void updateLayoutFromSettings() {
        if (!_layout) return;

        XUtils.check(0 != _columnsNum);
        _layout.cellSize = new Vector2(
            _rectTransform.rect.width / _columnsNum,
            _rectTransform.rect.height / _rowsNum
        );
    }

    //-Editor features
#   if UNITY_EDITOR
    void Update() {
        if (Application.IsPlaying(gameObject)) return;
        set(_columnsNum, _rowsNum);
    }
#   endif

    //Fields
    private RectTransform _rectTransform = null;
    private GridLayoutGroup _layout = null;

    [SerializeField] private int _columnsNum = 3;
    [SerializeField] private int _rowsNum = 3;
}
