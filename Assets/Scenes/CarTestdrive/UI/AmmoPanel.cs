using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPanel : MonoBehaviour
{
    [SerializeField]
    private Text text;
   

    private int clipCapacity = 0;
    public int ClipCapacity {
        set {
            clipCapacity = value;
            OnValueChanged();
        }
    }

    private int ammoQuantity = 0;
    public int AmmoQuantity {
        set {
            ammoQuantity = value;
            OnValueChanged();
        }
    }
    void Start()
    {
        text = GetComponentInChildren<Text>();
    }
    private void OnValueChanged() {
        text.text = ammoQuantity.ToString() + '/' + clipCapacity.ToString();
    }

    private string dataToString() {
        return ammoQuantity.ToString() + '/' + clipCapacity.ToString();
    }
}
