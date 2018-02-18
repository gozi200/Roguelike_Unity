/*
    制作者 石倉

    最終更新日 2018/02/08
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーの移動処理
/// </summary>
public class Player_Move : MonoBehaviour {
    /// <summary>
    /// 移動が済んだかどうかのフラグ
    /// </summary>
    bool moved = false;

    /// <summary>
    /// プレイヤーのいるx座標
    /// </summary>
    int player_width;

    /// <summary>
    /// プレイヤーのいるy座標
    /// </summary>
    int player_height;

    ePlayer_Mode mode;

    [SerializeField]
    Enemy enemy;

    [SerializeField]
    Player player;

    public GameObject stair;

    static Dungeon_Base dungeon_base;


    void Start() {
        mode = ePlayer_Mode.Nomal_Mode;

        player_width = player.GetComponent<Object_Coordinates>().Width;

        player_height = player.GetComponent<Object_Coordinates>().Height;
    }

    /// <summary>
    /// Dungeon_Baseにセットする
    /// </summary>
    /// <param name="set_dungeon_base">情報を持ったDungeon_Base</param>
    public static void Set_Dungeon_Base(Dungeon_Base set_dungeon_base) {
        dungeon_base = set_dungeon_base;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Action_Move() {
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

        // 足踏み
        if (Input.GetKey("q")) {
            moved = true;
            enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
        }
 
        // 通常モード時の移動処理
        if (mode == ePlayer_Mode.Nomal_Mode) {
            Debug.Log(gameObject);
            if (Input.GetKeyDown("right")) {
                player.GetComponent<Player_Action>().direction = eDirection.Right;

                if (Dungeon_Base.Is_Check_Move(player_height, player_width + 1, 2)) {
                    moved = true;
                    Position.x += player.speed.x;
                    enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                }
            }
            else if (Input.GetKeyDown("down")) {
                player.GetComponent<Player_Action>().direction = eDirection.Down;

                if (Dungeon_Base.Is_Check_Move(player_height - 1, player_width, 2)) {
                    moved = true;
                    Position.y -= player.speed.y;
                    enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                }
            }
            else if (Input.GetKeyDown("left")) {
                player.GetComponent<Player_Action>().direction = eDirection.Left;

                if (Dungeon_Base.Is_Check_Move(player_height, player_width - 1, 2)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                }
            }
            else if (Input.GetKeyDown("up")) {
                player.GetComponent<Player_Action>().direction = eDirection.Up;

                if (Dungeon_Base.Is_Check_Move(player_height + 1, player_width + 1, 2)) {
                    moved = true;
                    Position.y += player.speed.y;
                    enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                }
            }
        }

        // 斜め移動モード時の移動処理
        else if (mode == ePlayer_Mode.Diagonally_Mode) {
            if (Input.GetKeyDown("right") && Input.GetKeyDown("up")) {
                player.GetComponent<Player_Action>().direction = eDirection.Upright;

                if (Dungeon_Base.Is_Check_Move(player_height + 1, player_width + 1, 2)) {
                    moved = true;
                    Position.x += player.speed.x;
                    Position.y += player.speed.y;
                    enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                }
            }
            else if (Input.GetKeyDown("right") && Input.GetKeyDown("down")) {
                player.GetComponent<Player_Action>().direction = eDirection.Downright;

                if (Dungeon_Base.Is_Check_Move(player_height - 1, player_width + 1, 2)) {
                    moved = true;
                    Position.x += player.speed.x;
                    Position.y -= player.speed.y;
                    enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                }
            }
            else if (Input.GetKey("left") && Input.GetKey("down")) {
                player.GetComponent<Player_Action>().direction = eDirection.Downleft;

                if (Dungeon_Base.Is_Check_Move(player_height - 1, player_width - 1, 2)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    Position.y -= player.speed.y;
                    enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                }
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("up")) {
                player.GetComponent<Player_Action>().direction = eDirection.Upleft;

                if (Dungeon_Base.Is_Check_Move(player_height + 1, player_width - 1, 2)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    Position.y += player.speed.y;
                    enemy.GetComponent<Enemy_Action>().Move_Enemy(player.GetComponent<Player_Status>());
                }
            }
        }

        // 方向転換モード時の処理
        else if (mode == ePlayer_Mode.Change_Direction_Mode) {
            if (Input.GetKeyDown("right")) {
                player.GetComponent<Player_Action>().direction = eDirection.Right;
            }
            else if (Input.GetKeyDown("down")) {
                player.GetComponent<Player_Action>().direction = eDirection.Down;
            }
            else if (Input.GetKeyDown("left")) {
                player.GetComponent<Player_Action>().direction = eDirection.Left;
            }
            else if (Input.GetKeyDown("up")) {
                player.GetComponent<Player_Action>().direction = eDirection.Up;
            }
            else if (Input.GetKey("right") && Input.GetKey("up")) {
                player.GetComponent<Player_Action>().direction = eDirection.Upright;
            }
            else if (Input.GetKey("right") && Input.GetKey("down")) {
                player.GetComponent<Player_Action>().direction = eDirection.Downright;
            }
            else if (Input.GetKey("left") && Input.GetKey("down")) {
                player.GetComponent<Player_Action>().direction = eDirection.Downleft;
            }
            else if (Input.GetKey("left") && Input.GetKey("up")) {
                player.GetComponent<Player_Action>().direction = eDirection.Upleft;
            }
        }

        // 現在の位置に加算減算を行ったPositionを代入する
        transform.position = Position;

        // TODO: アイテムの取得処理

        // 移動コマンド未入力時はいつでもコマンド操作を受け付ける
        if (!moved) {
            Move_Check_Command();
        }
        // 移動していればターンのカウントを進める
        else if (moved) {
            Add_Player_Turn();
        }
    }

    void Add_Player_Turn() {
            player.GetComponent<Player_Status>().Turn();
    }


    void Move_Check_Command() {
        if (Input.GetKey("return")) {
            player.GetComponent<Player_Action>().action = ePlayer_Action.Battle_Menu;
        }

        else if (Input.GetKey("space")) {
            player.GetComponent<Player_Action>().action = ePlayer_Action.Attack;
        }
    }
}
