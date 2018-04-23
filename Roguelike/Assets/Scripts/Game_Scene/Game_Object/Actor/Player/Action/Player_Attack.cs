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
    /// プレイヤースクリプト
    /// </summary>
    Player player_script;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    Player_Action player_action;
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
    /// <summary>
    /// ダンジョン制作クラス
    /// </summary>
    Dungeon_Generator dungeon_generator;

    void Start() {
        player = Player_Manager.Instance.player;
        player_script = player.GetComponent<Player>();
        player_action = Player_Manager.Instance.action;
        enemy = Enemy_Manager.Instance.enemy_script;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
        damage_calculation = new Damage_Calculation();
        dungeon_generator = Dungeon_Manager.Instance.dungeon_generator;
    }

    /// <summary>
    /// プレイヤーの攻撃処理
    /// </summary>
    public void Action_Attack() {
        var player_x = (int)player.transform.position.x;
        var player_y = (int)player.transform.position.y;

        //TODO:攻撃先にいるエネミーの情報を得れるように
        switch (player_script.direction) {
            case eDirection.Up:
                // 自分の向いている方向の１マス先に敵がいるか判断
                if (map_layer.Get(player_x, player_y + Define_Value.TILE_SCALE) ==
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    enemy.enemy_type[0].hit_point -=　
                        (int)damage_calculation.Damage(player_script.attack,
                                                       enemy.enemy_type[0].defence);
                }
                player_action.Set_Action(ePlayer_State.Move);
                break;
            case eDirection.Upright:
                if (map_layer.Get(player_x + Define_Value.TILE_SCALE, player_y + Define_Value.TILE_SCALE) ==
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    enemy.enemy_type[0].hit_point -=
                       (int)damage_calculation.Damage(player_script.attack,
                       enemy.enemy_type[0].defence);
                }
                player_action.Set_Action(ePlayer_State.Move);
                break;
            case eDirection.Right:
                if (map_layer.Get(player_x + Define_Value.TILE_SCALE, player_y) ==
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    enemy.enemy_type[0].hit_point -=
                       (int)damage_calculation.Damage(player_script.attack,
                       enemy.enemy_type[0].defence);
                }
                player_action.Set_Action(ePlayer_State.Move);
                break;
            case eDirection.Downright:
                if (map_layer.Get(player_x + Define_Value.TILE_SCALE, player_y - Define_Value.TILE_SCALE) ==
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    enemy.enemy_type[0].hit_point -=
                        (int)damage_calculation.Damage(player_script.attack,
                        enemy.enemy_type[0].defence);
                }
                player_action.Set_Action(ePlayer_State.Move);
                break;
            case eDirection.Down:
                if (map_layer.Get(player_x, player_y - Define_Value.TILE_SCALE) ==
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    enemy.enemy_type[0].hit_point -=
                       (int)damage_calculation.Damage(player_script.attack,
                       enemy.enemy_type[0].defence);

                }
                player_action.Set_Action(ePlayer_State.Move);
                break;
            case eDirection.Downleft:
                if (map_layer.Get(player_x - Define_Value.TILE_SCALE, player_y - Define_Value.TILE_SCALE) ==
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    enemy.enemy_type[0].hit_point -=
                       (int)damage_calculation.Damage(player_script.attack,
                       enemy.enemy_type[0].defence);
                }
                player_action.Set_Action(ePlayer_State.Move);
                break;
            case eDirection.Left:
                if (map_layer.Get(player_x - Define_Value.TILE_SCALE, player_y) ==
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    enemy.enemy_type[0].hit_point -=
                       (int)damage_calculation.Damage(player_script.attack,
                       enemy.enemy_type[0].defence);
                }
                player_action.Set_Action(ePlayer_State.Move);
                break;
            case eDirection.Upleft:
                if (map_layer.Get(player_x - Define_Value.TILE_SCALE, player_y + Define_Value.TILE_SCALE) ==
                    Define_Value.ENEMY_LAYER_NUMBER) {
                    enemy.enemy_type[0].hit_point -=
                       (int)damage_calculation.Damage(player_script.attack,
                       enemy.enemy_type[0].defence);
                }
                player_action.Set_Action(ePlayer_State.Move);
                break;
        }
    }
}
