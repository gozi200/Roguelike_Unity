using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// エネミーのステータスを設定する
/// </summary>
public class Enemy_Data : MonoBehaviour {
    private int ID;              // 番号
    private new string name;     // 名前
    private int class_type;      // クラス
    public int level;            // レベル
    public int hit_point;        // 体力
    public int max_hitpoint;     // 最大体力
    public int attack;           // 攻撃力
    public int defence;          // 防御力
    public int activity;         // 行動力
    public int critical;         // クリティカル
    public int experience_point; // 経験値(倒されたときに与える量)
    private int skill;           // スキル(種類)
    public int AI_paturn;        // AI
    public int first_floor;      // 出現開始階層
    public int last_floor;       // 出現終了階層
    public int turn_count;       // 経過ターンをカウント

    void Start() {
        var enemy_data = csv_Reader.Load_csv("csv/Actor/Enemy/Enemy_csv", 3);

        ID               = int.Parse(enemy_data[0][0]);  // 番号
        name             = enemy_data[0][1];             // 名前
        class_type       = int.Parse(enemy_data[0][2]);  // クラス
        level            = int.Parse(enemy_data[0][3]);  // レベル
        hit_point        = int.Parse(enemy_data[0][4]);  // 体力
        max_hitpoint     = int.Parse(enemy_data[0][5]);  // 最大体力
        attack           = int.Parse(enemy_data[0][6]);  // 攻撃力
        defence          = int.Parse(enemy_data[0][7]);  // 防御力
        activity         = int.Parse(enemy_data[0][8]);  // 行動力
        critical         = int.Parse(enemy_data[0][9]); // クリティカル
        experience_point = int.Parse(enemy_data[0][10]); // 経験値
        skill            = int.Parse(enemy_data[0][11]); // スキル(種類)
        AI_paturn        = int.Parse(enemy_data[0][12]); // AI
        first_floor      = int.Parse(enemy_data[0][13]); // 出開始階層
        last_floor       = int.Parse(enemy_data[0][14]); // 出現終了階層
        turn_count       = int.Parse(enemy_data[0][15]); // 経過ターンをカウント
    }

    // Update is called once per frame
    void Update() {

    }
}