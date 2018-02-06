using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーのマネージャークラス
/// </summary>
public class Enemy_Manager : MonoBehaviour {
    [SerializeField]
    static Enemy enemy;

    public static void Set_Enemy(Enemy set_enemy) {
        enemy = set_enemy;
    }

    public static Enemy Get_Enemy() {
        return enemy;
    }
}
