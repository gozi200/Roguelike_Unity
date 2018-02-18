/*
制作者 石倉

最終編集日 2018/02/16
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 座標をセットして現在いる座標を表示する TODO: 役割分ける？ 名前をObject_Coordinatesに変える
/// </summary>
public class Object_Coordinates : MonoBehaviour {
    /// <summary>
    /// 現在いるy座標
    /// </summary>
    [SerializeField]
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
    /// 現在いるx座標
    /// </summary>
    [SerializeField]
    int width;
    public int Width {
        get {
            return width;
        }
        private set {
            width = value;
        }
    }

    /// <summary>
    /// 最初にセットする座標
    /// </summary>
    /// <param name = "set_width" >セットするx座標</param>
    /// <param name = "set_height">セットするy座標</param>
    public void Set_Init_Number(int set_height, int set_width) {
        height = set_height;
        width  = set_width;
    }
}
