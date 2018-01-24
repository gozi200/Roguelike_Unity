using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour {
    [SerializeField]
    static Enemy enemy;

    //[SerializeField]
    //static Enemy_Status enemy_status;
    //
    //[SerializeField]
    //static Enemy_Action enemy_action;

    public static void Set_Enemy(Enemy set_enemy) {
        enemy = set_enemy;
    }

    //public static void Set_Enemy_Status(Enemy_Status set_enemy_status) {
    //    enemy_status = set_enemy_status;
    //}
    //
    //public static void Set_Enemy_Action(Enemy_Action set_enemy_action) {
    //    enemy_action = set_enemy_action;
    //}

    public static Enemy Get_Enemy() {
        return enemy;
    }
}
