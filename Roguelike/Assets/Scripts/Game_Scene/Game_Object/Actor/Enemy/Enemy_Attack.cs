﻿using UnityEngine;

/// <summary>
/// エネミーの攻撃処理
/// </summary>
public class Enemy_Attack {
    /// <summary>
    /// エネミー本体のクラス
    /// </summary>
    Enemy enemy_script;
    /// <summary>
    /// マップを２次元で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;
    /// <summary>
    /// 攻撃されるプレイヤーの本体
    /// </summary>
    GameObject player;

    public void Initialize() {
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
        player = GameObject.Find("Player");
    }

    /// <summary>
    /// 実際に攻撃処理を行い、ゲームを進める
    /// </summary>
    /// <param name="enemy">攻撃をおこなうエネミー</param>
    /// <param name="direction">攻撃を繰り出す方向</param>
    public void Attack_Process(GameObject enemy, eDirection direction) {
        // プレイヤーのステータスを取得
        var player_status = Player_Manager.Instance.player_status;
        // エネミーのステータスを取得
        var enemy_status = enemy.GetComponent<Enemy_Controller>().enemy_status;
        var my_status = enemy.GetComponent<Enemy_Controller>().enemy_status.my_status;

        // 攻撃する方向を向く
        enemy.GetComponent<Enemy>().Direction = direction;

        // プレイヤーにダメージを与える
        player_status.Hit_Point.Value -= Damage_Calculation.Damage(my_status.Attack,
                                                                   player_status.Defence);
        // ダメージのログを流す
        Message_Window_Manager.Enemy_Attack_Log(my_status, player_status,
                                    Damage_Calculation.Damage(my_status.Attack,
                                                              player_status.Defence));
    }
}