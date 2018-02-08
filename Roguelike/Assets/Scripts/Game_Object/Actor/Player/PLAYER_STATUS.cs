/*
    制作者 石倉

    最終更新日 2018/02/08
*/

using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Status : MonoBehaviour {
    [SerializeField]
    Player player;

    [SerializeField]
    Actor_Coordinates player_coodinates;

    Dungeon_Map map;

    /// <summary>
    /// レベルの最大値
    /// </summary>
    const int MAX_LV = 999;
    
    /// <summary>
    /// 経験値の最大値(仮)
    /// </summary>
    const int MAX_EXP = 999999999;

    /// <summary>
    /// プレイアブルキャラクターの数
    /// </summary>
    const int SERVANT_NUMBER = 10;

    /// <summary>
    /// レベルアップに必要な経験値量をは苦悩する配列
    /// </summary>
    public int[] exp_data_base = new int[999];

    void Start() {
    int[] exp_data_base = new int[] {
           5 // 1 から次のレベルに必要な経験値(仮)
	,     10 // 2 以下略
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
	,8000000 // 24 ひとまずはここまで
	,MAX_EXP // 25
        };
    }

    /// <summary>
    /// プレイヤーのステータスを設定する。 スポーン時とキャラクターチェンジの時に呼ばれる
    /// </summary>
    /// <param name="use_chara">使用するキャラクターの番号</param>
    public void Set_Parameter(int use_chara) {
        var player_status = csv_Reader.Load_csv("csv/Actor/Player/Player_csv", 3);
            player.ID                 = int.Parse(player_status[use_chara][0]);  // 番号
            player.name               = player_status          [use_chara][1];             // 名前
            player.class_type         = int.Parse(player_status[use_chara][2]);  // クラス
            player.saint_graph        = int.Parse(player_status[use_chara][3]);  // 再臨状態
            player.level              = int.Parse(player_status[use_chara][4]);  // レベル
            player.hit_point          = int.Parse(player_status[use_chara][5]);  // 体力
            player.max_hit_point      = int.Parse(player_status[use_chara][6]);  // 最大体力
            player.power              = int.Parse(player_status[use_chara][7]);  // ちから(攻撃力見加味するボーナス値)
            player.max_power          = int.Parse(player_status[use_chara][8]);  // 力の最大値
            player.activity           = int.Parse(player_status[use_chara][9]);  // 行動力
            player.attack             = int.Parse(player_status[use_chara][10]); // 攻撃力
            player.defence            = int.Parse(player_status[use_chara][11]); // 防御力
            player.hunger_point       = int.Parse(player_status[use_chara][12]); // はらへりポイント
            player.skill              = int.Parse(player_status[use_chara][13]); // スキル(種類)
            player.star_generate      = int.Parse(player_status[use_chara][14]); // スター発生率
            player.keep_star          = int.Parse(player_status[use_chara][15]); // スター保持数
            player.command_card       = int.Parse(player_status[use_chara][16]); // コマンドカード枚数
            player.arts_card          = int.Parse(player_status[use_chara][17]); // アーツカード枚数
            player.arts_hit_conut     = int.Parse(player_status[use_chara][18]); // アーツでの攻撃時のヒット回数
            player.quick_card         = int.Parse(player_status[use_chara][19]); // クイックのカード枚数
            player.quick_hit_attack   = int.Parse(player_status[use_chara][20]); // クイックでの攻撃時のヒット回数
            player.buster_card        = int.Parse(player_status[use_chara][21]); // バスターのカード枚数
            player.buster_hit_count   = int.Parse(player_status[use_chara][22]); // バスターでの攻撃時のヒット回数
            player.noble_weapon       = int.Parse(player_status[use_chara][24]); // 宝具(種類)
            player.extra_attack       = int.Parse(player_status[use_chara][23]); // エクストラアタック(種類)
            player.noble_phantasm     = int.Parse(player_status[use_chara][25]); // 宝具を撃つためのポイント(以下NP)
            player.max_noble_phantasm = int.Parse(player_status[use_chara][26]); // NPの最大値
            player.attack_rise_NP     = int.Parse(player_status[use_chara][27]); // 攻撃時のNPの上昇量
            player.defence_rise_NP    = int.Parse(player_status[use_chara][28]); // 被ダメージ時のNPの上昇量
            player.experience_point   = int.Parse(player_status[use_chara][29]); // 経験値
            player.turn_count         = int.Parse(player_status[use_chara][30]); // 経過ターンをカウント
    }

    public void Set_Coordinates(Actor_Coordinates set_coodinates, Dungeon_Map set_map) {
        player_coodinates = set_coodinates;
        map = set_map;
    }

    /// <summary>
    /// Playerのターン経過の処理(自動回復、はらへり) プレイヤーの行動が終了したときに呼ばれる
    /// </summary>
    public void Turn() {
        ++player.GetComponent<Player>().turn_count;

        // 空腹であるか
        if (player.GetComponent<Player>().hunger_point <= 0) {
            --player.GetComponent<Player>().hit_point;

            //if (Is_Dead()) {
            // ログに"餓死したと流す"
            //}
        }
        // 3ターンに一度空腹ポイントを１減らす
        else if (0 == player.GetComponent<Player>().turn_count % 3) {
            --player.GetComponent<Player>().hunger_point;
            // 満腹値20で"お腹がすいてきた"     のログを表示
            // 満腹度 0で"お腹がすいて死にそうだ"のログを表示
        }
        else {
            // TODO: 体力の自動回復
        }
    }

    /// <summary>
    /// 経験値を加算 敵を倒したときに呼ばれる プレイヤー、パートナーどちらが倒しても呼ばれる
    /// </summary>
    /// <param name="exp"></param>
    void Add_Experience_Point(int exp) {
        player.GetComponent<Player>().experience_point += exp;

        // 上限を越しても設定した最大値を超えないようにする
        if (player.GetComponent<Player>().experience_point > MAX_EXP) {
            player.GetComponent<Player>().experience_point = MAX_EXP;
        }

        // レベルアップに必要な経験値量を超えたか確かめる
        // TODO: - 1はいらない？ 要テスト
        if (exp_data_base[player.GetComponent<Player>().level - 1] <= player.GetComponent<Player>().experience_point) {
            int new_Lv = Get_Exp_Level();

            // 一度に２レベル以上あがる場合にも対応
            for (; player.GetComponent<Player>().level < new_Lv; ++player.GetComponent<Player>().level) {
                int add_hp;
                int add_atk;
                int add_def;

                // 体力の最大値をランダム(3~5)で増やす
                add_hp = Random.Range(0, 2) + 3;

                player.GetComponent<Player>().max_hit_point += add_hp;

                // TODO: 攻撃力,防御力の最大値を仕様書を参考に増やす
            }
        }
    }

    /// <summary>
    /// 経験値量からレベルを算出
    /// </summary>
    /// <returns>実際に上がった後のレベル</returns>
    int Get_Exp_Level() {
        int lv;

        for (lv = 0; lv < MAX_LV; ++lv) {
            if (exp_data_base[lv] > player.GetComponent<Player>().experience_point) {
                break;
            }
        }
        return lv + 1;
    }

    /// <summary>
    /// 死亡したかを判定する 毎アクターのターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns></returns>
    public bool Is_Dead(int now_HP) {
        if (now_HP <= 0) {
            now_HP = 0;
            return true;
        }
        return false;
    }
}