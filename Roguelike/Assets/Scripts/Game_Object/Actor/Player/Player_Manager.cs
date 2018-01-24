using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのマネージャークラス
/// </summary>
public class Player_Manager : MonoBehaviour {
    [SerializeField]
     static Player player;

    //[SerializeField]
    //static Player_Action player_action;
    //
    //[SerializeField]
    //static Player_Status player_status;

    public　static  void Set_Player(Player set_player) {
        player = set_player;
    }

    //public static void Set_Player_Action(Player_Action set_player_action) {
    //    player_action = set_player_action;
    //    player = Player_Manager.GetPlayer();
    //}
    //
    //public static void Set_Player_Status(Player_Status set_player_status) {
    //    player_status = set_player_status;
    //}

    public static Player Get_Player() {
        return player;
    }
}
