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
    /// エネミークラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// プレイヤークラス
    /// </summary>
    Player player;
    /// <summary>
    /// プレイヤーの移動処理のクラス
    /// </summary>
    Player_Move player_move;
    /// <summary>
    /// プレイヤーのステータス関係のクラス
    /// </summary>
    Player_Status player_status;
    /// <summary>
    /// ダメージ計算を行うクラス
    /// </summary>
    Damage_Calculation damage_calculation;
    /// <summary>
    /// キーの入力を流すクラス
    /// </summary>
    Key_Observer key_observer;

    void Start() {
        enemy  = Enemy.Instance.enemy;
        player = Player.Instance.player;
        key_observer = Game.Instance.key_observer;

        ///Return(Enterキー)が押されたときにバトルメニュー画面を表示する
        key_observer.On_Key_Down_AsObservable()
            .Where(key_code => key_code == KeyCode.Return)
            .Subscribe(_ =>
                player.state = ePlayer_State.Battle_Menu
           ).AddTo(this);
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
        Debug.Log(player.state);
        Debug.Log(player.mode);
        Debug.Log(player.direction);

        switch (player.state) {
            case ePlayer_State.Move:
                player.move.Action_Move();
                break;

            case ePlayer_State.Attack:
                Action_Attack();
                break;

            case ePlayer_State.Battle_Menu:
                Action_Battle_Menu();
                break;

            case ePlayer_State.Game_Over:
                break;
        }

  //      // 体力が 0 以下ならゲームオーバー処理に切り替える
  //      if (action != ePlayer_State.Game_Over && player_status.Is_Dead(player.hit_point) ) {
  //          Set_Action(ePlayer_State.Game_Over);
  //      }
    }

    /// <summary>
    /// 新しく入力されたアクションに切り替える
    /// /// </summary>
    /// <param name="set_action">新しく切り替える状態</param>
    void Set_Action(ePlayer_State set_action) {
        player.state = set_action;
    }

    /// <summary>
    /// バトルメニューの処理
    /// </summary>
    void Action_Battle_Menu() {
        int flag_number = 1;

        if (Input.GetKeyDown("up")) {
            flag_number = 1;
        }
        else if (Input.GetKeyDown("right")) {
            flag_number = 2;
        }
        else if (Input.GetKeyDown("down")) {
            flag_number = 3;
        }
        else if (Input.GetKeyDown("left")) {
            flag_number = 4;
        }

        if (Input.GetKeyDown("return")) {
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
            Run_Action();
        }
    }

    /// <summary>
    /// プレイヤーの攻撃処理
    /// </summary>
    void Action_Attack() {
        switch (player.direction) {
            case eDirection.Down:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y == enemy.Y &&
                        gameObject.transform.position.x == enemy.X) {
                        enemy.enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.attack,
                           UnityEngine.Random.Range(87, 112 + 1),
                           enemy.enemys[i].defence);
                    }
                }
                Set_Action(ePlayer_State.Move);
                break;

            case eDirection.Downleft:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y - 5 == enemy.Y &&
                        gameObject.transform.position.x     == enemy.X) {
                        enemy.enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.attack,
                           UnityEngine.Random.Range(87, 112 + 1),
                           enemy.enemys[i].defence);
                    }
                }
                Set_Action(ePlayer_State.Move);
                break;

            case eDirection.Left:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y     == enemy.Y &&
                        gameObject.transform.position.x - 5 == enemy.X) {
                        enemy.enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.attack,
                           UnityEngine.Random.Range(87, 112 + 1),
                           enemy.enemys[i].defence);
                    }
                }
                Set_Action(ePlayer_State.Move);
                break;

            case eDirection.Upleft:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.Y &&
                        gameObject.transform.position.x - 5 == enemy.X) {
                        enemy.enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.attack,
                           UnityEngine.Random.Range(87, 112 + 1),
                           enemy.enemys[i].defence);
                    }
                }
                Set_Action(ePlayer_State.Move);
                break;

            case eDirection.Up:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.Y &&
                        gameObject.transform.position.x     == enemy.X) {
                        enemy.enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.attack,
                           UnityEngine.Random.Range(87, 112 + 1),
                           enemy.enemys[i].defence);
                    }
                }
                Set_Action(ePlayer_State.Move);
                break;

            case eDirection.Upright:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.Y &&
                        gameObject.transform.position.x + 5 == enemy.X) {
                        enemy.enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.attack,
                           UnityEngine.Random.Range(87, 112 + 1),
                           enemy.enemys[i].defence);
                    }
                }
                Set_Action(ePlayer_State.Move);
                break;

            case eDirection.Right:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y     == enemy.Y &&
                        gameObject.transform.position.x + 5 == enemy.X) {
                        enemy.enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.attack,
                           UnityEngine.Random.Range(87, 112 + 1),
                           enemy.enemys[i].defence);
                    }
                }
                Set_Action(ePlayer_State.Move);
                break;

            case eDirection.Downright:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y - 5 == enemy.Y &&
                        gameObject.transform.position.x + 5 == enemy.X) {
                        enemy.enemys[i].hit_point -= 
                            (int)damage_calculation.Damage(player.attack,
                            UnityEngine.Random.Range(87, 112 + 1),
                            enemy.enemys[i].defence);
                    }
                }
                Set_Action(ePlayer_State.Move);
                break;
        }
    }
}
