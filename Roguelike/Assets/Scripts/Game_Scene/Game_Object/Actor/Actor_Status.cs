using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの共通のステータスの関係の処理を行うクラス
/// </summary>
public class Actor_Status : MonoBehaviour {
    /// <summary>
    /// 自分のいる部屋番号
    /// </summary>
    public int now_room;

    /// <summary>
    /// 死亡したかを判定する 毎アクターのターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns>死亡していたらtrue</returns>
    public virtual bool Is_Dead(int now_HP) { return true; }

    /// <summary>
    /// どこの部屋にいるのかを調べる
    /// </summary>
    /// <param name="x">知りたいもののx座標</param>
    /// <param name="y">知りたいもののy座標</param>
    public virtual void Where_Floor(int x, int y) {}
}
