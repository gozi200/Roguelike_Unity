using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor_Status : MonoBehaviour {
    /// <summary>
    /// 現在の体力を取得
    /// </summary>
    /// <param name="now_HP"></param>
    /// <returns></returns>
    public int Get_HP(int now_HP) {
        int hp;

        hp = now_HP;

        return hp;
    }

    /// <summary>
    /// 体力の最大値を取得
    /// </summary>
    /// <returns></returns>
    public int Get_Max_HP(int now_Max_HP) {
        int max_hp;

        max_hp = now_Max_HP;

        // TODO: 体力UPのアイテムを装備していたら、加算する処理を追加

        return max_hp;
    }

    /// <summary>
    /// 攻撃力を取得
    /// </summary>//
    /// <returns></returns>
    public int Get_Attack(int now_ATK) {
        int ATK;

        ATK = now_ATK;

        // TODO: 武器装備時の計算

        // TODO: 礼装装備時の計算

        return ATK;
    }

    /// <summary>
    /// 防御力の取得
    /// </summary>
    /// <returns></returns>
    int Get_Defence(int now_DEF) {
        int DEF;

        DEF = now_DEF;

        // TODO: 防具装備時の計算

        // TODO: 礼装装備時の計算

        return DEF;
    }

    /// <summary>
    /// 行動力を取得
    /// </summary>
    /// <returns></returns>
    int Get_Activity(int now_activity) {
        int activity;

        activity = now_activity;

        return activity;
    }

    /// <summary>
    /// 死亡判定
    /// </summary>
    /// <returns></returns>
    public bool Is_Dead(int now_HP) {
        if (now_HP <= 0) {
            now_HP = 0;
            return true;
        }
        return false;
    }
}
