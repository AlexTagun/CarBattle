using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlanUIInterface : MonoBehaviour
{
    //TODO: Find better way to pass ConstructionSiteUIObject prefab info
    public void init(
        CarCityObject inCarCity, ConstructionSiteUIObject inConstructionSiteUIPrefab)
    {
        _carCity = inCarCity;
        _constructionSiteUIPrefab = inConstructionSiteUIPrefab;
    }

    public void processConstructionStart(
        ConstructionSiteObject inConstructionSite)
    {
        var theWorldObjectsAttachedUIManager =
            FindObjectOfType<WorldObjectsAttachedUIManger>();

        ConstructionSiteUIObject theConstructionSiteUI =
            XUtils.createObject(XUtils.verify(_constructionSiteUIPrefab));
        theConstructionSiteUI.init(_carCity, inConstructionSite);
        theWorldObjectsAttachedUIManager.attach(
            theConstructionSiteUI.gameObject, inConstructionSite.gameObject
        );
    }

    //Fields
    private CarCityObject _carCity = null;
    private ConstructionSiteUIObject _constructionSiteUIPrefab = null;
}
