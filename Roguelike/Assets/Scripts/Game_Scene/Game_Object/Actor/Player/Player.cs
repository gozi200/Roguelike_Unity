using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー本体のクラス
/// </summary>
public class Player : Actor {
    /// <summary>
    /// プレイヤーのステータス関係を管理するクラス
    /// </summary>
    public Player_Status status;
    /// <summary>
    /// 自分のモードを認識する
    /// </summary>
    public ePlayer_Mode mode;
    /// <summary>
    /// 自分の状態を認識する
    /// </summary>
    public ePlayer_State state;
    /// <summary>
    /// 自分の向いている方向を認識する
    /// </summary>
    public eDirection direction;
    /// <summary>
    /// ２次元配列上の移動距離
    /// </summary>
    public int move_value { get; set; }

    void Start() {
        gameObject.transform.localScale = new Vector2(0.4f, 0.4f);
        status = Actor_Manager.Instance.player_status;
    }

    /// <summary>
    /// 自分の初期座標を設定 
    /// </summary>
    /// <param name="width">スポーン座標(x座標)</param>
    /// <param name="height">スポーン座標(y座標)</param>
    public override void Set_Initialize_Position(int width, int height) {
        position.x = width;
        position.y = height;
        gameObject.transform.position = position;
    }

    /// <summary>
    /// transform.positionに変更をかけた座標を合わせる
    /// </summary>
    /// <param name="new_position">変更後の座標</param>
    public override void Set_Position(Vector2 new_position) {
        this.position.x = new_position.x;
        this.position.y = new_position.y;
        gameObject.transform.position = this.position;
    }

    /// <summary>
    /// 現在の座標を取得
    /// </summary>
    /// <returns></returns>
    public override Vector2 GetPosition() {
        return position;
    }

    /// <summary>
    /// 足元にあるものを設定する
    /// </summary>
    /// <param name="layer_number">今のいる場所のレイヤー番号</param>
    public override void Set_Feet(int layer_number) {
        feet = layer_number;
    }
}
