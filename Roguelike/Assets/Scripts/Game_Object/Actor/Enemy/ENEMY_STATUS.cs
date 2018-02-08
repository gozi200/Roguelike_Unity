/*
    制作者 石倉

    最終更新日 2018/02/07
*/

using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// エネミーのステータスを設定する
/// </summary>
public class Enemy_Status : MonoBehaviour {
    [SerializeField]
    Enemy enemy;
    
    Enemy_Data enemy_data = new Enemy_Data();

    /// <summary>
    /// 登場エネミーの種類
    /// </summary>
    const int ENEMY_NUMBER = 2;

    /// <summary>
    /// ステータスを設定し、リストに格納する
    /// </summary>
    public void Set_Parameter() {
        var enemy_status = csv_Reader.Load_csv("csv/Actor/Enemy/Enemy_csv", 3);

        for(int i = 0; i < 2; ++i) { // TODO: マジックナンバー ENEMY_NUMBERが決まり次第置き換え
        enemy_data.ID               = int.Parse(enemy_status[i][0]);  // 番号
        enemy_data.name             = enemy_status[i][1];             // 名前
        enemy_data.class_type       = int.Parse(enemy_status[i][2]);  // クラス
        enemy_data.level            = int.Parse(enemy_status[i][3]);  // レベル
        enemy_data.hit_point        = int.Parse(enemy_status[i][4]);  // 体力
        enemy_data.max_hitpoint     = int.Parse(enemy_status[i][5]);  // 最大体力
        enemy_data.attack           = int.Parse(enemy_status[i][6]);  // 攻撃力
        enemy_data.defence          = int.Parse(enemy_status[i][7]);  // 防御力
        enemy_data.activity         = int.Parse(enemy_status[i][8]);  // 行動力
        enemy_data.critical         = int.Parse(enemy_status[i][9]);  // クリティカル
        enemy_data.experience_point = int.Parse(enemy_status[i][10]); // 経験値
        enemy_data.skill            = int.Parse(enemy_status[i][11]); // スキル(種類)
        enemy_data.AI_pattern       = int.Parse(enemy_status[i][12]); // AI
        enemy_data.first_floor      = int.Parse(enemy_status[i][13]); // 出開始階層
        enemy_data.last_floor       = int.Parse(enemy_status[i][14]); // 出現終了階層
        enemy_data.turn_count       = int.Parse(enemy_status[i][15]); // 経過ターンをカウント

            enemy.GetComponent<Enemy>().enemys.Add(enemy_data);
        }
    }
}