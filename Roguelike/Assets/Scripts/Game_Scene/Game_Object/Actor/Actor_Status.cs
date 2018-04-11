/*
制作者 石倉

最終編集日 2018/02/08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの共通のステータスの変更を行うクラス
/// </summary>
public class Actor_Status : MonoBehaviour {
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
