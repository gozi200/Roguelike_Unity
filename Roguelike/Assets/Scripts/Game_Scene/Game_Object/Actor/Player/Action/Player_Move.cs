using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの移動処理を制御するクラス
/// </summary>
public class Player_Move : MonoBehaviour {
    /// <summary>
    /// プレイヤー本体のクラス
    /// </summary>
    Player player;
    /// <summary>
    /// プレイヤーのステータス関係を管理するクラス
    /// </summary>
    Player_Status player_status;
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
    Vector2Int player_position;

    void Start() {
        player = Player_Manager.Instance.player_script;
        key_observer = Game.Instance.key_observer;
        enemy_action = new Enemy_Action();
        player_status = Player_Manager.Instance.player_status;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;

        player.player_mode = ePlayer_Mode.Nomal_Mode;
    }

    /// <summary>
    /// 移動状態の時にできる動き
    /// </summary>
    public void Action_Move() {
        // 移動はまだ完了していない
        move_end = false;

        // 方向転換モードの切り替え
        if (Input.GetKey(KeyCode.RightAlt)) {
            player.player_mode = ePlayer_Mode.Change_Direction_Mode;
        }
        if (Input.GetKeyUp(KeyCode.RightAlt)) {
            player.player_mode = ePlayer_Mode.Nomal_Mode;
        }

        // 足踏み
        if (Input.GetKey(KeyCode.S)) {
            Stamp_Feet();
        }

        // 周囲に敵がいたらそちらを向く
        if (Input.GetKeyDown(KeyCode.R)) {
            Search_Enemy();
        }

        // 通常モード時の移動処理。上から始まって時計回り
        if (player.player_mode == ePlayer_Mode.Nomal_Mode) {
            Move();
        }
        // 方向転換モード時の処理 向きだけを変更させる 上から順に時計回り
        else if (player.player_mode == ePlayer_Mode.Change_Direction_Mode) {
            Change_Direction();
        }
        //TODO:アイテムの取得処理

        // 移動コマンド未入力時はいつでもコマンド操作を受け付ける
        if (!move_end) {
            Move_Check_Command();
        }
        // 移動していればターンのカウントを進める
        else if (move_end) {
            // 移動後に階段に到達しているか調べる
            Check_Stair();
            // ダンジョン移動マスに踏んでいるか調べる
            Check_Move_Dungeon();
            player_status.Add_Turn();
        }
    }

    /// <summary>
    /// 足踏みの処理
    /// </summary>
    void Stamp_Feet() {
        // ゲームの状態を取得に使用
        var game_manager = GameManager.Instance;

        // 移動を完了する
        move_end = true;

        // 敵がいたら敵のターンへ
        if (Enemy_Manager.Instance.enemies.Count > 0) {
            game_manager.Set_Game_State(eGame_State.Enemy_Trun);
        }
        // ダンジョンが存在していたらダンジョンのターンへ
        else if (Dungeon_Manager.Instance.Is_Exist) {
            game_manager.Set_Game_State(eGame_State.Dungeon_Turn);
        }
        // それ以外はパートナーのターンへ
        else {
            //TODO:パートナーターン
        }
    }

