using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー本体のクラス
/// </summary>
public class Player : MonoBehaviour {
    public List<Player_Data> players = new List<Player_Data>();

    [SerializeField]
    Player_Status player_status;

    public Vector2 speed = new Vector2(5f, 5f);

    public bool is_dead;

    // Use this for initialization
    void Start() {
        player_status = GetComponent<Player_Status>();
        player_status.Set_Parameter();

        is_dead = false;

        speed.x = 5; // 移動量
        speed.y = 5;
    }
}
