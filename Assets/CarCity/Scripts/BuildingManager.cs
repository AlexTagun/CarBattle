using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    public GameObject _builderPattern;

    private List<GameObject> _buildings;
    private List<GameObject> _builders;
    void Start()
    {
        _buildings = new List<GameObject>();
    }

    public void Build() {
        Vector3 curPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        worldPos.z = 0;

        GameObject builder = Instantiate(_builderPattern, worldPos, Quaternion.identity);

    }
    
}
