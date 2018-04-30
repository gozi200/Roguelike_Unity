using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの行動を設定する
/// </summary>
public class Enemy_Action : MonoBehaviour {
    /// <summary>
    /// ゲームマネジャー
    /// </summary>
    GameManager game_manager;
    /// <summary>
    /// アクターのマネージャー
    /// </summary>
    Actor_Manager actor_manager;
    /// <summary>
    /// プレイヤー本体のクラス
    /// </summary>
    Player player;
    /// <summary>
    /// エネミー本体のクラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// エネミーのステータス関係を管理するクラス
    /// </summary>
    Enemy_Status enemy_status;
    /// <summary>
    /// アクター共通で使える行動制御を管理するクラス
    /// </summary>
    Actor_Action actor_action;
    /// <summary>
    /// プレイヤーのステータス関係のクラス
    /// </summary>
    Player_Status player_status;
    /// <summary>
    /// マップを２次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;
    /// <summary>
    /// ダメージ計算クラス
    /// </summary>
    Damage_Calculation damage_calculation;

    /// <summary>
    /// エネミーのいる座標
    /// </summary>
    Vector3 enemy_position;

    /// <summary>
    /// 移動が終了したかを判断
    /// </summary>
    bool move_end;

    void Start() {
        game_manager = GameManager.Instance.game_manager;
        actor_manager = Actor_Manager.Instance.actor_manager;
        player = Actor_Manager.Instance.player_script;
        enemy = Actor_Manager.Instance.enemy_script;
        enemy_status = Actor_Manager.Instance.enemy_status;
        player_status = Actor_Manager.Instance.player_status;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
        actor_action = Actor_Manager.Instance.actor_action;
        damage_calculation = Game.Instance.damage_calculation;

        enemy.direction = eDirection.Down;
    }

    /// <summary>
    /// エネミーの行動処理
    /// </summary>
    public void Move_Enemy() {
        for (int i = 0; i < actor_manager.enemys.Count; ++i) {
            // 全部やっていたら重いのでこれを使う
            var enemy_status = actor_manager.enemys[i].GetComponent<Enemy_Status>();
            switch (enemy_status.AI_pattern) {
                case 1:
                    if (Search_Player(i)) {
                        player_status.hit_point -= (int)damage_calculation.Damage(enemy_status.attack, player_status.defence);
                        break;
                    }
                    Move(i);
                    break;
            }
        // ターンを進める
        enemy_status.Add_Turn(i);
        }
    }

