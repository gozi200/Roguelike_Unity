using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ計算するクラス
/// </summary>
public static class Damage_Calculation {
    /// <summary>
    /// 攻撃のダメージを決定し、計算後のHPを返す
    /// </summary>
    /// <param name="attack">攻撃を撃つ側の攻撃力</param>
    /// <param name="defence">攻撃を受ける側の防御力</param>
    /// <returns>実際に受ける側のHPから減らされる値</returns>
    public static int Damage(int attack, int defence) {
        var random_number = UnityEngine.Random.Range(87, 112);
        // ダメージ計算
        float damage = attack * random_number / 100 - defence;

        // 防御力高すぎて攻撃したら回復。なんてことがないように
        if (damage < 0) {
            damage = 0;
        }

        return (int)damage;
    }
}
