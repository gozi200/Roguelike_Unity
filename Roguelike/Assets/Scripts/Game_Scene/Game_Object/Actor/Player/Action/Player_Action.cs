using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーの行動を行うクラス
/// </summary>
public class Player_Action : MonoBehaviour {
    /// <summary>
    /// プレイヤークラス
    /// </summary>
    Player player;
    /// プレイヤーの移動処理のクラス
    /// </summary>
    Player_Move player_move;
    /// <summary>
    /// 階段に着いたときの処理を行うクラス
    /// </summary>
    Action_On_Stair action_stair;
    /// <summary>
    /// ダンジョン選択画面を管理するスクリプト
    /// </summary>
    Decide_Dungeon decide_dungeon_script;

    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    ePlayer_State player_state;
    public ePlayer_State Player_State { set { before_state = player_state; player_state = value; } get { return player_state; } }
    /// <summary>
    /// プレイヤーの直前の状態。イベントシーンの時など、前のものに戻したいときに使用
    /// </summary>
    ePlayer_State before_state;
    public ePlayer_State Before_State { get { return before_state; } }

    public void Start() {
        player = Player_Manager.Instance.player_script;
        player_move = Player_Manager.Instance.player_move;
        action_stair = Player_Manager.Instance.action_stair;

        decide_dungeon_script = GameObject.Find("Decide_Dungeon").GetComponent<Decide_Dungeon>();
    }

    /// <summary>
    /// 現在の状態に合った行動をする 毎ループ呼び出す ここでゲームオーバー判定を行う
    /// </summary>
    public void Run_Action() {
        // プレイヤーのステータス関係のクラス 死亡判定に使用
        Player_Status player_status = Player_Manager.Instance.player_status;

        switch (player_state) {
            case ePlayer_State.Move:
                player_move.Action_Move();
                break;
            case ePlayer_State.On_Stair:
                action_stair.Action_Stair();
                break;
            case ePlayer_State.Decide_Command:
                decide_dungeon_script.In_Decide();
                break;
            case ePlayer_State.Non_Active:
                Can_Not_Move();
                break;
            case ePlayer_State.Battle_Menu:
                Action_Battle_Menu();
                break;
            case ePlayer_State.Game_Over:
                SceneManager.LoadScene("Result");
                Enemy_Manager.Instance.enemies.Clear();
                break;
        }

        // 部屋の入口に到達したときにどこ部屋に来たのかを調べる TODO:同じ部屋内でも探してしまう
        if (player.Feet == Define_Value.ENTRANCE_LAYER_NUMBER) {
            player_status.Where_Room((int)player.Position.x, (int)player.Position.y);
        }

        // プレイヤーが生きていたら死亡判定をする
        if (player.Exist == true) {
            // 体力が 0 以下ならゲームオーバー処理に切り替える
            if (player_state != ePlayer_State.Game_Over && player_status.Is_Dead(player_status.Hit_Point.Value)) {
                player_state = ePlayer_State.Game_Over;
            }
        }
    }

    /// <summary>
    /// 移動不可にする
    /// </summary>
    public void Can_Not_Move() {
        player.Move_Value = 0;
    }

    /// <summary>
    /// バトルメニューの処理 //TODO: 未完
    /// </summary>
    void Action_Battle_Menu() {
        int flag_number = 1;

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            flag_number = 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            flag_number = 2;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            flag_number = 3;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            flag_number = 4;
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            switch (flag_number) {
                case 1:
                    //道具画面を開く
                    break;

                case 2:
                    //発明画面を開く(αではいらない)
                    break;

                case 3:
                    //足元画面を開く
                    break;

                case 4:
                    //ステータスを開く
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            player_state = ePlayer_State.Move;
        }
    }
}
