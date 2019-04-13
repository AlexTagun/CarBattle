using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private AmmoPanel ammoPanel;

    void Start() { }
    void Update() { }

    public void setAmmoPanelData(int clipCapacity, int ammoQuantity) {
        ammoPanel.ClipCapacity = clipCapacity;
        //ammoPanel.AmmoQuantity = 1;
        ammoPanel.AmmoQuantity = ammoQuantity;
    }

    public void setAmmoPanelQuantity(int ammoQuantity) {
        ammoPanel.AmmoQuantity = ammoQuantity;
    }
}
