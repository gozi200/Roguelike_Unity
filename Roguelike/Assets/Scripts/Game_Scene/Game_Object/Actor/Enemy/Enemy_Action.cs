using UnityEngine;

/// <summary>
/// エネミーの行動を設定する
/// </summary>
public class Enemy_Action {
    /// <summary>
    /// ゲームマネジャー
    /// </summary>
    GameManager game_manager;

    /// <summary>
    /// アクターのマネージャクラス
    /// </summary>
    Enemy_Manager enemy_manager;
    /// <summary>
    /// プレイヤー本体のクラス
    /// </summary>
    Player player;
    /// <summary>
    /// エネミーのステータス関係を管理するクラス
    /// </summary>
    Enemy_Status enemy_status;
    /// <summary>
    /// エネミーの攻撃処理クラス
    /// </summary>
    Enemy_Attack enemy_attack;

    /// <summary>
    /// プレイヤーのステータス関係のクラス
    /// </summary>
    Player_Status player_status;
    /// <summary>
    /// マップを２次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;

    public void Initialize() {
        game_manager = GameManager.Instance;
        enemy_manager = Enemy_Manager.Instance;
        player = Player_Manager.Instance.player_script;
        player_status = Player_Manager.Instance.player_status;
        enemy_status = new Enemy_Status();
        map_layer = Dungeon_Manager.Instance.map_layer_2D;

        enemy_attack = new Enemy_Attack();
        enemy_attack.Initialize();
    }

    /// <summary>
    /// エネミーの行動を制御
    /// </summary>
    public void Action() {
        for (int i = 0; i < enemy_manager.enemies.Count; ++i) {
            if (enemy_manager.enemies[i] == null) {
                continue;
            }
            // 全部Getしていたら重いのでこれを使う
            var my_status = enemy_manager.enemies[i].GetComponent<Enemy_Controller>().enemy_status.my_status;
            // AIによって入れる処理を変える
            switch (my_status.AI_Pattern) {
                case 1:
                    // プレイヤー(攻撃対象)のいる方向
                    eDirection which_player_direction;
                    // いなかったら移動
                    if (!Search_Player(i, out which_player_direction)) {
                        enemy_manager.enemies[i].GetComponent<Enemy_Controller>().enemy_move.Move_Action(i);
                        Set_Enemy_State(enemy_manager.enemies[i], enemy_manager.enemies[i].GetComponent<Enemy>().Feet);
                        break;
                    }
                    // いたら攻撃を行う
                    else if (Search_Player(i, out which_player_direction)) {
                        // プレイヤー(攻撃対象)が斜め方向にいるかを調べる
                        if (Slant_Check(which_player_direction)) {
                            // 調べる敵
                            var enemy = enemy_manager.enemies[i].GetComponent<Enemy>();
                            // 斜め方向へのアクションは不可能になる場合があるので調べる
                            if (Actor_Action.Slant_Action_Check(enemy as Actor, which_player_direction)) {
                                enemy_attack.Attack_Process(enemy_manager.enemies[i], which_player_direction);
                                break;
                            }
                            break;
                        }
                        // 斜め方向を向いていなければ普通に攻撃が通るのでダメージ計算へ
                        else {
                            enemy_attack.Attack_Process(enemy_manager.enemies[i], which_player_direction);
                            break;
                        }
                    }
                    // 隣接しているが攻撃不可能であれば移動する
                    else {
                        enemy_manager.enemies[i].GetComponent<Enemy_Controller>().enemy_move.Move_Action(i);
                        Set_Enemy_State(enemy_manager.enemies[i], enemy_manager.enemies[i].GetComponent<Enemy>().Feet);
                        break;
                    }
                case 2:
                    break;
            }
            // ターンを終える
            enemy_status.End_Turn();
        }
    }

    /// <summary>
    /// 斜め方向を向いているかを調べる
    /// </summary>
    /// <param name="direction">調べる方向</param>
    /// <returns>斜めを向いていたらtrue</returns>
    bool Slant_Check(eDirection direction) {
        return direction == eDirection.Upleft    ||
               direction == eDirection.Downright ||
               direction == eDirection.Downleft  ||
               direction == eDirection.Upright;
    }

