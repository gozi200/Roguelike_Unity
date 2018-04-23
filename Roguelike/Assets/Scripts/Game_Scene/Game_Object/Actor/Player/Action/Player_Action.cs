using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// プレイヤーの行動を行うクラス
/// </summary>
public class Player_Action : MonoBehaviour {
    /// <summary>
    /// 自身のインスタンス
    /// </summary>
    public Player_Action player_action;
    /// <summary>
    /// エネミークラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// プレイヤーのマネージャークラス
    /// </summary>
    Player_Manager player_manager;
    /// <summary>
    /// プレイヤースクリプト
    /// </summary>
    Player player_script;
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
    /// キーの入力を流すクラス
    /// </summary>
    Key_Observer key_observer;
    /// <summary>
    /// マップを2次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D layer;

    void Start() {
        player_action = gameObject.GetComponent<Player_Action>();
        enemy  = Enemy_Manager.Instance.enemy_script;
        player_manager = Player_Manager.Instance.manager;
        player_script = Player_Manager.Instance.player_script;
        player_move = Player_Manager.Instance.move;
        player_attack = Player_Manager.Instance.attack;
        action_stair = Player_Manager.Instance.action_stair;
        layer = Dungeon_Manager.Instance.map_layer_2D;
        key_observer = Game.Instance.key_observer;

        /////Return(Enterキー)が押されたときにバトルメニュー画面を表示する
        //key_observer.On_Key_Down_AsObservable()
        //    .Where(key_code => key_code == KeyCode.Tab)
        //    .Subscribe(_ =>
        //        player_script.state = ePlayer_State.Battle_Menu
        //   ).AddTo(this);
    }

    /// <summary>
    /// 毎フレームごとに自分の状態とチェックし、それにふさわしい処理に移行する
    /// </summary>
    void Update() {
        Run_Action();
    }

    /// <summary>
    /// 現在の状態に合った行動をする 毎ループ呼び出す ここでゲームオーバー判定を行う
    /// </summary>
    public void Run_Action() {
        Debug.Log(player_script.state);
        Debug.Log(player_script.mode);
        Debug.Log(player_script.direction);

        switch (player_script.state) {
            case ePlayer_State.Move:
                player_move.Action_Move();
                break;
            case ePlayer_State.Attack:
                player_attack.Action_Attack();
                break;
            case ePlayer_State.On_Stair:
                action_stair.Action_Stair();
                break;
            case ePlayer_State.Battle_Menu:
                Action_Battle_Menu();
                break;
            case ePlayer_State.Game_Over:
                break;
        }

        // 体力が 0 以下ならゲームオーバー処理に切り替える
        if (player_script.state != ePlayer_State.Game_Over && player_manager.status.Is_Dead(player_script.hit_point) ) {
            Set_Action(ePlayer_State.Game_Over);
        }
    }

    /// <summary>
    /// 新しく入力されたアクションに切り替える
    /// </summary>
    // TODO:現在は使ってないがカプセル化するときに使う
    /// <param name="set_action">新しく切り替える状態</param>
    public void Set_Action(ePlayer_State set_action) {
        player_script.state = set_action;
    }

    /// <summary>
    /// バトルメニューの処理 //TODO: TEST中
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
