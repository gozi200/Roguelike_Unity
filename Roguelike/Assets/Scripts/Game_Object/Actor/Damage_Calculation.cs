/*
制作者　石倉 

最終編集日 2018/01/10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ計算をするクラス プレイヤー、パートナー、エネミーとすべてのアクターが使用する
/// </summary>
public class Damage_Calculation : MonoBehaviour {
    /// <summary>
    /// ダメージ計算をする。自分、敵、味方で共有
    /// </summary>
    /// <param name = "attack">       攻撃する側攻撃力</param>
    /// <param name = "random_number">乱数</param>
    /// <param name = "defence">      攻撃を受ける側の防御力</param>
    /// <returns>実際にHPから減らされる値</returns>
    public float Damage(int attack, int random_number, float defence) {
        float damage;

        damage = attack * random_number / 100 - defence;

        return damage;
    }
}
