using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの行動を設定する
/// </summary>
public class Enemy_Action : MonoBehaviour {
    /// <summary>
    /// エネミーのいる横座標の位置
    /// </summary>
    int enemy_width;

    /// <summary>
    /// エネミーのいる縦座標の位置
    /// </summary>
    int enemy_height;

    /// <summary>
    /// プレイヤーの体力
    /// </summary>
    int player_hit_point;

    eEnemy_Mode mode;
    eDirection direction;

    [SerializeField]
    Enemy enemy;

    [SerializeField]
    Dungeon_Manager dungeon_manager;

    static Dungeon_Generator dungeon_generator;

    static Dungeon_Base dungeon_base;

    Damage_Calculation damage_calculation;

    static List<GameObject> enemy_list;

    GM.Player player;

    void Awake() {
        player = GM.Instance.player;
    }

    void Start() {
        

        direction = eDirection.Down;

        enemy_width  = enemy.GetComponent<Object_Coordinates>().Width;
        enemy_height = enemy.GetComponent<Object_Coordinates>().Height;
    }

    public static void Set_Dungeon_Base(Dungeon_Base set_dungeon_base) {
        dungeon_base = set_dungeon_base;
    }

    public static void Set_Dungeon_Generator(Dungeon_Generator set_dungeon_generator) {
        dungeon_generator = set_dungeon_generator;
    }

    //public static void Set_Enemy_List(List<GameObject> set_coordinates) {
    //    enemy_list = set_coordinates;
    //}

    /// <summary>
    /// エネミーの行動処理
    /// </summary>
    public void Move_Enemy() {
        for (int i = 0; i < 1; ++i) {
        Debug.Log(enemy);
            // TODO: Startで書いておきたいけど配列はどうやって？
            switch (enemy.GetComponent<Enemy>().enemys[i].AI_pattern) {
                case 2:
                    if (Dungeon_Base.Is_Check_Move(enemy_height, enemy_width + 1, 2)) {
                       // if (Search_Player(player.transform.position.x, player.transform.position.y)) {
                       //     player_hit_point -= (int)damage_calculation.Damage(enemy.GetComponent<Enemy>().enemys[i].attack, Random.Range(87, 112 + 1), 0);
                       //     Debug.Log(player_hit_point);
                       //     break;
                       // }
                    }
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
    private void Move() {
        for (int index = 0; index < enemy_list.Count; ++index) {
            /// <summary>
            /// 移動後の横座標
            /// </summary>
            int width = 0;

            /// <summary>
            /// 移動後の縦座標
            /// </summary>
            int height = 0;

            /// <summary>
            /// 移動方向を乱数で決める
            /// </summary>
            int random_direction = Random.Range(0, (int)eDirection.Finish);

            /// <summary>
            /// 上述のint型乱数をenum型にキャスト
            /// </summary>
            eDirection cast_random_direction = (eDirection)random_direction;

            /// <summary>
            /// 移動が完了したかどうかのフラグ 完了であればtrue
            /// </summary>
            bool move_flag = false;

            /// <summary>
            /// 移動が可能かどうかのフラグ 可能であればtrue
            /// </summary>
            bool movement = false;

            int layer_number = 2; // テスト中

            switch (cast_random_direction) {
                case eDirection.Up:
                    // TODO: 問題点有り 詳しくは Dungeon_Base の IS_Check_Move にて
                    width    = (int)enemy_list[index].transform.position.x;
                    height   = (int)enemy_list[index].transform.position.y + 1;
                    movement = Dungeon_Base.Is_Check_Move(enemy_list[index].transform.position.y + 1, enemy_list[index].transform.position.x, layer_number);
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

            if (movement) {
                Enemy_Move(index, height, width);
                if(index >= enemy_list.Count) {
                    move_flag = true;
                }
            }
        }
    }

    void Enemy_Move(int index, int height, int width) {
        Vector3 position = Dungeon_Map.Get_Position(height, width);
        --position.z;
        enemy_list[index].transform.position = position;
        enemy_list[index].GetComponent<Object_Coordinates>().Set_Init_Number(height, width);
    }
}