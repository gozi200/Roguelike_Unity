using System;
using UnityEngine;

/// <summary>
/// エネミー共通で扱えるステータス(設定後に値の変わらないもの)
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "Creat_Enemy")]
public class Enemy_Status_Base : ScriptableObject {
    /// <summary>
    /// クラス
    /// </summary>
    [SerializeField]
    eClass_Type class_type;
    public eClass_Type Class_Type { set { class_type = value; } get { return class_type; } }

    /// <summary>
    /// 番号
    /// </summary>
    [SerializeField]
    int id;
    public int ID { set { id = value; } get { return id; } }
    /// <summary>
    /// 名前
    /// </summary>
    [SerializeField]
    new string name;
    public string Name { set { name = value; } get { return name; } }
    /// <summary>
    /// 最大HP
    /// </summary>
    [SerializeField]
    int max_HP;
    public int Max_HP { set { max_HP = value; } get { return max_HP; } }
    /// <summary>
    /// 元攻撃力(実際に与えるダメージはこれに計算を加えたもの)
    /// </summary>
    [SerializeField]
    int attack;
    public int Attack { set { attack = value; } get { return attack; } }
    /// <summary>
    /// 元防御力(実際に受けるダメージはこれに計算を加えたもの)
    /// </summary>
    [SerializeField]
    int defence;
    public int Defence {  set { defence = value; } get { return defence; } }
    /// <summary>
    /// クリティカル率(バフなどはこれに計算を加える)
    /// </summary>
    [SerializeField]
    int critical;
    public int Critical { set { critical = value; } get { return critical; } }
    /// <summary>
    /// スキル
    /// </summary>
    [SerializeField]
    int skill;
    public int Skill { set { skill = value; } get { return skill; } }
    /// <summary>
    /// 必殺技(ゲージが溜まり切った時に出るアレ)
    /// </summary>
    [SerializeField]
    int finish_move;
    public int Finish_Move { set { finish_move = value; } get { return finish_move; } }
    /// <summary>
    /// 出現開始階層
    /// </summary>
    [SerializeField]
    int first_floor;
    public int First_Floor { set { first_floor = value; } get { return first_floor; } }
    /// <summary>
    /// 出現終了階層
    /// </summary>
    [SerializeField]
    int last_floor;
    public int Last_Floor { set { last_floor = value; } get { return last_floor; } }
    /// <summary>
    /// 倒されたときに与える経験値量
    /// </summary>
    [SerializeField]
    int experience_point;
    public int Experience_Point { set { experience_point = value; } get { return experience_point; } }
    /// <summary>
    /// 行動パターン
    /// </summary>
    [SerializeField]
    int AI_pattern;
    public int AI_Pattern { set { AI_pattern = value; } get { return AI_pattern; } }

    /// <summary>
    /// コピーする
    /// </summary>
    /// <param name="obj">コピーする方</param>
    public void Copy(Enemy_Status_Base obj) {
        Class_Type       = obj.Class_Type;
        ID               = obj.ID;
        Name             = obj.Name;
        Max_HP           = obj.Max_HP;
        Attack           = obj.Attack;
        Defence          = obj.Defence;
        Critical         = obj.Critical;
        Skill            = obj.Skill;
        Finish_Move      = obj.Finish_Move;
        First_Floor      = obj.First_Floor;
        Last_Floor       = obj.Last_Floor;
        Experience_Point = obj.Experience_Point;
        AI_Pattern       = obj.AI_Pattern;
    }
}
