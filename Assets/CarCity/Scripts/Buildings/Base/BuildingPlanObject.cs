using UnityEngine;

public class BuildingPlanObject : MonoBehaviour
{
    //Methods
    //-API
    private static Collider2D[] UNUSED = new Collider2D[1];
    public bool isPossibleToBuild() {
        return (0 == _collider.OverlapCollider(new ContactFilter2D(), UNUSED));
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
        ).onMouseMove += (Vector3 inMousePosition)=>{
            updateColor();
        };

        XUtils.getComponent<MouseClickTrackingComponent>(
            gameObject, XUtils.AccessPolicy.ShouldBeCreated
        ).onClick += () => {
            if (!isPossibleToBuild()) return;
            startConstruction();
        };
    }

    private void updateColor() {
        GetComponent<SpriteRenderer>().color =
            isPossibleToBuild() ? Color.green : Color.red;
    }

    public ConstructionSiteObject startConstruction() {
        XUtils.check(isPossibleToBuild());

        ConstructionSiteObject theConstructionSite = XUtils.createObject(
            getBuildingScheme().constructionSite, transform
        );
        getUIInterface().processConstructionStart(theConstructionSite);

        Destroy(gameObject);
        return theConstructionSite;
    }

    private BuildingScheme getBuildingScheme() {
        return XUtils.verify(_buildingScheme);
    }

    //Fields
    public BuildingScheme _buildingScheme = null;

    private BoxCollider2D _collider = null;
    private SpriteRenderer _sprite = null;
}
