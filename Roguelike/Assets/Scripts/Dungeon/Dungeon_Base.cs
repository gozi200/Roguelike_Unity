/*
    制作者 石倉

    最終更新日 2018/02/08
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動可能であるかどうかを判断するクラス　TODO: Move_Checkとかに名前を変える
/// </summary>
public class Dungeon_Base : MonoBehaviour {
    GameObject game_manager;

    [SerializeField]
    Dungeon_Generator dungeon_generator;

    GameObject wall;

    void Start() {
        Player_Move.Set_Dungeon_Base(this);
    }

    // TODO: ななめの処理はどうしよう
    /// <summary>
    /// 攻撃、移動の時に呼ばれる処理
    /// </summary>
    /// <param name = "height">縦座標</param>
    /// <param name = "width" >横座標</param>
    /// <returns>移動可能、攻撃発生であればtrue</returns>
    public static bool Is_Check_Move(int coordinates1, int coordinates2, int object_) {
        if (Dungeon_Map.Get_Map_layer(coordinates1, coordinates2) == object_) {
            if (Dungeon_Map.Get_Map_layer(coordinates1, coordinates2) == object_) {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    ///  移動、攻撃を壁越しに行っていないかを判断する もっといいやり方ある？
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <param name="slant_direction1"></param>
    /// <param name="slant_direction2"></param>
    /// <param name="object_"></param>
    /// <returns>壁越しであればtrue</returns>
    public static bool Is_Check_Slant_Move(int height, int width, int slant_direction1, int slant_direction2, int object_) {
        if (Dungeon_Map.Get_Map_layer(height, width) == object_) {
            if (Dungeon_Map.Get_Map_layer(slant_direction1, slant_direction2) == object_) {
                return false;
            }
        }
        return true;
    }

    ///// <summary>
    ///// 移動先が移動可能地形であるか
    ///// </summary>
    ///// <param name="x">移動先のx座標</param>
    ///// <param name="y">移動先のy座標</param>
    ///// <returns>移動可能であればfalse</returns>
    //bool Is_Move(float x, float y) {
    //    wall = GameObject.Find("Grass_Wall(Clone)");
    //
    //    // 範囲外は移動不可
    //    if (x < 0 || x >= dungeon_generator.GetComponent<Dungeon_Generator>().width  * 5 ||
    //        y < 0 || y >= dungeon_generator.GetComponent<Dungeon_Generator>().height * 5) {
    //        return true;
    //    }
    //
    //    // 壁があるか？敵がいるか？
    //    for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
    //        if (x == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //            y == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    //
    ///// <summary>
    ///// 壁越しに移動をしようとしていないかを判断
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <param name="direction"></param>
    ///// <returns>移動可能であればtrue そうでなければfalse</returns>
    //bool Is_Diagonal_Move(float x, float y, eDirection direction) {
    //    wall = GameObject.Find("Grass_Wall(Clone)");
    //
    //    if (direction == eDirection.Upleft) {
    //        for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
    //            if (x + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
    //                x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //                return true;
    //            }
    //        }
    //    }
    //    else if (direction == eDirection.Upright) {
    //        for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
    //            if (x - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
    //                x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //                return true;
    //            }
    //        }
    //    }
    //    else if (direction == eDirection.Downright) {
    //        for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
    //
    //            if (x - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
    //                x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //                return true;
    //            }
    //        }
    //    }
    //    else if (direction == eDirection.Downleft) {
    //        for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
    //            if (x + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
    //                x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}
    //
    ///// <summary>
    ///// 壁越しに攻撃しようとしていないかを判断
    ///// </summary>
    ///// <returns>壁越しであったらtrue</returns>
    //public bool Is_Diagonal_Attack(float x, float y, eDirection direction) {
    //    wall = GameObject.Find("Grass_Wall(Clone)");
    //
    //    for (int i = 0; i < dungeon_generator.GetComponent<Dungeon_Generator>().walls.Count; ++i) {
    //        if (direction == eDirection.Upleft) {
    //            if (x + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
    //                x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //                return true;
    //            }
    //        }
    //        else if (direction == eDirection.Upright) {
    //            if (x + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
    //                x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //                return true;
    //            }
    //        }
    //        else if (direction == eDirection.Downright) {
    //            if (x - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
    //                x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y + 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //                return true;
    //            }
    //        }
    //        else if (direction == eDirection.Downleft) {
    //            if (x - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y ||
    //                x     == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.x &&
    //                y - 5 == dungeon_generator.GetComponent<Dungeon_Generator>().walls[i].transform.position.y) {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}
    //
    ///// <summary>
    ///// 指定したaから見て、bは周囲８マスであり、かつ移動可能地形かを判断
    ///// </summary>
    ///// <param name="ax">現在のx座標</param>
    ///// <param name="ay">現在のy座標</param>
    ///// <param name="bx">移動先のx座標</param>
    ///// <param name="by">移動先のy座標</param>
    ///// <returns>移動可能であればtrue</returns>
    //public bool Check_Move(float ax, float ay, float bx, float by, eDirection direction) {
    //    // a・bが同一だったりしないか？
    //    if (ax == bx && ay == by) {
    //        return false;
    //    }
    //
    //    // a・bは周囲８マスにいないか？
    //    if (Mathf.Abs(ax - bx) > 5 || Mathf.Abs(ay - by) > 5) {
    //        return false;
    //    }
    //
    //    // bは移動可能地形か？
    //    if (Is_Nomal_Mode(direction)) {
    //        if (Is_Diagonal_Move(bx, by, direction)) {
    //            return false;
    //        }
    //    }
    //
    //    if (Is_Move(bx, by)) {
    //        return false;
    //    }
    //    return true;
    //}
    //
    ///// <summary>
    ///// プレイヤーの移動モードを判断する
    ///// </summary>
    ///// <returns>ノーマルモードであればtrue</returns>
    //bool Is_Nomal_Mode(eDirection direction) {
    //    if(direction == eDirection.Upleft   ||
    //       direction == eDirection.Upright  ||
    //       direction == eDirection.Downleft ||
    //       direction == eDirection.Downright) {
    //        return true;
    //    }
    //    return false;
    //}
}
