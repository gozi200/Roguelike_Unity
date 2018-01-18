using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ計算をするクラス
/// </summary>
public class Damage_Calculation : MonoBehaviour {
    public float Damage(int attack, int random_number, float defence) {
        float damage;

        damage = attack * random_number / 100 - defence;

        return damage;
    }
}
