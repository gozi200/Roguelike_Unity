/*
    制作者 石倉

    最終更新日 2018/02/08
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マスの情報を管理するクラス
/// </summary>
public class Cell : MonoBehaviour {
    /// <summary>
    /// x軸の座標
    /// </summary>
    int width;
    public int Width {
        get {
            return width;
        }
        set {
            width = value;
        }
    }

    /// <summary>
    /// y軸の座標
    /// </summary>
    int height;
    public int Height {
        get {
            return height;
        }
        set {
            height = value;
        }
    }

    /// <summary>
    /// レイヤーの番号(種類)
    /// </summary>
    int layer_number;
    public int Layer_Number {
        get {
            return layer_number;
        }
        set {
            layer_number = value;
        }
    }

    /// <summary>
    /// どこのマスに何を配置するのかを設定する
    /// </summary>
    /// <param name = "set_height"      >縦軸の番号</param>
    /// <param name = "set_width"       >横軸の番号</param>
    /// <param name = "set_layer_number">レイヤーの番号(種類)</param>
    public void Set_Numbers(int set_height, int set_width, int set_layer_number) {
        height = set_height;
        width  = set_width;
        layer_number  = set_layer_number;
    }
}
