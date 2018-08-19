using UnityEngine;

/// <summary>
/// プレイヤーの移動処理を制御
/// </summary>
public class Player_Move : MonoBehaviour {
    /// <summary>
    /// プレイヤー本体のクラス
    /// </summary>
    Player player;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    Player_Action player_action;
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
        player_action = Player_Manager.Instance.player_action;
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
        //Debug.Log(player_action.Player_State);

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
            // 何を踏んでいる(どこにいる)のかを調べて、それに応じた処理をする
            Check_Feet(player.Feet);
            // ターンを進める
            player_status.Add_Turn();
        }
    }

    /// <summary>
    /// Move_Action中にできる行動がなされたときにStateに変更をかける
    /// </summary>
    void Move_Check_Command() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            player_action.Player_State = ePlayer_State.Attack;
            var player_attack = Player_Manager.Instance.player_attack;
            player_attack.Action_Attack();
            player_status.Add_Turn();
        }
    }

    /// <summary>
    /// どこを進んでいるかを調べる。部屋の中か通路かで分ける
    /// </summary>
    void Check_Feet(int feet) {
        switch (feet) {
            case Define_Value.STAIR_LAYER_NUMBER:
                player_action.Player_State = ePlayer_State.On_Stair;
                break;
            case Define_Value.MOVE_DUNGEON_TILE:
                player_action.Player_State = ePlayer_State.Decide_Command;
                break;
            case Define_Value.ENTRANCE_LAYER_NUMBER:
                Change_Move_State();
                break;
        }
    }

    /// <summary>
    /// 入口に着いたら進行場所を変える
    /// </summary>
    void Change_Move_State() {
        if(player.Where_Move == ePlayer_Where_Move.Room_Move) {
            player.Where_Move = ePlayer_Where_Move.Road_Move;
        }
        else {
            player.Where_Move = ePlayer_Where_Move.Room_Move;
            // 通路からの部屋に入った時に何番目の部屋に入ったのかを調べる
            player_status.Where_Room(player.Position.x, player.Position.y);
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
        int move_value = Define_Value.MOVE_VALUE;
        // 移動しないときは0
        int not_move = 0;

        // Wキーが押されたとき
        if (Input.GetKeyDown(KeyCode.W)) {
            player.Direction = eDirection.Up;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get_Layer_Number(player_x, player_y),
                                        map_layer.Get_Layer_Number(player_x, player_y + move_value))) {
                Move_Process(not_move, move_value);
            }
        }
        // Eキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.E)) {
            player.Direction = eDirection.Upright;
            // 進行方向が移動可能かを判断
            if (!Actor_Action.Move_Check(map_layer.Get_Layer_Number(player_x, player_y),
                                         map_layer.Get_Layer_Number(player_x + move_value, player_y + move_value))) {
                return;
            }
            // 右方向か上方向に壁があるとき(移動不可になる)
            if(Actor_Action.Slant_Action_Check(player as Actor, player.Direction)) {
            Move_Process(move_value, move_value);
            }
        }
        // Dキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.D)) {
            player.Direction = eDirection.Right;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get_Layer_Number(player_x, player_y),
                                        map_layer.Get_Layer_Number(player_x + move_value, player_y))) {
                Move_Process(move_value, not_move);
            }
        }
        // Cキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.C)) {
            player.Direction = eDirection.Downright;
            // 進行方向が移動可能かを判断
            if (!Actor_Action.Move_Check(map_layer.Get_Layer_Number(player_x, player_y),
                                         map_layer.Get_Layer_Number(player_x + move_value, player_y - move_value))) {
                return;
            }
            // 右方向か下方向に壁があるかを判断(移動不可になる)
            if (Actor_Action.Slant_Action_Check(player as Actor, player.Direction)) {
                Move_Process(move_value, -move_value);
            }
        }
        // Xキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.X)) {
            player.Direction = eDirection.Down;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get_Layer_Number(player_x, player_y),
                                        map_layer.Get_Layer_Number(player_x, player_y - move_value))) {
                Move_Process(not_move, -move_value);
            }
        }
        // Zキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.Z)) {
            player.Direction = eDirection.Downleft;
            if (!Actor_Action.Move_Check(map_layer.Get_Layer_Number(player_x, player_y),
                                         map_layer.Get_Layer_Number(player_x - move_value, player_y - move_value))) {
                return;
            }
            // 左方向か下方向に壁があるとき(移動不可になる)
            if (Actor_Action.Slant_Action_Check(player as Actor, player.Direction)) {
                Move_Process(-move_value, -move_value);
            }
        }
        // Aキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.A)) {
            player.Direction = eDirection.Left;
            // 進行方向が移動可能かを判断
            if (Actor_Action.Move_Check(map_layer.Get_Layer_Number(player_x, player_y),
                                        map_layer.Get_Layer_Number(player_x - move_value, player_y))) {
                Move_Process(-move_value, not_move);
            }
        }
        // Qキーが押されたとき
        else if (Input.GetKeyDown(KeyCode.Q)) {
            player.Direction = eDirection.Upleft;
            if (!Actor_Action.Move_Check(map_layer.Get_Layer_Number(player_x, player_y),
                                         map_layer.Get_Layer_Number(player_x - move_value, player_y + move_value))) {
                return;
            }
            // 左方向か上方向に壁があるとき(移動不可になる)
            if (Actor_Action.Slant_Action_Check(player as Actor, player.Direction)) {
                Move_Process(-move_value, move_value);
            }
        }
    }

    /// <summary>
    /// キー入力に合った方向に向き直る
    /// </summary>
    void Change_Direction() {
        // 上方向を向く
        if (Input.GetKeyDown(KeyCode.W)) {
            player.Direction = eDirection.Up;
        }
        // 右上方向を向く
        else if (Input.GetKeyDown(KeyCode.E)) {
            player.Direction = eDirection.Upright;
        }
        // 右方向を向く
        else if (Input.GetKeyDown(KeyCode.D)) {
            player.Direction = eDirection.Right;
        }
        // 右下方向を向く
        else if (Input.GetKeyDown(KeyCode.C)) {
            player.Direction = eDirection.Downright;
        }
        // 下方向を向く
        else if (Input.GetKeyDown(KeyCode.X)) {
            player.Direction = eDirection.Down;
        }
        // 左下方向を向く
        else if (Input.GetKeyDown(KeyCode.Z)) {
            player.Direction = eDirection.Downleft;
        }
        // 左方向を向く
        else if (Input.GetKeyDown(KeyCode.A)) {
            player.Direction = eDirection.Left;
        }
        // 左上方向を向く
        else if (Input.GetKeyDown(KeyCode.Q)) {
            player.Direction = eDirection.Upleft;
        }
    }

    /// <summary>
    /// 実際にプレイヤーを動かしゲームを進める
    /// </summary>
    /// <param name="move_value_x">プレイヤーの移動量</param>
    /// <param name="move_value_y">プレイヤーの移動量</param>
    void Move_Process(int move_value_x, int move_value_y) {
        var room_list = Dungeon_Manager.Instance.room_list;

        // 移動前に今いる座標を元のレイヤーナンバーに戻す
        map_layer.Tile_Swap(player.Position, player.Feet);

        // 実際に座標を変える(移動)
        player_position.x += move_value_x;
        player_position.y += move_value_y;
        player.Set_Position(player_position);

        // 移動後の座標のレイヤーナンバーを取得する
        player.Set_Feet(map_layer.Get_Layer_Number(player_position.x, player_position.y));

        // フロアのレイヤーの今いる座標に自分のレイヤー番号を敷く
        map_layer.Tile_Swap(player.Position, Define_Value.PLAYER_LAYER_NUMBER);
        //// ルームごとのレイヤーも同様
        //room_list[.Now_Room].Tile_Swap(before_position, enemy.Feet);


        // 移動完了
        move_end = true;
    }

    /// <summary>
    /// 周囲８マスにいる敵の方向を向く
    /// </summary>
    void Search_Enemy() { // TODO: ２体以上いるときに１体しか向けない
        var player = Player_Manager.Instance.player_script;
        var player_position = player.Position;
        int tile_scale = Define_Value.TILE_SCALE;

        // 上方向から時計回りに検索
        if (map_layer.Is_Enemy(player_position.x, player_position.y + tile_scale)) {
            player.Direction = eDirection.Up;
        }
        // 右上
        else if (map_layer.Is_Enemy(player_position.x + tile_scale, player_position.y + tile_scale)) {
            player.Direction = eDirection.Upright;
        }
        // 右
        else if (map_layer.Is_Enemy(player_position.x + tile_scale, player_position.y)) {
            player.Direction = eDirection.Right;
        }
        // 右下
        else if (map_layer.Is_Enemy(player_position.x + tile_scale, player_position.y - tile_scale)) {
            player.Direction = eDirection.Downright;
        }
        // 下
        else if (map_layer.Is_Enemy(player_position.x, player_position.y - tile_scale)) {
            player.Direction = eDirection.Down;
        }
        // 左下
        else if (map_layer.Is_Enemy(player_position.x - tile_scale, player_position.y - tile_scale)) {
            player.Direction = eDirection.Downleft;
        }
        // 左
        else if (map_layer.Is_Enemy(player_position.x - tile_scale, player_position.y)) {
            player.Direction = eDirection.Left;
        }
        // 左上
        else if (map_layer.Is_Enemy(player_position.x - tile_scale, player_position.y + tile_scale)) {
            player.Direction = eDirection.Upleft;
        }
    }
}
