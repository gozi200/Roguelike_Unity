using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームシーンのループを回す
/// </summary>
public class GameManager : MonoBehaviour {
    /// <summary>
    /// ダンジョン全体を管理するクラス
    /// </summary>
    Dungeon_Manager dungeon_manager;

    /// <summary>
    /// ゲームの状態を取得
    /// </summary>
    eGame_State game_state;

    void Start() {
        game_state = eGame_State.Create_Dungeon;
        dungeon_manager = Dungeon_Manager.Instance.dungeon_manager;
        dungeon_manager.Create();
    }

    /// <summary>
    /// ゲームループの状態を変える。シーン変更時に呼ばれる
    /// </summary>
    /// <param name = "set_state">遷移後の状態</param>
    public void Set_Game_State(eGame_State set_state) {
        game_state = set_state;
        Game_Loop(set_state);
    }

    /// <summary>
    /// ゲームの状態を管理
    /// </summary>
    /// <param name = "game_state">新しい状態</param>
    void Game_Loop(eGame_State game_state) {
        switch (game_state) {
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
