using UnityEngine;

namespace Values
{
    // ============================= LimitedFloat =================================

    public struct LimitedFloat {
        //Types
        [System.Serializable]
        public struct ZeroBasedState {
            public ZeroBasedState(float inMaximum, bool inSetValueAsMaximum = false)
                : this(inSetValueAsMaximum ? inMaximum : 0.0f, inMaximum) { }

            public ZeroBasedState(float inMaximum, float inValue) {
                Maximum = inMaximum;
                Value =inValue;
            }

            public float Maximum;
            public float Value;
        }

        [System.Serializable]
        public struct State {
            public State(float inMinimum, float inMaximum, float inValue = 0.0f) {
                Minimum = inMinimum;
                Maximum = inMaximum;
                Value = inValue;
            }

            public float Minimum, Maximum;
            public float Value;
        }

        public delegate void OnAchievedMinimum();
        public delegate void OnAchievedMaximum();

        //Methods
        //-Initialization
        public LimitedFloat(float inValue = 0.0f)
            : this(0.0f, 1.0f, inValue) { }
        public LimitedFloat(float inMinimum, float inMaximum, float inValue = 0.0f)
            : this(new State(inMinimum, inMaximum, inValue)) { }
        public LimitedFloat(ZeroBasedState inState)
            : this(new State(0.0f, inState.Maximum, inState.Value)) { }

        public LimitedFloat(State inState) {
            _state = inState;

            onAchievedMinimum = new OnAchievedMinimum(() => {});
            onAchievedMaximum = new OnAchievedMaximum(() => { });

            normalizeState();
        }

        public LimitedFloat(LimitedFloat inValue)
            : this(inValue._state) { }

        //-Accessors
        public float getMinimum() { return _state.Minimum; }
        public void setMinimum(float inMinimum) {
            _state.Minimum = inMinimum;
            normalizeState();
        }

        public float getMaximum() { return _state.Maximum; }
        public void setMaximum(float inMaximum) {
            _state.Maximum = inMaximum;
            normalizeState();
        }

        public void setLimits(float inMinimum, float inMaximum) {
            _state.Minimum = inMinimum;
            _state.Maximum = inMaximum;
            normalizeState();
        }
        public void set(float inMinimum, float inMaximum, float inValue) {
            _state.Minimum = inMinimum;
            _state.Maximum = inMaximum;
            _state.Value = inValue;
            normalizeState();
        }

        public float getRange() { return getMaximum() - getMinimum(); }

        public float getValue() { return _state.Value; }
        public float setValue(float inNewValue) {
            float theOldValue = _state.Value;
            float theNewValue = Mathf.Clamp(inNewValue, getMinimum(), getMaximum());
            _state.Value = theNewValue;

            if (theOldValue != theNewValue) {
                if (isValueMinimum()) {
                    onAchievedMinimum?.Invoke();
                }
                if (isValueMaximum()) {
                    onAchievedMaximum?.Invoke();
                }
            }

            return theNewValue;
        }
        public float changeValue(float inValueDelta) {
            float theOldValue = getValue();
            float theNewValue = setValue(theOldValue + inValueDelta);
            return theNewValue - theOldValue;
        }

        public float getValueFromMinimum() { return getValue() - getMinimum(); }
        public float getValuePercentFromMinimum() { return getValueFromMinimum()/getRange(); }

        public float getValueFromMaximum() { return getMaximum() - getValue(); }
        public float getValuePercenFromMaximum() { return getValueFromMaximum()/getRange(); }

        public bool isValueMaximum() { return getValue() == getMaximum(); }
        public bool isValueMinimum() { return getValue() == getMinimum(); }

        //-Events
        public event OnAchievedMinimum onAchievedMinimum;
        public event OnAchievedMaximum onAchievedMaximum;

        //-Implementation
        //--Utils
        private void normalizeState() {
            if (_state.Minimum > _state.Maximum) {
                XUtils.swap(ref _state.Minimum, ref _state.Maximum);
            }
            setValue(_state.Value);
        }

        //Fields
        private State _state;
    }

    // ======================== LimitedFloatAngle =============================

    public struct LimitedFloatAngle
    {
        //Types
        [System.Serializable]
        public struct State {
            public State(float inAngle = 0.0f)
                : this(-180.0f, 180.0f, inAngle) { }
            public State(float inLimit, bool inLimitFromNegativeToPositive = true, float inAngle = 0.0f)
                : this(inLimitFromNegativeToPositive ? -inLimit : inLimit,
                      inLimitFromNegativeToPositive ? inLimit : -inLimit,
                      inAngle) { }

            public State(float inLimitFrom, float inLimitTo, float inAngle = 0.0f) {
                LimitFrom = inLimitFrom;
                LimitTo = inLimitTo;
                Angle = inAngle;
            }

            public float LimitFrom, LimitTo;
            public float Angle;
        }

        //Methods
        //-Initialization
        public LimitedFloatAngle(float inAngle = 0.0f)
            : this(inAngle, -180.0f, 180.0f) { }
        public LimitedFloatAngle(float inLimitFrom, float inLimitTo, float inAngle = 0.0f)
            : this(new State(inLimitFrom, inLimitTo, inAngle)) { }

        public LimitedFloatAngle(State inState) { _state = inState; normalizeState(); }

        public LimitedFloatAngle(LimitedFloatAngle inValue) { _state = inValue._state; normalizeState(); }

