using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーの行動を行うクラス
/// </summary>
public class Player_Action : MonoBehaviour {
    /// <summary>
    /// エネミークラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// プレイヤークラス
    /// </summary>
    Player player;
    /// プレイヤーの移動処理のクラス
    /// </summary>
    Player_Move player_move;
    /// <summary>
    /// プレイヤーの攻撃処理を管理するクラス
    /// </summary>
    Player_Attack player_attack;
    /// <summary>
    /// 階段に着いたときの処理を行うクラス
    /// </summary>
    Action_On_Stair action_stair;
    /// <summary>
    /// アクター共通で使えるステータス関係のクラス
    /// </summary>
    Actor_Status actor_status;
    /// <summary>
    /// キーの入力を流すクラス
    /// </summary>
    Key_Observer key_observer;
    /// <summary>
    /// マップを2次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D layer;

    void Start() {
        enemy  = Actor_Manager.Instance.enemy_script;
        player = Actor_Manager.Instance.player_script;
        player_move = Actor_Manager.Instance.player_move;
        player_attack = Actor_Manager.Instance.player_attack;
        action_stair = Actor_Manager.Instance.action_stair;
        actor_status = Actor_Manager.Instance.actor_status;

        layer = Dungeon_Manager.Instance.map_layer_2D;
        key_observer = Game.Instance.key_observer;
    }

    /// <summary>
    /// 現在の状態に合った行動をする 毎ループ呼び出す ここでゲームオーバー判定を行う
    /// </summary>
    public void Run_Action() {
        // プレイヤーのステータス関係のクラス 死亡判定に使用
        Player_Status player_status = Actor_Manager.Instance.player_status;

        Debug.Log(player.state);
        Debug.Log(player.mode);
        Debug.Log(player.direction);

        switch (player.state) {
            case ePlayer_State.Move:
                player_move.Action_Move();
                break;
            case ePlayer_State.Attack:
                player_attack.Action_Attack();
                break;
            case ePlayer_State.On_Stair:
                action_stair.Action_Stair();
                break;
            case ePlayer_State.Decide_Command:
                var decide_dungeon= new Decide_Dungeon();
                decide_dungeon.In_Decide();
                break;
            case ePlayer_State.Battle_Menu:
                Action_Battle_Menu();
                break;
            case ePlayer_State.Game_Over:
                SceneManager.LoadScene("Result");
                break;
        }

        // プレイヤーが生きていたら死亡判定をする
        if (player.exist == true) {
            // 体力が 0 以下ならゲームオーバー処理に切り替える
            if (player.state != ePlayer_State.Game_Over && player_status.Is_Dead(player_status.hit_point.Value)) {
                Set_Action(ePlayer_State.Game_Over);
            }
        }
    }

    /// <summary>
    /// 新しく入力されたアクションに切り替える
    /// </summary>
    /// <param name="set_action">新しく切り替える状態</param>
    public void Set_Action(ePlayer_State set_action) {
        player.state = set_action;
    }

    /// <summary>
    /// バトルメニューの処理 //TODO: テスト中
    /// </summary>
    void Action_Battle_Menu() {
        int flag_number = 1;

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            flag_number = 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            flag_number = 2;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            flag_number = 3;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            flag_number = 4;
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            switch (flag_number) {
                case 1:
                    //道具画面を開く
                    break;

                case 2:
                    //発明画面を開く(αではいらない)
                    break;

                case 3:
                    //足元画面を開く
                    break;

                case 4:
                    //ステータスを開く
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Set_Action(ePlayer_State.Move);
        }
    }
}
