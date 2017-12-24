using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PLAYER_STATUS {
    public int ID;                 // 番号
    public string name;            // 名前
    public int class_type;         // クラス
    public int saint_graph;        // 再臨状態
    public int level;              // レベル
    public int hit_point;          // 体力
    public int max_hitpoint;       // 最大体力
    public int power;              // ちから(攻撃力見加味するボーナス値)
    public int max_power;          // 力の最大値
    public int activity;           // 行動力
    public int attack;             // 攻撃力
    public int defence;            // 防御力
    public int hunger_point;       // はらへりポイント
    public int skill;              // スキル(種類)
    public int star_generate;      // スター発生率
    public int keep_star;          // スター保持数
    public int command_card;       // コマンドカード枚数
    public int arts_card;          // アーツカード枚数
    public int arts_hit_conut;     // アーツでの攻撃時のヒット回数
    public int quick_card;         // クイックのカード枚数
    public int quick_hit_attack;   // クイックでの攻撃時のヒット回数
    public int buster_card;        // バスターのカード枚数
    public int buster_hit_count;   // バスターでの攻撃時のヒット回数
    public int extra_attack;       // エクストラアタック(種類)
    public int noble_weapon;       // 宝具(種類)
    public int noble_phantasm;     // 宝具を撃つためのポイント(以下NP)
    public int max_noble_phantasm; // NPの最大値
    public int attack_rise_NP;     // 攻撃時のNPの上昇量
    public int defence_rise_NP;    // 被ダメージ時のNPの上昇量
    public int experience_point;   // 経験値
    public int turn_count;         // 経過ターンをカウント
}

//ublic class PLAYE_DATA_BASE : MonoBehaviour {
//
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//


