using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Status {
    /// <summary>
    /// プレイヤースクリプト
    /// </summary>
    Player player_script;
    /// <summary>
    /// csv読み込みクラス
    /// </summary>
    csv_Reader reader;

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
    /// レベルアップに必要な経験値量を格納する配列
    /// </summary>
    public int[] exp_data_base = new int[25] {
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

    /// <summary>
    /// プレイヤーのステータスを設定する。 ゲーム開始時とキャラクターチェンジの時に呼ばれる
    /// </summary>
    /// <param name="use_chara">使用するキャラクターの番号</param>
    public void Set_Parameter(int use_chara) {
        player_script = Player_Manager.Instance.player_script;
        reader = Game.Instance.reader;
        var player_status = reader.Load_csv("csv/Actor/Player/Player_csv", 3);
        player_script.ID                 = int.Parse(player_status[use_chara][0]);  // 番号
        player_script.name               = player_status[use_chara][1];             // 名前
        player_script.class_type         = int.Parse(player_status[use_chara][2]);  // クラス
        player_script.saint_graph        = int.Parse(player_status[use_chara][3]);  // 再臨状態
        player_script.level              = int.Parse(player_status[use_chara][4]);  // レベル
        player_script.hit_point          = int.Parse(player_status[use_chara][5]);  // 体力
        player_script.max_hit_point      = int.Parse(player_status[use_chara][6]);  // 最大体力
        player_script.power              = int.Parse(player_status[use_chara][7]);  // ちから(攻撃力に加味するボーナス値)
        player_script.max_power          = int.Parse(player_status[use_chara][8]);  // 力の最大値
        player_script.activity           = int.Parse(player_status[use_chara][9]);  // 行動力
        player_script.attack             = int.Parse(player_status[use_chara][10]); // 攻撃力
        player_script.defence            = int.Parse(player_status[use_chara][11]); // 防御力
        player_script.hunger_point       = int.Parse(player_status[use_chara][12]); // はらへりポイント
        player_script.skill              = int.Parse(player_status[use_chara][13]); // スキル(種類)
        player_script.star_generate      = int.Parse(player_status[use_chara][14]); // スター発生率
        player_script.keep_star          = int.Parse(player_status[use_chara][15]); // スター保持数
        player_script.command_card       = int.Parse(player_status[use_chara][16]); // コマンドカード枚数
        player_script.arts_card          = int.Parse(player_status[use_chara][17]); // アーツカード枚数
        player_script.arts_hit_conut     = int.Parse(player_status[use_chara][18]); // アーツでの攻撃時のヒット回数
        player_script.quick_card         = int.Parse(player_status[use_chara][19]); // クイックのカード枚数
        player_script.quick_hit_attack   = int.Parse(player_status[use_chara][20]); // クイックでの攻撃時のヒット回数
        player_script.buster_card        = int.Parse(player_status[use_chara][21]); // バスターのカード枚数
        player_script.buster_hit_count   = int.Parse(player_status[use_chara][22]); // バスターでの攻撃時のヒット回数
        player_script.noble_weapon       = int.Parse(player_status[use_chara][24]); // 宝具(種類)
        player_script.extra_attack       = int.Parse(player_status[use_chara][23]); // エクストラアタック(種類)
        player_script.noble_phantasm     = int.Parse(player_status[use_chara][25]); // 宝具を撃つためのポイント(以下NP)
        player_script.max_noble_phantasm = int.Parse(player_status[use_chara][26]); // NPの最大値
        player_script.attack_rise_NP     = int.Parse(player_status[use_chara][27]); // 攻撃時のNPの上昇量
        player_script.defence_rise_NP    = int.Parse(player_status[use_chara][28]); // 被ダメージ時のNPの上昇量
        player_script.experience_point   = int.Parse(player_status[use_chara][29]); // 経験値
        player_script.turn_count         = int.Parse(player_status[use_chara][30]); // 経過ターンをカウント
    }

    /// <summary>
    /// Playerのターン経過の処理(自動回復、はらへり) プレイヤーの行動が終了したときに呼ばれる
    /// </summary>
    public void Add_Turn() {
        player_script = Player_Manager.Instance.player_script;
        ++player_script.turn_count;

        // 空腹であるか
        if (player_script.hunger_point <= 0) {
            --player_script.hit_point;

            //if (Is_Dead()) {
            // ログに"餓死したと流す"
            //}
        }
        // 3ターンに一度空腹ポイントを１減らす
        else if (0 == player_script.turn_count % 3) {
            --player_script.hunger_point;
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
    /// <param name="exp">加算する経験値量</param>
    void Add_Experience_Point(int exp) {
        player_script.experience_point += exp;

        // 上限を越しても設定した最大値を超えないようにする
        if (player_script.experience_point > MAX_EXP) {
            player_script.experience_point = MAX_EXP;
        }

        // レベルアップに必要な経験値量を超えたか確かめる
        // TODO: - 1はいらない？ 要テスト
        if (exp_data_base[player_script.level - 1] <= player_script.experience_point) {
            int new_Lv = Get_Exp_Level();

            // 一度に２レベル以上あがる場合にも対応
            for (; player_script.level < new_Lv; ++player_script.level) {
                int add_hp;
                int add_atk;
                int add_def;

                // 体力の最大値をランダム(3~5)で増やす
                add_hp = Random.Range(0, 2) + 3;

                player_script.max_hit_point += add_hp;

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
            if (exp_data_base[lv] > player_script.experience_point) {
                break;
            }
        }
        return lv + 1;
    }

    /// <summary>
    /// 死亡したかを判定する 毎アクターのターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns>死亡していたらtrue</returns>
    public bool Is_Dead(int now_HP) {
        if (now_HP <= 0) {
            now_HP = 0;
            return true;
        }
        return false;
    }
}
