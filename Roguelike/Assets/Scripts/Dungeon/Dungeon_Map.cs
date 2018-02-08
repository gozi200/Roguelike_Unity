/*
    制作者 石倉

    最終更新日 2018/02/08
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンのマップを２次元配列で管理するクラス
/// </summary>
public class Dungeon_Map : MonoBehaviour {
    public static Cell[,] map_layer = new Cell[30, 30];

    // TODO: プロパティ使う？
    public static void Set_Map_Layer(int y_coordinates, int x_coordinates, int number, Cell cell) {
        map_layer[y_coordinates, x_coordinates] = cell;
        map_layer[y_coordinates, x_coordinates].Layer_Number = number;
    }

    public static int Get_Map_layer(int y_coordinates, int x_coordinates) {
        return map_layer[y_coordinates, x_coordinates].Layer_Number;
    }

    public static Vector3 Get_Position(int y_coordinates, int x_coordinates) {
        return map_layer[y_coordinates, x_coordinates].transform.position;
    }
}
