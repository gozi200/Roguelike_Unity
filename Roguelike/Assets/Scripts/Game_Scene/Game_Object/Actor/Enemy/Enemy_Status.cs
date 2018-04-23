using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// エネミーのステータスを設定する
/// </summary>
public class Enemy_Status : MonoBehaviour {
    /// <summary>
    /// エネミークラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// csvを読み込むクラス
    /// </summary>
    csv_Reader reader;

    void Start() {
        enemy = Enemy_Manager.Instance.enemy_script;
        reader = Game.Instance.reader;
    }

    /// <summary>
    /// ステータスを設定し、リストに格納する
    /// </summary>
    public void Set_Parameter() {
        var enemy_status = reader.Load_csv("csv/Actor/Enemy/Enemy_csv", 3);

        for(int enemy_type = 0; enemy_type < Define_Value.ENEMY_NUMBER; ++enemy_type) {
            enemy.ID               = int.Parse(enemy_status[enemy_type][0]);  // 番号
            enemy.name             = enemy_status          [enemy_type][1] ;  // 名前
            enemy.class_type       = int.Parse(enemy_status[enemy_type][2]);  // クラス
            enemy.level            = int.Parse(enemy_status[enemy_type][3]);  // レベル
            enemy.hit_point        = int.Parse(enemy_status[enemy_type][4]);  // 体力
            enemy.max_hitpoint     = int.Parse(enemy_status[enemy_type][5]);  // 最大体力
            enemy.attack           = int.Parse(enemy_status[enemy_type][6]);  // 攻撃力
            enemy.defence          = int.Parse(enemy_status[enemy_type][7]);  // 防御力
            enemy.activity         = int.Parse(enemy_status[enemy_type][8]);  // 行動力
            enemy.critical         = int.Parse(enemy_status[enemy_type][9]);  // クリティカル
            enemy.experience_point = int.Parse(enemy_status[enemy_type][10]); // 経験値
            enemy.skill            = int.Parse(enemy_status[enemy_type][11]); // スキル(種類)
            enemy.AI_pattern       = int.Parse(enemy_status[enemy_type][12]); // AI
            enemy.first_floor      = int.Parse(enemy_status[enemy_type][13]); // 出開始階層
            enemy.last_floor       = int.Parse(enemy_status[enemy_type][14]); // 出現終了階層
            enemy.turn_count       = int.Parse(enemy_status[enemy_type][15]); // 経過ターンをカウント

            enemy.enemy_type.Add(enemy);
        }
    }
}