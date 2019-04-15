using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldUIManager : MonoBehaviour
{
    public GameObject TestObjectToAttach = null;
    public GameObject UIPrefab = null;

    private Canvas _canvas = null;
    private WorldObjectsAttachedUIManger _worldObjectsAttachedUIManager = null;

    private GameObject testHealthbar = null;

    void Start() {
        _canvas = gameObject.GetComponent<Canvas>();

        _worldObjectsAttachedUIManager =
            gameObject.GetComponent<WorldObjectsAttachedUIManger>();

        //Test attach
        testHealthbar = GameObject.Instantiate(UIPrefab);
        testHealthbar.transform.SetParent(_canvas.transform, false);

        var theTransform = testHealthbar.GetComponent<RectTransform>();
        theTransform.anchorMin = new Vector2(0.0f, 0.0f);
        theTransform.anchorMax = new Vector2(0.0f, 0.0f);
        theTransform.sizeDelta = new Vector2(80.0f, 10.0f);

        _worldObjectsAttachedUIManager.attach(
            testHealthbar, TestObjectToAttach.GetComponent<WorldObjectAttachPoint>()
        );
    }

    void Update() {
        if (XUtils.isValid(TestObjectToAttach)) {
            var theDurability = TestObjectToAttach.GetComponent<DurabilityComponent>();
            testHealthbar.GetComponent<LimitedValueStripeIndicatorObject>().set(
                0.0f, theDurability.getMaxHitPoints(), theDurability.getHitPoints()
            );
        }
    }
}
