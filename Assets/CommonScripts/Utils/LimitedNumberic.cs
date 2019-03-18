using UnityEngine;

namespace Values
{
    // ============================= LimitedFloat =================================

    public class LimitedFloat
    {
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

        //Methods
        //-Initialization
        public LimitedFloat(float inValue = 0.0f)
            : this(0.0f, 1.0f, inValue) { }
        public LimitedFloat(float inMinimum, float inMaximum, float inValue = 0.0f)
            : this(new State(inMinimum, inMaximum, inValue)) { }
        public LimitedFloat(ZeroBasedState inState)
            : this(new State(0.0f, inState.Maximum, inState.Value)) { }

        public LimitedFloat(State inState) { _state = inState; normalizeState(); }

        public LimitedFloat(LimitedFloat inValue) { _state = inValue._state; normalizeState(); }

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
            return _state.Value = Mathf.Clamp(inNewValue, getMinimum(), getMaximum());
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

        //-Implementation
        //--Utils
        private void normalizeState() {
            if (_state.Minimum > _state.Maximum) {
                float theTmp = _state.Minimum;
                _state.Minimum = _state.Maximum;
                _state.Maximum = theTmp;
            }
            setValue(_state.Value);
        }

        //Fields
        private State _state;
    }

    // ======================== LimitedFloatAngle =============================

    public class LimitedFloatAngle
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
        public LimitedFloatAngle()
            : this(0.0f) { }
        public LimitedFloatAngle(float inAngle)
            : this(inAngle, -180.0f, 180.0f) { }
        public LimitedFloatAngle(float inLimitFrom, float inLimitTo, float inAngle = 0.0f)
            : this(new State(inLimitFrom, inLimitTo, inAngle)) { }

        public LimitedFloatAngle(State inState) { _state = inState; normalizeState(); }

        public LimitedFloatAngle(LimitedFloatAngle inValue) { _state = inValue._state; normalizeState(); }

        //-Accessors
        public float getLimitFrom() { return _state.LimitFrom; }
        public float getLimitTo() { return _state.LimitTo; }
        public float getLimitingAngle() {
            return getNormalizedAnglesClockwiseDelta(getLimitFrom(), getLimitTo());
        }
        public bool isUnlimited() { return Mathf.Approximately(getLimitingAngle(), 360.0f); }

        public float getAngle() { return _state.Angle; }
        public float setAngle(float inAngle) {
            float theNormalizedAngle = getNormalizedAngle(inAngle);
            return _state.Angle = isAngleAchievable(theNormalizedAngle) ?
                    theNormalizedAngle : getNearestLimitForAngle(theNormalizedAngle);
        }

        public float changeAngle(float inValueDelta) {
            if (Mathf.Approximately(inValueDelta, 0.0f)) return 0.0f;

            float theOldAngle = _state.Angle;

            float theNewAngle = getNormalizedAngle(_state.Angle + inValueDelta);
            float theDelta = getNormalizedAnglesClockwiseDelta(
                getLimitFrom(), theNewAngle
            );
            float theDirectedDelta = getNormalizedAngle(theDelta);

            if (theDirectedDelta < 0.0f) {
                _state.Angle = getLimitFrom();
            } else if (theDirectedDelta > getLimitingAngle()) {
                _state.Angle = getLimitTo();
            } else {
                _state.Angle = theNewAngle;
            }

            return getNormalizedAngle(_state.Angle - theOldAngle);
        }

        public float changeAngleToAchieveTargetAngleWithSpeed(float inTargetAngle, float inSpeed) {
            float theAngleToAchieve = getNormalizedAngle(inTargetAngle);
            if (Mathf.Approximately(theAngleToAchieve, getAngle())) return 0.0f;

            float theNearestDiraction = getNormalizedAngle(theAngleToAchieve - getAngle());
            if (Mathf.Abs(theNearestDiraction) <= inSpeed) {
                setAngle(inTargetAngle);
                return 0.0f; //TODO: Return real delta!!!
            }

            float theDiraction = getDiractionToAchieveAngle(inTargetAngle);
            return changeAngle(theDiraction * inSpeed);
        }

