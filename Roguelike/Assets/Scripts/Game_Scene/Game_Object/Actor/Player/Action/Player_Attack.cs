using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの攻撃処理を管理するクラス
/// </summary>
public class Player_Attack : MonoBehaviour{
    /// <summary>
    /// プレイヤーの本体
    /// </summary>
    GameObject player;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    Player_Action player_action;
    /// <summary>
    /// プレイヤーのステータス関係を管理するクラス
    /// </summary>
    Player_Status player_status;
    /// <summary>
    /// エネミーの本体のクラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// マップを2次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;
    /// <summary>
    /// 攻撃で発生するダメージを扱うクラス
    /// </summary>
    Damage_Calculation damage_calculation;

    void Start() {
        player = Actor_Manager.Instance.player;
        player_action = Actor_Manager.Instance.player_action;
        player_status = Actor_Manager.Instance.player_status;
        enemy = Actor_Manager.Instance.enemy_script;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
        damage_calculation = new Damage_Calculation();
    }

    /// <summary>
    /// プレイヤーの攻撃処理
    /// </summary>
    public void Action_Attack() {
        var player_script = player.GetComponent<Player>();
        // プレイヤーの座標
        var player_x = (int)player.transform.position.x;
        var player_y = (int)player.transform.position.y;
        // タイルの大きさ
        var tile_scale = Define_Value.TILE_SCALE;
        // 調整用変数 ４方向は斜めがいらないので0
        var adjust_value_zero = 0; 
        // 調整用変数 １マス先
        var adjust_value_add = Define_Value.TILE_SCALE;

        switch (player_script.direction) {
            case eDirection.Up:
                // 自分の向いている方向の１マス先に敵がいるか判断
                if (map_layer.Get(player_x, player_y + tile_scale) !=
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    player_action.Set_Action(ePlayer_State.Move);
                    return;
                }
                Attack_Process(adjust_value_zero, adjust_value_add);
                break;
            case eDirection.Upright:
                if (map_layer.Get(player_x + tile_scale, player_y + tile_scale) !=
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    player_action.Set_Action(ePlayer_State.Move);
                    return;
                }
                Attack_Process(adjust_value_add, adjust_value_add);
                break;
            case eDirection.Right:
                if (map_layer.Get(player_x + tile_scale, player_y) !=
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    player_action.Set_Action(ePlayer_State.Move);
                    return;
                }
                Attack_Process(adjust_value_add, adjust_value_zero);
                break;
            case eDirection.Downright:
                if (map_layer.Get(player_x + tile_scale, player_y - tile_scale) !=
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    player_action.Set_Action(ePlayer_State.Move);
                    return;
                }
                Attack_Process(adjust_value_add, -adjust_value_add);
                break;
            case eDirection.Down:
                if (map_layer.Get(player_x, player_y - tile_scale) !=
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    player_action.Set_Action(ePlayer_State.Move);
                    return;
                }
                Attack_Process(adjust_value_zero, -adjust_value_add);
                break;
            case eDirection.Downleft:
                if (map_layer.Get(player_x - tile_scale, player_y - tile_scale) !=
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    player_action.Set_Action(ePlayer_State.Move);
                    return;
                }
                Attack_Process(-adjust_value_add, -adjust_value_add);
                break;
            case eDirection.Left:
                if (map_layer.Get(player_x - tile_scale, player_y) !=
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    player_action.Set_Action(ePlayer_State.Move);
                    return;
                }
                Attack_Process(-adjust_value_add, adjust_value_zero);
                break;
            case eDirection.Upleft:
                if (map_layer.Get(player_x - tile_scale, player_y + tile_scale) !=
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    player_action.Set_Action(ePlayer_State.Move);
                    return;
                }
                Attack_Process(-adjust_value_add, adjust_value_add);
                break;
        }
    }

    /// <summary>
    /// 実際に攻撃を行い、ゲームを進める
    /// </summary>
    /// <param name="on_first">1マス先を取るのに使用</param>
    void Attack_Process(int adjust_value1, int adjust_value2) {
        var actor_manager = Actor_Manager.Instance.actor_manager;

        // 隣接してるエネミーを調べる
        var side_enemy = actor_manager.Find_Enemy((int)player.transform.position.x + adjust_value1,
                                                  (int)player.transform.position.y + adjust_value2);
        // 隣接してるエネミーのステータスを取ってくる
        var target_enemy = side_enemy.GetComponent<Enemy_Status>();

        target_enemy.hit_point -= (int)damage_calculation.Damage(player_status.attack,
                                                                 target_enemy.defence);
        player_action.Set_Action(ePlayer_State.Move);

        // 攻撃した敵が死んでしたら経験値を取得
        if (target_enemy.hit_point <= 0) {
            player_status.Add_Experience_Point(target_enemy.experience_point);
        }
    }
}
