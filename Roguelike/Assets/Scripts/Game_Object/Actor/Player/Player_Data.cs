using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Data : MonoBehaviour {
    Player player;
    const int SERVANT_NUMBER = 10; // 登場サーヴァントの人数

    //[SerializeField]
    //private int ID;                 // 番号
    //[SerializeField]
    //private new string name;        // 名前
    //[SerializeField]
    //private int class_type;         // クラス
    //public  int saint_graph;        // 再臨状態
    //public  int level;              // レベル
    //public  int hit_point;          // 体力
    //public  int max_hitpoint;       // 最大体力
    //public  int power;              // ちから(攻撃力見加味するボーナス値)
    //public  int max_power;          // 力の最大値
    //public  int activity;           // 行動力
    //public  int attack;             // 攻撃力
    //public  int defence;            // 防御力
    //public  int hunger_point;       // はらへりポイント
    //[SerializeField]
    //private int skill;              // スキル(種類)
    //public  int star_generate;      // スター発生率
    //private int keep_star;          // スター保持数
    //public  int command_card;       // コマンドカード枚数
    //public  int arts_card;          // アーツカード枚数
    //[SerializeField]
    //private int arts_hit_conut;     // アーツでの攻撃時のヒット回数
    //public  int quick_card;         // クイックのカード枚数
    //private int quick_hit_attack;   // クイックでの攻撃時のヒット回数
    //public  int buster_card;        // バスターのカード枚数
    //[SerializeField]
    //private int buster_hit_count;   // バスターでの攻撃時のヒット回数
    //[SerializeField]
    //private int extra_attack;       // エクストラアタック(種類)
    //[SerializeField]
    //private int noble_weapon;       // 宝具(種類)
    //public  int noble_phantasm;     // 宝具を撃つためのポイント(以下NP)
    //public  int max_noble_phantasm; // NPの最大値
    //public  int attack_rise_NP;     // 攻撃時のNPの上昇量
    //public  int defence_rise_NP;    // 被ダメージ時のNPの上昇量
    //public  int experience_point;   // 経験値
    //public  int turn_count;         // 経過ターンをカウント

    void Start() {
        var player_data = csv_Reader.Load_csv("csv/Actor/Player/Player_csv", 3);
        PLAYER_DATA_BASE player_data_base;

        for(int i = 0; i < 2; ++i) { // TODO: マジックナンバー 10騎分のデータができ次第SERVANT_NUMBERを使う
        player_data_base.ID                 = int.Parse(player_data[i][0]);  // 番号
        player_data_base.name               = player_data[i][1];            // 名前
        player_data_base.class_type         = int.Parse(player_data[i][2]);  // クラス
        player_data_base.saint_graph        = int.Parse(player_data[i][3]);  // 再臨状態
        player_data_base.level              = int.Parse(player_data[i][4]);  // レベル
        player_data_base.hit_point          = int.Parse(player_data[i][5]);  // 体力
        player_data_base.max_hitpoint       = int.Parse(player_data[i][6]);  // 最大体力
        player_data_base.power              = int.Parse(player_data[i][7]);  // ちから(攻撃力見加味するボーナス値)
        player_data_base.max_power          = int.Parse(player_data[i][8]);  // 力の最大値
        player_data_base.activity           = int.Parse(player_data[i][9]);  // 行動力
        player_data_base.attack             = int.Parse(player_data[i][10]); // 攻撃力
        player_data_base.defence            = int.Parse(player_data[i][11]); // 防御力
        player_data_base.hunger_point       = int.Parse(player_data[i][12]); // はらへりポイント
        player_data_base.skill              = int.Parse(player_data[i][13]); // スキル(種類)
        player_data_base.star_generate      = int.Parse(player_data[i][14]); // スター発生率
        player_data_base.keep_star          = int.Parse(player_data[i][15]); // スター保持数
        player_data_base.command_card       = int.Parse(player_data[i][16]); // コマンドカード枚数
        player_data_base.arts_card          = int.Parse(player_data[i][17]); // アーツカード枚数
        player_data_base.arts_hit_conut     = int.Parse(player_data[i][18]); // アーツでの攻撃時のヒット回数
        player_data_base.quick_card         = int.Parse(player_data[i][19]); // クイックのカード枚数
        player_data_base.quick_hit_attack   = int.Parse(player_data[i][20]); // クイックでの攻撃時のヒット回数
        player_data_base.buster_card        = int.Parse(player_data[i][21]); // バスターのカード枚数
        player_data_base.buster_hit_count   = int.Parse(player_data[i][22]); // バスターでの攻撃時のヒット回数
        player_data_base.extra_attack       = int.Parse(player_data[i][23]); // エクストラアタック(種類)
        player_data_base.noble_weapon       = int.Parse(player_data[i][24]); // 宝具(種類)
        player_data_base.noble_phantasm     = int.Parse(player_data[i][25]); // 宝具を撃つためのポイント(以下NP)
        player_data_base.max_noble_phantasm = int.Parse(player_data[i][26]); // NPの最大値
        player_data_base.attack_rise_NP     = int.Parse(player_data[i][27]); // 攻撃時のNPの上昇量
        player_data_base.defence_rise_NP    = int.Parse(player_data[i][28]); // 被ダメージ時のNPの上昇量
        player_data_base.experience_point   = int.Parse(player_data[i][29]); // 経験値
        player_data_base.turn_count         = int.Parse(player_data[i][30]); // 経過ターンをカウント

            player.players.Add(player_data_base);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}