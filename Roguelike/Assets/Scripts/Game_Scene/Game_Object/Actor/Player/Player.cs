using UniRx;
using UnityEngine;

/// <summary>
/// プレイヤー本体のクラス
/// </summary>
public class Player : Actor {
    /// <summary>
    /// 自分のモードを認識する
    /// </summary>
    public ePlayer_Mode player_mode;
    /// <summary>
    /// 自分がダンジョン内でどこを移動しているのかを知る
    /// </summary>
    ePlayer_Where_Move where_move;
    public ePlayer_Where_Move Where_Move { set { where_move = value; } get { return where_move; } }

    /// <summary>
    /// 自分の向いている方向を認識する
    /// </summary>
    public override ReactiveProperty<eDirection> Direction { set { direction = value; } get { return direction; } }
    
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
    /// ２次元配列上の移動距離
    /// </summary>
    int move_value;
    public int Move_Value { set { move_value = value; } get { return move_value; } }

    void Awake() {
        Direction = new ReactiveProperty<eDirection>();
    }

    void Start() {
        // スポーンは部屋の中なのでRoomで初期化
        where_move = ePlayer_Where_Move.Room_Move;

        gameObject.transform.localScale = new Vector2(0.15f, 0.15f);
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
        position.x = new_position.x;
        position.y = new_position.y;
        gameObject.transform.position = new Vector2(position.x, position.y);
    }

    /// <summary>
    /// 現在の座標を取得
    /// </summary>
    /// <returns>現在の座標</returns>
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
