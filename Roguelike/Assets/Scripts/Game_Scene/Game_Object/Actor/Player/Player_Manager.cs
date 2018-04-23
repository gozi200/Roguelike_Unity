using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのマネージャー TODO:Enemyと一緒にする？
/// </summary>
public class Player_Manager : Unique_Component<Player_Manager> {
    /// <summary>
    /// 自身のインスタンス
    /// </summary>
    public Player_Manager manager;
    /// <summary>
    /// プレイヤー本体
    /// </summary>
    public GameObject player;
    /// <summary>
    /// プレイヤースクリプト
    /// </summary>
    public Player player_script;
    /// <summary>
    /// プレイヤーのステータスに関する処理を行うクラス
    /// </summary>
    public Player_Status status;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    public Player_Action action;
    /// <summary>
    /// プレイヤーの動きを制御するクラス
    /// </summary>
    public Player_Move move;
    /// <summary>
    /// プレイヤーの攻撃処理を管理するクラス
    /// </summary>
    public Player_Attack attack;
    /// <summary>
    /// プレイヤーが階段についたときの処理を行うクラス
    /// </summary>
   　public Action_On_Stair action_stair;

    void Awake() {
        manager = gameObject.GetComponent<Player_Manager>();
        player = GameObject.Find("Player");
        player_script = player.GetComponent<Player>();
        move = gameObject.AddComponent<Player_Move>();
        status = new Player_Status();
    }

    void Start() {
        action = gameObject.AddComponent<Player_Action>();
        attack = gameObject.AddComponent<Player_Attack>();
        action_stair = gameObject.AddComponent<Action_On_Stair>();
    }
}
