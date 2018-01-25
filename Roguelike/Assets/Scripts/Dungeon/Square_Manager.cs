using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square_Manager : MonoBehaviour {
    int sideNumber;
    int layerNumber;
    int lengthNumber;

    public int LengthNumber {
        get { return lengthNumber;
        }
        set {
            lengthNumber = value; }
    }

    public int SideNumber {
        get {
            return sideNumber;
        }
        set {
            sideNumber = value;
        }
    }

    public int LayerNumber {
        get {
            return layerNumber;
        }
        set {
            layerNumber = value; }
    }

    public void SetNumbers(int length, int side, int layernumber) {
        sideNumber   = side;
        layerNumber  = layernumber;
        lengthNumber = length;
    }
}
