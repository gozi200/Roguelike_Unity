using UniRx;
using UnityEngine;

/// <summary>
/// エネミー本体のクラス
/// </summary>
public class Enemy : Actor {
    /// <summary>
    /// 現在の座標の足元にあるもの
    /// </summary>
    public override int Feet { set { feet = value; } get { return feet; } }
    /// <summary>
    /// 死亡判定 trueで死亡
    /// </summary>
    public override bool Exist { set { exist = value; } get { return exist; } }
    /// <summary>
    /// 自分の番号
    /// </summary>
    public override int My_Number { set { my_number = value; } get { return my_number; } }
    /// <summary>
    /// 自分のいる座標
    /// </summary>
    public override Vector2Int Position { set { position = value; } get { return position; } }
    /// <summary>
    /// 自分の向いている方向を認識する
    /// </summary>
    public override ReactiveProperty<eDirection> Direction { set { direction = value; } get { return direction; } }

    /// <summary>
    /// 自分が迷子になっているか。
    /// </summary>
    bool is_lost_myself;
    public bool Is_Lost_Myself { set { is_lost_myself = value; } get { return is_lost_myself; } }

    /// <summary>
    /// 表示する画像
    /// </summary>
    SpriteRenderer sprite_renderer;

    /// <summary>
    /// エネミーのモードを判断する
    /// </summary>
    public eEnemy_Mode mode;

    void Awake() {
        direction = new ReactiveProperty<eDirection>();
    }

    void Start() {
        Exist = true;
        Is_Lost_Myself = false;
        // 部屋の中でしかスポーンしない
        mode = eEnemy_Mode.Move_Room_Mode;
    
        gameObject.AddComponent<SpriteRenderer>();

        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sortingOrder = Define_Value.ENEMY_LAYER_NUMBER;
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
        position.x = new_position.x;
        position.y = new_position.y;
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
