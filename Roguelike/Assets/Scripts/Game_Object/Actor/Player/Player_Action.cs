using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour {
    ePlayer_Action action;
    eDirection direction;
    eMode mode;

    [SerializeField]
    Player player;

    [SerializeField]
    Enemy_Action enemy_action;

    Dungeon_Base dungeon_base;

    [SerializeField]
    Player_Status player_status;

    public GameObject stair;

    int action_count; // 汎用変数

    // Use this for initialization
    void Start() {
        direction = eDirection.Down;
        mode      = eMode.Nomal_Mode;
        action    = ePlayer_Action.Move;

        dungeon_base = new Dungeon_Base();

        action_count = 0;
    }

    // Update is called once per frame
    void Update() {
        Run_Action();
    }

    /// <summary>
    /// 毎ループ呼び出す ここでゲームオーバー判定を行う
    /// </summary>
    public void Run_Action() {
        Debug.Log(action);

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

            case ePlayer_Action.Game_Over: break;
        }

        // 体力が 0 以下ならゲームオーバー処理に切り替える
        if (action != ePlayer_Action.Game_Over && player_status.Is_Dead()) {
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
        bool moved = false;
        // 現在位置をPositionに代入
        Vector2 Position = transform.position;

        // 斜め移動モードの切り替え
        if (Input.GetKey("a")) {
            mode = eMode.Diagonally_Mode;
        }
        if (Input.GetKeyUp("a")) {
            mode = eMode.Nomal_Mode;
        }

        // 方向転換モードの切り替え
        if (Input.GetKey("z")) {
            mode = eMode.Change_Direction_Mode;
        }
        if (Input.GetKeyUp("z")) {
            mode = eMode.Nomal_Mode;
        }

        // 通常モード時の移動処理
        if (mode == eMode.Nomal_Mode) {
            if (Input.GetKeyDown("right")) {
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y, 
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y)) {
                    moved = true;
                    direction = eDirection.Right;
                    Position.x += player.SPEED.x;
                    enemy_action.Move_Enemy(player_status);
                }
            }
            else if (Input.GetKeyDown("down")) {
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y, 
                                            gameObject.transform.position.x, gameObject.transform.position.y - 5)) {
                    moved = true;
                    direction = eDirection.Down;
                    Position.y -= player.SPEED.y;
                    //enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("left")) {
                if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y, 
                                            gameObject.transform.position.x - 5, gameObject.transform.position.y)) {
                    moved = true;
                    direction = eDirection.Left;
                    Position.x -= player.SPEED.x;
                    //enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("up")) {
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y, 
                                            gameObject.transform.position.x, gameObject.transform.position.y + 5)) {
                    moved = true;
                    direction = eDirection.Up;
                    Position.y += player.SPEED.y;
                    //enemy_action.Move_Enemy();
                }
            }
        }

        // 斜め移動モード時の移動処理
        else if (mode == eMode.Diagonally_Mode) {
            if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y, 
                                        gameObject.transform.position.x + 5, gameObject.transform.position.y - 5)) {
                if (Input.GetKeyDown("right") && Input.GetKeyDown("up")) {
                    moved = true;
                    direction = eDirection.Upleft;
                    Position.x += player.SPEED.x;
                    Position.y += player.SPEED.y;
                    //enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("right") && Input.GetKeyDown("down")) {
                if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y, 
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y + 5)) {
                    moved = true;
                    direction = eDirection.Downleft;
                    Position.x += player.SPEED.x;
                    Position.y -= player.SPEED.y;
                    //enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("down")) {
                if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y,
                                            gameObject.transform.position.x - 5, gameObject.transform.position.y + 5)) {
                    moved = true;
                    direction = eDirection.Downleft;
                    Position.x -= player.SPEED.x;
                    Position.y -= player.SPEED.y;
                    //enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("up")) {
                if (dungeon_base.Check_Move(gameObject.transform.position.x,     gameObject.transform.position.y, 
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y - 5)) {
                    moved = true;
                    direction = eDirection.Upleft;
                    Position.x -= player.SPEED.x;
                    Position.y += player.SPEED.y;
                    //enemy_action.Move_Enemy();
                }
            }
        }

        // 方向転換モード時の処理
        else if (mode == eMode.Change_Direction_Mode) {
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

    void Action_Attack() {
        switch (direction) {
            //case eDirection.Down:
            //    if(/*player.transform.position.y + 5.0fに敵がいるとき*/) {
            //        //Damage_Calculation(); // ダメージ計算の関数
            //    }
            //    break;
        }
    }

    #endregion

}
