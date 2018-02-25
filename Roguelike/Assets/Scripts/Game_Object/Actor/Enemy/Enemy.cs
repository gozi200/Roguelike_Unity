/*
    制作者 石倉

    最終更新日 2018/02/22
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミー本体のクラス
/// </summary>
public class Enemy : MonoBehaviour {
    [SerializeField]
    Enemy_Status enemy_status;

    [SerializeField]
    public List<Enemy_Data> enemys = new List<Enemy_Data>();

    Vector2 speed = new Vector2(5f, 5f);
    int x;
    public int X { get; set; }
    int y;
    public int Y { get; set; }

    //public float X { get; set; }
    //public float Y { get; set; }
    //
    //public Enemy(float x, float y) {
    //    this.x = x;
    //    this.y = y;
    //}

    /// <summary>
    /// 死亡判定 死んでいたらtrue
    /// </summary>
    public bool is_dead;

    // Use this for initialization
    void Start () {
        enemy_status = GetComponent<Enemy_Status>();
        enemy_status.Set_Parameter();

        is_dead = false;

        speed.x = 5; // 縦軸の移動量
        speed.y = 5; // 横軸の移動量
    }
}