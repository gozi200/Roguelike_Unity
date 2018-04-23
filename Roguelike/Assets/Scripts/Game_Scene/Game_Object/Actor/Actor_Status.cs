using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの共通のステータスの関係の処理を行うクラス
/// </summary>
public class Actor_Status {
    /// <summary>
    /// 死亡判定
    /// </summary>
    /// <param name = "now_HP">アクターの現在の体力</param>
    /// <returns>死亡したらtrue 生きていたらfalse</returns>
    public bool Is_Dead(int now_HP) {
        if (now_HP <= 0) {
            now_HP = 0;
            return true;
        }
        return false;
    }
}
