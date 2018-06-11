using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクター共通の行動を管理クラス
/// </summary>
public static class Actor_Action {
    /// <summary>
    /// 移動可能かを判断
    /// </summary>
    /// <param name="my_layer">自分のレイヤーナンバー</param>
    /// <param name="new_position">移動先の座標</param>
    /// <returns>移動不可能であればtrue</returns>
    public static bool Move_Check(int my_layer, int new_position) {
        // 自分のレイヤー番号と移動先のレイヤー番号を比べる
        if (my_layer > new_position) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// ななめ移動時に移動不可になる場所に壁があるかを調べる。 
    /// </summary>
    /// <param name="check_layer1">壁1</param>
    /// <param name="check_layer2">壁2</param>
    /// <returns>移動不可能であればtrue</returns>
     public static bool Slant_Check(int check_layer1, int check_layer2) {
         // ななめ移動時に移動不可となる場合の壁の位置を調べる
         if (check_layer1 >= Define_Value.WALL_LAYER_NUMBER ||
             check_layer2 >= Define_Value.WALL_LAYER_NUMBER) {
             return true;
         }
         return false;
     }

    // ↑↓どっち使う？　↓を使うなら第一引数をアクターの持っているfeetに変えなきゃダメ
    /*
    /// <summary>
    /// ななめ移動ができない場合を調べる
    /// </summary>
    /// <param name="now_feet">自分が踏んでいる床の情報</param>
    /// <param name="after_feet">移動先の場所</param>
    /// <returns></returns>
    public bool Slant_Check(int before_feet, int after_feet) {
        if (before_feet == Define_Value.ENTRANCE_LAYER_NUMBER ||
            after_feet == Define_Value.ENTRANCE_LAYER_NUMBER) {
            return true;
        }
        return false;
    }
    */
}
