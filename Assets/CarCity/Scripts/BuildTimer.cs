using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildTimer : MonoBehaviour
{
    private TextMeshPro _text;
    private float _currCountdownValue;

    private void Awake() {
        _text = GetComponentInChildren<TextMeshPro>();
    }

    public void setTimer(float seconds) {
        StartCoroutine(StartCountdown(seconds));
    }

    public IEnumerator StartCountdown(float countdownValue = 10) {
        _currCountdownValue = countdownValue;
        setText(_currCountdownValue);
        while (_currCountdownValue > 0) {
            yield return new WaitForSeconds(1.0f);
            _currCountdownValue--;
            setText(_currCountdownValue);
        }
        Destroy(gameObject);
    }

    private void setText(float value) {
        int minutes = (int) value / 60;
        int seconds = (int) value % 60;
        string text = (minutes < 10) ? "0" + minutes : "" + minutes;
        text += ':';
        text += (seconds < 10) ? "0" + seconds : "" + seconds;
        _text.text = text;
    }

    private void OnDestroy() {
        //GetComponentInParent<Builder>().OnTimerDestroy();
    }
}
