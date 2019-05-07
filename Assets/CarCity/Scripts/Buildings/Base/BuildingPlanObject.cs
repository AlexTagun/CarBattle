using UnityEngine;

public class BuildingPlanObject : MonoBehaviour
{
    //Methods
    //-API
    public void init(CarCityObject inCarCity) {
        _carCity = XUtils.verify(inCarCity);

        GameObject theCityLevel = inCarCity.getInnerLevel();
        gameObject.layer = XUtils.verify(theCityLevel).layer;
        gameObject.transform.SetParent(theCityLevel.transform, true);

        Vector3 theLocalPosition = gameObject.transform.localPosition;
        theLocalPosition.z = 0.0f;
        gameObject.transform.localPosition = theLocalPosition;
    }

    public bool isPossibleToBuild() {
        var theFilter = new ContactFilter2D();
        theFilter.SetLayerMask(1 << gameObject.layer);
        theFilter.useTriggers = true;

        return (0 == _collider.OverlapCollider(theFilter, UNUSED));
    }

    public void cancelBuilding() {
        Destroy(gameObject);
    }

    public BuildingPlanUIInterface getUIInterface() {
        return XUtils.getComponent<BuildingPlanUIInterface>(
            this, XUtils.AccessPolicy.CreateIfNo
        );
    }

    //-Implementation
    private void Awake() {
        _sprite = XUtils.getComponent<SpriteRenderer>(
            gameObject, XUtils.AccessPolicy.ShouldExist
        );

        _collider = XUtils.getComponent<BoxCollider2D>(
            gameObject, XUtils.AccessPolicy.ShouldExist
        );

        XUtils.getComponent<MouseAttachComponent>(
            gameObject, XUtils.AccessPolicy.ShouldBeCreated
        ).onMouseMove += (Vector2 inMousePosition)=>{
            updateColor();
        };

        XUtils.getComponent<MouseClickTrackingComponent>(
            gameObject, XUtils.AccessPolicy.ShouldBeCreated
        ).onClick += () => {
            if (!isPossibleToBuild()) return;

            ConstructionSiteObject theConstructionSite = createConstructionSite();
            getUIInterface().processConstructionStart(theConstructionSite);

            Destroy(gameObject);
        };
    }

    private void updateColor() {
        GetComponent<SpriteRenderer>().color =
            isPossibleToBuild() ? Color.green : Color.red;
    }

    public ConstructionSiteObject createConstructionSite() {
        XUtils.check(isPossibleToBuild());

        ConstructionSiteObject theConstructionSite = XUtils.createObject(
            getBuildingScheme().constructionSite, transform
        );
        theConstructionSite.init(_carCity);

        theConstructionSite.transform.SetParent(transform.parent, true);
        theConstructionSite.gameObject.layer = gameObject.layer;

        return theConstructionSite;
    }

    private BuildingScheme getBuildingScheme() {
        return XUtils.verify(_buildingScheme);
    }

    //Fields
    public BuildingScheme _buildingScheme = null;

    private CarCityObject _carCity = null;

    private BoxCollider2D _collider = null;
    private SpriteRenderer _sprite = null;

    //-Misc
    private static Collider2D[] UNUSED = new Collider2D[1];
}
