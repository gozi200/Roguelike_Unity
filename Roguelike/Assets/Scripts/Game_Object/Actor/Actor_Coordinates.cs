/*
制作者 石倉

最終編集日 2018/02/08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 座標をセットして現在いる座標を表示する TODO: 役割分ける？
/// </summary>
public class Actor_Coordinates : MonoBehaviour {
    /// <summary>
    /// 現在いるy座標
    /// </summary>
    [SerializeField]
    int y_coordinates;
    public int Y_Coordinates {
        get {
            return y_coordinates;
        }
        private set {
            y_coordinates = value;
        }
    }

    /// <summary>
    /// 現在いるx座標
    /// </summary>
    [SerializeField]
    int x_coordinates;
    public int X_Coordinates {
        get {
            return x_coordinates;
        }
        private set {
            x_coordinates = value;
        }
    }

    /// <summary>
    /// 最初にセットする座標
    /// </summary>
    /// <param name = "set_y_coordinates">セットするy座標</param>
    /// <param name = "set_x_coordinates">セットするx座標</param>
    public void Set_Init_Number(int set_y_coordinates, int set_x_coordinates) {
        y_coordinates = set_y_coordinates;
        x_coordinates = set_x_coordinates;
    }
}
