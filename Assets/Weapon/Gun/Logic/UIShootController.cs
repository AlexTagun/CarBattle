using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShootController : MonoBehaviour
{
    UIManager uiManager;

    private void Start() {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }
    public void setClipStats(int clipCapacity, int ammoQuantity) {
        uiManager.setAmmoPanelData(clipCapacity, ammoQuantity);
    }

    public void setAmmoQuantity(int ammoQuantity) {
        uiManager.setAmmoPanelQuantity(ammoQuantity);
    }
    
}
