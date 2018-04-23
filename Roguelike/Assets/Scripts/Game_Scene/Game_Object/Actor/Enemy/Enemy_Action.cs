using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの行動を設定する
/// </summary>
public class Enemy_Action : MonoBehaviour {
    /// <summary>
    /// エネミー本体のクラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// プレイヤーのマネージャー
    /// </summary>
    Player_Manager manager;
    /// <summary>
    /// プレイヤースクリプト
    /// </summary>
    Player player_script;
    /// <summary>
    /// ダンジョンの管理クラス
    /// </summary>
    Dungeon_Manager dungeon_manager;
    /// <summary>
    /// ダンジョンを制作するクラス
    /// </summary>
    static Dungeon_Generator dungeon_generator;
    /// <summary>
    /// エネミーのモードを判断する
    /// </summary>
    eEnemy_Mode mode;
    /// <summary>
    /// エネミーの向いている方向を判断する
    /// </summary>
    eDirection direction;

    void Start() {
        enemy = Enemy_Manager.Instance.enemy_script;
        player_script = Player_Manager.Instance.player_script;

        direction = eDirection.Down;
    }

    /// <summary>
    /// エネミーの行動処理
    /// </summary>
    public void Move_Enemy() {
        for (int i = 0; i < 1; ++i) {
        Debug.Log(enemy);
            switch (enemy.enemy_type[i].AI_pattern) {
                case 2:
                       // if (Search_Player(player.transform.position.x, player.transform.position.y)) {
                       //     player_hit_point -= (int)damage_calculation.Damage(enemy.GetComponent<Enemy>().enemys[i].attack, Random.Range(87, 112 + 1), 0);
                       //     Debug.Log(player_hit_point);
                       //     break;
                       // }
                    Move();
                    break;
            }
        }
        // dungeon_manager.GetComponent<Dungeon_Generator>().Turn_Tick();
    }

    /// <summary>
    /// プレイヤーが隣接したマスにいるかどうか
    /// </summary>
    /// <returns>プレイヤーがいた場合はtrue</returns>
    bool Search_Player(float player_position_x, float player_position_y) {
        // 下方向から時計回りに検索
        if (gameObject.transform.position.x == player_position_x && gameObject.transform.position.y - 5 == player_position_y) {
            direction = eDirection.Down;
            return true;
        }
        else if (gameObject.transform.position.x - 5 == player_position_x && gameObject.transform.position.y - 5 == player_position_y) {
            direction = eDirection.Downleft;
            return true;
        }
        else if (gameObject.transform.position.x - 5 == player_position_x && gameObject.transform.position.y == player_position_y) {
            direction = eDirection.Left;
            return true;
        }
        else if (gameObject.transform.position.x - 5 == player_position_x && gameObject.transform.position.y + 5 == player_position_y) {
            direction = eDirection.Upleft;
            return true;
        }
        else if (gameObject.transform.position.x == player_position_x && gameObject.transform.position.y + 5 == player_position_y) {
            direction = eDirection.Up;
            return true;
        }
        else if (gameObject.transform.position.x + 5 == player_position_x && gameObject.transform.position.y + 5 == player_position_y) {
            direction = eDirection.Upright;
            return true;
        }
        else if (gameObject.transform.position.x + 5 == player_position_x && gameObject.transform.position.y == player_position_y) {
            direction = eDirection.Right;
            return true;

        }
        else if (gameObject.transform.position.x + 5 == player_position_x && gameObject.transform.position.y - 5 == player_position_y) {
            direction = eDirection.Downright;
            return true;
        }
        return false;
    }

    /// <summary>
    /// エネミーの移動処理
    /// </summary>
    void Move() {
        for (int index = 0; index < dungeon_generator.enemys.Count; ++index) {
            // 移動後の横座標
            int width = 0;
            // 移動後の縦座標
            int height = 0;
            // 移動方向を乱数で決める
            int random_direction = Random.Range(0, (int)eDirection.Finish);
            // 上述のint型乱数をenum型にキャスト
            eDirection cast_random_direction = (eDirection)random_direction;
            // 移動が完了したかどうかのフラグ 完了であればtrue
            bool move_flag = false;
            // 移動が可能かどうかのフラグ 可能であればtrue
            bool movement = false;

            switch (cast_random_direction) {
                case eDirection.Up:
                    width    = (int)dungeon_generator.enemys[index].transform.position.x;
                    height   = (int)dungeon_generator.enemys[index].transform.position.y + 1;
                    break;

                /*case eDirection.Upright:
                    width  = enemy_list[index].Width  + 1;
                    height = enemy_list[index].Height + 1;
                    if (!Dungeon_Base.Is_Check_Slant_Move(enemy_list[index].Height + 1, enemy_list[index].Width, enemy_list[index].Height, enemy_list[index].Width + 1, layer_number)) {
                        movement = Dungeon_Base.Is_Check_Move(enemy_list[index].Height + 1, enemy_list[index].Width + 1, layer_number);
                    }
                    break;

                case eDirection.Right:
                    width  = enemy_list[index].Width + 1;
                    height = enemy_list[index].Height;
                    movement = Dungeon_Base.Is_Check_Move(enemy_list[index].Height - 1, enemy_list[index].Width, layer_number);
                    break;

                case eDirection.Downright:
                    width  = enemy_list[index].Width  + 1;
                    height = enemy_list[index].Height - 1;
                    if (!Dungeon_Base.Is_Check_Slant_Move(enemy_list[index].Height - 1, enemy_list[index].Width, enemy_list[index].Height, enemy_list[index].Width + 1, layer_number)) {
                        movement = Dungeon_Base.Is_Check_Move(enemy_list[index].Height + 1, enemy_list[index].Width + 1, layer_number);
                    }
                    break;

                case eDirection.Down:
                    width = enemy_list[index].Width;
                    height = enemy_list[index].Height - 1;
                    movement = Dungeon_Base.Is_Check_Move(enemy_list[index].Height - 1, enemy_list[index].Width, layer_number);
                    break;

                case eDirection.Downleft:
                    width  = enemy_list[index].Width  - 1;
                    height = enemy_list[index].Height - 1;
                    if (!Dungeon_Base.Is_Check_Slant_Move(enemy_list[index].Height - 1, enemy_list[index].Width, enemy_list[index].Height, enemy_list[index].Width - 1, layer_number)) {
                        movement = Dungeon_Base.Is_Check_Move(enemy_list[index].Height + 1, enemy_list[index].Width + 1, layer_number);
                    }
                    break;

                case eDirection.Left:
                    width  = enemy_list[index].Width - 1;
                    height = enemy_list[index].Height;
                    movement = Dungeon_Base.Is_Check_Move(enemy_list[index].Height - 1, enemy_list[index].Width, layer_number);
                    break;

                case eDirection.Upleft:
                    width  = enemy_list[index].Width  - 1;
                    height = enemy_list[index].Height + 1;
                    if (!Dungeon_Base.Is_Check_Slant_Move(enemy_list[index].Height + 1, enemy_list[index].Width, enemy_list[index].Height, enemy_list[index].Width - 1, layer_number)) {
                        movement = Dungeon_Base.Is_Check_Move(enemy_list[index].Height + 1, enemy_list[index].Width + 1, layer_number);
                    }
                    break;
                    */
            }
        }
    }
}
