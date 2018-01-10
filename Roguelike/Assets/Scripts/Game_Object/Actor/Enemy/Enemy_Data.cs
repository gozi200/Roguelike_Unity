using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// エネミーのステータスを設定する
/// </summary>
public class Enemy_Data : MonoBehaviour {
    Enemy enemy = new Enemy();

    const int ENEMY_NUMBER = 1; // TODO: 敵の種類数が決まり次第与える

    public void Set_Parameter() {
        var enemy_data = csv_Reader.Load_csv("csv/Actor/Enemy/Enemy_csv", 3);

        Enemy_Status enemy_status;

        for(int i = 0; i < 1; ++i) { // TODO: マジックナンバー ENEMY_NUMBERが決まり次第置き換え
        enemy_status.ID               = int.Parse(enemy_data[0][0]);  // 番号
        enemy_status.name             = enemy_data[0][1];             // 名前
        enemy_status.class_type       = int.Parse(enemy_data[0][2]);  // クラス
        enemy_status.level            = int.Parse(enemy_data[0][3]);  // レベル
        enemy_status.hit_point        = int.Parse(enemy_data[0][4]);  // 体力
        enemy_status.max_hitpoint     = int.Parse(enemy_data[0][5]);  // 最大体力
        enemy_status.attack           = int.Parse(enemy_data[0][6]);  // 攻撃力
        enemy_status.defence          = int.Parse(enemy_data[0][7]);  // 防御力
        enemy_status.activity         = int.Parse(enemy_data[0][8]);  // 行動力
        enemy_status.critical         = int.Parse(enemy_data[0][9]); // クリティカル
        enemy_status.experience_point = int.Parse(enemy_data[0][10]); // 経験値
        enemy_status.skill            = int.Parse(enemy_data[0][11]); // スキル(種類)
        enemy_status.AI_paturn        = int.Parse(enemy_data[0][12]); // AI
        enemy_status.first_floor      = int.Parse(enemy_data[0][13]); // 出開始階層
        enemy_status.last_floor       = int.Parse(enemy_data[0][14]); // 出現終了階層
        enemy_status.turn_count       = int.Parse(enemy_data[0][15]); // 経過ターンをカウント
    }

}
    // Update is called once per frame
    void Update() {

    }
}