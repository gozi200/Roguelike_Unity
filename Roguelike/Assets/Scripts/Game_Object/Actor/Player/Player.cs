/*
    制作者 石倉

    最終更新日 2018/02/07
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー本体のクラス
/// </summary>
//public class Player : MonoBehaviour {
//    [SerializeField]
//    Player_Status player_status;
//
//    public Vector2 speed = new Vector2(5f, 5f);
//
//    // 死亡フラグ 死んでいたらtrue
//    public bool is_dead;
//
//#region 変数
//
//    [System.NonSerialized]
//    public int ID;                 // 番号
//
//    [System.NonSerialized]
//    public string name;            // 名前
//
//    [System.NonSerialized]
//    public int class_type;         // クラス
//
//    [System.NonSerialized]
//    public int saint_graph;        // 再臨状態
//
//    [System.NonSerialized]
//    public int level;              // レベル
//
//    [System.NonSerialized]
//    public int hit_point;          // 現在の体力
//
//    [System.NonSerialized]
//    public int max_hit_point;      // 体力の最大値
//
//    [System.NonSerialized]
//    public int power;              // 現在のちから
//
//    [System.NonSerialized]
//    public int max_power;          // ちからの最大値
//
//    [System.NonSerialized]
//    public int activity;           // 行動力(行動回数)
//
//    [System.NonSerialized]
//    public int attack;             // 攻撃力
//
//    [System.NonSerialized]
//    public int defence;            // 防御力
//
//    [System.NonSerialized] 
//    public int hunger_point;       // はらへりポイント
//
//    [System.NonSerialized]  
//    public int skill;              // スキル種類
//
//    [System.NonSerialized]
//    public int star_generate;      // スター発生率
//
//    [System.NonSerialized]
//    public int keep_star;          // スター保持数
//
//    [System.NonSerialized]
//    public int command_card;       // コマンドカード種類
//
//    [System.NonSerialized]
//    public int arts_card;          // アーツード枚数
//
//    [System.NonSerialized]
//    public int arts_hit_conut;     // アーツカードでの攻撃ヒット回数
//
//    [System.NonSerialized]
//    public int quick_card;         // クイックカード
//
//    [System.NonSerialized]
//    public int quick_hit_attack;   // クイックカードでの攻撃ヒット回数
//
//    [System.NonSerialized]
//    public int buster_card;        // バスターカード
//
//    [System.NonSerialized]
//    public int buster_hit_count;   // バスターカードでの攻撃ヒット回数
//
//    [System.NonSerialized]
//    public int noble_weapon;       // 宝具
//
//    [System.NonSerialized]
//    public int extra_attack;       // エクストラアタック
//
//    [System.NonSerialized]
//    public int noble_phantasm;     // 宝具を撃つためのポイント
//
//    [System.NonSerialized]
//    public int max_noble_phantasm; // NPの最大値
//
//    [System.NonSerialized]
//    public int attack_rise_NP;     // 攻撃時のNPの上昇量
//
//    [System.NonSerialized]
//    public int defence_rise_NP;    // 被ダメージ時のNPの上昇量
//
//    [System.NonSerialized]
//    public int experience_point;   // 取得経験値
//
//    [System.NonSerialized]
//    public int turn_count;         // 経過ターンをカウント
//
//# endregion
//
//    // Use this for initialization
//    void Start() {
//        player_status.Set_Parameter(ID);
//
//        is_dead = false;
//
//        speed.x = 5; // 移動量
//        speed.y = 5;
//    }
//}
