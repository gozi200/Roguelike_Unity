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
    /// プレイヤーのスクリプト
    /// </summary>
    Player player_script;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    Player_Action player_action;
    /// <summary>
    /// プレイヤーのステータス関係を管理するクラス
    /// </summary>
    Player_Status player_status;
    /// <summary>
    /// マップを2次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;
    /// <summary>
    /// 攻撃で発生するダメージを扱うクラス
    /// </summary>
    Damage_Calculation damage_calculation;

    void Start() {
        player = Player_Manager.Instance.player;
        player_script = Player_Manager.Instance.player_script;
        player_action = Player_Manager.Instance.player_action;
        player_status = Player_Manager.Instance.player_status;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
        damage_calculation = new Damage_Calculation();
    }

    /// <summary>
    /// プレイヤーの攻撃処理
    /// </summary>
    public void Action_Attack() {
        // プレイヤーの座標
        var player_x = (int)player_script.position.x;
        var player_y = (int)player_script.position.y;
        // タイルの大きさ
        int tile_scale = Define_Value.TILE_SCALE;
        // 調整用変数 ４方向は斜めがいらないので0
        int adjust_value_zero = 0; 
        // 調整用変数 １マス先
        int adjust_value_add = Define_Value.TILE_SCALE;

        switch (player_script.direction) {
            case eDirection.Up:
                // 自分の向いている方向の１マス先に敵がいるか判断 上から順に時計回りに検索
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
        var enemy_manager = new Enemy_Manager();

        // 隣接してるエネミーを調べる
        GameObject side_enemy = enemy_manager.Find_Enemy(player_script.position.x + adjust_value1,
                                                         player_script.position.y + adjust_value2);
        // 隣接してるエネミーのステータスを取ってくる
        var enemy_status = side_enemy.GetComponent<Enemy_Controller>().enemy_status;

        enemy_status.hit_point -= damage_calculation.Damage(player_status.attack,
                                                            enemy_status.defence);
        // ダメージのログを流す
        Log_Scroll.Player_Attack_Log(player_status, side_enemy, damage_calculation.Damage(player_status.attack,
                                                                                          enemy_status.defence));

        // 攻撃した敵が死んでいたら経験値を取得
        if (enemy_status.hit_point <= 0) {
            //　レベルアップより先に経験値を取得
            Log_Scroll.Get_Experience_Point(player_status, side_enemy, enemy_status.experience_point);

            // 実際に経験値を取得し、次レベルに必要な経験値量に達しているかを見る
            player_status.Add_Experience_Point(enemy_status.experience_point);

            // ダンジョンに出現中の敵のリストを取得
            List<GameObject> enemy_list = Enemy_Manager.Instance.enemies;
            // プレイヤーに隣接しているものを抽出
            enemy_manager.Dead_Enemy(enemy_list.IndexOf(side_enemy));
        }
        // 移動状態に戻す
        player_action.Set_Action(ePlayer_State.Move);
    }
}
