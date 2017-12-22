using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Data : MonoBehaviour {
    private int ID;                 // 番号
    private new string name;        // 名前
    private int class_type;         // クラス
    public  int saint_graph;        // 再臨状態
    public  int level;              // レベル
    public  int hit_point;          // 体力
    public  int max_hitpoint;       // 最大体力
    public  int power;              // ちから(攻撃力見加味するボーナス値)
    public  int max_power;          // 力の最大値
    public  int activity;           // 行動力
    public  int attack;             // 攻撃力
    public  int defence;            // 防御力
    public  int hunger_point;       // はらへりポイント
    private int skill;              // スキル(種類)
    public  int star_generate;      // スター発生率
    private int keep_star;          // スター保持数
    public  int command_card;       // コマンドカード枚数
    public  int arts_card;          // アーツカード枚数
    private int arts_hit_conut;     // アーツでの攻撃時のヒット回数
    public  int quick_card;         // クイックのカード枚数
    private int quick_hit_attack;   // クイックでの攻撃時のヒット回数
    public  int buster_card;        // バスターのカード枚数
    private int buster_hit_count;   // バスターでの攻撃時のヒット回数
    private int extra_attack;       // エクストラアタック(種類)
    private int noble_weapon;       // 宝具(種類)
    public  int noble_phantasm;     // 宝具を撃つためのポイント(以下NP)
    public  int max_noble_phantasm; // NPの最大値
    public  int attack_rise_NP;     // 攻撃時のNPの上昇量
    public  int defence_rise_NP;    // 被ダメージ時のNPの上昇量
    public  int experience_point;   // 経験値
    public  int turn_count;         // 経過ターンをカウント

    void Start() {
        var player_data = csv_Reader.Load_csv("csv/Actor/Player/Player_csv", 3);

        ID                 = int.Parse(player_data[0][0]);  // 番号
        name               = player_data[0][1];             // 名前
        class_type         = int.Parse(player_data[0][2]);  // クラス
        saint_graph        = int.Parse(player_data[0][3]);  // 再臨状態
        level              = int.Parse(player_data[0][4]);  // レベル
        hit_point          = int.Parse(player_data[0][5]);  // 体力
        max_hitpoint       = int.Parse(player_data[0][6]);  // 最大体力
        power              = int.Parse(player_data[0][7]);  // ちから(攻撃力見加味するボーナス値)
        max_power          = int.Parse(player_data[0][8]);  // 力の最大値
        activity           = int.Parse(player_data[0][9]);  // 行動力
        attack             = int.Parse(player_data[0][10]); // 攻撃力
        defence            = int.Parse(player_data[0][11]); // 防御力
        hunger_point       = int.Parse(player_data[0][12]); // はらへりポイント
        skill              = int.Parse(player_data[0][13]); // スキル(種類)
        star_generate      = int.Parse(player_data[0][14]); // スター発生率
        keep_star          = int.Parse(player_data[0][15]); // スター保持数
        command_card       = int.Parse(player_data[0][16]); // コマンドカード枚数
        arts_card          = int.Parse(player_data[0][17]); // アーツカード枚数
        arts_hit_conut     = int.Parse(player_data[0][18]); // アーツでの攻撃時のヒット回数
        quick_card         = int.Parse(player_data[0][19]); // クイックのカード枚数
        quick_hit_attack   = int.Parse(player_data[0][20]); // クイックでの攻撃時のヒット回数
        buster_card        = int.Parse(player_data[0][21]); // バスターのカード枚数
        buster_hit_count   = int.Parse(player_data[0][22]); // バスターでの攻撃時のヒット回数
        extra_attack       = int.Parse(player_data[0][23]); // エクストラアタック(種類)
        noble_weapon       = int.Parse(player_data[0][24]); // 宝具(種類)
        noble_phantasm     = int.Parse(player_data[0][25]); // 宝具を撃つためのポイント(以下NP)
        max_noble_phantasm = int.Parse(player_data[0][26]); // NPの最大値
        attack_rise_NP     = int.Parse(player_data[0][27]); // 攻撃時のNPの上昇量
        defence_rise_NP    = int.Parse(player_data[0][28]); // 被ダメージ時のNPの上昇量
        experience_point   = int.Parse(player_data[0][29]); // 経験値
        turn_count         = int.Parse(player_data[0][30]); // 経過ターンをカウント
    }

    // Update is called once per frame
    void Update() {

    }
}