using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// UI(主に数値)を管理するクラス
/// </summary>
public class UI_Manager : MonoBehaviour {
    /// <summary>
    /// ダンジョンのマネージャクラス
    /// </summary>
    Dungeon_Manager dungeon_manager;
    /// <summary>
    /// プレイヤーのステータスを管理するクラス
    /// </summary>
    Player_Status player_status;
    
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

    void Start () {
        player_status = Actor_Manager.Instance.player_status;
        dungeon_manager = Dungeon_Manager.Instance;

        // 最大の値と現在の値をまとめる
        var HP = Observable.Merge(player_status.hit_point, player_status.max_hit_point);
        var power = Observable.Merge(player_status.power, player_status.max_power);
        var floor = Observable.Merge(dungeon_manager.floor, dungeon_manager.max_floor);

        // 現在の階層に変更がかかったらUIを更新
        floor.Subscribe(_ =>
            Set_Floor_UI()
        ).AddTo(this);
        // 現在HP、最大HPに変更がかかったらUIを更新
        HP.Subscribe(_ =>
            Set_HP_UI()
        ).AddTo(this);
        player_status.level.Subscribe(_ =>
            Set_Level_UI()
        ).AddTo(this);
    }

    /// <summary>
    /// 階層が変わった時に書き直すUIをまとめておく
    /// </summary>
    void Follow_Floor() {
        Set_Floor_UI();
    }

    /// <summary>
    /// 階層を表示
    /// </summary>
    void Set_Floor_UI() {
        var dungeon_manager = Dungeon_Manager.Instance;
        floor_text.text = string.Format("{0}階 / {1}階", new string[] { dungeon_manager.floor.ToString(), dungeon_manager.max_floor.Value.ToString() });
    }

    /// <summary>
    /// 体力を表示
    /// </summary>
    void Set_HP_UI() {
        HP_text.text = string.Format("{0} / {1}", new string[] { player_status.hit_point.Value.ToString(), player_status.max_hit_point.Value.ToString() });
    }

    /// <summary>
    /// レベルを表示
    /// </summary>
    void Set_Level_UI() {
        level_text.text = string.Format("Level.{0}", new string[] { player_status.level.Value.ToString() });
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
