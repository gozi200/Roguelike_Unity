/*
    制作者 石倉

    最終更新日 2018/02/07
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーのパラメータを定義しておくクラス
/// </summary>
public class Enemy_Data {
    public int ID;               // 番号
    public string name;          // 名前
    public int class_type;       // クラス
    public int level;            // レベル
    public int hit_point;        // 体力
    public int max_hitpoint;     // 最大体力
    public int attack;           // 攻撃力
    public int defence;          // 防御力
    public int activity;         // 行動力
    public int critical;         // クリティカル
    public int experience_point; // 経験値(倒されたときに与える量)
    public int skill;            // スキル(種類)
    public int AI_pattern;       // AI
    public int first_floor;      // 出現開始階層
    public int last_floor;       // 出現終了階層
    public int turn_count;       // 経過ターンをカウント
}