        //-Utils
        public static float getNormalizedAngle(float inAngle) {
            float theAngle = inAngle % 360.0f;
            return theAngle > 180.0f ? theAngle - 360.0f :
                theAngle < -180.0f ? theAngle + 360.0f : theAngle;
        }

        public static float getNormalizedAnglesClockwiseDelta(float inFrom, float inTo) {
            float theDelta = inTo - inFrom;
            if (theDelta < 0.0f) theDelta += 360;
            return theDelta;
        }

        public bool isAngleAchievable(float inAngle) {
            float theNormalizedAngle = getNormalizedAngle(inAngle);
            float theDelta = getNormalizedAnglesClockwiseDelta(getLimitFrom(), theNormalizedAngle);
            return theDelta < getLimitingAngle() || Mathf.Approximately(theDelta, getLimitingAngle());
        }

        public float getDiractionToAchieveAngle(float inAngleToAchieve) {
            float theCurrentAngle = getAngle();
            float theAngleToAchieve = getNormalizedAngle(inAngleToAchieve);

            if (Mathf.Approximately(theCurrentAngle, theAngleToAchieve)) return 0.0f;

            if (isAngleAchievable(theAngleToAchieve)) {
                float theNearestDiraction = getNormalizedAngle(theAngleToAchieve - theCurrentAngle);
                if (isUnlimited()) return theNearestDiraction > 0.0f ? 1.0f : -1.0f;

                if (theNearestDiraction > 0.0f) {
                    float theTestAngle = getNormalizedAnglesClockwiseDelta(getLimitFrom(), theCurrentAngle);
                    theTestAngle += getNormalizedAnglesClockwiseDelta(theCurrentAngle, theAngleToAchieve);
                    return theTestAngle < getLimitingAngle() || Mathf.Approximately(theTestAngle, getLimitingAngle()) ? 1.0f : -1.0f;
                } else {
                    float theTestAngle = getNormalizedAnglesClockwiseDelta(getLimitFrom(), theCurrentAngle);
                    theTestAngle -= getNormalizedAnglesClockwiseDelta(getLimitFrom(), theAngleToAchieve);
                    return theTestAngle > 0.0f || Mathf.Approximately(theTestAngle, 0.0f) ? -1.0f : 1.0f;
                }
            } else {
                return getDiractionToAchieveAngle(getNearestLimitForAngle(inAngleToAchieve));
            }
        }

        public float getNearestLimitForAngle(float inAngle) {
            float theNormalizedAngle = getNormalizedAngle(inAngle);

            float theDeltaToAchieveFromLimit = 0.0f;
            float theDeltaToAchieveToLimit = 0.0f;
            if (isAngleAchievable(inAngle)) {
                theDeltaToAchieveFromLimit = getNormalizedAnglesClockwiseDelta(getLimitFrom(), theNormalizedAngle);
                theDeltaToAchieveToLimit = getNormalizedAnglesClockwiseDelta(theNormalizedAngle, getLimitTo());
            } else {
                theDeltaToAchieveFromLimit = getNormalizedAnglesClockwiseDelta(theNormalizedAngle, getLimitFrom());
                theDeltaToAchieveToLimit = getNormalizedAnglesClockwiseDelta(getLimitTo(), theNormalizedAngle);
            }
            return theDeltaToAchieveFromLimit < theDeltaToAchieveToLimit ? getLimitFrom() : getLimitTo();
        }

        //-Implementation
        //--Utils
        private void normalizeState() {
            _state.LimitFrom = LimitedFloatAngle.getNormalizedAngle(_state.LimitFrom);
            _state.LimitTo = LimitedFloatAngle.getNormalizedAngle(_state.LimitTo);

            setAngle(_state.Angle);
        }

        //Fields
        private State _state;
    }
}
