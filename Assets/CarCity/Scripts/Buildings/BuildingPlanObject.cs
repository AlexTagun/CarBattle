using UnityEngine;

public class BuildingPlanObject : MonoBehaviour
{
    //Settings
    public GameObject ConstructionSitePrefab = null;

    //Methods
    //-API
    private static Collider2D[] UNUSED = new Collider2D[1];
    public bool isPossibleToBuild() {
        return (0 == _collider.OverlapCollider(new ContactFilter2D(), UNUSED));
    }

    public ConstructionSiteObject startConstruction() {
        XUtils.check(isPossibleToBuild());
        Destroy(gameObject);
        return spawnConstructionSite();
    }

    public void cancelBuilding() {
        Destroy(gameObject);
    }

    //-Implementation
    private void Start() {
        _sprite = XUtils.verify(GetComponent<SpriteRenderer>());
        _collider = XUtils.verify(GetComponent<BoxCollider2D>());

        XUtils.check(ConstructionSitePrefab);
    }

    private void Update() {
        GetComponent<SpriteRenderer>().color =
            isPossibleToBuild() ? Color.green : Color.red;
    }

    private ConstructionSiteObject spawnConstructionSite() {
        GameObject theBuilding = Instantiate(ConstructionSitePrefab);
        XMath.assignTransform(theBuilding.transform, transform);
        return XUtils.verify(theBuilding.GetComponent<ConstructionSiteObject>());
    }

    //Fields
    private BoxCollider2D _collider = null;
    private SpriteRenderer _sprite = null;
}
