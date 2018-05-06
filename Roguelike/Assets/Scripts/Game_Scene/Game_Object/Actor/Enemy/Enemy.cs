using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミー本体のクラス
/// </summary>
public class Enemy : Actor {
    /// <summary>
    /// 表示する画像を編集する
    /// </summary>
    SpriteRenderer sprite_renderer;

    /// <summary>
    /// エネミーのモードを判断する
    /// </summary>
    public eEnemy_Mode mode;
    /// <summary>
    /// エネミーの向いている方向を判断する
    /// </summary>
    public eDirection direction;

    /// <summary>
    /// 種類ごとにエネミーを格納する
    /// </summary>
    public List<Enemy_Status> enemy_type = new List<Enemy_Status>();

    void Start() {
        exist = true;
        gameObject.AddComponent<SpriteRenderer>();
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sortingOrder = Define_Value.ENEMY_LAYER_NUMBER;
        sprite_renderer.sprite = Resources.Load<Sprite>("Enemy/Wyvern");

        position = transform.position;
        gameObject.transform.localScale = new Vector2(0.4f, 0.4f);
    }

    /// <summary>
    /// 自分の初期座標をマス情報を管理するクラスに知らせる
    /// </summary>
    /// <param name="width">スポーン座標(x座標)</param>
    /// <param name="height">スポーン座標(y座標)</param>
    public override void Set_Initialize_Position(int width, int height) {
        position.x = width;
        position.y = height;
        gameObject.transform.position = position;
    }

    /// <summary>
    /// transform.positionに変更をした座標を合わせる
    /// </summary>
    /// <param name="new_position">変更後の座標</param>
    public override void Set_Position(Vector2 new_position) {
        this.position.x = new_position.x;
        this.position.y = new_position.y;
        gameObject.transform.position = this.position;
    }

    /// <summary>
    /// 現在のポジションを取得する
    /// </summary>
    /// <returns></returns>
    public override Vector2 Get_Position() {
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
