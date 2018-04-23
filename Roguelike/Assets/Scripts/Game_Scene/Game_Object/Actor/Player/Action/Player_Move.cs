using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの移動処理を制御するクラス
/// </summary>
public class Player_Move : MonoBehaviour {
    /// <summary>
    /// エネミー本体のクラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// プレイヤー本体
    /// </summary>
    GameObject player;
    /// <summary>
    /// プレイヤースクリプト
    /// </summary>
    Player player_script;
    /// <summary>
    /// 入力されたキーを流す
    /// </summary>
    Key_Observer key_observer;
    /// <summary>
    /// エネミーの行動を制御するクラス
    /// </summary>
    Enemy_Action enemy_action;
    /// <summary>
    /// マップを２次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;

    /// <summary>
    ///  移動が完了したらtrue
    /// </summary>
    bool move_end = false;

    /// <summary>
    /// プレイヤーの現在いる座標
    /// </summary>
    Vector3 player_position;

    void Start() {
        player = Player_Manager.Instance.player;
        player_script = Player_Manager.Instance.player_script;
        enemy = Enemy_Manager.Instance.enemy_script;
        key_observer = Game.Instance.key_observer;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
        enemy_action = Enemy_Manager.Instance.action;

        player_script.mode = ePlayer_Mode.Nomal_Mode;
    }

