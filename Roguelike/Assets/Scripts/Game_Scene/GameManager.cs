using UnityEngine;
using UniRx;

/// <summary>
/// ゲームシーンのループを回す
/// </summary>
public class GameManager : Dynamic_Unique_Component<GameManager> {
    /// <summary>
    /// 拠点用マネージャーのオブジェクト
    /// </summary>
    GameObject base_manager_object;
    /// <summary>
    /// 拠点のマネージャースクリプト
    /// </summary>
    Base_Manager base_manager;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    Player_Action player_action;
    /// <summary>
    /// エネミーの行動を管理するクラス
    /// </summary>
    Enemy_Action enemy_action;
    /// <summary>
    /// ダンジョン生成クラス
    /// </summary>
    Dungeon_Generator dungeon_generator;

    /// <summary>
    /// ゲームの状態を取得
    /// </summary>
    public eGame_State game_state;
    /// <summary>
    /// 現在の場所(安全地帯かダンジョン)
    /// </summary>
    ReactiveProperty<eNow_Place> now_place;
    public ReactiveProperty<eNow_Place> Now_Place { set { now_place = value; } get { return now_place; } }

    void Awake() {
        // 拠点からなので、安全地帯を入れる
        now_place = new ReactiveProperty<eNow_Place> {
            Value = eNow_Place.Safety_Zone
        };
    }

    void Start() {
        // 開始時は拠点からなので、拠点を作る
        game_state = eGame_State.Create_Base;

        player_action = Player_Manager.Instance.player_action;
        enemy_action  = Enemy_Manager.Instance.enemy_action;
        dungeon_generator = Dungeon_Manager.Instance.dungeon_generator;

        base_manager_object = GameObject.Find("Base_Manager");
        base_manager = base_manager_object.GetComponent<Base_Manager>();
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
        switch (game_state) {
            // 拠点を作る
            case eGame_State.Create_Base:
                base_manager.Create_Base();
                Set_Game_State(eGame_State.Player_Turn);

                if (Do_Not_Destroy.First_Talk) {
                    var chara_message_data = UI_Manager.Instance.character_message_data;
                    var stand_chara_changer = UI_Manager.Instance.standing_chara_changer;
                    // 最初のセリフなのでここで初期化
                    chara_message_data.Initialize();
                    // 開始時のセリフをしゃべらせる
                    chara_message_data.Start_Talk();
                    // 誰に喋らせるかを設定する
                    stand_chara_changer.Set_Sprite(1);
                    Do_Not_Destroy.First_Talk = false;
                }
                break;
            // ダンジョンを作る
            case eGame_State.Create_Dungeon:
                var decide_dungeon = GameObject.Find("Decide_Dungeon").GetComponent<Decide_Dungeon>();
                dungeon_generator.Load_Dungeon(decide_dungeon.dungeon_data.level);
                Set_Game_State(eGame_State.Player_Turn);
                break;
            // プレイヤーターン
            case eGame_State.Player_Turn:
                player_action.Run_Action();
                break;
            // パートナーの行動
            case eGame_State.Partner_Turn:
                Set_Game_State(eGame_State.Enemy_Trun);
                break;
            // エネミーの行動
            case eGame_State.Enemy_Trun:
                enemy_action.Action();
                Set_Game_State(eGame_State.Dungeon_Turn);
                break;
            // ダンジョンのターン(敵のスポーンなど)
            case eGame_State.Dungeon_Turn:
                dungeon_generator.Turn_Tick();
                Set_Game_State(eGame_State.Player_Turn);
                break;
        }
    }
}
