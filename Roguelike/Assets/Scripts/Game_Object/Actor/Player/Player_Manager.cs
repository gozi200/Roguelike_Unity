using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのマネージャークラス
/// </summary>
public class Player_Manager : MonoBehaviour {
    [SerializeField]
    Player player;

    [SerializeField]
    Player_Action player_action;

    [SerializeField]
    Player_Status player_status;

    // いる？
    [SerializeField]
    Player_Data player_data;

    private void Start() {
    }

    void Set_Player(Player set_player) {
        player = set_player;
    }

    void Set_Player_Action(Player_Action set_player_action) {
        player_action = set_player_action;
    }

    void Set_Player_Status(Player_Status set_player_status) {
        player_status = set_player_status;
    }

    void Set_Player_Data(Player_Data set_player_data) {
        player_data = set_player_data;
    }
}
