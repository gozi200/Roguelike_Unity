using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの共通のステータスの関係の処理を行うクラス
/// </summary>
public class Actor_Status : MonoBehaviour {
    /// <summary>
    /// 死亡したかを判定する 毎アクターのターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns>死亡していたらtrue</returns>
    public virtual bool Is_Dead(int now_HP) { return true; }
}
