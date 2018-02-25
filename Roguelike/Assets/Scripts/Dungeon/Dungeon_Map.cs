/*
    制作者 石倉

    最終更新日 2018/02/22
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンのマップを２次元配列で管理するクラス
/// </summary>
public class Dungeon_Map : MonoBehaviour {
    public static Cell[,] map_layer = new Cell[30, 30];

    // TODO: プロパティ使う？ 引数有りの場合を調べる
    public static void Set_Map_Layer(int height, int width, int number, Cell cell) {
        map_layer[height, width] = cell;
        map_layer[height, width].Layer_Number = number;
    }

    public static int Get_Map_Layer(float height, float width) {
        return map_layer[(int)height, (int)width].Layer_Number;
    }

    public static Vector2 Get_Position(int height, int width) {
        return map_layer[height, width].transform.position;
    }
}
