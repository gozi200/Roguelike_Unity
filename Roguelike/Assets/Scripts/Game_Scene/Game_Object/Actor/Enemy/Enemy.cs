using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミー本体のクラス
/// </summary>
public class Enemy : Unique_Component<Enemy> {
    //[SerializeField]
    //Enemy_Status enemy_status;

    [SerializeField]
    public List<Enemy_Data> enemys = new List<Enemy_Data>();

    Vector2 speed = new Vector2(5f, 5f);

    /// <summary>
    /// 自分のいる２次元配列上のx座標
    /// </summary>
   [SerializeField]
   int x;
   public int X { get; set; }
    /// <summary>
    /// 自分のいる２次元配列上のy座標
    /// </summary>
    [SerializeField]
    int y;
    public int Y { get; set; }
    /// <summary>
    /// 死亡判定 死んでいたらtrue
    /// </summary>
    public bool is_dead;

    public Enemy enemy;

    Vector2 enemy_position;

    void Awake() {
        enemy = gameObject.GetComponent<Enemy>();
    }

    void Start() {
        is_dead = false;

        this.x = X;
        this.y = Y;

        speed.x = 5;
        speed.y = 5;

        enemy_position = transform.position;
    }

    /// <summary>
    /// 自分の初期座標をマス情報を管理するクラスに知らせる
    /// </summary>
    /// <param name="width">スポーン座標(x座標)</param>
    /// <param name="height">スポーン座標(y座標)</param>
    public void Set_Initialize_Position(int width, int height) {
        //this.cell.Width = width;
        //this.cell.Height = height;

        enemy_position.x = width;
        enemy_position.y = height;
        gameObject.transform.position = enemy_position;
    }
}
