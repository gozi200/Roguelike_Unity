/*
    制作者 石倉

    最終更新日 2018/02/07
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの行動を設定する
/// </summary>
public class Enemy_Action : MonoBehaviour {
    eEnemy_Mode mode;
    eDirection direction;

    [SerializeField]
    Player player;

    [SerializeField]
    Enemy enemy;

    [SerializeField]
    static Dungeon_Generator dungeon_generator;

    [SerializeField]
    static Dungeon_Base dungeon_base;

    Damage_Calculation damage_calculation;

    /// <summary>
    /// エネミーの横座標
    /// </summary>
    int enemy_width;

    /// <summary>
    /// エネミーの縦座標
    /// </summary>
    int enemy_height;

    private void Start() {
        direction = eDirection.Down;

        enemy_width  = enemy.GetComponent<Object_Coordinates>().Width;
        enemy_height = enemy.GetComponent<Object_Coordinates>().Height;

    }

    /// <summary>
    /// Dungeon_Baseをセットする
    /// </summary>
    /// <param name="set_dungeon_base">情報を持ったDungeon_Base</param>
    public static void Set_Dungeon_Base(Dungeon_Base set_dungeon_base) {
        dungeon_base = set_dungeon_base;
    }

    /// <summary>
    /// Dungeon_Generatorをセットする
    /// </summary>
    /// <param name="set_dungeon_generator">情報を持ったDungeon_Base</param>
    public static void Set_Dungeon_Generator(Dungeon_Generator set_dungeon_generator) {
        dungeon_generator = set_dungeon_generator;
    }

    /// <summary>
    /// エネミーの行動処理
    /// </summary>
    /// <param name="player_status">プレイヤーのステータス。計算時に使用するために知っておく</param>
    public void Move_Enemy(Player_Status player_status) {
        for (int i = 0; i < 1; ++i) {
            //switch (/*enemy.GetComponent<Enemy>().enemys[i].AI_pattern*/) {
            //    case 0:
            if (Dungeon_Base.Is_Check_Move(enemy_height, enemy_width + 1, 2)) {
                if (Search_Player(player.GetComponent<Player>().transform.position.x, player.GetComponent<Player>().transform.position.y)) {
                    player.GetComponent<Player>().hit_point -= (int)damage_calculation.Damage(enemy.GetComponent<Enemy>().enemys[i].attack, Random.Range(87, 112 + 1), 0);
                    break;
                }
                //      }
                Move();
                break;
            }
        }

        // TODO: この処理どこでやる？
        //dungeon_manager.GetComponent<Dungeon_Generator>().Turn_Tick();
    }

    /// <summary>
    /// プレイヤーが隣接したマスにいるかどうか
    /// </summary>
    /// <returns>プレイヤーがいた場合はtrue</returns>
    bool Search_Player(float player_position_x, float player_position_y) {
        // 下
        if (gameObject.transform.position.x == player_position_x && gameObject.transform.position.y - 5 == player_position_y) {
            direction = eDirection.Down;
            return true;
        }
        // 左下
        else if (gameObject.transform.position.x - 5 == player_position_x && gameObject.transform.position.y - 5 == player_position_y) {
            direction = eDirection.Downleft;
            return true;
        }
        // 左
        else if (gameObject.transform.position.x - 5 == player_position_x && gameObject.transform.position.y == player_position_y) {
            direction = eDirection.Left;
            return true;
        }
        // 左上
        else if (gameObject.transform.position.x - 5 == player_position_x && gameObject.transform.position.y + 5 == player_position_y) {
            direction = eDirection.Upleft;
            return true;
        }
        // 上
        else if (gameObject.transform.position.x == player_position_x && gameObject.transform.position.y + 5 == player_position_y) {
            direction = eDirection.Up;
            return true;
        }
        // 右上
        else if (gameObject.transform.position.x + 5 == player_position_x && gameObject.transform.position.y + 5 == player_position_y) {
            direction = eDirection.Upright;
            return true;
        }
        // 右
        else if (gameObject.transform.position.x + 5 == player_position_x && gameObject.transform.position.y == player_position_y) {
            direction = eDirection.Right;
            return true;
        }
        // 右下
        else if (gameObject.transform.position.x + 5 == player_position_x && gameObject.transform.position.y - 5 == player_position_y) {
            direction = eDirection.Downright;
            return true;
        }
        return false;
    }

    /// <summary>
    /// エネミーの移動処理
    /// </summary>
    private void Move() {
        bool flag = false;
        int rand_x;
        int rand_y;

        while (true) {
            rand_x = Random.Range(0, 2 + 1);
            rand_y = Random.Range(0, 2 + 1);

            if (rand_x == 0 && rand_y == 0) {
                continue;
            }
            if (rand_x == 1) {
                rand_x = 5;
            }
            else if (rand_x == 2) {
                rand_x = -5;
            }
            if (rand_y == 1) {
                rand_y = 5;
            }
            else if (rand_y == 2) {
                rand_y = -5;
            }

            // 下方向から時計回りに処理
            if (rand_x == 0 && rand_y == -5) {
                direction = eDirection.Down;
                if (Dungeon_Base.Is_Check_Move(enemy_height - 1, enemy_width,     2)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == -5 && rand_y == -5) {
                direction = eDirection.Downleft;
                if (Dungeon_Base.Is_Check_Move(enemy_height - 1, enemy_width - 1, 2)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == -5 && rand_y == 0) {
                direction = eDirection.Left;
                if (Dungeon_Base.Is_Check_Move(enemy_height,     enemy_width - 1, 2)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == -5 && rand_y == 5) {
                direction = eDirection.Upleft;
                if (Dungeon_Base.Is_Check_Move(enemy_height + 1, enemy_width - 1, 2)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == 0 && rand_y == 5) {
                direction = eDirection.Up;
                if (Dungeon_Base.Is_Check_Move(enemy_height + 1, enemy_width,     2)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == 5 && rand_y == 5) {
                direction = eDirection.Upright;
                if (Dungeon_Base.Is_Check_Move(enemy_height + 1, enemy_width + 1, 2)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == 5 && rand_y == 0) {
                direction = eDirection.Right;
                if (Dungeon_Base.Is_Check_Move(enemy_height    , enemy_width + 1, 2)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == 5 && rand_y == -5) {
                direction = eDirection.Downright;
                if (Dungeon_Base.Is_Check_Move(enemy_height - 1, enemy_width + 1, 2)) {
                    flag = true; break;
                }
                continue;
            }
        }

        if (flag) {
            Vector3 vec = gameObject.transform.position;
            vec.x += rand_x;
            vec.y += rand_y;
            gameObject.transform.position = vec;
        }
    }
}