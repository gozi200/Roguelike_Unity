using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    int height_number;
    int width_number;
    int layer_number;

    public int Height_Number {
        get {
            return height_number;
        }
        set {
            height_number = value;
        }
    }

    public int Width_Number {
        get {
            return width_number;
        }
        set {
            width_number = value;
        }
    }

    public int Layer_Number {
        get {
            return layer_number;
        }
        set {
            layer_number = value;
        }
    }

    public void Set_Numbers(int set_height, int set_width, int set_layernumber) {
        height_number = set_height;
        width_number = set_width;
        layer_number = set_layernumber;
    }
}
