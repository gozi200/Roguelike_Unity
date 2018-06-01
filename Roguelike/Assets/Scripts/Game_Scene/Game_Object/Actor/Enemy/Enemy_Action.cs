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
    /// 自分の座標
    /// </summary>
    Vector3 enemy_position;

    /// <summary>
    /// 移動が終了したかを判断
    /// </summary>
    bool move_end;

    void Start() {
        game_manager = GameManager.Instance;
        actor_manager = Actor_Manager.Instance;
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
    /// エネミーの行動を制御
    /// </summary>
    public void Action() {
        for (int i = 0; i < actor_manager.enemys.Count; ++i) {
            // 全部Getしていたら重いのでこれを使う
            var enemy_status = actor_manager.enemys[i].GetComponent<Enemy_Status>();

            // AIによって入れる処理を変える
            switch (enemy_status.AI_pattern) {
                case 1:
                    if (Search_Player(i)) {
                        player_status.hit_point.Value -= damage_calculation.Damage(enemy_status.attack, player_status.defence);
                        break;
                    }
                    else {
                        actor_manager.enemys[i].GetComponent<Enemy_Move>().Move_Action();
                        Set_Enemy_State(actor_manager.enemys[i].GetComponent<Enemy>().mode, actor_manager.enemys[i].GetComponent<Enemy>().feet);
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
        var enemy = Actor_Manager.Instance.enemys[index].GetComponent<Enemy>();
        // 長くなるので１時変数に格納
        float enemy_x = actor_manager.enemys[index].transform.position.x;
        float enemy_y = actor_manager.enemys[index].transform.position.y;
        // 移動量
        int move_value = Define_Value.MOVE_VAULE;

        // 上方向から時計回りに検索
        if (map_layer.Get(enemy_x, enemy_y + Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            enemy.direction = eDirection.Up;
            return true;
        }
        // 右上
        else if (map_layer.Get(enemy_x + Define_Value.TILE_SCALE, enemy_y + Define_Value.TILE_SCALE) == Define_Value.PLAYER_LAYER_NUMBER) {
            if (actor_action.Slant_Check(map_layer.Get(enemy_x + move_value, enemy_y),
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
            if (actor_action.Slant_Check(map_layer.Get(enemy_x + move_value, enemy_y),
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
            if (actor_action.Slant_Check(map_layer.Get(enemy_x - move_value, enemy_y),
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
            if (actor_action.Slant_Check(map_layer.Get(enemy_x - move_value, enemy_y),
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
                    actor_manager.enemy_move.Stack_List();
                }
                // 部屋から通路への進入
                else if (mode == eEnemy_Mode.Move_Floor_Mode) {
                    mode = eEnemy_Mode.Move_Road_Mode;
                }
                break;
        }
    }

    /// <summary>
    /// レイヤー番号を入れ替える
    /// </summary>
    /// <param name="index">要素番号。何番目の敵なのか</param>
    public void Set_Tile(int index) {
        // 要素数に使うので0からの値に合わせる
        index -= 1;
        var enemy_position = actor_manager.enemys[index].transform.position;
        enemy.Set_Feet(map_layer.Get(enemy_position.x, enemy_position.y));

        Debug.Log("Enemyfeet: = " + gameObject.GetComponent<Enemy>().feet);

        map_layer.Tile_Swap(actor_manager.enemys[index].GetComponent<Enemy>().transform.position,
                            Define_Value.ENEMY_LAYER_NUMBER);
        move_end = true;
    }
}
