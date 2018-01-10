using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour {
    Action action;
    Direction direction;
    Mode mode;

    Player_Status player_status;
    Enemy_Turn enemy_turn;

    GameObject player;

    public GameObject stair;

    int action_count; // 汎用変数

    // Use this for initialization
    void Start() {
        mode = Mode.Nomal_Mode;
        action = Action.Move;
        direction = Direction.Down;

        player_status = new Player_Status();
        enemy_turn = new Enemy_Turn();

        action_count = 0;

        player = GameObject.Find("Player");
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
            case Action.Move:
                Action_Move();
                break;

            case Action.Attack:
                Action_Attack();
                break;

            case Action.Battle_Menu:
                Action_Battle_Menu();
                break;

            case Action.Game_Over: break;
        }

        // 体力が 0 以下ならゲームオーバー処理に切り替える
        if (action != Action.Game_Over && player_status.Is_Dead()) {
            Set_Action(Action.Game_Over);
        }
    }

    /// <summary>
    /// 現在の行っているアクションに切り替える
    /// /// </summary>
    /// <param name="set_action"></param>
    void Set_Action(Action set_action) {
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
        Debug.Log(direction);
        Debug.Log(moved);
        // 現在位置をPositionに代入
        Vector2 Position = transform.position;

        // 斜め移動モードの切り替え
        if (Input.GetKey("a")) {
            mode = Mode.Diagonally_Mode;
        }
        if (Input.GetKeyUp("a")) {
            mode = Mode.Nomal_Mode;
        }

        // 方向転換モードの切り替え
        if (Input.GetKey("z")) {
            mode = Mode.Change_Direction_Mode;
        }
        if (Input.GetKeyUp("z")) {
            mode = Mode.Nomal_Mode;
        }

        // 通常モード時の移動処理
        if (mode == Mode.Nomal_Mode) {
            if (Input.GetKeyDown("right")) {
                Position.x += player.GetComponent<Player>().SPEED.x;
                direction = Direction.Right;
                moved = true;
            }
            else if (Input.GetKeyDown("down")) {
                Position.y -= player.GetComponent<Player>().SPEED.y;
                direction = Direction.Down;
                moved = true;
            }
            else if (Input.GetKeyDown("left")) {
                Position.x -= player.GetComponent<Player>().SPEED.x;
                direction = Direction.Left;
                moved = true;
            }
            else if (Input.GetKeyDown("up")) {
                Position.y += player.GetComponent<Player>().SPEED.y;
                direction = Direction.Up;
                moved = true;
            }
        }

        // 斜め移動モード時の移動処理
        else if (mode == Mode.Diagonally_Mode) {
            if (Input.GetKeyDown("right") && Input.GetKeyDown("up")) {
                Position.x += player.GetComponent<Player>().SPEED.x;
                Position.y += player.GetComponent<Player>().SPEED.y;
                direction = Direction.Upleft;
                moved = true;
                Debug.Log(moved);
            }
            else if (Input.GetKeyDown("right") && Input.GetKeyDown("down")) {
                Position.x += player.GetComponent<Player>().SPEED.x;
                Position.y -= player.GetComponent<Player>().SPEED.y;
                direction = Direction.Downleft;
                moved = true;
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("down")) {
                Position.x -= player.GetComponent<Player>().SPEED.x;
                Position.y -= player.GetComponent<Player>().SPEED.y;
                direction = Direction.Downleft;
                moved = true;
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("up")) {
                Position.x -= player.GetComponent<Player>().SPEED.x;
                Position.y += player.GetComponent<Player>().SPEED.y;
                direction = Direction.Upleft;
                moved = true;
            }
        }

        // 方向転換モード時の処理
        else if (mode == Mode.Change_Direction_Mode) {
            if (Input.GetKeyDown("right")) {
                direction = Direction.Right;
            }
            else if (Input.GetKeyDown("down")) {
                direction = Direction.Down;
            }
            else if (Input.GetKeyDown("left")) {
                direction = Direction.Left;
            }
            else if (Input.GetKeyDown("up")) {
                direction = Direction.Up;
            }
        }

        // 現在の位置に加算減算を行ったPositionを代入する
        transform.position = Position;

        // 移動コマンド未入力時はいつでもコマンド操作を受け付ける
        if (moved == false) {
            Move_Check_Command();
        }

        if (transform.position == stair.transform.position) {
            Application.LoadLevel("Main");
        }
    }

    /// <summary>
    /// 移動処理がなかった時に常時受け付けるコマンド
    /// </summary>
    void Move_Check_Command() {
        if (Input.GetKeyDown("return")) {
            Set_Action(Action.Battle_Menu);
            Run_Action();
        }
        else if (Input.GetKeyDown("space")) {
            Set_Action(Action.Attack);
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
            Set_Action(Action.Move);
            Run_Action();
        }
    }

    #endregion

    #region Action.Attak時の処理

    void Action_Attack() {
        
    }

    #endregion

}