        //-Accessors
        public float getLimitFrom() { return _state.LimitFrom; }
        public float getLimitTo() { return _state.LimitTo; }
        public float getLimitingAngle() {
            return XMath.getNormalizedAnglesClockwiseDelta(getLimitFrom(), getLimitTo());
        }
        public bool isUnlimited() { return Mathf.Approximately(getLimitingAngle(), 360.0f); }

        public float getAngle() { return _state.Angle; }
        public float setAngle(float inAngle) {
            float theNormalizedAngle = XMath.getNormalizedAngle(inAngle);
            return _state.Angle = isAngleAchievable(theNormalizedAngle) ?
                    theNormalizedAngle : getNearestLimitForAngle(theNormalizedAngle);
        }

        public float changeAngle(float inValueDelta) {
            if (Mathf.Approximately(inValueDelta, 0.0f)) return 0.0f;

            float theOldAngle = _state.Angle;

            float theNewAngle = XMath.getNormalizedAngle(_state.Angle + inValueDelta);
            float theDelta = XMath.getNormalizedAnglesClockwiseDelta(getLimitFrom(), theNewAngle);
            float theDirectedDelta = XMath.getNormalizedAngle(theDelta);

            if (!isUnlimited() && theDirectedDelta < 0.0f) {
                _state.Angle = getLimitFrom();
            } else if (!isUnlimited() && theDirectedDelta > getLimitingAngle()) {
                _state.Angle = getLimitTo();
            } else {
                _state.Angle = theNewAngle;
            }

            return XMath.getNormalizedAngle(_state.Angle - theOldAngle);
        }

        public float changeAngleToAchieveTargetAngleWithSpeed(float inTargetAngle, float inSpeed) {
            float theAngleToAchieve = XMath.getNormalizedAngle(inTargetAngle);
            if (Mathf.Approximately(theAngleToAchieve, getAngle())) return 0.0f;

            float theNearestDiraction = XMath.getNormalizedAngle(theAngleToAchieve - getAngle());
            if (Mathf.Abs(theNearestDiraction) <= inSpeed) {
                setAngle(inTargetAngle);
                return 0.0f; //TODO: Return real delta!!!
            }

            float theDiraction = getDiractionToAchieveAngle(inTargetAngle);
            return changeAngle(theDiraction * inSpeed);
        }

        //-Utils
        public bool isAngleAchievable(float inAngle) {
            float theNormalizedAngle = XMath.getNormalizedAngle(inAngle);
            float theDelta = XMath.getNormalizedAnglesClockwiseDelta(getLimitFrom(), theNormalizedAngle);
            return theDelta < getLimitingAngle() || Mathf.Approximately(theDelta, getLimitingAngle());
        }

        public float getDiractionToAchieveAngle(float inAngleToAchieve) {
            float theCurrentAngle = getAngle();
            float theAngleToAchieve = XMath.getNormalizedAngle(inAngleToAchieve);

            if (Mathf.Approximately(theCurrentAngle, theAngleToAchieve)) return 0.0f;

            if (isAngleAchievable(theAngleToAchieve)) {
                float theNearestDiraction = XMath.getNormalizedAngle(theAngleToAchieve - theCurrentAngle);
                if (isUnlimited()) return theNearestDiraction > 0.0f ? 1.0f : -1.0f;

                if (theNearestDiraction > 0.0f) {
                    float theTestAngle = XMath.getNormalizedAnglesClockwiseDelta(getLimitFrom(), theCurrentAngle);
                    theTestAngle += XMath.getNormalizedAnglesClockwiseDelta(theCurrentAngle, theAngleToAchieve);
                    return theTestAngle < getLimitingAngle() || Mathf.Approximately(theTestAngle, getLimitingAngle()) ?
                        1.0f : -1.0f;
                } else {
                    float theTestAngle = XMath.getNormalizedAnglesClockwiseDelta(getLimitFrom(), theCurrentAngle);
                    theTestAngle -= XMath.getNormalizedAnglesClockwiseDelta(getLimitFrom(), theAngleToAchieve);
                    return theTestAngle > 0.0f || Mathf.Approximately(theTestAngle, 0.0f) ? -1.0f : 1.0f;
                }
            } else {
                return getDiractionToAchieveAngle(getNearestLimitForAngle(inAngleToAchieve));
            }
        }

        public float getNearestLimitForAngle(float inAngle) {
            float theNormalizedAngle = XMath.getNormalizedAngle(inAngle);

            float theDeltaToAchieveFromLimit = 0.0f;
            float theDeltaToAchieveToLimit = 0.0f;
            if (isAngleAchievable(inAngle)) {
                theDeltaToAchieveFromLimit = XMath.getNormalizedAnglesClockwiseDelta(getLimitFrom(), theNormalizedAngle);
                theDeltaToAchieveToLimit = XMath.getNormalizedAnglesClockwiseDelta(theNormalizedAngle, getLimitTo());
            } else {
                theDeltaToAchieveFromLimit = XMath.getNormalizedAnglesClockwiseDelta(theNormalizedAngle, getLimitFrom());
                theDeltaToAchieveToLimit = XMath.getNormalizedAnglesClockwiseDelta(getLimitTo(), theNormalizedAngle);
            }
            return theDeltaToAchieveFromLimit < theDeltaToAchieveToLimit ? getLimitFrom() : getLimitTo();
        }

        //-Implementation
        //--Utils
        private void normalizeState() {
            _state.LimitFrom = XMath.getNormalizedAngle(_state.LimitFrom);
            _state.LimitTo = XMath.getNormalizedAngle(_state.LimitTo);

            setAngle(_state.Angle);
        }

        //Fields
        private State _state;
    }
}
