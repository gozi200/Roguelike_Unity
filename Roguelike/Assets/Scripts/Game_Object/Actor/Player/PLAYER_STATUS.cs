using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Status : MonoBehaviour {
    [SerializeField]
    Player player;

    Actor_Status actor_status;

    Player_Data player_data = new Player_Data();

    const int MAX_LV = 999;        // レベルの最大値
    const int MAX_EXP = 999999999; // 経験値の最大値
    const int SERVANT_NUMBER = 10; // 登場サーヴァントの騎数

    public int[] exp_data_base = new int[999]; // レベルアップに必要な経験値量

    void Start() {
        actor_status = new Actor_Status();

        int[] exp_data_base = new int[] {
           5 // 1 から次のレベルに必要な経験値
	,     10 // 2
	,     20 // 3
	,     40 // 4
	,     80 // 5
	,    160 // 6
	,    320 // 7
	,    640 // 8
	,   1300 // 9
	,   2600 // 10
	,   5200 // 11
	,  10000 // 12
	,  20000 // 13
	,  39000 // 14
	,  68000 // 15
	, 100000 // 16
	, 200000 // 17
	, 400000 // 18
	, 800000 // 19
	,1600000 // 20
	,2500000 // 21
	,4600000 // 22
	,6700000 // 23
	,8000000 // 24
	,MAX_EXP // 25(最終レベルはカンストのMAX_EXPにすること)
        };
    }

    public void Set_Parameter() {
        var player_status = csv_Reader.Load_csv("csv/Actor/Player/Player_csv", 3);

        for (int i = 0; i < 2; ++i) { // TODO: マジックナンバー 10騎分のデータができ次第SERVANT_NUMBERを使う
            player_data.ID                 = int.Parse(player_status[i][0]);  // 番号
            player_data.name               = player_status[i][1];             // 名前
            player_data.class_type         = int.Parse(player_status[i][2]);  // クラス
            player_data.saint_graph        = int.Parse(player_status[i][3]);  // 再臨状態
            player_data.level              = int.Parse(player_status[i][4]);  // レベル
            player_data.hit_point          = int.Parse(player_status[i][5]);  // 体力
            player_data.max_hit_point      = int.Parse(player_status[i][6]);  // 最大体力
            player_data.power              = int.Parse(player_status[i][7]);  // ちから(攻撃力見加味するボーナス値)
            player_data.max_power          = int.Parse(player_status[i][8]);  // 力の最大値
            player_data.activity           = int.Parse(player_status[i][9]);  // 行動力
            player_data.attack             = int.Parse(player_status[i][10]); // 攻撃力
            player_data.defence            = int.Parse(player_status[i][11]); // 防御力
            player_data.hunger_point       = int.Parse(player_status[i][12]); // はらへりポイント
            player_data.skill              = int.Parse(player_status[i][13]); // スキル(種類)
            player_data.star_generate      = int.Parse(player_status[i][14]); // スター発生率
            player_data.keep_star          = int.Parse(player_status[i][15]); // スター保持数
            player_data.command_card       = int.Parse(player_status[i][16]); // コマンドカード枚数
            player_data.arts_card          = int.Parse(player_status[i][17]); // アーツカード枚数
            player_data.arts_hit_conut     = int.Parse(player_status[i][18]); // アーツでの攻撃時のヒット回数
            player_data.quick_card         = int.Parse(player_status[i][19]); // クイックのカード枚数
            player_data.quick_hit_attack   = int.Parse(player_status[i][20]); // クイックでの攻撃時のヒット回数
            player_data.buster_card        = int.Parse(player_status[i][21]); // バスターのカード枚数
            player_data.buster_hit_count   = int.Parse(player_status[i][22]); // バスターでの攻撃時のヒット回数
            player_data.noble_weapon       = int.Parse(player_status[i][24]); // 宝具(種類)
            player_data.extra_attack       = int.Parse(player_status[i][23]); // エクストラアタック(種類)
            player_data.noble_phantasm     = int.Parse(player_status[i][25]); // 宝具を撃つためのポイント(以下NP)
            player_data.max_noble_phantasm = int.Parse(player_status[i][26]); // NPの最大値
            player_data.attack_rise_NP     = int.Parse(player_status[i][27]); // 攻撃時のNPの上昇量
            player_data.defence_rise_NP    = int.Parse(player_status[i][28]); // 被ダメージ時のNPの上昇量
            player_data.experience_point   = int.Parse(player_status[i][29]); // 経験値
            player_data.turn_count         = int.Parse(player_status[i][30]); // 経過ターンをカウント

            player.GetComponent<Player>().players.Add(player_data);
        }

    }

    /// <summary>
    /// Playerのターン経過の処理(自動回復、はらへり)
    /// </summary>
    public void Turn() {
        ++player.GetComponent<Player>().players[0].turn_count;
        // 空腹であるか
        if (player.GetComponent<Player>().players[0].hunger_point <= 0) {
            Decrease_Hit_Point(-1);

            //if (Is_Dead()) {
            // ログに"餓死したと流す"
            //}
        }
        else if (0 == player.GetComponent<Player>().players[0].turn_count % 3) {
            Decrease_Hunger_Point(-1);
        }
        else {
            // TODO: 体力の自動回復
        }
    }

    void Decrease_Hunger_Point(int value) {
        int old =  player.GetComponent<Player>().players[0].hunger_point;

        player.GetComponent<Player>().players[0].hunger_point += value;

        // 満腹値20で"お腹がすいてきた"     のログを表示
        // 満腹度 0で"お腹がすいて死にそうだ"のログを表示
    }

    /// <summary>
    /// 体力の減少
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    int Decrease_Hit_Point(int value) {
        int old = player.GetComponent<Player>().players[0].hit_point;
    
        player.GetComponent<Player>().players[0].hit_point += value;
    
        if (actor_status.Get_Max_HP(player.GetComponent<Player>().players[0].max_hit_point) < player.GetComponent<Player>().players[0].hit_point) {
            player.GetComponent<Player>().players[0].hit_point = actor_status.Get_Max_HP(player.GetComponent<Player>().players[0].max_hit_point);
        }
        else if (player.GetComponent<Player>().players[0].hit_point < 0) {
            player.GetComponent<Player>().players[0].hit_point = 0;
        }
    
        return player.GetComponent<Player>().players[0].hit_point - old;
    }

    /// <summary>
    /// 経験値を加算
    /// </summary>
    /// <param name="exp"></param>
    void Add_Experience_Point(int exp) {
        player.GetComponent<Player>().players[0].experience_point += exp;

        if (player.GetComponent<Player>().players[0].experience_point > MAX_EXP) {
            player.GetComponent<Player>().players[0].experience_point = MAX_EXP;
        }

        if (exp_data_base[player.GetComponent<Player>().players[0].level - 1] <= player.GetComponent<Player>().players[0].experience_point) {
            int new_Lv = Get_Exp_Level();

            for (; player.GetComponent<Player>().players[0].level < new_Lv; ++player.GetComponent<Player>().players[0].level) {
                int add_hp;
                int add_atk;
                int add_def;

                // 体力の最大値をランダム(3~5)で増やす
                add_hp = Random.Range(0, 2) + 3;

                player.GetComponent<Player>().players[0].max_hit_point += add_hp;

                // TODO: 攻撃力の最大値を仕様書を参考に増やす

                // TODO: 防御力の最大値を仕様書を参考に増やす
            }
        }
    }

    /// <summary>
    /// 経験値量からレベルを算出
    /// </summary>
    /// <returns></returns>
    int Get_Exp_Level() {
        int lv;

        for (lv = 0; lv < MAX_LV; ++lv) {
            if (exp_data_base[lv] > player.GetComponent<Player>().players[0].experience_point) {
                break;
            }
        }
        return lv + 1;
    }
}