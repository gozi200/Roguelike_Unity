using UnityEngine;

/// <summary>
/// プレイヤーのマネージャクラス
/// </summary>
public class Player_Manager : Dynamic_Unique_Component<Player_Manager> {
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
    [SerializeField]
    public Player_Status player_status;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    public Player_Action player_action;
    /// <summary>
    /// プレイヤーの動きを制御するクラス
    /// </summary>
    public Player_Move player_move;
    /// <summary>
    /// プレイヤーの攻撃処理を管理するクラス
    /// </summary>
    public Player_Attack player_attack;
    /// <summary>
    /// プレイヤーが階段についたときの処理を行うクラス
    /// </summary>
   　public Action_On_Stair action_stair;

    void Awake() {
        player = GameObject.Find("Player");
        player_script = player.GetComponent<Player>();
        player_move = gameObject.AddComponent<Player_Move>();
        player_status = new Player_Status();
        player_status.Initialize();
        player_action = gameObject.AddComponent<Player_Action>();
        action_stair = gameObject.AddComponent<Action_On_Stair>();
    }

    void Start() {
        player_attack = gameObject.AddComponent<Player_Attack>();
    }
}
