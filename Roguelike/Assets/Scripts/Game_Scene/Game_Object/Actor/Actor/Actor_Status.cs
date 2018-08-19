using UnityEngine;

/// <summary>
/// アクターの共通のステータスの関係の処理を行うクラス
/// </summary>
abstract public class Actor_Status {
    /// <summary>
    /// 自分のいる部屋番号
    /// </summary>
    [SerializeField]
    protected int now_room;
    public abstract int Now_Room { set; get; }

    /// <summary>
    /// 死亡したかを判定する 毎アクターのターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns>死亡していたらtrue</returns>
    public abstract bool Is_Dead(int now_HP);

    /// <summary>
    /// どこの部屋にいるのかを調べる
    /// </summary>
    /// <param name="x">知りたいもののx座標</param>
    /// <param name="y">知りたいもののy座標</param>
    public abstract void Where_Room(int x, int y);
}
