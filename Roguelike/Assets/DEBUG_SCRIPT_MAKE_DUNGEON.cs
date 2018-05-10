using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ダンジョンを生成しなおす。ボタンクリックでダンジョンを再生成。デバッグに使用
/// </summary>
public class DEBUG_SCRIPT_MAKE_DUNGEON : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) 
            Remake();
    }

    /// <summary>
    /// Rキーが押されたら押されたら次の階へ移動
    /// </summary>
    public void Remake() {
        var dun_mana = Dungeon_Manager.Instance;
        var dun_data = new Dungeon_Data();

        dun_mana.Next_Level(dun_data.level);
    }
}
