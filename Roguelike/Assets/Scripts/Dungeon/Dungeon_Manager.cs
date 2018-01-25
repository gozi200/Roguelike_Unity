using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ計算数rクラス
/// </summary>
public class Dungeon_Manager : MonoBehaviour {
    public static Square_Manager[,] map_layer = new Square_Manager[10, 10];
    public static void SetMaplayer(int length, int side, int number, Square_Manager square) {
        map_layer[length, side] = square;
        map_layer[length, side].LayerNumber = number;
    }

    public static int GetMaplayer(int length, int side) {
        return map_layer[length, side].LayerNumber;
    }

    public static Vector3 GetPosition(int length, int side) {
        return map_layer[length, side].transform.position;
    }
}
