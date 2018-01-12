using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy_Action : MonoBehaviour {
    [SerializeField]
    Dungeon_Generator dungeon_generator;

    [SerializeField]
    Player_Status player_status;

    public void Move_Enemy(Player_Status player_status) {
       // player_status = new Player_Status();
        player_status.Turn();

        // TODO: エネミーの移動関数

        dungeon_generator.GetComponent<Dungeon_Generator>().Turn_Tick();
    }
}
