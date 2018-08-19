using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの攻撃処理を管理
/// </summary>
public class Player_Attack : MonoBehaviour { //TODO:MonoBehaviourいる？
    /// <summary>
    /// プレイヤー本体のクラス
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
    /// マップを2次元で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;

    void Start() {
        player_script = Player_Manager.Instance.player_script;
        player_action = Player_Manager.Instance.player_action;
        player_status = Player_Manager.Instance.player_status;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
    }

    /// <summary>
    /// プレイヤーの攻撃処理
    /// </summary>
    public void Action_Attack() {
        // プレイヤーの座標
        var player_x = (int)player_script.Position.x;
        var player_y = (int)player_script.Position.y;
        // タイルの大きさ
        int tile_scale = Define_Value.TILE_SCALE;
        // 調整用変数 ４方向は斜めがいらないので0
        int adjust_value_zero = 0; 
        // 調整用変数 １マス先
        int adjust_value_add = Define_Value.TILE_SCALE;

        switch (player_script.Direction) {
            case eDirection.Up:
                // 自分の向いている方向の１マス先に敵がいるか判断
                if (!(map_layer.Is_Enemy(player_x, player_y + tile_scale))) {
                    // 誰もいなかったら素振り後移動状態へ戻す
                    player_action.Player_State = ePlayer_State.Move;
                    break;
                }
                // 攻撃処理を行う
                Attack_Process(adjust_value_zero, adjust_value_add);
                break;
                // 右上
            case eDirection.Upright:
                if (!(map_layer.Is_Enemy(player_x + tile_scale, player_y + tile_scale))) {
                    player_action.Player_State = ePlayer_State.Move;
                    return;
                }
                if (Actor_Action.Slant_Action_Check(player_script as Actor, player_script.Direction)) {
                    Attack_Process(adjust_value_add, adjust_value_add);
                }
                break;
                // 右
            case eDirection.Right:
                if (!(map_layer.Is_Enemy(player_x + tile_scale, player_y))) {
                    player_action.Player_State = ePlayer_State.Move;
                    return;
                }
                Attack_Process(adjust_value_add, adjust_value_zero);
                break;
                // 右下
            case eDirection.Downright:
                if (!(map_layer.Is_Enemy(player_x + tile_scale, player_y - tile_scale))) {
                    player_action.Player_State = ePlayer_State.Move;
                    return;
                }
                if (Actor_Action.Slant_Action_Check(player_script as Actor, player_script.Direction)) {
                    Attack_Process(adjust_value_add, -adjust_value_add);
                }
                break;
                // 下
            case eDirection.Down:
                if (!(map_layer.Is_Enemy(player_x, player_y - tile_scale))) {
                    player_action.Player_State = ePlayer_State.Move;
                    return;
                }
                Attack_Process(adjust_value_zero, -adjust_value_add);
                break;
                　// 左下
            case eDirection.Downleft:
                if (!(map_layer.Is_Enemy(player_x - tile_scale, player_y - tile_scale))) {
                    player_action.Player_State = ePlayer_State.Move;
                    return;
                }
                if (Actor_Action.Slant_Action_Check(player_script as Actor, player_script.Direction)) {
                    Attack_Process(-adjust_value_add, -adjust_value_add);
                }
                break;
                // 左
            case eDirection.Left:
                if (!(map_layer.Is_Enemy(player_x - tile_scale, player_y))) {
                    player_action.Player_State = ePlayer_State.Move;
                    return;
                }
                Attack_Process(-adjust_value_add, adjust_value_zero);
                break;
                // 左上
            case eDirection.Upleft:
                if (!(map_layer.Is_Enemy(player_x - tile_scale, player_y + tile_scale))) {
                    player_action.Player_State = ePlayer_State.Move;
                    return;
                }
                if (Actor_Action.Slant_Action_Check(player_script as Actor, player_script.Direction)) {
                    Attack_Process(-adjust_value_add, adjust_value_add);
                }
                break;
        }
    }

    /// <summary>
    /// 実際に攻撃を行い、ゲームを進める
    /// </summary>
    /// <param name="adjust_value1">これを足し引きして周囲８マスを見る</param>
    /// <param name="adjust_value2">これを足し引きして周囲８マスを見る</param>
    void Attack_Process(int adjust_value1, int adjust_value2) {
        var enemy_manager = Enemy_Manager.Instance;

        // 隣接してるエネミーを調べる
        GameObject side_enemy = enemy_manager.Find_Enemy(player_script.Position.x + adjust_value1,
                                                         player_script.Position.y + adjust_value2);
        // 隣接してるエネミーのステータスを取ってくる
        var enemy_status = side_enemy.GetComponent<Enemy_Controller>().enemy_status;
        var individual_status = side_enemy.GetComponent<Enemy_Controller>().enemy_status.my_status;

        enemy_status.Hit_Point -= Damage_Calculation.Damage(player_status.Attack,
                                                            individual_status.Defence);
        // ダメージのログを流す
        Message_Window_Manager.Player_Attack_Log(player_status, side_enemy,
                                     Damage_Calculation.Damage(player_status.Attack,
                                                               individual_status.Defence));

        // 攻撃した敵が死んでいたら経験値を取得
        if (enemy_status.Is_Dead(enemy_status.Hit_Point)) {
            // レベルアップより先に経験値を取得
            Message_Window_Manager.Get_Experience_Point(player_status, side_enemy, individual_status.Experience_Point);

            // 実際に経験値を取得し、次レベルに必要な経験値量に達しているかを見る
            player_status.Add_Experience_Point(individual_status.Experience_Point);

            // ダンジョンに出現中の敵のリストを取得
            List<GameObject> enemy_list = Enemy_Manager.Instance.enemies;
            // プレイヤーに隣接しているものを抽出
            enemy_manager.Dead_Enemy(enemy_list.IndexOf(side_enemy));
        }

        // 移動状態に戻す
        player_action.Player_State = ePlayer_State.Move;
    }
}
