using UnityEngine;

/// <summary>
/// アクターの基底クラス
/// </summary>
public abstract class Actor : MonoBehaviour {
    /// <summary>
    /// 現在の座標の足元にあるもの
    /// </summary>
    [SerializeField]
    protected int feet;
    public abstract int Feet { set; get; }
    /// <summary>
    /// 死亡判定 trueで死亡
    /// </summary>
    protected bool exist;
    public abstract bool Exist { set ; get; }
    /// <summary>
    /// 自分の番号
    /// </summary>
    protected int my_number;
    public abstract int My_Number { set; get; }
    /// <summary>
    /// 自分のいる座標
    /// </summary>
    protected Vector2Int position;
    public abstract Vector2Int Position { set; get; }
    /// <summary>
    /// エネミーの向いている方向を判断する
    /// </summary>
    protected eDirection direction;
    public abstract eDirection Direction { set; get; }

    /// <summary>
    /// 自分の初期座標を設定
    /// </summary>
    /// <param name="width">スポーン座標</param>
    /// <param name="height"><スポーン座標/param>
    public abstract void Set_Initialize_Position(int width, int height);

    /// <summary>
    /// transform.positionに合わせる
    /// </summary>
    /// <param name="new_position">移動後の座標</param>
    public abstract void Set_Position(Vector2Int new_position);

    /// <summary>
    /// 現在のポジションを取得する
    /// </summary>
    public abstract Vector2Int GetPosition();

    /// <summary>
    /// 足元にあるものを設定する
    /// </summary>
    /// <param name="layer_number">足元のレイヤー番号</param>
    public abstract void Set_Feet(int layer_number);
}
