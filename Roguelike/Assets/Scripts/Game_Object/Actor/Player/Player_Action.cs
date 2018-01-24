using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの行動を行うクラス
/// </summary>
public class Player_Action : MonoBehaviour {
    ePlayer_Mode mode;
    eDirection direction;
    ePlayer_Action action;

    [SerializeField]
    Player player;

    [SerializeField]
    Enemy enemy;

    [SerializeField]
    Enemy_Action enemy_action;

    [SerializeField]
    Player_Status player_status;

    Actor_Status actor_status;
    Dungeon_Base dungeon_base;
    Damage_Calculation damage_calculation;

    public GameObject stair;

    int action_count; // 汎用変数

    bool moved = false;

    public void Set_Enemy(Enemy set_enemy) {
        enemy = set_enemy;
    }

    public void Set_Enemy_Action(Enemy_Action set_enemy_action) {
        enemy_action = set_enemy_action;
    }

    // Use this for initialization
    void Start() {
        mode = ePlayer_Mode.Nomal_Mode;
        action = ePlayer_Action.Move;
        direction = eDirection.Down;

        dungeon_base = new Dungeon_Base();
        actor_status = new Actor_Status();
        damage_calculation = new Damage_Calculation();

        action_count = 0;

        enemy = Enemy_Manager.Get_Enemy();
    }

