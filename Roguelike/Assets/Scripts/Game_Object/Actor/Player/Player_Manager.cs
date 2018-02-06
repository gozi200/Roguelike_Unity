using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのマネージャークラス
/// </summary>
public class Player_Manager : MonoBehaviour {
    [SerializeField]
     static Player player;

    public static void Set_Player(Player set_player) {
        player = set_player;
    }

    public static Player Get_Player() {
        return player;
    }
}
