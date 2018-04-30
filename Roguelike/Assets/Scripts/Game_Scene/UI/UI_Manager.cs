using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// UIを管理するクラス
/// </summary>
public class UI_Manager : MonoBehaviour {
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
    /// ダンジョンのマネージャクラス
    /// </summary>
    Dungeon_Manager dungeon_manager;

    void Start () {
        dungeon_manager = Dungeon_Manager.Instance.manager;
        var game_manager = GameManager.Instance.game_manager;
        var player_status = Actor_Manager.Instance.player_status;

        dungeon_manager.floor.Subscribe(_ =>
                   Follow_Floor()
        ).AddTo(this);

//        player_status.hit_point.
    }

    /// <summary>
    /// 階層が変わった時に変えきなおすUIをまとめておく
    /// </summary>
    void Follow_Floor() {
        Set_Floor_UI();
    }

    /// <summary>
    /// 階層を表示する
    /// </summary>
    void Set_Floor_UI() {
        var dungeon_manager = Dungeon_Manager.Instance.manager;
        floor_text.text = string.Format("{0}階 / {1}階", new string[] { dungeon_manager.floor.ToString(), dungeon_manager.max_floor.ToString() });
    }

    /// <summary>
    /// 体力を表示
    /// </summary>
    void Set_HP_UI() {
        var player_status = Actor_Manager.Instance.player_status;
        HP_text.text = string.Format("{0} / {1}", new string[] { player_status.hit_point.ToString(), player_status.max_hit_point.ToString() });
    }
}
