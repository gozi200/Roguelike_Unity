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

    public Vector2 speed = new Vector2(5f, 5f);

    public bool is_dead;

    // Use this for initialization
    void Start () {
        enemy_status = GetComponent<Enemy_Status>();
        enemy_status.Set_Parameter();

        is_dead = false;

        speed.x = 5; // 縦軸の移動量
        speed.y = 5; // 横軸の移動量

        Enemy_Manager.Set_Enemy(this);
    }
}