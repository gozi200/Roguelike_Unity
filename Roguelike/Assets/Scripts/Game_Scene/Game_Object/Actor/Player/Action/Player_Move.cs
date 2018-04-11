using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;

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
    ///  移動が完了したらtrue
    /// </summary>
    bool move_end = false;

    /// <summary>
    /// 移動が完了したらtrue
    /// </summary>
    ReactiveProperty<bool> move_check_flag;

    Enemy enemy;
    Player player;

    Key_Observer key_observer;

    Player_Action player_action;

    Player_Status player_status;

    Enemy_Action enemy_action;

    Map_Layer_2D map_layer;

    GameObject stair;

    // 現在位置をplayer_positionに代入
    Vector2 player_position;

    void Awake() {
        player = Player.Instance.player;
    }

    void Start() {
        enemy  = Enemy.Instance.enemy;
        key_observer = Game.Instance.key_observer;
        map_layer = Game.Instance.map_layer_2D;
        player.mode = ePlayer_Mode.Nomal_Mode;
        player_position = transform.position;
        move_check_flag = new ReactiveProperty<bool>();

        // 移動するときに居た座標のレイヤーを元のレイヤー番号に変える
        move_check_flag
            .Where(flag => !flag)
            .Subscribe(_ =>
                map_layer.Tile_Swap_Before(map_layer.Get((int)player.position.x / Define_Value.SPRITE_SIZE, (int)player.position.y / Define_Value.SPRITE_SIZE), player.Get_Feet())
                );

        // 移動が完了したら今いる座標を自分のレイヤー番号に変える
        move_check_flag
            .Where(flag => !!flag)
            .Subscribe(_ =>
                player.Set_Feet(map_layer.Get((int)player.position.x / Define_Value.SPRITE_SIZE, (int)player.position.y / Define_Value.SPRITE_SIZE)),
           _ => map_layer.Tile_Swap_After(map_layer.Get((int)player.position.x / Define_Value.SPRITE_SIZE, (int)player.position.y / Define_Value.SPRITE_SIZE), Define_Value.PLAYER_LAYER_NUMBER)
                // エネミーターン
            );
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Action_Move() {
        move_end = false;

        // 方向転換モードの切り替え
        if (Input.GetKey(KeyCode.RightAlt)) {
            player.mode = ePlayer_Mode.Change_Direction_Mode;
        }
        if (Input.GetKeyUp(KeyCode.RightAlt)) {
            player.mode = ePlayer_Mode.Nomal_Mode;
        }

        // 足踏み
        if (Input.GetKey(KeyCode.S)) {
            move_end = true;
            for (int i = 0; i < enemy.enemys.Count; ++i) {
                enemy_action.Move_Enemy();
            }
        }

        // 通常モード時の移動処理 上から始まって時計回り
        //TODO: 配列上の数字とワールド座標の数字があっていないので調整してる 2018/04/03
        if (player.mode == ePlayer_Mode.Nomal_Mode) {
            if (Input.GetKeyDown(KeyCode.W)) {
                player.direction = eDirection.Up;
                if (map_layer.Get((int)player.position.x / Define_Value.SPRITE_SIZE, (int)player.position.y / Define_Value.SPRITE_SIZE)
                  > map_layer.Get((int)player.position.x / Define_Value.SPRITE_SIZE, ((int)player.position.y + Define_Value.SPRITE_SIZE)) / Define_Value.SPRITE_SIZE) {
                    Debug.Log("移動完了");
                    move_end = true;
                    move_check_flag.Value = true;
                    player_position.y += player.move_value.y;
                    move_check_flag.Value = false;
                    Debug.Log("今の座標のレイヤーナンバー" + map_layer.Get((int)player.position.x / Define_Value.SPRITE_SIZE, (int)player.position.y / Define_Value.SPRITE_SIZE) + " > " + "移動先のレイヤーナンバー" + map_layer.Get((int)player.position.x / Define_Value.SPRITE_SIZE, ((int)player.position.y + Define_Value.SPRITE_SIZE) / Define_Value.SPRITE_SIZE) + " なら移動完了");
                    Move(player_position);
                }
            }
            else if (Input.GetKeyDown(KeyCode.E)) {
                player.direction = eDirection.Upright;

                move_end = true;
                player.position.x += player.move_value.x;
                player_position.y += player.move_value.y;
                enemy_action.Move_Enemy();
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                player.direction = eDirection.Right;
                move_end = true;
                player_position.x += player.move_value.x;
                enemy_action.Move_Enemy();
            }
            else if (Input.GetKeyDown(KeyCode.C)) {
                player.direction = eDirection.Downright;
                move_end = true;
                player_position.x += player.move_value.x;
                player_position.y -= player.move_value.y;
                enemy_action.Move_Enemy();
            }
            else if (Input.GetKeyDown(KeyCode.X)) {
                player.direction = eDirection.Down;
                move_end = true;
                player_position.y -= player.move_value.y;
                enemy_action.Move_Enemy();
            }
            else if (Input.GetKeyDown(KeyCode.Z)) {
                player.direction = eDirection.Downleft;
                move_end = true;
                player_position.x -= player.move_value.x;
                player_position.y -= player.move_value.y;
                enemy_action.Move_Enemy();
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
                player.direction = eDirection.Left;
                move_end = true;
                player_position.x -= player.move_value.x;
                enemy_action.Move_Enemy();
            }
            else if (Input.GetKeyDown(KeyCode.Q)) {
                player.direction = eDirection.Upleft;
                move_end = true;
                player_position.x -= player.move_value.x;
                player_position.y += player.move_value.y;
                enemy_action.Move_Enemy();
            }
        }

        // 方向転換モード時の処理 上から順に時計回り
        else if (player.mode == ePlayer_Mode.Change_Direction_Mode) {
            if (Input.GetKeyDown(KeyCode.W)) {
                player.direction = eDirection.Up;
            }
            else if (Input.GetKeyDown(KeyCode.E)) {
                player.direction = eDirection.Upright;
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                player.direction = eDirection.Right;
            }
            else if (Input.GetKeyDown(KeyCode.C)) {
                player.direction = eDirection.Downright;
            }
            else if (Input.GetKeyDown(KeyCode.X)) {
                player.direction = eDirection.Down;
            }
            else if (Input.GetKeyDown(KeyCode.Z)) {
                player.direction = eDirection.Downleft;
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
                player.direction = eDirection.Left;
            }
            else if (Input.GetKeyDown(KeyCode.Q)) {
                player.direction = eDirection.Upleft;
            }
        }

        // 現在の位置に加算減算を行ったplayer_positionを代入する
        transform.position = player_position;

        // TODO: アイテムの取得処理

        // 移動コマンド未入力時はいつでもコマンド操作を受け付ける
        if (!move_end) {
            Move_Check_Command();
        }
        // 移動していればターンのカウントを進める
        else if (move_end) {
            Add_Player_Turn();
        }
    }

    /// <summary>
    /// プレイヤーのターン数を加算する
    /// </summary>
    void Add_Player_Turn() {
        //player_status.Turn();
    }

    /// <summary>
    /// Move＿Action中にできる行動がなされたときにStateに変更をかける
    /// </summary>
    void Move_Check_Command() {
        if (Input.GetKey(KeyCode.Return)) {
            player.state = ePlayer_State.Attack;
        }
    }

    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    /// <param name="x">移動後の座標</param>
    /// <param name="y">移動後の座標</param>
    void Move(Vector3 new_position) {
        player.position.x = new_position.x;
        player.position.y = new_position.y;
        player.gameObject.transform.position = player.position;
    }
}