    /// <summary>
    /// 移動状態の時にできる動き
    /// </summary>
    public void Action_Move() {
        // 移動に使う変数
        var move_value = Define_Value.MOVE_VAULE;
        var not_move = 0;
        player_position = player_script.Get_Position();
        move_end = false;

        // 方向転換モードの切り替え
        if (Input.GetKey(KeyCode.RightAlt)) {
            player_script.mode = ePlayer_Mode.Change_Direction_Mode;
        }
        if (Input.GetKeyUp(KeyCode.RightAlt)) {
            player_script.mode = ePlayer_Mode.Nomal_Mode;
        }

        // 足踏み
        if (Input.GetKey(KeyCode.S)) {
            move_end = true;
            //TODO: 先にエネミーの処理完成させる
            //enemy_action.Move_Enemy();
        }

        // 通常モード時の移動処理。上から始まって時計回り
        if (player_script.mode == ePlayer_Mode.Nomal_Mode) {
            // Wキーが押されたとき
            if (Input.GetKeyDown(KeyCode.W)) {
                player_script.direction = eDirection.Up;
                // 進行方向が移動可能かを判断
                if (Move_Check(map_layer.Get_(player_position.x, player_position.y),
                               map_layer.Get_(player_position.x, player_position.y + move_value))) {
                    return;
                }
                Move_Process(not_move, move_value);
            }
            // Eキーが押されたとき
            else if (Input.GetKeyDown(KeyCode.E)) {
                player_script.direction = eDirection.Upright;
                // 進行方向が移動可能かを判断
                if (Move_Check(map_layer.Get_(player_position.x, player_position.y),
                               map_layer.Get_(player_position.x + move_value, player_position.y + move_value))) {
                    return;
                }
                // 右方向か上方向に壁があるとき(移動不可になる)
                if (Slant_Move_Check(map_layer.Get_(player_position.x, player_position.y + move_value),
                                     map_layer.Get_(player_position.x + move_value, player_position.y))) {
                    return;
                }
                Move_Process(move_value, move_value);
            }
            // Dキーが押されたとき
            else if (Input.GetKeyDown(KeyCode.D)) {
                player_script.direction = eDirection.Right;
                // 進行方向が移動可能かを判断
                if (Move_Check(map_layer.Get_(player_position.x, player_position.y),
                               map_layer.Get_(player_position.x + move_value, player_position.y))) {
                    return;
                }
                Move_Process(move_value, not_move);
            }
            // Cキーが押されたとき
            else if (Input.GetKeyDown(KeyCode.C)) {
                player_script.direction = eDirection.Downright;
                // 進行方向が移動可能かを判断
                if (Move_Check(map_layer.Get_(player_position.x, player_position.y),
                               map_layer.Get_(player_position.x + move_value, player_position.y - move_value))) {
                    return;
                }
                // 右方向か下方向に壁があるかを判断(移動不可になる)
                if (Slant_Move_Check(map_layer.Get_(player_position.x, player_position.y - move_value),
                                     map_layer.Get_(player_position.x + move_value, player_position.y))) {
                    return;
                }
                Move_Process(move_value, -move_value);
            }
            // Xキーが押されたとき
            else if (Input.GetKeyDown(KeyCode.X)) {
                player_script.direction = eDirection.Down;
                // 進行方向が移動可能かを判断
                if (Move_Check(map_layer.Get_(player_position.x, player_position.y),
                               map_layer.Get_(player_position.x, player_position.y - move_value))) {
                    return;
                }
                Move_Process(not_move, -move_value);
            }
            // Zキーが押されたとき
            else if (Input.GetKeyDown(KeyCode.Z)) {
                player_script.direction = eDirection.Downleft;
                if (Move_Check(map_layer.Get_(player_position.x, player_position.y),
                               map_layer.Get_(player_position.x - move_value, player_position.y - move_value))) {
                    return;
                }
                // 左方向か下方向に壁があるとき(移動不可になる)
                if (Slant_Move_Check(map_layer.Get_(player_position.x - move_value, player_position.y),
                                     map_layer.Get_(player_position.x, player_position.y - move_value))) {
                    return;
                }
                Move_Process(-move_value, -move_value);
            }
            // Aキーが押されたとき
            else if (Input.GetKeyDown(KeyCode.A)) {
                player_script.direction = eDirection.Left;
                // 進行方向が移動可能かを判断
                if (Move_Check(map_layer.Get_(player_position.x, player_position.y),
                               map_layer.Get_(player_position.x - move_value, player_position.y))) {
                    return;
                }
                Move_Process(-move_value, not_move);
            }
            // Qキーが押されたとき
            else if (Input.GetKeyDown(KeyCode.Q)) {
                player_script.direction = eDirection.Upleft;
                if (Move_Check(map_layer.Get_(player_position.x, player_position.y),
                               map_layer.Get_(player_position.x - move_value, player_position.y + move_value))) {
                    return;
                }
                // 左方向か上方向に壁があるとき(移動不可になる)
                if (Slant_Move_Check(map_layer.Get_(player_position.x - move_value, player_position.y),
                                     map_layer.Get_(player_position.x, player_position.y + move_value))) {
                    return;
                }
                Move_Process(-move_value, move_value);
            }
        }

        // 方向転換モード時の処理 上から順に時計回り
        else if (player_script.mode == ePlayer_Mode.Change_Direction_Mode) {
            if (Input.GetKeyDown(KeyCode.W)) {
                player_script.direction = eDirection.Up;
            }
            else if (Input.GetKeyDown(KeyCode.E)) {
                player_script.direction = eDirection.Upright;
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                player_script.direction = eDirection.Right;
            }
            else if (Input.GetKeyDown(KeyCode.C)) {
                player_script.direction = eDirection.Downright;
            }
            else if (Input.GetKeyDown(KeyCode.X)) {
                player_script.direction = eDirection.Down;
            }
            else if (Input.GetKeyDown(KeyCode.Z)) {
                player_script.direction = eDirection.Downleft;
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
                player_script.direction = eDirection.Left;
            }
            else if (Input.GetKeyDown(KeyCode.Q)) {
                player_script.direction = eDirection.Upleft;
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
            // 移動後に階段に到達しているか調べる
            Check_Stair();
            player_script.status.Add_Turn();
        }
    }

    /// <summary>
    /// 移動可能かを判断
    /// </summary>
    /// <param name="my_layer">自分のレイヤーナンバー</param>
    /// <param name="new_position">移動先の座標</param>
    /// <returns>移動不可能であればtrue</returns>
    bool Move_Check(int my_layer, int new_position) {
        // 自分のレイヤー番号と移動先のレイヤー番号を比べる
        if (my_layer > new_position) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// ななめ移動時に移動不可になる場所に壁があるかを調べる。 
    /// </summary>
    /// <param name="check_layer1">壁1</param>
    /// <param name="check_layer2">壁2</param>
    /// <returns>移動不可能であればtrue</returns>
    bool Slant_Move_Check(int check_layer1, int check_layer2) {
        // ななめ移動時に移動不可となる場合の壁の位置を調べる
        if (check_layer1 >= Define_Value.WALL_LAYER_NUMBER ||
           check_layer2 >= Define_Value.WALL_LAYER_NUMBER) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Move_Action中にできる行動がなされたときにStateに変更をかける
    /// </summary>
    void Move_Check_Command() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            player_script.state = ePlayer_State.Attack;
        }
    }

    /// <summary>
    /// ワールド座標と合わせる
    /// </summary>
    /// <param name="x">移動後の座標</param>
    /// <param name="y">移動後の座標</param>
    void Set_Position(Vector3 new_position) {
        player_script.position.x = new_position.x;
        player_script.position.y = new_position.y;
        player.gameObject.transform.position = player_script.position;
    }

    /// <summary>
    /// プレイヤーが階段に乗っているかを調べる
    /// </summary>
    public void Check_Stair() {
        if (player_script.feet == Define_Value.STAIR_LAYER_NUMBER) {
            player_script.state = ePlayer_State.On_Stair;
        }
    }

    /// <summary>
    /// 実際にプレイヤーを動かしゲームを進める
    /// </summary>
    /// <param name="move_value_x">プレイヤーの移動量</param>
    /// <param name="move_value_y">プレイヤーの移動量</param>
    void Move_Process(int move_value_x, int move_value_y) {
        map_layer.Tile_Swap(player_script.position, player_script.feet);
        player_position.x += move_value_x;
        player_position.y += move_value_y;
        Set_Position(player_position);
        player_script.Set_Feet(map_layer.Get_(player_position.x, player_position.y));
        map_layer.Tile_Swap(player_script.position, Define_Value.PLAYER_LAYER_NUMBER);
        move_end = true;
    }
}
