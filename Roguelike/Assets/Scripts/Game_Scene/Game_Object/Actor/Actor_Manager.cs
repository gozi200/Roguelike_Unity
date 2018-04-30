using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターを共通の動きを管理するクラス
/// </summary>
public class Actor_Manager : Unique_Component<Actor_Manager> {
    /// <summary>
    /// 自身のインスタンス
    /// </summary>
    public Actor_Manager actor_manager;
    /// <summary>
    /// アクター共通のステータスを関係の処理を管理するクラス
    /// </summary>
    public Actor_Status actor_status;
    /// <summary>
    /// アクター共通の行動を管理するクラス
    /// </summary>
    public Actor_Action actor_action;

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

    /// <summary>
    /// エネミースクリプト
    /// </summary>
    public Enemy enemy_script;
    /// <summary>
    /// エネミーの行動を管理するクラス
    /// </summary>
    public Enemy_Action enemy_action;
    /// <summary>
    /// エネミーのステータス関係を管理するクラス
    /// </summary>
    public Enemy_Status enemy_status;
    /// <summary>
    /// フロア内に存在しているエネミーを格納する
    /// </summary>
    public List<GameObject> enemys = new List<GameObject>();

    void Awake() {
        player = GameObject.Find("Player");
        player_script = player.GetComponent<Player>();
        player_move = gameObject.AddComponent<Player_Move>();
        player_status = gameObject.AddComponent<Player_Status>();
        player_action = gameObject.AddComponent<Player_Action>();
        action_stair = gameObject.AddComponent<Action_On_Stair>();

        enemy_script = gameObject.AddComponent<Enemy>();
        enemy_action = gameObject.AddComponent<Enemy_Action>();
        enemy_status = gameObject.AddComponent<Enemy_Status>();

        actor_action = gameObject.AddComponent<Actor_Action>();
    }

    void Start() {
        player_attack = gameObject.AddComponent<Player_Attack>();

        actor_manager = gameObject.GetComponent<Actor_Manager>();
        actor_status = new Actor_Status();

        enemy_status.Create_Enemy();
    }

    /// <summary>
    /// 座標で敵を探す
    /// </summary>
    /// <param name="x">探す座標</param>
    /// <param name="y">探す座標</param>
    /// <returns>そこにいる敵</returns>
    public GameObject Find_Enemy(int x, int y) {
        for (int i = 0; i < actor_manager.enemys.Count; ++i) {
            if (actor_manager.enemys[i].transform.position.x == x &&
                actor_manager.enemys[i].transform.position.y == y) {
                return actor_manager.enemys[i];
            }
        }
        return null;
    }
}
