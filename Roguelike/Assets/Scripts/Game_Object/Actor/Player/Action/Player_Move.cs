using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 移動処理
/// </summary>
public class Player_Move : MonoBehaviour {
    /// <summary>
    /// プレイヤーのいるx座標
    /// </summary>
    int player_width;

    /// <summary>
    /// プレイヤーのいるy座標
    /// </summary>
    int player_height;

    /// <summary>
    /// 死亡フラグ 死んでいたらtrue
    /// </summary>
    bool moved = false;

    ePlayer_Mode mode;

    [SerializeField]
    Enemy enemy;

    GM.Player player;

    Player_Action player_action;

    Player_Status player_status;

    Enemy_Action enemy_action;

    public GameObject stair;

    static Dungeon_Base dungeon_base;

    static Dungeon_Generator dungeon_generator;

    void Awake() {
        player = GM.Instance.player;
    }

    void Start() {
        mode = ePlayer_Mode.Nomal_Mode;

      //  player_width = player.GetComponent<Object_Coordinates>().Width;
      //
      //  player_height = player.GetComponent<Object_Coordinates>().Height;
      //
      //  player_status = player.GetComponent<Player_Status>();
      //
      //  player_action = player.GetComponent<Player_Action>();

        enemy_action = enemy.GetComponent<Enemy_Action>();
    }

    public static void Set_Dungeon_Base(Dungeon_Base set_dungeon_base) {
        dungeon_base = set_dungeon_base;

    }

    public static void Set_Dungeon_Generator(Dungeon_Generator set_dungeon_generator) {
        dungeon_generator = set_dungeon_generator;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Action_Move() {
        List<GameObject> enemy_list = dungeon_generator.enemy_list;
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
            foreach (GameObject enemy_prefab in enemy_list) {
                enemy = enemy_prefab.GetComponent<Enemy>();
                enemy_action.Move_Enemy();
            }
        }

        // 通常モード時の移動処理
        if (mode == ePlayer_Mode.Nomal_Mode) {
            Debug.Log(gameObject);
            if (Input.GetKeyDown("right")) {
                player_action.direction = eDirection.Right;

                if (Dungeon_Base.Is_Check_Move(player_height, player_width + 1, 2)) {
                    moved = true;
                    Position.x += player.speed.x;
                    enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("down")) {
                player_action.direction = eDirection.Down;

                if (Dungeon_Base.Is_Check_Move(player_height - 1, player_width, 2)) {
                    moved = true;
                    Position.y -= player.speed.y;
                    enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("left")) {
                player_action.direction = eDirection.Left;

                if (Dungeon_Base.Is_Check_Move(player_height, player_width - 1, 2)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("up")) {
                player_action.direction = eDirection.Up;

                if (Dungeon_Base.Is_Check_Move(player_height + 1, player_width, 2)) {
                    moved = true;
                    Position.y += player.speed.y;
                    enemy_action.Move_Enemy();
                }
            }
        }

        // 斜め移動モード時の移動処理
        else if (mode == ePlayer_Mode.Diagonally_Mode) {
            if (Input.GetKeyDown("right") && Input.GetKeyDown("up")) {
                player_action.direction = eDirection.Upright;

                if (Dungeon_Base.Is_Check_Move(player_height + 1, player_width + 1, 2)) {
                    moved = true;
                    Position.x += player.speed.x;
                    Position.y += player.speed.y;
                    enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("right") && Input.GetKeyDown("down")) {
                player_action.direction = eDirection.Downright;

                if (Dungeon_Base.Is_Check_Move(player_height - 1, player_width + 1, 2)) {
                    moved = true;
                    Position.x += player.speed.x;
                    Position.y -= player.speed.y;
                    enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKey("left") && Input.GetKey("down")) {
                player_action.direction = eDirection.Downleft;

                if (Dungeon_Base.Is_Check_Move(player_height - 1, player_width - 1, 2)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    Position.y -= player.speed.y;
                    enemy_action.Move_Enemy();
                }
            }
            else if (Input.GetKeyDown("left") && Input.GetKeyDown("up")) {
                player_action.direction = eDirection.Upleft;

                if (Dungeon_Base.Is_Check_Move(player_height + 1, player_width - 1, 2)) {
                    moved = true;
                    Position.x -= player.speed.x;
                    Position.y += player.speed.y;
                    enemy_action.Move_Enemy();
                }
            }
        }

        // 方向転換モード時の処理
        else if (mode == ePlayer_Mode.Change_Direction_Mode) {
            if (Input.GetKeyDown("right")) {
                player_action.direction = eDirection.Right;
            }
            else if (Input.GetKeyDown("down")) {
                player_action.direction = eDirection.Down;
            }
            else if (Input.GetKeyDown("left")) {
                player_action.direction = eDirection.Left;
            }
            else if (Input.GetKeyDown("up")) {
                player_action.direction = eDirection.Up;
            }
            else if (Input.GetKey("right") && Input.GetKey("up")) {
                player_action.direction = eDirection.Upright;
            }
            else if (Input.GetKey("right") && Input.GetKey("down")) {
                player_action.direction = eDirection.Downright;
            }
            else if (Input.GetKey("left") && Input.GetKey("down")) {
                player_action.direction = eDirection.Downleft;
            }
            else if (Input.GetKey("left") && Input.GetKey("up")) {
                player_action.direction = eDirection.Upleft;
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
            player_status.Turn();
    }

    void Move_Check_Command() {
        if (Input.GetKey("return")) {
            player_action.action = ePlayer_Action.Battle_Menu;
        }

        else if (Input.GetKey("space")) {
            player_action.action = ePlayer_Action.Attack;
        }
    }
}
