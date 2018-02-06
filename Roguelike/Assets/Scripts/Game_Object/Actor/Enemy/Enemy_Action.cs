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
    Dungeon_Generator dungeon_generator;

    [SerializeField]
    Dungeon_Base dungeon_base;

    Damage_Calculation damage_calculation;

    private void Start() {
        //enemy  = Enemy_Manager.Get_Enemy();
        //player = Player_Manager.Get_Player();

        //player_status = player.GetComponent<Player_Status>();
        direction = eDirection.Down;
    }

    public void Set_Dungeon_Generator(Dungeon_Generator set_dungeon_generator) {
        dungeon_generator = set_dungeon_generator;
       // player = Player_Manager.Get_Player();
    }

    /// <summary>
    /// エネミーの行動処理
    /// </summary>
    /// <param name="player_status"></param>
    public void Move_Enemy(Player_Status player_status) {
        for (int i = 0; i < 1; ++i) {
            switch (enemy.GetComponent<Enemy>().enemys[i].AI_pattern) {
                case 2:
                    if (!dungeon_base.Is_Diagonal_Attack(gameObject.transform.position.x, gameObject.transform.position.y, direction)) {
                        if (Search_Player(player.GetComponent<Player>().transform.position.x, player.GetComponent<Player>().transform.position.y)) {
                            player.GetComponent<Player>().hit_point -= (int)damage_calculation.Damage(enemy.GetComponent<Enemy>().enemys[i].attack,Random.Range(87,112 + 1),0);
                            Debug.Log(player.GetComponent<Player>().hit_point);
                            break;
                        }
                    }
                    Move();
                    break;
            }
        }
        dungeon_generator.GetComponent<Dungeon_Generator>().Turn_Tick();
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
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x, gameObject.transform.position.y - 5,
                                            direction)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == -5 && rand_y == -5) {
                direction = eDirection.Downleft;
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x - 5, gameObject.transform.position.y - 5,
                                            direction)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == -5 && rand_y == 0) {
                direction = eDirection.Left;
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x - 5, gameObject.transform.position.y,
                                            direction)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == -5 && rand_y == 5) {
                direction = eDirection.Upleft;
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x - 5, gameObject.transform.position.y + 5,
                                            direction)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == 0 && rand_y == 5) {
                direction = eDirection.Up;
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x, gameObject.transform.position.y + 5,
                                            direction)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == 5 && rand_y == 5) {
                direction = eDirection.Upright;
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y + 5,
                                            direction)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == 5 && rand_y == 0) {
                direction = eDirection.Right;
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y,
                                            direction)) {
                    flag = true; break;
                }
                continue;
            }
            else if (rand_x == 5 && rand_y == -5) {
                direction = eDirection.Downright;
                if (dungeon_base.Check_Move(gameObject.transform.position.x, gameObject.transform.position.y,
                                            gameObject.transform.position.x + 5, gameObject.transform.position.y - 5,
                                            direction)) {
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