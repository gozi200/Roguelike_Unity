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
    /// エネミーのステータスを保持してるクラス
    /// </summary>
    Enemy_Data enemy_data = new Enemy_Data();

    void Start() {
        enemy = Enemy.Instance.enemy;
    }

    /// <summary>
    /// ステータスを設定し、リストに格納する
    /// </summary>
    public void Set_Parameter() {
        var enemy_status = csv_Reader.Load_csv("csv/Actor/Enemy/Enemy_csv", 3);

        for(int enemy_type = 0; enemy_type < 2; ++enemy_type) { // TODO: マジックナンバー ENEMY_NUMBERが決まり次第置き換え
            enemy_data.ID               = int.Parse(enemy_status[enemy_type][0]);  // 番号
            enemy_data.name             = enemy_status          [enemy_type][1] ;  // 名前
            enemy_data.class_type       = int.Parse(enemy_status[enemy_type][2]);  // クラス
            enemy_data.level            = int.Parse(enemy_status[enemy_type][3]);  // レベル
            enemy_data.hit_point        = int.Parse(enemy_status[enemy_type][4]);  // 体力
            enemy_data.max_hitpoint     = int.Parse(enemy_status[enemy_type][5]);  // 最大体力
            enemy_data.attack           = int.Parse(enemy_status[enemy_type][6]);  // 攻撃力
            enemy_data.defence          = int.Parse(enemy_status[enemy_type][7]);  // 防御力
            enemy_data.activity         = int.Parse(enemy_status[enemy_type][8]);  // 行動力
            enemy_data.critical         = int.Parse(enemy_status[enemy_type][9]);  // クリティカル
            enemy_data.experience_point = int.Parse(enemy_status[enemy_type][10]); // 経験値
            enemy_data.skill            = int.Parse(enemy_status[enemy_type][11]); // スキル(種類)
            enemy_data.AI_pattern       = int.Parse(enemy_status[enemy_type][12]); // AI
            enemy_data.first_floor      = int.Parse(enemy_status[enemy_type][13]); // 出開始階層
            enemy_data.last_floor       = int.Parse(enemy_status[enemy_type][14]); // 出現終了階層
            enemy_data.turn_count       = int.Parse(enemy_status[enemy_type][15]); // 経過ターンをカウント

            enemy.enemys.Add(enemy_data);
        }
    }
}