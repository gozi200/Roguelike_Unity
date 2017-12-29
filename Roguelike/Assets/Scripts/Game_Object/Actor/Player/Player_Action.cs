using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour {
    Action action;
    Direction direction;
    Mode mode;

    GameObject player;

    int action_count; // 汎用変数

    // Use this for initialization
    void Start() {
        mode = Mode.Nomal_Mode;
        action = Action.Move;
        direction = Direction.Down;

        action_count = 0;

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
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

#region Action.Move時の処理

    void Set_Action(Action set_action) {
        // 識別変数の変更
        action = set_action;

        // 汎用変数のゼロリセット
        action_count = 0;
    }

    void Action_Move() {
        Debug.Log(direction);
        // 現在位置をPositionに代入
        Vector2 Position = transform.position;

        if (Input.GetKey("a")) {
            mode = Mode.Diagonally_Mode;
        }
        if (Input.GetKeyUp("a")) {
            mode = Mode.Nomal_Mode;
        }

        if (Input.GetKey("z")) {
            mode = Mode.Change_Direction_Mode;
        }
        if (Input.GetKeyUp("z")) {
            mode = Mode.Nomal_Mode;
        }

        if (mode == Mode.Nomal_Mode) {
            if (Input.GetKeyDown("right")) {
                Position.x += player.GetComponent<Player>().SPEED.x;
                direction = Direction.Right;
            }
            else if (Input.GetKeyDown("down")) {
                Position.y -= player.GetComponent<Player>().SPEED.y;
                direction = Direction.Down;
            }
            else if (Input.GetKeyDown("left")) {
                Position.x -= player.GetComponent<Player>().SPEED.x;
                direction = Direction.Left;
            }
            else if (Input.GetKeyDown("up")) {
                Position.y += player.GetComponent<Player>().SPEED.y;
                direction = Direction.Up;
            }
        }

        else if (mode == Mode.Diagonally_Mode) {
            if (Input.GetKeyDown("right") && Input.GetKeyDown("up")) {
                Position.x += player.GetComponent<Player>().SPEED.x;
                Position.y += player.GetComponent<Player>().SPEED.y;
                direction = Direction.Upleft;
            }
            else if (Input.GetKeyDown("right") && Input.GetKeyDown("down")) {
                Position.x += player.GetComponent<Player>().SPEED.x;
                Position.y -= player.GetComponent<Player>().SPEED.y;
                direction = Direction.Downleft;
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("down")) {
                Position.x -= player.GetComponent<Player>().SPEED.x;
                Position.y -= player.GetComponent<Player>().SPEED.y;
                direction = Direction.Downleft;
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("up")) {
                Position.x -= player.GetComponent<Player>().SPEED.x;
                Position.y += player.GetComponent<Player>().SPEED.y;
                direction = Direction.Upleft;
            }
        }

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
    }

#endregion



}
