using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour {
    Action action;
    Direction direction;
    Mode mode;

    Dungeon_Base dungeon_base;

    GameObject player;

    int action_count; // 汎用変数

    // Use this for initialization
    void Start() {
        mode = Mode.Nomal_Mode;
        action = Action.Move;
        direction = Direction.Down;

        dungeon_base = new Dungeon_Base();

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
        switch (action) {
            case Action.Move:
                Action_Move();
                break;

            case Action.Attack: break;

            case Action.Get_Item: break;

            case Action.Equip: break;

            case Action.Drop_Item: break;

            case Action.Use_Item: break;

            case Action.Step: break;

            case Action.Game_Over: break;
        }

        // 体力が 0 以下ならゲームオーバー処理に切り替える
        if (action != Action.Game_Over && player.GetComponent<Player>().Is_Dead()) {
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

    /// <summary>
    /// 移動に関する処理
    /// </summary>
    #region Action.Move時の処理

    void Action_Move() {
        bool moved = false;
        Debug.Log(direction);
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
               
        }
    }

    /// <summary>
    /// 移動処理がなかった時に常時受け付けるコマンド
    /// </summary>
    void Move_Check_Command() {
        if (Input.GetKeyDown("e")) {
            Set_Action(Action.Equip);
            Run_Action();
        }

        // 移動終了後に自分のいる位置に応じた処理。(例えば、階段に移送したら階層移動。罠に移動したなら罠作動)
    }

#endregion



}