    /// <summary>
    /// プレイヤーが隣接したマスにいるかどうか
    /// </summary>
    /// <returns>プレイヤーがいた場合はtrue</returns>
    bool Search_Player(int index) {
        // 長くなるので１時変数に格納
        var enemy_x = actor_manager.enemys[index].transform.position.x;
        var enemy_y = actor_manager.enemys[index].transform.position.y;
        // 移動量
        var move_value = Define_Value.MOVE_VAULE;
        // 上方向から時計回りに検索
        if (map_layer.Get_(enemy_x, enemy_y + Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Up;
            return true;
        }
        // 右上
        else if (map_layer.Get_(enemy_x + Define_Value.TILE_SCALE, enemy_y + Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (actor_action.Slant_Check(map_layer.Get_(enemy_x + move_value, enemy_y),
                                         map_layer.Get_(enemy_x, enemy_y + move_value))) {
                return false;
            }
            enemy.direction = eDirection.Upright;
            return true;
        }
        // 右
        else if (map_layer.Get_(enemy_x + Define_Value.TILE_SCALE, enemy_y) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Right;
            return true;

        }
        // 右下
        else if (map_layer.Get_(enemy_x + Define_Value.TILE_SCALE, enemy_y - Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (actor_action.Slant_Check(map_layer.Get_(enemy_x + move_value, enemy_y),
                                         map_layer.Get_(enemy_x, enemy_y - move_value))) {
                return false;
            }
            enemy.direction = eDirection.Downright;
            return true;
        }
        // 下
        else if (map_layer.Get_(enemy_x, enemy_y - Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Down;
            return true;
        }
        // 左下
        else if (map_layer.Get_(enemy_x - Define_Value.TILE_SCALE, enemy_y - Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (actor_action.Slant_Check(map_layer.Get_(enemy_x - move_value, enemy_y),
                                         map_layer.Get_(enemy_x, enemy_y - move_value))) {
                return false;
            }
            enemy.direction = eDirection.Downleft;
            return true;
        }
        // 左
        else if (map_layer.Get_(enemy_x - Define_Value.TILE_SCALE, enemy_y) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Left;
            return true;
        }
        // 左上
        else if (map_layer.Get_(enemy_x - Define_Value.TILE_SCALE, enemy_y + Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (actor_action.Slant_Check(map_layer.Get_(enemy_x - move_value, enemy_y),
                                         map_layer.Get_(enemy_x, enemy_y + move_value))) {
                return false;
            }
            enemy.direction = eDirection.Upleft;
            return true;
        }
        return false;
        //TODO:次にパートナーを探す
    }

    /// <summary>
    /// エネミーの移動処理
    /// </summary>
    void Move(int index) {
        // リスト要素ごとの持っているエネミースクリプトを格納
        var enemy_script = actor_manager.enemys[index].GetComponent<Enemy>();
        // エネミーのx座標
        var enemy_x = enemy_script.transform.position.x;
        // エネミーのy座標
        var enemy_y = enemy_script.transform.position.y;
        // 移動量
        var move_value = Define_Value.MOVE_VAULE;
        // 上下左右の動きに使う(斜め方向には0を足す)
        var not_move = 0;
        move_end = false;

        while (!move_end) {
            // 移動方向を乱数で決める
            int random_direction = Random.Range(0, (int)eDirection.Finish);
            // 上述のint型乱数をenum型にキャスト
            eDirection cast_random_direction = (eDirection)random_direction;

            switch (cast_random_direction) {
                case eDirection.Up:
                    // 進行方向が移動可能かを判断
                    if (actor_action.Move_Check(map_layer.Get_(enemy_x, enemy_y),
                                                map_layer.Get_(enemy_x, enemy_y + move_value))) {
                        return;
                    }
                    Move_Process(not_move, move_value, index);
                    move_end = true;
                    break;
                case eDirection.Upright:
                    // 進行方向が移動可能かを判断
                    if (actor_action.Move_Check(map_layer.Get_(enemy_x, enemy_y),
                                                map_layer.Get_(enemy_x + move_value, enemy_y + move_value))) {
                        return;
                    }
                    // 右方向か上方向に壁があるとき(移動不可になる)
                    if (actor_action.Slant_Check(map_layer.Get_(enemy_x + move_value, enemy_y),
                                                      map_layer.Get_(enemy_x, enemy_y + move_value))) {
                        return;
                    }
                    Move_Process(move_value, move_value, index);
                    move_end = true;
                    break;
                case eDirection.Right:
                    if (actor_action.Move_Check(map_layer.Get_(enemy_x , enemy_y),
                                                map_layer.Get_(enemy_x + move_value, enemy_y))) {
                        return;
                    }
                    Move_Process(move_value, not_move, index);
                    move_end = true;
                    break;
                case eDirection.Downright:
                    // 進行方向が移動可能かを判断
                    if (actor_action.Move_Check(map_layer.Get_(enemy_x, enemy_y),
                                                map_layer.Get_(enemy_x + move_value, enemy_y - move_value))) {
                        return;
                    }
                    // 右方向か下方向に壁があるとき(移動不可になる)
                    if (actor_action.Slant_Check(map_layer.Get_(enemy_x + move_value, enemy_y),
                                                      map_layer.Get_(enemy_x, enemy_y - move_value))) {
                        return;
                    }
                    Move_Process(move_value, -move_value, index);
                    move_end = true;
                    break;
                case eDirection.Down:
                    if (actor_action.Move_Check(map_layer.Get_(enemy_x, enemy_y),
                                                map_layer.Get_(enemy_x, enemy_y - move_value))) {
                        return;
                    }
                    Move_Process(not_move, -move_value, index);
                    move_end = true;
                    break;
                case eDirection.Downleft:
                    // 進行方向が移動可能かを判断
                    if (actor_action.Move_Check(map_layer.Get_(enemy_x, enemy_y),
                                                map_layer.Get_(enemy_x + move_value, enemy_y - move_value))) {
                        return;
                    }
                    // 左方向か下方向に壁があるとき(移動不可になる)
                    if (actor_action.Slant_Check(map_layer.Get_(enemy_x - move_value, enemy_y),
                                                      map_layer.Get_(enemy_x, enemy_y - move_value))) {
                        return;
                    }
                    Move_Process(-move_value, -move_value, index);
                    move_end = true;
                    break;
                case eDirection.Left:
                    if (actor_action.Move_Check(map_layer.Get_(enemy_x, enemy_y),
                                                map_layer.Get_(enemy_x - move_value, enemy_y))) {
                        return;
                    }
                    Move_Process(-move_value, not_move, index);
                    move_end = true;
                    break;
                case eDirection.Upleft:
                    // 進行方向が移動可能かを判断
                    if (actor_action.Move_Check(map_layer.Get_(enemy_x, enemy_y),
                                                map_layer.Get_(enemy_x - move_value, enemy_y + move_value))) {
                        return;
                    }
                    // 左方向か上方向に壁があるとき(移動不可になる)
                    if (actor_action.Slant_Check(map_layer.Get_(enemy_x - move_value, enemy_y),
                                                      map_layer.Get_(enemy_x, enemy_y + move_value))) {
                        return;
                    }
                    Move_Process(-move_value, +move_value, index);
                    break;
            }
        }
    }

    /// <summary>
    /// 実際にエネミーを動かしゲームを進める
    /// </summary>
    /// <param name="move_value_x">プレイヤーの移動量</param>
    /// <param name="move_value_y">プレイヤーの移動量</param>
    void Move_Process(int move_value_x, int move_value_y, int index) {
        // リスト要素ごとの持っているエネミースクリプトを格納
        var enemy_script = actor_manager.enemys[index].GetComponent<Enemy>();

        enemy_position = enemy_script.Get_Position();
        map_layer.Tile_Swap(enemy_script.transform.position,
                            enemy_script.feet);
        enemy_position.x += move_value_x;
        enemy_position.y += move_value_y;
        enemy_script.Set_Position(enemy_position);
        enemy_script.Set_Feet(map_layer.Get_(enemy_position.x, enemy_position.y));
        map_layer.Tile_Swap(actor_manager.enemys[index].GetComponent<Enemy>().transform.position, 
                            Define_Value.ENEMY_LAYER_NUMBER);
        move_end = true;
    }
}