    /// <summary>
    /// プレイヤーが隣接しているマスにいるかを調べる
    /// </summary>
    /// <param name="index">エネミーリストのインデックス番号</param>
    /// <param name="direction">自分から見てどの方向にいるのかを保持しておく</param>
    /// <returns>プレイヤーがいればtrue</returns>
    bool Search_Player(int index, out eDirection direction) {
        var enemy = Enemy_Manager.Instance.enemies[index].GetComponent<Enemy>();
        // 長くなるので１時変数に格納
        int enemy_x = enemy_manager.enemies[index].GetComponent<Enemy>().Position.x;
        int enemy_y = enemy_manager.enemies[index].GetComponent<Enemy>().Position.y;

        // 上方向から時計回りに検索
        if (map_layer.Is_Player(enemy_x, enemy_y + Define_Value.TILE_SCALE)) {
            direction = eDirection.Up;
            return true;
        }
        // 右上
        else if (map_layer.Is_Player(enemy_x + Define_Value.TILE_SCALE, enemy_y + Define_Value.TILE_SCALE)) {
            if (Actor_Action.Slant_Action_Check(enemy as Actor, eDirection.Upright)) {
                direction = eDirection.Upright;
                return true;
            }
        }
        // 右
        else if (map_layer.Is_Player(enemy_x + Define_Value.TILE_SCALE, enemy_y)) {
            direction = eDirection.Right;
            return true;
        }
        // 右下
        else if (map_layer.Is_Player(enemy_x + Define_Value.TILE_SCALE, enemy_y - Define_Value.TILE_SCALE)) {
            if (Actor_Action.Slant_Action_Check(enemy as Actor, eDirection.Downright)) {
                direction = eDirection.Downright;
                return true;
            }
        }
        // 下
        else if (map_layer.Is_Player(enemy_x, enemy_y - Define_Value.TILE_SCALE)) {
            direction = eDirection.Down;
            return true;
        }
        // 左下
        else if (map_layer.Is_Player(enemy_x - Define_Value.TILE_SCALE, enemy_y - Define_Value.TILE_SCALE)) {
            if (Actor_Action.Slant_Action_Check(enemy as Actor, eDirection.Downleft)) {
                direction = eDirection.Downleft;
                return true;
            }
        }
        // 左
        else if (map_layer.Is_Player(enemy_x - Define_Value.TILE_SCALE, enemy_y)) {
            direction = eDirection.Left;
            return true;
        }
        // 左上
        else if (map_layer.Is_Player(enemy_x - Define_Value.TILE_SCALE, enemy_y + Define_Value.TILE_SCALE)) {
            if (Actor_Action.Slant_Action_Check(enemy as Actor, eDirection.Upleft)) {
                direction = eDirection.Upleft;
                return true;
            }
        }
        direction = eDirection.Finish;
        return false;
        //TODO:次にパートナーを探す
    }

    /// <summary>
    /// エネミーの状態を設定する
    /// </summary>
    /// <param name="feet">足元のレイヤー番号</param>
    void Set_Enemy_State(GameObject enemy, int feet) {
        var enemy_script = enemy.GetComponent<Enemy>();


        switch (feet) {
            case Define_Value.ROOM_LAYER_NUMBER:
                enemy_script.mode = eEnemy_Mode.Move_Room_Mode;
                break;
            case Define_Value.ENTRANCE_LAYER_NUMBER:
                // 迷子になっていたら検索させる
                if (enemy_script.Is_Lost_Myself) {
                    enemy.GetComponent<Enemy_Controller>().enemy_move.Stack_Route_Until_Entrance();
                    break;
                }

                // 通路から部屋への進入
                if (enemy_script.mode == eEnemy_Mode.Move_Road_Mode) {
                    var enemy_status = enemy.GetComponent<Enemy_Controller>().enemy_status;

                    enemy_status.Where_Room(enemy_script.Position.x, enemy_script.Position.y);
                    enemy_script.mode = eEnemy_Mode.Move_Room_Mode;
                    enemy.GetComponent<Enemy_Controller>().enemy_move.Stack_Route_Until_Entrance();
                }
                // 部屋から通路への進入
                else if (enemy_script.mode == eEnemy_Mode.Move_Room_Mode) {
                    enemy_script.mode = eEnemy_Mode.Move_Road_Mode;
                }
                break;
        }
    }
}
