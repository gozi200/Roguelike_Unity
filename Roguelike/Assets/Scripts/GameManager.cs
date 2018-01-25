using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // 現在の状態
    eGame_State game_state;

    [SerializeField]
    Enemy_Manager enemy_manager;

    [SerializeField]
    Player_Manager player_manager;

    [SerializeField]
    Dungeon_Generator dungeon_generator;

    public static GameManager Instance;

    // Use this for initialization
    void Start () {
        Instance = this;
        game_state = eGame_State.Dungeon_Create;
    }

    void Game_Loop(eGame_State state) {
        switch (state) {
            case eGame_State.Dungeon_Create:
                // ダンジョンを作る
                break;

            case eGame_State.Player_Turn:
                // プレイヤーの行動
                break;

            case eGame_State.Partner_Turn:
                // パートナーの行動
                break;

            case eGame_State.Enemy_Trun:
                // エネミーの行動
                break;

            case eGame_State.Dungeon_Turn:
                // ダンジョンのターン(敵のスポーンなど)
                break;
        }
    }

    public void Set_Game_State(eGame_State set_state) {
        game_state = set_state;
        Game_Loop(set_state);
    }
}
