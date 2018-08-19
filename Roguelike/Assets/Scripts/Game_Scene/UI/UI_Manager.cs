using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using System.Linq;

/// <summary>
/// UI(主に数値)を管理
/// </summary>
public class UI_Manager : MonoBehaviour {
    /// <summary>
    /// ゲームシーンのマネージャクラス
    /// </summary>
    GameManager game_manager;
    /// <summary>
    /// ダンジョンのマネージャクラス
    /// </summary>
    Dungeon_Manager dungeon_manager;
    /// <summary>
    /// メッセッージウィンドゥを管理するクラス
    /// </summary>
    [SerializeField]
    Message_Window_Manager message_window_manager;
    /// <summary>
    /// プレイヤーのステータスを管理するクラス
    /// </summary>
    Player_Status player_status;
    /// <summary>
    /// キャラクターのセリフデータを扱うクラス
    /// </summary>
    public Character_Message_Data character_message_data;
    /// <summary>
    /// 立ち絵を管理するクラス
    /// </summary>
    public Standing_Character_Sprite_Changer standing_chara_changer;

    /// <summary>
    /// 階層UI
    /// </summary>
    [SerializeField]
    Text floor_text;
    /// <summary>
    /// HP_UI
    /// </summary>
    [SerializeField]
    Text HP_text;
    /// <summary>
    /// レベルUI
    /// </summary>
    [SerializeField]
    Text level_text;
    /// <summary>
    /// ちからのUI 
    /// </summary>
    [SerializeField]
    Text power_text;
    /// <summary>
    /// ちからの最大値のUI
    /// </summary>
    [SerializeField]
    Text max_power_text;
    /// <summary>
    /// はらへりポイントのUI
    /// </summary>
    [SerializeField]
    Text hunger_point_text;
    /// <summary>
    /// スター保持数のUI
    /// </summary>
    [SerializeField]
    Text keep_star_text;
    /// <summary>
    /// NPのUI
    /// </summary>
    [SerializeField]
    Text noble_phantasm_text;

    /// <summary>
    /// ログ表示用オブジェクト
    /// </summary>
    [SerializeField]
    GameObject scroll_view;
    /// <summary>
    /// ログの表示非表示
    /// </summary>
    ReactiveProperty<bool> is_said_scroll_view;
    public ReactiveProperty<bool> Is_Said_Scroll_View { set { is_said_scroll_view = value; } get{ return is_said_scroll_view; } }

    /// <summary>
    /// 会話での表示か、そうでないときかで分ける。
    /// </summary>
    bool talk = true;

    /// <summary>
    /// 自身のインスタンス
    /// </summary>
    public static UI_Manager Instance;

    void Awake() {
        // 作られていなかったら自身のデータを入れる
        if (Instance == null) {
            Instance = this;
        }

        is_said_scroll_view = new ReactiveProperty<bool>(false);
        character_message_data = new Character_Message_Data();
    }

    void Start() {
        game_manager = GameManager.Instance;

        // ログを表示
        is_said_scroll_view.Where(flag => !!flag && !talk).Subscribe(flag => {
            scroll_view.SetActive(flag);

            Observable.NextFrame().Subscribe(_ =>
            // 文字は上詰め
            scroll_view.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 0.0f
            );

            // 4秒経過でログを非表示
            Observable.Timer(TimeSpan.FromMilliseconds(3500))
            .Subscribe(_ => {
                is_said_scroll_view.Value = false;
            });
        }).AddTo(this);

        // 会話時のログ表示
        is_said_scroll_view.Where(flag => !!flag && talk).Subscribe(flag => {
            scroll_view.SetActive(flag);

            Observable.NextFrame().Subscribe(_ =>
            // 文字は上詰め
            scroll_view.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 0.0f
            );
        }).AddTo(this);

        // ウィンドウは非表示で初期化
        is_said_scroll_view.Where(flag => !flag).Subscribe(flag =>
            scroll_view.SetActive(flag)
        ).AddTo(this);

        // 現在の階層の表示非表示。安全地帯(拠点など)にいるときは非表示
        game_manager.Now_Place.Subscribe(now_place => {
            floor_text.enabled = now_place == eNow_Place.Dungeon;
        }).AddTo(this);

        player_status = Player_Manager.Instance.player_status;
        //TODO: 沖田しかいないので今は狙い撃ち
        player_status.Set_Parameter(Define_Value.OKITA);
        dungeon_manager = Dungeon_Manager.Instance;
        // 最大の値と現在の値の双方を表示する項目は１つにまとめておく
        var HP    = Observable.Merge(player_status.Hit_Point, player_status.Max_Hit_Point);
        var power = Observable.Merge(player_status.Power, player_status.Max_Power);
        var floor = Observable.Merge(dungeon_manager.Floor, dungeon_manager.Max_Floor);

        // 現在の階層に変更がかかったらUIを更新
        floor.Subscribe(_ =>
            Set_Floor_UI()
        ).AddTo(this);

        // 現在HP、最大HPに変更がかかったらUIを更新
        HP.Subscribe(_ =>
            Set_HP_UI()
        ).AddTo(this);
        
        // レベルに変更がかかったらUIを更新
        player_status.Level.Subscribe(_ =>
            Set_Level_UI()
        ).AddTo(this);
    }

    /// <summary>
    /// 階層を表示
    /// </summary>
    void Set_Floor_UI() {
        floor_text.text = string.Format("{0}階 / {1}階", new string[] { dungeon_manager.Floor.ToString(), dungeon_manager.Max_Floor.Value.ToString() });
    }

    /// <summary>
    /// 体力を表示
    /// </summary>
    void Set_HP_UI() {
        HP_text.text = string.Format("{0} / {1}", new string[] { player_status.Hit_Point.Value.ToString(), player_status.Max_Hit_Point.Value.ToString() });
    }

    /// <summary>
    /// レベルを表示
    /// </summary>
    void Set_Level_UI() {
        level_text.text = string.Format("Level.{0}", new string[] { player_status.Level.Value.ToString() });
    }

    /// <summary>
    /// ちからを表示
    /// </summary>
    void Set_Power_UI() {

    }

    /// <summary>
    /// はらへり値を表示
    /// </summary>
    void Hunger_UI() {

    }

    /// <summary>
    /// 取得スターを表示
    /// </summary>
    void Keep_Star_UI() {

    }

    /// <summary>
    /// NPを表示
    /// </summary>
    void Noble_Phantasm_UI() {

    }
}
