using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームシーンのループを回す
/// </summary>
public class GameManager : Unique_Component<GameManager> {
    /// <summary>
    /// 自身のインスタンス
    /// </summary>
    public GameManager game_manager;
    /// <summary>
    /// プレイヤーアクションを管理するクラス
    /// </summary>
    Player_Action player_action;
    /// <summary>
    /// エネミーアクションを管理するクラス
    /// </summary>
    Enemy_Action enemy_action;

    /// <summary>
    /// ゲームの状態を取得
    /// </summary>
    public eGame_State game_state;

    void Awake() {
        game_manager = gameObject.GetComponent<GameManager>();
    }

    void Start() {
        game_state = eGame_State.Create_Base;
        player_action = Actor_Manager.Instance.player_action;
        enemy_action = Actor_Manager.Instance.enemy_action;
    }

    void Update() {
        Game_Loop(game_state);
    }

    /// <summary>
    /// ゲームループの状態を変える。シーン変更時に呼ばれる
    /// </summary>
    /// <param name = "set_state">遷移後の状態</param>
    public void Set_Game_State(eGame_State set_state) {
        game_state = set_state;
    }

    /// <summary>
    /// ゲームの状態を管理
    /// </summary>
    /// <param name = "game_state">新しい状態</param>
    void Game_Loop(eGame_State game_state) {
        var dungeon_manager = Dungeon_Manager.Instance.manager;

        switch (game_state) {
            case eGame_State.Create_Base:
                // 拠点を作る
                Set_Game_State(eGame_State.Create_Dungeon);
                break;

            case eGame_State.Create_Dungeon:
                // ダンジョンを作る
                dungeon_manager.Create();
                Set_Game_State(eGame_State.Player_Turn);
                break;

            case eGame_State.Player_Turn:
                // プレイヤーのターン
                player_action.Run_Action();
                break;

            case eGame_State.Partner_Turn:
                // パートナーの行動
                break;

            case eGame_State.Enemy_Trun:
                // エネミーのターン
                enemy_action.Move_Enemy();
                break;

            case eGame_State.Dungeon_Turn:
                // ダンジョンのターン(敵のスポーンなど)
                break;
        }
    }
}
