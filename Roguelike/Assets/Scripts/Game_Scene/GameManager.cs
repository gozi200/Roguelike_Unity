﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームシーンのループを回す
/// </summary>
public class GameManager : Unique_Component<GameManager> {
    /// <summary>
    /// 拠点用マネージャーのオブジェクト
    /// </summary>
    GameObject base_manager_object;
    /// <summary>
    /// 拠点のマネージャースクリプト
    /// </summary>
    Base_Manager base_manager;

    /// <summary>
    /// ゲームの状態を取得
    /// </summary>
    public eGame_State game_state;

    void Start() {
        game_state = eGame_State.Create_Base;
        base_manager_object = GameObject.Find("Base_Manager");
        base_manager = base_manager_object.GetComponent<Base_Manager>();
    }

    void Update() {
        Game_Loop(game_state);
    }

    /// <summary>
    /// ゲームループの状態を変える。シーン変更時に呼ばれる
    /// </summary>
    /// <param name = "set_state">遷移後の状態</param>
    public void Set_Game_State(eGame_State set_state) {
        game_state = set_state;
    }

    /// <summary>
    /// ゲームの状態を管理
    /// </summary>
    /// <param name = "game_state">新しい状態</param>
    void Game_Loop(eGame_State game_state) {
        switch (game_state) {
            case eGame_State.Create_Base:
                // 拠点を作る
                base_manager.Create_Base();
                Set_Game_State(eGame_State.Player_Turn);
                break;
            case eGame_State.Create_Dungeon:
                var dungeon_manager = Dungeon_Manager.Instance;
                var decide_dungeon = GameObject.Find("Decide_Dungeon").GetComponent<Decide_Dungeon>();
                // ダンジョンを作る
                dungeon_manager.Create(decide_dungeon.dungeon_data.level);
                Set_Game_State(eGame_State.Player_Turn);
                break;
            case eGame_State.Player_Turn:
                var player_action = Actor_Manager.Instance.player_action;
                // プレイヤーのターン
                player_action.Run_Action();
                break;
            case eGame_State.Partner_Turn:
                // パートナーの行動
                Set_Game_State(eGame_State.Enemy_Trun);
                break;
            case eGame_State.Enemy_Trun:
                var enemy_action = Actor_Manager.Instance.enemy_action;
                // エネミーの行動
                enemy_action.Action();
                Set_Game_State(eGame_State.Dungeon_Turn);
                break;
            case eGame_State.Dungeon_Turn:
                // ダンジョンのターン(敵のスポーンなど)
                var dungeon_generator = Dungeon_Manager.Instance.dungeon_generator;
                dungeon_generator.Turn_Tick();
                Set_Game_State(eGame_State.Player_Turn);
                break;
        }
    }
}
