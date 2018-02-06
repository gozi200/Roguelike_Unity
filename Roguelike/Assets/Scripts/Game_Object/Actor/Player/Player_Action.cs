using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの行動を行うクラス
/// </summary>
public class Player_Action : MonoBehaviour {
    public eDirection direction;

    public ePlayer_Action action;

    [SerializeField]
    Player player;

    [SerializeField]
    Enemy enemy;

    [SerializeField]
    Actor_Action actor_action;

    Damage_Calculation damage_calculation;

    int action_count; // 汎用変数

    // Use this for initialization
    void Start() {

        action = ePlayer_Action.Move;
        direction = eDirection.Down;

        action_count = 0;

        //enemy = Enemy_Manager.Get_Enemy();
//        player = Player_Manager.Get_Player();
    }

    void Update() {
        Run_Action();

        Debug.Log(action);
        Debug.Log(direction);
        // TODO: 後に追加したものに上書きされている
    }

    /// <summary>
    /// 毎ループ呼び出す ここでゲームオーバー判定を行う
    /// </summary>
    public void Run_Action() {
        switch (action) {
            case ePlayer_Action.Move:
                player.GetComponent<Move>().Action_Move();
                break;

            case ePlayer_Action.Attack:
                Action_Attack();
                break;

            case ePlayer_Action.Battle_Menu:
                Action_Battle_Menu();
                break;

            case ePlayer_Action.Game_Over:
                break;
        }

        // 体力が 0 以下ならゲームオーバー処理に切り替える
        Debug.Log(player.GetComponent<Player>().hit_point);
        if (action != ePlayer_Action.Game_Over && player.GetComponent<Player_Status>().Is_Dead(player.GetComponent<Player>().hit_point) ) {
            Set_Action(ePlayer_Action.Game_Over);
        }
    }

    /// <summary>
    /// 現在の行っているアクションに切り替える
    /// /// </summary>
    /// <param name="set_action"></param>
    void Set_Action(ePlayer_Action set_action) {
        // 識別変数の変更
        action = set_action;

        // 汎用変数のゼロリセット
        action_count = 0;
    }

    #region Action.Battle_Manu時の処理

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

        if (Input.GetKeyDown("escape")) {
            Set_Action(ePlayer_Action.Move);
            Run_Action();
        }
    }

    #endregion

    #region Action.Attak時の処理

    /// <summary>
    /// プレイヤーの攻撃処理
    /// </summary>
    void Action_Attack() {
        switch (direction) {
            case eDirection.Down:
                for (int i = 0; i < 1; ++i) { //TODO: テスト　forで回すループの回数と攻撃対象の座標を修正する
                    if (gameObject.transform.position.y - 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x     == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.GetComponent<Player>().attack,
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Downleft:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y - 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x     == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.GetComponent<Player>().attack,
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Left:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y     == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x - 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.GetComponent<Player>().attack,
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Upleft:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x - 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.GetComponent<Player>().attack,
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Up:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x     == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.GetComponent<Player>().attack,
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Upright:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x + 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.GetComponent<Player>().attack,
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Right:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y     == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x + 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(player.GetComponent<Player>().attack,
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Downright:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y - 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x + 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -= 
                            (int)damage_calculation.Damage(player.GetComponent<Player>().attack,
                            Random.Range(87, 112 + 1),
                            enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                Set_Action(ePlayer_Action.Move);
                break;
        }
    }
}

#endregion