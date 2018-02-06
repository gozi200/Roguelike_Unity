using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Base : MonoBehaviour {
    GameObject wall;
    GameObject dungeon_generator;

    /// <summary>
    /// 移動先が移動可能地形であるか
    /// </summary>
    /// <param name="x">移動先のx座標</param>
    /// <param name="y">移動先のy座標</param>
    /// <returns>移動可能であればfalse</returns>
    bool Is_Move(float x, float y) {
        wall = GameObject.Find("Wall(Clone)");
        dungeon_generator = GameObject.Find("Dungeon_Generator");

        // 範囲外は移動不可
        if (x < 0 || x >= dungeon_generator.GetComponent<Dungeon_Generator>().width * 5 ||
            y < 0 || y >= dungeon_generator.GetComponent<Dungeon_Generator>().height * 5) {
            return true;
        }
 
        // 壁があるか？敵がいるか？
        for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
            if (x == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                y == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y /*|| player.transform.position　== エネミーのポジション*/) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 壁越しに移動をしようとしていないかを判断
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool Is_Diagonal_Move(float x, float y, eDirection direction) {
        wall = GameObject.Find("Wall(Clone)");
        dungeon_generator = GameObject.Find("Dungeon_Generator");

        if (direction == eDirection.Upleft) {
            for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
                if (x + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
                    x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
                    return true;
                }
            }
        }
        else if (direction == eDirection.Upright) {
            for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
                if (x - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
                    x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
                    return true;
                }
            }
        }
        else if (direction == eDirection.Downright) {
            for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {

                if (x - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
                    x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
                    return true;
                }
            }
        }
        else if (direction == eDirection.Downleft) {
            for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
                if (x + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
                    x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 壁越しに攻撃しようとしていないかを判断
    /// </summary>
    /// <returns>壁越しであったらtrue</returns>
    public bool Is_Diagonal_Attack(float x, float y, eDirection direction) {
        wall = GameObject.Find("Wall(Clone)");
        dungeon_generator = GameObject.Find("Dungeon_Generator");

        for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
            if (direction == eDirection.Upleft) {
                if (x + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
                    x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
                    return true;
                }
            }
            else if (direction == eDirection.Upright) {
                if (x + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
                    x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
                    return true;
                }
            }
            else if (direction == eDirection.Downright) {
                if (x - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
                    x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
                    return true;
                }
            }
            else if (direction == eDirection.Downleft) {
                if (x - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
                    x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
                    y - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
                    return true;
                }
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
    public bool Check_Move(float ax, float ay, float bx, float by, eDirection direction) {
        // a・bが同一だったりしないか？
        if (ax == bx && ay == by) {
            return false;
        }

        // a・bは周囲８マスにいないか？
        if (Mathf.Abs(ax - bx) > 5 || Mathf.Abs(ay - by) > 5) {
            return false;
        }

        // bは移動可能地形か？
        if (Is_Nomal_Mode(direction)) {
            if (Is_Diagonal_Move(bx, by, direction)) {
                return false;
            }
        }

        if (Is_Move(bx, by)) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// プレイヤーの移動モードを判断する
    /// </summary>
    /// <returns>ノーマルモードであればtrue</returns>
    bool Is_Nomal_Mode(eDirection direction) {
        if(direction == eDirection.Upleft   ||
           direction == eDirection.Upright  ||
           direction == eDirection.Downleft ||
           direction == eDirection.Downright) {
            return true;
        }
        return false;
    }
}