    // Update is called once per frame
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
                Action_Move();
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
        if (action != ePlayer_Action.Game_Over && actor_status.Is_Dead(player.GetComponent<Player>().players[0].hit_point)) {
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

    #region Action.Move時の処理

    

    /// <summary>
    /// 移動に関する処理
    /// </summary>
    void Action_Move() {
        moved = false;

        // 現在位置をPositionに代入
        Vector2 Position = transform.position;

        // 斜め移動モードの切り替え
        if (Input.GetKey("a")) {
            mode = ePlayer_Mode.Diagonally_Mode;
        }
        if (Input.GetKeyUp("a")) {
            mode = ePlayer_Mode.Nomal_Mode;
        }

        // 方向転換モードの切り替え
        if (Input.GetKey("z")) {
            mode = ePlayer_Mode.Change_Direction_Mode;
        }
        if (Input.GetKeyUp("z")) {
            mode = ePlayer_Mode.Nomal_Mode;
        }

        //　 足踏み
        if (Input.GetKey("q")) {
            moved = true;
            enemy_action.Move_Enemy(player_status);
        }

        // 通常モード時の移動処理
        if (mode == ePlayer_Mode.Nomal_Mode) {
            if (Input.GetKeyDown("right")) {
                direction = eDirection.Right;

                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y,
                                            direction)) {
                    moved = true;
                    Position.x += player.speed.x;
                    enemy_action.Move_Enemy(player_status);
                }
            }
            else if (Input.GetKeyDown("down")) {
                direction = eDirection.Down;

                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x, gameObject.transform.position.y - 5,
                                            direction)) {
                    moved = true;
                    Position.y -= player.speed.y;
                    enemy_action.Move_Enemy(player_status);
                }
            }
            else if (Input.GetKeyDown("left")) {
                direction = eDirection.Left;

                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x - 5, gameObject.transform.position.y,
                                            direction)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    enemy_action.Move_Enemy(player_status);
                }
            }
            else if (Input.GetKeyDown("up")) {
                direction = eDirection.Up;

                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x, gameObject.transform.position.y + 5,
                                            direction)) {
                    moved = true;
                    Position.y += player.speed.y;
                    enemy_action.Move_Enemy(player_status);
                }
            }
        }

        // 斜め移動モード時の移動処理
        else if (mode == ePlayer_Mode.Diagonally_Mode) {
            if (Input.GetKeyDown("right") && Input.GetKeyDown("up")) {
                direction = eDirection.Upright;

                if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y,
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y + 5,
                                            direction)) {
                    moved = true;
                    Position.x += player.speed.x;
                    Position.y += player.speed.y;
                    enemy_action.Move_Enemy(player_status);
                }
            }
            else if (Input.GetKeyDown("right") && Input.GetKeyDown("down")) { 
                direction = eDirection.Downright;

                if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y,
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y + 5,
                                            direction)) {
                    moved = true;
                    Position.x += player.speed.x;
                    Position.y -= player.speed.y;
                    enemy_action.Move_Enemy(player_status);
                }
            }
            else if (Input.GetKey("left") && Input.GetKey("down")) {
                direction = eDirection.Downleft;

                if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y,
                                            gameObject.transform.position.x - 5, gameObject.transform.position.y - 5,
                                            direction)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    Position.y -= player.speed.y;
                    enemy_action.Move_Enemy(player_status);
                }
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("up")) {
                direction = eDirection.Upleft;

                if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y,
                                            gameObject.transform.position.x - 5, gameObject.transform.position.y - 5,
                                            direction)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    Position.y += player.speed.y;
                    enemy_action.Move_Enemy(player_status);
                }
            }
        }

        // 方向転換モード時の処理
        else if (mode == ePlayer_Mode.Change_Direction_Mode) {
            if (Input.GetKeyDown("right")) {
                direction = eDirection.Right;
            }
            else if (Input.GetKeyDown("down")) {
                direction = eDirection.Down;
            }
            else if (Input.GetKeyDown("left")) {
                direction = eDirection.Left;
            }
            else if (Input.GetKeyDown("up")) {
                direction = eDirection.Up;
            }
            else if (Input.GetKey("right") && Input.GetKey("up")) {
                direction = eDirection.Upright;
            }
            else if (Input.GetKey("right") && Input.GetKey("down")) {
                direction = eDirection.Downright;
            }
            else if (Input.GetKey("left") && Input.GetKey("down")) {
                direction = eDirection.Downleft;
            }
            else if (Input.GetKey("left") && Input.GetKey("up")) {
                direction = eDirection.Upleft;
            }
        }

        // 現在の位置に加算減算を行ったPositionを代入する
        transform.position = Position;

        // 移動コマンド未入力時はいつでもコマンド操作を受け付ける
        if (moved == false) {
            Move_Check_Command();
        }

        // 階段の移動処理
        if (transform.position == stair.transform.position) {
            Application.LoadLevel("Main");
        }

        // TODO: アイテムの取得処理
    }

    /// <summary>
    /// 移動処理がなかった時に常時受け付けるコマンド
    /// </summary>
    void Move_Check_Command() {
        if (Input.GetKeyDown("return")) {
            Set_Action(ePlayer_Action.Battle_Menu);
            Run_Action();
        }
        else if (Input.GetKeyDown("space")) {
            Set_Action(ePlayer_Action.Attack);
            Run_Action();
        }
    }

    #endregion

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
                            // TODO: actor_statusがnull
                           (int)damage_calculation.Damage(actor_status.Get_Attack(player.GetComponent<Player>().players[0].attack),
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy_action.Move_Enemy(player_status);
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Downleft:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y - 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x     == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(actor_status.Get_Attack(player.GetComponent<Player>().players[0].attack),
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy_action.Move_Enemy(player_status);
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Left:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y     == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x - 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(actor_status.Get_Attack(player.GetComponent<Player>().players[0].attack),
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy_action.Move_Enemy(player_status);
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Upleft:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x - 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(actor_status.Get_Attack(player.GetComponent<Player>().players[0].attack),
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy_action.Move_Enemy(player_status);
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Up:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x     == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(actor_status.Get_Attack(player.GetComponent<Player>().players[0].attack),
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy_action.Move_Enemy(player_status);
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Upright:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y + 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x + 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(actor_status.Get_Attack(player.GetComponent<Player>().players[0].attack),
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy_action.Move_Enemy(player_status);
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Right:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y     == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x + 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -=
                           (int)damage_calculation.Damage(actor_status.Get_Attack(player.GetComponent<Player>().players[0].attack),
                           Random.Range(87, 112 + 1),
                           enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy_action.Move_Enemy(player_status);
                Set_Action(ePlayer_Action.Move);
                break;

            case eDirection.Downright:
                for (int i = 0; i < 1; ++i) {
                    if (gameObject.transform.position.y - 5 == enemy.GetComponent<Enemy>().transform.position.y &&
                        gameObject.transform.position.x + 5 == enemy.GetComponent<Enemy>().transform.position.x) {
                        enemy.GetComponent<Enemy>().enemys[i].hit_point -= 
                            (int)damage_calculation.Damage(actor_status.Get_Attack(player.GetComponent<Player>().players[0].attack),
                            Random.Range(87, 112 + 1),
                            enemy.GetComponent<Enemy>().enemys[i].defence);
                        Debug.Log(enemy.GetComponent<Enemy>().enemys[i].hit_point);
                    }
                }
                enemy_action.Move_Enemy(player_status);
                Set_Action(ePlayer_Action.Move);
                break;
        }
    }
}

#endregion