﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタンを押したときの処理
/// </summary>
public class Stair_Command : MonoBehaviour {
    /// <summary>
    /// プレイヤースクリプト
    /// </summary>
    Player player_script;
    /// <summary>
    /// プレイヤーアクション
    /// </summary>
    Player_Action player_action;
    /// <summary>
    /// 階段の移動処理
    /// </summary>
    Action_On_Stair action_stair;

    void Start() {
        player_script = Actor_Manager.Instance.player_script;
        player_action = Actor_Manager.Instance.player_action;
        action_stair = Actor_Manager.Instance.action_stair;
    }

    /// <summary>
    /// 進むボタンを押したときの処理
    /// </summary>
    public void Decide_Progress() {
        var dungeon_manager = Dungeon_Manager.Instance;

        dungeon_manager.NextLevel();
        player_script.move_value = Define_Value.MOVE_VAULE;
        player_action.Set_Action(ePlayer_State.Move);
        action_stair.said_stair_command.Value = false;
    }

    /// <summary>
    /// そのままボタンを押したときの処理
    /// </summary>
    public void Not_Decide_Progress() {
        player_script.move_value = Define_Value.MOVE_VAULE;
        action_stair.said_stair_command.Value = false;
        player_action.Set_Action(ePlayer_State.Move);
    }
}
