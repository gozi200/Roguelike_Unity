using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// アクターの共通の動きを管理するクラス
/// </summary>
public class Actor_Manager : Unique_Component<Actor_Manager> {
    /// <summary>
    /// アクター共通のステータスを関係の処理を管理するクラス
    /// </summary>
    public Actor_Status actor_status;
    /// <summary>
    /// アクター共通の行動を管理するクラス
    /// </summary>
    /// <summary>
    /// プレイヤー本体のクラス
    /// </summary>
    public Player player;

    void Start() {
        player = new Player();
    }

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
    public virtual void Where_Floor(int x, int y) { }
}
