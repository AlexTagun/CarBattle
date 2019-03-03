using UnityEngine;

namespace Values
{
    // ============================= LimitedFloat =================================

    public class LimitedFloat {

        //Types
        [System.Serializable]
        public struct ZeroBasedState {
            public ZeroBasedState(float InMaximum, bool SetValueAsMaximum = false)
                : this(SetValueAsMaximum ? InMaximum : 0.0f, InMaximum) { }

            public ZeroBasedState(float InValue, float InMaximum) {
                Value = InValue;
                Maximum = InMaximum;
            }

            public float Value;
            public float Maximum;
        }

        [System.Serializable]
        public struct State {
            public State(float InValue, float InMinimum, float InMaximum) {
                Value = InValue;
                Minimum = InMinimum;
                Maximum = InMaximum;
            }

            public float Value;
            public float Minimum, Maximum;
        }

        //Methods
        //-Initialization
        public LimitedFloat()
            : this(0.0f) { }
        public LimitedFloat(float inValue)
            : this(inValue, 0.0f, 1.0f) { }
        public LimitedFloat(float inMinimum, float inMaximum)
            : this(0.0f, inMinimum, inMaximum) { }
        public LimitedFloat(float inValue, float inMinimum, float inMaximum)
            : this(new State(inValue, inMinimum, inMaximum)) { }

        public LimitedFloat(ZeroBasedState inState)
            : this(new State(inState.Value, 0.0f, inState.Maximum)) { }

        public LimitedFloat(State inState) {
            //TODO: Put assert if Max > Min
            _state = inState;
        }

        //-Accessors
        public float getMinimum() { return _state.Minimum; }
        public void setMinimum(float inMinimum) {
            _state.Minimum = inMinimum;
            setValue(_state.Value);
        }

        public float getMaximum() { return _state.Maximum; }
        public void setMaximum(float inMaximum) {
            _state.Maximum = inMaximum;
            setValue(_state.Value);
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

        //Fields
        private State _state;
    }

    // ======================== LimitedFloatAngle =============================

    public class LimitedFloatAngle
    {
        //Methods
        //-Initialization
        public LimitedFloatAngle()
            : this(0.0f) { }
        public LimitedFloatAngle(float inAngle)
            : this(inAngle, -180.0f, 180.0f) { }
        public LimitedFloatAngle(float inLimitFrom, float inLimitTo)
            : this(0.0f, inLimitFrom, inLimitTo) { }

        public LimitedFloatAngle(float inAngle, float inLimitFrom, float inLimitTo) {
            _limitFrom = LimitedFloatAngle.getNormalizedAngle(inLimitFrom);
            _limitTo = LimitedFloatAngle.getNormalizedAngle(inLimitTo);

            setAngle(inAngle);
        }

        //-Accessors
        public float getLimitFrom() { return _limitFrom; }
        public float getLimitTo() { return _limitTo; }
        public float getLimitingAngle() {
            return getNormalizedAnglesClockwiseDelta(getLimitFrom(), getLimitTo());
        }
        public bool isUnlimited() { return Mathf.Approximately(getLimitingAngle(), 360.0f); }

        public float getAngle() { return _angle; }
        public float setAngle(float inAngle) {
            float theNormalizedAngle = getNormalizedAngle(inAngle);
            return _angle = isAngleAchievable(theNormalizedAngle) ?
                    theNormalizedAngle : getNearestLimitForAngle(theNormalizedAngle);
        }

        public float changeAngle(float inValueDelta) {
            if (0.0f == inValueDelta) return 0.0f;

            float theOldAngle = _angle;

            float theNewAngle = getNormalizedAngle(_angle + inValueDelta);
            float theDelta = getNormalizedAnglesClockwiseDelta(
                getLimitFrom(), theNewAngle
            );

            _angle = theDelta < getLimitingAngle() ? theNewAngle :
                (inValueDelta > 0.0f ? getLimitTo() : getLimitFrom());

            return getNormalizedAngle(_angle - theOldAngle);
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
        public static float getNormalizedAngle(float inAngle)
        {
            float theAngle = inAngle % 360.0f;
            return theAngle > 180.0f ? theAngle - 360.0f :
                theAngle < -180.0f ? theAngle + 360.0f : theAngle;
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
                theDeltaToAchieveFromLimit = getNormalizedAnglesClockwiseDelta(_limitFrom, theNormalizedAngle);
                theDeltaToAchieveToLimit = getNormalizedAnglesClockwiseDelta(theNormalizedAngle, _limitTo);
            } else {
                theDeltaToAchieveFromLimit = getNormalizedAnglesClockwiseDelta(theNormalizedAngle, _limitFrom);
                theDeltaToAchieveToLimit = getNormalizedAnglesClockwiseDelta(_limitTo, theNormalizedAngle);
            }
            return theDeltaToAchieveFromLimit < theDeltaToAchieveToLimit ? getLimitFrom() : getLimitTo();
        }

        //-Implementation
        //--Utils
        private static float getNormalizedAnglesClockwiseDelta(float inFrom, float inTo) {
            float theDelta = inTo - inFrom;
            if (theDelta < 0.0f) theDelta += 360;
            return theDelta;
        }

        //Fields
        private float _angle = 0.0f;
        private float _limitFrom = -180.0f;
        private float _limitTo = 180.0f;
    }
}
