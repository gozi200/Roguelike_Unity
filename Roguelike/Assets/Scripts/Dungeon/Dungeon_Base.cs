using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Base : MonoBehaviour {
    GameObject dungeon_generator;

    GameObject wall;
    GameObject player;

    /// <summary>
    /// 移動先が移動可能地形であるか
    /// </summary>
    /// <param name="x">移動先のx座標</param>
    /// <param name="y">移動先のy座標</param>
    /// <returns>移動可能であればfalse</returns>
    bool Is_Move(float x, float y) {
        wall = GameObject.Find("Wall(Clone)");
        player = GameObject.Find("Player");
        dungeon_generator = GameObject.Find("Dungeon_Generator");

        // 範囲外は移動不可
        if (x < 0 || x >= dungeon_generator.GetComponent<Dungeon_Generator>().width * 5 ||
            y < 0 || y >= dungeon_generator.GetComponent<Dungeon_Generator>().height * 5) {
            return true;
        }
 
        // 壁があるか？敵がいるか？
        for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count/*wallsの枚数分ループを回す*/; ++i) {
            if (x == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                y == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y /*|| player.transform.position　== エネミーのポジション*/) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 指定したaから見て、bは周囲８マスであり、かつ移動可能地形かを判断
    /// </summary>
    /// <param name="ax">現在のx座標</param>
    /// <param name="ay">現在のy座標</param>
    /// <param name="bx">移動先のx座標</param>
    /// <param name="by">移動先のy座標</param>
    /// <returns>移動可能であればtrue</returns>
    public bool Check_Move(float ax, float ay, float bx, float by) {
        // a・bが同一だったりしないか？
        if (ax == bx && ay == by) {
            return false;
        }

        // a・bは周囲８マスにいないか？
        if (Mathf.Abs(ax - bx) > 5 || Mathf.Abs(ay - by) > 5) {
            return false;
        }

        // bは移動可能地形か？
        if (Is_Move(bx, by)) {
            return false;
        }

        // aからみて、bは左上？
        if (ax - 1 == bx && ay - 1 == by) {
            // 左と上が移動不能地形か？
            if (Is_Move(ax - 1, ay) && Is_Move(ax, ay - 1)) {
                return false;
            }
            return true;
        }
        // aからみて、bは右上？
        if (ax + 1 == bx && ay - 1 == by) {
            // 右と上が移動不能地形か？
            if (Is_Move(ax + 1, ay) && Is_Move(ax, ay - 1)) {
                return false;
            }
            return true;
        }
        // aからみて、bは左下？
        if (ax - 1 == bx && ay + 1 == by) {
            // 右と下が移動不能地形か？
            if (Is_Move(ax - 1, ay) && Is_Move(ax, ay + 1)) {
                return false;
            }
            return true;
        }
        // aからみて、bは右下？
        if (ax + 1 == bx && ay + 1 == by) {
            // 右と下が移動不能地形か？
            if (Is_Move(ax + 1, ay) && Is_Move(ax, ay + 1)) {
                return false;
            }
            return true;
        }
        return true;
    }
}