    /// <summary>
    /// キー入力に合わせた方向に移動する
    /// </summary>
    void Move() {
        // 現在の座標を取得
        player_position = player.GetPosition();
        // プレイヤーのx座標
        int player_x = player_position.x;
        // プレイヤーのy座標
        int player_y = player_position.y;

        // 移動量
        int move_value = Define_Value.MOVE_VAULE;
        // 移動しないときは0
        int not_move = 0;

        // Wキーが押されたとき
        if (Input.GetKeyDown(KeyCode.W)) {
            player.direction = eDirection.Up;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get(player_x, player_y),
                                        map_layer.Get(player_x, player_y + move_value))) {
                return;
            }
            Move_Process(not_move, move_value);
        }
        // Eキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.E)) {
            player.direction = eDirection.Upright;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get(player_x, player_y),
                                        map_layer.Get(player_x + move_value, player_y + move_value))) {
                return;
            }
            // 右方向か上方向に壁があるとき(移動不可になる)
            if (Actor_Action.Slant_Check(map_layer.Get(player_x + move_value, player_y),
                                              map_layer.Get(player_x, player_y + move_value))) {
                return;
            }
            Move_Process(move_value, move_value);
        }
        // Dキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.D)) {
            player.direction = eDirection.Right;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get(player_x, player_y),
                                        map_layer.Get(player_x + move_value, player_y))) {
                return;
            }
            Move_Process(move_value, not_move);
        }
        // Cキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.C)) {
            player.direction = eDirection.Downright;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get(player_x, player_y),
                                        map_layer.Get(player_x + move_value, player_y - move_value))) {
                return;
            }
            // 右方向か下方向に壁があるかを判断(移動不可になる)
            if (Actor_Action.Slant_Check(map_layer.Get(player_x + move_value, player_y),
                                              map_layer.Get(player_x, player_y - move_value))) {
                return;
            }
            Move_Process(move_value, -move_value);
        }
        // Xキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.X)) {
            player.direction = eDirection.Down;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get(player_x, player_y),
                                        map_layer.Get(player_x, player_y - move_value))) {
                return;
            }
            Move_Process(not_move, -move_value);
        }
        // Zキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.Z)) {
            player.direction = eDirection.Downleft;
            if (Actor_Action.Move_Check(map_layer.Get(player_x, player_y),
                                        map_layer.Get(player_x - move_value, player_y - move_value))) {
                return;
            }
            // 左方向か下方向に壁があるとき(移動不可になる)
            if (Actor_Action.Slant_Check(map_layer.Get(player_x - move_value, player_y),
                                              map_layer.Get(player_x, player_y - move_value))) {
                return;
            }
            Move_Process(-move_value, -move_value);
        }
        // Aキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.A)) {
            player.direction = eDirection.Left;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get(player_x, player_y),
                                        map_layer.Get(player_x - move_value, player_y))) {
                return;
            }
            Move_Process(-move_value, not_move);
        }
        // Qキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.Q)) {
            player.direction = eDirection.Upleft;
            if (Actor_Action.Move_Check(map_layer.Get(player_x, player_y),
                                        map_layer.Get(player_x - move_value, player_y + move_value))) {
                return;
            }
            // 左方向か上方向に壁があるとき(移動不可になる)
            if (Actor_Action.Slant_Check(map_layer.Get(player_x - move_value, player_y),
                                              map_layer.Get(player_x, player_y + move_value))) {
                return;
            }
            Move_Process(-move_value, move_value);
        }
    }

    /// <summary>
    /// キー入力に合った方向に向き直る
    /// </summary>
    void Change_Direction() {
        // 上方向を向く
        if (Input.GetKeyDown(KeyCode.W)) {
            player.direction = eDirection.Up;
        }
        // 右上方向を向く
        else if (Input.GetKeyDown(KeyCode.E)) {
            player.direction = eDirection.Upright;
        }
        // 右方向を向く
        else if (Input.GetKeyDown(KeyCode.D)) {
            player.direction = eDirection.Right;
        }
        // 右下方向を向く
        else if (Input.GetKeyDown(KeyCode.C)) {
            player.direction = eDirection.Downright;
        }
        // 下方向を向く
        else if (Input.GetKeyDown(KeyCode.X)) {
            player.direction = eDirection.Down;
        }
        // 左下方向を向く
        else if (Input.GetKeyDown(KeyCode.Z)) {
            player.direction = eDirection.Downleft;
        }
        // 左方向を向く
        else if (Input.GetKeyDown(KeyCode.A)) {
            player.direction = eDirection.Left;
        }
        // 左上方向を向く
        else if (Input.GetKeyDown(KeyCode.Q)) {
            player.direction = eDirection.Upleft;
        }
    }

    /// <summary>
    /// Move_Action中にできる行動がなされたときにStateに変更をかける
    /// </summary>
    void Move_Check_Command() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            player.player_state = ePlayer_State.Attack;
            player_status.Add_Turn();
        }
    }

    /// <summary>
    /// プレイヤーが階段に乗っているかを調べる
    /// </summary>
    void Check_Stair() {
        if (player.Feet == Define_Value.STAIR_LAYER_NUMBER) {
            player.player_state = ePlayer_State.On_Stair;
        }
    }

    /// <summary>
    /// ダンジョン移動用のマスを踏んでいるか調べる
    /// </summary>
    void Check_Move_Dungeon() {
        if (player.Feet == Define_Value.MOVE_DUNGEON_TILE) {
            player.player_state = ePlayer_State.Decide_Command;
        }
    }

    /// <summary>
    /// 実際にプレイヤーを動かしゲームを進める
    /// </summary>
    /// <param name="move_value_x">プレイヤーの移動量</param>
    /// <param name="move_value_y">プレイヤーの移動量</param>
    void Move_Process(int move_value_x, int move_value_y) {
        map_layer.Tile_Swap(player.position, player.Feet);
        player_position.x += move_value_x;
        player_position.y += move_value_y;
        player.Set_Position(player_position);
        player.Set_Feet(map_layer.Get(player_position.x, player_position.y));
        map_layer.Tile_Swap(player.position, Define_Value.PLAYER_LAYER_NUMBER);
        move_end = true;
    }

    /// <summary>
    /// 周囲８マスにいる敵の方向を向く
    /// </summary>
    void Search_Enemy() { // TODO: ２体以上いるときに１体しか向けない
        var player = Player_Manager.Instance.player_script;
        var pla_pos = player.position;
        int move_value = Define_Value.MOVE_VAULE;
        int tile_scale = Define_Value.TILE_SCALE;
        int enemy_layer = Define_Value.ENEMY_LAYER_NUMBER;

        // 上方向から時計回りに検索
        if (map_layer.Get(pla_pos.x, pla_pos.y + tile_scale) == enemy_layer) {
            player.direction = eDirection.Up;
        }
        // 右上
        else if (map_layer.Get(pla_pos.x + tile_scale, pla_pos.y + tile_scale) == enemy_layer) {
            if (Actor_Action.Slant_Check(map_layer.Get(pla_pos.x + move_value, pla_pos.y),
                                         map_layer.Get(pla_pos.x, pla_pos.y + move_value))) {
            }
            player.direction = eDirection.Upright;
        }
        // 右
        else if (map_layer.Get(pla_pos.x + tile_scale, pla_pos.y) == enemy_layer) {
            player.direction = eDirection.Right;
        }
        // 右下
        else if (map_layer.Get(pla_pos.x + tile_scale, pla_pos.y - tile_scale) == enemy_layer) {
            if (Actor_Action.Slant_Check(map_layer.Get(pla_pos.x + move_value, pla_pos.y),
                                         map_layer.Get(pla_pos.x, pla_pos.y - move_value))) {
            }
            player.direction = eDirection.Downright;
        }
        // 下
        else if (map_layer.Get(pla_pos.x, pla_pos.y - tile_scale) == Define_Value.ENEMY_LAYER_NUMBER) {
            player.direction = eDirection.Down;
        }
        // 左下
        else if (map_layer.Get(pla_pos.x - tile_scale, pla_pos.y - tile_scale) == enemy_layer) {
            if (Actor_Action.Slant_Check(map_layer.Get(pla_pos.x - move_value, pla_pos.y),
                                         map_layer.Get(pla_pos.x, pla_pos.y - move_value))) {
            }
            player.direction = eDirection.Downleft;
        }
        // 左
        else if (map_layer.Get(pla_pos.x - tile_scale, pla_pos.y) == Define_Value.ENEMY_LAYER_NUMBER) {
            player.direction = eDirection.Left;
        }
        // 左上
        else if (map_layer.Get(pla_pos.x - tile_scale, pla_pos.y + tile_scale) == enemy_layer) {
            if (Actor_Action.Slant_Check(map_layer.Get(pla_pos.x - move_value, pla_pos.y),
                                         map_layer.Get(pla_pos.x, pla_pos.y + move_value))) {
            }
            player.direction = eDirection.Upleft;
        }
    }
}
