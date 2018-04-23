using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ計算するクラス
/// </summary>
public class Damage_Calculation {
    /// <summary>
    /// ダメージ計算に使う乱数
    /// </summary>
    float random_number;

    /// <summary>
    /// 攻撃のダメージを決定し、計算後のHPを返す
    /// </summary>
    /// <param name="attack">攻撃を撃つ側の攻撃力</param>
    /// <param name="defence">攻撃を受ける側の防御力</param>
    /// <returns>実際に受ける側のHPから減らされる値</returns>
    public float Damage(int attack, int defence) {
        random_number = UnityEngine.Random.Range(87, 112);
        // ダメージ計算
        var damage = attack * random_number / 100 - defence;
        return (int)damage;
    }
}
