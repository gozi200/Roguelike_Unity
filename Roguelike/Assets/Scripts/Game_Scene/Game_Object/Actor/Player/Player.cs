using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// プレイヤー本体のクラス
/// </summary>
public class Player : Actor {
    /// <summary>
    /// 自分のモードを認識する
    /// </summary>
    public ePlayer_Mode player_mode;
    /// <summary>
    /// 自分の状態を認識する
    /// </summary>
    public ePlayer_State player_state;
    /// <summary>
    /// 自分の向いている方向を認識する
    /// </summary>
    public eDirection direction;
    /// <summary>
    /// ２次元配列上の移動距離
    /// </summary>
    int move_value;
    public int Move_Value { get; set; }

    void Start() {
        player_state = ePlayer_State.Move;
        gameObject.transform.localScale = new Vector2(0.4f, 0.4f);
    }

    /// <summary>
    /// 自分の初期座標を設定 
    /// </summary>
    /// <param name="width">スポーン座標(x座標)</param>
    /// <param name="height">スポーン座標(y座標)</param>
    public override void Set_Initialize_Position(int width, int height) {
        position.x = width;
        position.y = height;
        gameObject.transform.position = new Vector2(position.x,position.y);
    }

    /// <summary>
    /// transform.positionに変更をかけた座標を合わせる
    /// </summary>
    /// <param name="new_position">変更後の座標</param>
    public override void Set_Position(Vector2Int new_position) {
        this.position.x = new_position.x;
        this.position.y = new_position.y;
        gameObject.transform.position = new Vector2(position.x, position.y);
    }

    /// <summary>
    /// 現在の座標を取得
    /// </summary>
    /// <returns></returns>
    public override Vector2Int GetPosition() {
        return position;
    }

    /// <summary>
    /// 足元にあるものを設定する
    /// </summary>
    /// <param name="layer_number">今のいる場所のレイヤー番号</param>
    public override void Set_Feet(int layer_number) {
        Feet = layer_number;
    }
}
