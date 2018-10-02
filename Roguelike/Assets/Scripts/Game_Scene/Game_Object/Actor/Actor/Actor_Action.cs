using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクター共通の行動を管理クラス
/// </summary>
public static class Actor_Action {
    /// <summary>
    /// 斜めチェック用
    /// </summary>
    static Dictionary<eDirection, Tuple<Vector2Int, Vector2Int>> slant_direction;

    static Actor_Action() {
        slant_direction = new Dictionary<eDirection, Tuple<Vector2Int, Vector2Int>>();

        slant_direction[eDirection.Upright]   = new Tuple<Vector2Int, Vector2Int>(new Vector2Int( 1, 0), new Vector2Int(0,  1));
        slant_direction[eDirection.Downright] = new Tuple<Vector2Int, Vector2Int>(new Vector2Int( 1, 0), new Vector2Int(0, -1));
        slant_direction[eDirection.Downleft]  = new Tuple<Vector2Int, Vector2Int>(new Vector2Int(-1, 0), new Vector2Int(0, -1));
        slant_direction[eDirection.Upleft]    = new Tuple<Vector2Int, Vector2Int>(new Vector2Int(-1, 0), new Vector2Int(0,  1));
    }

    /// <summary>
    /// 移動可能かを判断
    /// </summary>
    /// <param name="my_layer">自分のレイヤーナンバー</param>
    /// <param name="new_position">移動先の座標</param>
    /// <returns>移動可能であればtrue</returns>
    public static bool Move_Check(int my_layer, int new_position) {
        return my_layer > new_position;
    }

    /// <summary>
    /// 斜め方向に攻撃、移動が可能かどうか。向いている方向+-45度の位置のいずれかに壁があったら移動不可
    /// </summary>
    /// <param name="actor">判断するアクター</param>
    /// <param name="direction">アクターの向いてる方向</param>
    /// <returns>斜め方向への攻撃、移動が可能であればtrue</returns>
    public static bool Slant_Action_Check(Actor actor, eDirection direction) {
        var map_layer = Dungeon_Manager.Instance.map_layer_2D;
        var check_wall1 = slant_direction[direction].Item1 + actor.Position;
        var check_wall2 = slant_direction[direction].Item2 + actor.Position;
        return !(map_layer.Is_Wall(check_wall1) || map_layer.Is_Wall(check_wall2));
    }
}
