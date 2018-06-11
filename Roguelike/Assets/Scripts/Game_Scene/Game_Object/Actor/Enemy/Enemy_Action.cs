using System.Collections;
using System.Collections.Generic;
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
    /// エネミー本体のクラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// エネミーのステータス関係を管理するクラス
    /// </summary>
    Enemy_Status enemy_status;
    /// <summary>
    /// エネミーの移動を制御するクラス
    /// </summary>
    Enemy_Move enemy_move;

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
    /// 自分の座標
    /// </summary>
    Vector3 enemy_position;

    public void Initialize() {
        game_manager = GameManager.Instance;
        enemy_manager = Enemy_Manager.Instance;
        player = Player_Manager.Instance.player_script;
        player_status = Player_Manager.Instance.player_status;
        enemy_status = new Enemy_Status();
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
        damage_calculation = Game.Instance.damage_calculation;
    }

    /// <summary>
    /// エネミーの行動を制御
    /// </summary>
    public void Action() {
        for (int i = 0; i < enemy_manager.enemies.Count; ++i) {
            // 全部Getしていたら重いのでこれを使う
            var enemy_status = enemy_manager.enemies[i].GetComponent<Enemy_Controller>().enemy_status;

            // AIによって入れる処理を変える
            switch (enemy_status.AI_pattern) {
                case 1:
                    if (Search_Player(i)) {
                        player_status.hit_point.Value -= damage_calculation.Damage(enemy_status.attack, player_status.defence);
                        break;
                    }
                    else {
                        enemy_manager.enemies[i].GetComponent<Enemy_Controller>().enemy_move.Move_Action(i);
                        Set_Enemy_State(enemy_manager.enemies[i].GetComponent<Enemy>().mode, enemy_manager.enemies[i].GetComponent<Enemy>().Feet);
                        break;
                    }
            }
            // ターンを終える
            enemy_status.End_Turn();
        }
    }

    /// <summary>
    /// プレイヤーが隣接したマスにいるかどうか
    /// </summary>
    /// <returns>プレイヤーがいた場合はtrue</returns>
    bool Search_Player(int index) {
        var enemy = Enemy_Manager.Instance.enemies[index].GetComponent<Enemy>();
        // 長くなるので１時変数に格納
        int enemy_x = enemy_manager.enemies[index].GetComponent<Enemy>().position.x;
        int enemy_y = enemy_manager.enemies[index].GetComponent<Enemy>().position.y;
        // 移動量
        int move_value = Define_Value.MOVE_VAULE;

        // 上方向から時計回りに検索
        if (map_layer.Get(enemy_x, enemy_y + Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Up;
            return true;
        }
        // 右上
        else if (map_layer.Get(enemy_x + Define_Value.TILE_SCALE, enemy_y + Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (Actor_Action.Slant_Check(map_layer.Get(enemy_x + move_value, enemy_y),
                            map_layer.Get(enemy_x, enemy_y + move_value))) {
                return false;
            }
            enemy.direction = eDirection.Upright;
            return true;
        }
        // 右
        else if (map_layer.Get(enemy_x + Define_Value.TILE_SCALE, enemy_y) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Right;
            return true;

        }
        // 右下
        else if (map_layer.Get(enemy_x + Define_Value.TILE_SCALE, enemy_y - Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (Actor_Action.Slant_Check(map_layer.Get(enemy_x + move_value, enemy_y),
                            map_layer.Get(enemy_x, enemy_y - move_value))) {
                return false;
            }
            enemy.direction = eDirection.Downright;
            return true;
        }
        // 下
        else if (map_layer.Get(enemy_x, enemy_y - Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Down;
            return true;
        }
        // 左下
        else if (map_layer.Get(enemy_x - Define_Value.TILE_SCALE, enemy_y - Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (Actor_Action.Slant_Check(map_layer.Get(enemy_x - move_value, enemy_y),
                            map_layer.Get(enemy_x, enemy_y - move_value))) {
                return false;
            }
            enemy.direction = eDirection.Downleft;
            return true;
        }
        // 左
        else if (map_layer.Get(enemy_x - Define_Value.TILE_SCALE, enemy_y) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Left;
            return true;
        }
        // 左上
        else if (map_layer.Get(enemy_x - Define_Value.TILE_SCALE, enemy_y + Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (Actor_Action.Slant_Check(map_layer.Get(enemy_x - move_value, enemy_y),
                            map_layer.Get(enemy_x, enemy_y + move_value))) {
                return false;
            }
            enemy.direction = eDirection.Upleft;
            return true;
        }
        return false;
        //TODO:次にパートナーを探す
    }

    /// <summary>
    /// エネミーの状態を設定する
    /// </summary>
    /// <param name="feet">足元のレイヤー番号</param>
    void Set_Enemy_State(eEnemy_Mode mode, int feet) {
        switch (feet) {
            case Define_Value.TILE_LAYER_NUMBER:
                mode = eEnemy_Mode.Move_Floor_Mode;
                break;
            case Define_Value.ENTRANCE_LAYER_NUMBER:
                // 通路から部屋への進入
                if (mode == eEnemy_Mode.Move_Road_Mode) {
                    mode = eEnemy_Mode.Move_Floor_Mode;
                    enemy_move.Stack_List();
                }
                // 部屋から通路への進入
                else if (mode == eEnemy_Mode.Move_Floor_Mode) {
                    mode = eEnemy_Mode.Move_Road_Mode;
                }
                break;
        }
    }
}
