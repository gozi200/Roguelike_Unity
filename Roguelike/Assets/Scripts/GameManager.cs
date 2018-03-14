/*
制作者 アントニオ 石倉

最終編集日 2018/02/08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームシーンのループを回す
/// </summary>
public class GameManager : MonoBehaviour {
    // 現在の状態
    eGame_State game_state;

    GM.Player player;

    static Enemy_Manager enemy_manager;
    [SerializeField]
    Dungeon_Manager dungeon_manager;

    void Awake() {
        player = GM.Instance.player;
    }

    void Start() {
        game_state = eGame_State.Create_Dungeon;
        dungeon_manager.Create();
    }

    public static void Set_Enemy(Enemy_Manager set_enemy_manager) {
        enemy_manager = set_enemy_manager;
    }

    /// <summary>
    /// 状態が変わるときに呼ばれる
    /// </summary>
    /// <param name = "set_state">遷移後の状態</param>
    public void Set_Game_State(eGame_State set_state) {
        game_state = set_state;
        Game_Loop(set_state);
    }

    /// <summary>
    /// ゲームの状態を管理
    /// </summary>
    /// <param name = "state">新しい状態</param>
    void Game_Loop(eGame_State state) {
        switch (state) {
            case eGame_State.Create_Base:
                // 拠点を作る
                break;

            case eGame_State.Create_Dungeon:
                // ダンジョンを作る
                dungeon_manager.NextLevel();
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
}
