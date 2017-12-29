using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class Player : MonoBehaviour {
    public List<Player_Data> players = new List<Player_Data>();

    public Player_Status player_status;

    public Vector2 SPEED = new Vector2(5f, 5f);

    //public bool is_dead = false;
    
    // Use this for initialization
    void Start() {
        player_status = new Player_Status();
        player_status.Set_Parameter();

        Debug.Log(players[1].attack);

        SPEED.x = 5;
        SPEED.y = 5;
    }

    // Update is called once per frame
    void Update() {
      //  Move(); // デバッグ
    }

    // 移動関数
    void Move() {
        // 現在位置をPositionに代入
        Vector2 Position = transform.position;

        // 左キーを押し続けていたら
        if (Input.GetKeyDown("left")) {
            // 代入したPositionに対して加算減算を行う
            Position.x -= SPEED.x;
        }
        else if (Input.GetKeyDown("right")) { // 右キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            Position.x += SPEED.x;
        }
        else if (Input.GetKeyDown("up")) { // 上キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            Position.y += SPEED.y;
        }
        else if (Input.GetKeyDown("down")) { // 下キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            Position.y -= SPEED.y;
        }

        // 現在の位置に加算減算を行ったPositionを代入する
        transform.position = Position;
    }

    public bool Is_Dead() {
        if (players[0].hit_point <= 0) {
            players[0].hit_point = 0;
            return true;
        }

        return false;
    }
}
