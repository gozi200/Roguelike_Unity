using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミー本体のクラス
/// </summary>
public class Enemy : Actor {
    /// <summary>
    /// 自分の番号
    /// </summary>
    int my_number;
    public int My_Number{ get; set; }

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

    void Start() {
        Exist = true;
        // 部屋の中でしかスポーンしない
        mode = eEnemy_Mode.Move_Floor_Mode;
    
        gameObject.AddComponent<SpriteRenderer>();
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sortingOrder = Define_Value.ENEMY_LAYER_NUMBER;
        sprite_renderer.sprite = Resources.Load<Sprite>("Enemy/Enemys");
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
        gameObject.transform.position = new Vector2(position.x, position.y);
    }

    /// <summary>
    /// transform.positionに変更をした座標を合わせる
    /// </summary>
    /// <param name="new_position">変更後の座標</param>
    public override void Set_Position(Vector2Int new_position) {
        this.position.x = new_position.x;
        this.position.y = new_position.y;
        gameObject.transform.position = new Vector2(position.x, position.y);
    }

    /// <summary>
    /// 現在のポジションを取得する
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
