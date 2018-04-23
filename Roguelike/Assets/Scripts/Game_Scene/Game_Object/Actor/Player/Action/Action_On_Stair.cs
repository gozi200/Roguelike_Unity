using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// プレイヤーが階段と重なった時の処理
/// </summary>
public class Action_On_Stair : MonoBehaviour {
    /// <summary>
    /// プレイヤーの本体
    /// </summary>
    GameObject player;
    /// <summary>
    /// プレイヤースクリプト
    /// </summary>
    Player player_script;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    Player_Action player_action;
    /// <summary>
    /// ダンジョンのマネージャークラス
    /// </summary>
    Dungeon_Manager dungeon_manager;
    /// <summary>
    /// 階段を進むか否かのコマンドを表示
    /// </summary>
    GameObject stair_command_UI;

    /// <summary>
    /// 階段にいるときの選択肢
    /// </summary>
    public eStair_Command stair_command;

    /// <summary>
    /// 階段コマンドの表示非表示
    /// </summary>
    public ReactiveProperty<bool> said_stair_command;


    void Start() {
        player = Player_Manager.Instance.player;
        player_script = Player_Manager.Instance.player_script;
        player_action = Player_Manager.Instance.action;
        dungeon_manager = Dungeon_Manager.Instance.dungeon_manager;

        stair_command_UI = GameObject.Find("Stair_Command_UI");
        said_stair_command = new ReactiveProperty<bool>(false);

        stair_command = eStair_Command.Progress;

        // trueで階段で進むか否かのコマンドを表示 falseで閉じる
        said_stair_command.Where(flag => !!flag)
            .Subscribe(flag =>
            stair_command_UI.SetActive(flag)
            ).AddTo(this);
        said_stair_command.Where(flag => !flag)
            .Subscribe(flag =>
            stair_command_UI.SetActive(flag)
            ).AddTo(this);
    }

    /// <summary>
    /// 階段にいるときの処理
    /// </summary>
    public void Action_Stair() {
        // 選ぶまで動けなくする
        player_script.move_value = 0;
        said_stair_command.Value = true;
    }
}
