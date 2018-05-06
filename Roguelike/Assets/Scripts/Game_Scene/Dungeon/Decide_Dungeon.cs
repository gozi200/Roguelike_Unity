using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:１時変数をまとめる
public class Decide_Dungeon : MonoBehaviour {
    /// <summary>
    /// プレイヤー本体のクラス
    /// </summary>
    Player player;
    /// <summary>
    /// ゲームのマネージャークラス
    /// </summary>
    GameManager game_manager;
    /// <summary>
    /// プレイヤーアクション
    /// </summary>
    Player_Action player_action;
    /// <summary>
    /// 拠点のマネージャー
    /// </summary>
    [SerializeField]
    Base_Manager base_manager;
    /// <summary>
    /// ダンジョンのマネージャークラス
    /// </summary>
    Dungeon_Manager dungeon_manager;
    /// <summary>
    /// ダンジョンのデータを管理するクラス
    /// </summary>
    public Dungeon_Data dungeon_data;

    void Start() {
        game_manager = GameManager.Instance;
        dungeon_manager = Dungeon_Manager.Instance;
        player_action = Actor_Manager.Instance.player_action;
        dungeon_data = new Dungeon_Data();
    }

    /// <summary>
    /// 草原への移動が選択されたときに呼ばれる
    /// </summary>
    public void Move_Grass() {
        var player = Actor_Manager.Instance.player_script;
        // ダンジョン移動後はプレイヤーのターンから
        player_action.Set_Action(ePlayer_State.Move);
        // 選択中は０なので1に戻して歩けるように
        player.move_value = 1;
        // ダンジョン選択UIを消す
        base_manager.dungeon_command.Value = false;
        // 移動先のダンジョンの情報をcsvファイルからロード
        dungeon_data.Load_Dungeon(eDungeon_Type.Beginning_Grass);
        // ゲームの状態をダンジョン制作のものに
        game_manager.Set_Game_State(eGame_State.Create_Dungeon);
        // 配置する床を進入ダンジョンに合ったのものに
        dungeon_manager.dungeon_tiles.dungeon_tiles.Value = eDungeon_Tile_State.Grass;
        // ダンジョンの進入するので拠点を消去
        Reset();
    }

    /// <summary>
    /// 洞窟への移動が選択されたときに呼ばれる
    /// </summary>
    public void Move_Cave() {
        var player = Actor_Manager.Instance.player_script;

        player_action.Set_Action(ePlayer_State.Move);
        player.move_value = 1;
        base_manager.dungeon_command.Value = false;
        dungeon_data.Load_Dungeon(eDungeon_Type.Dim_Cave);
        game_manager.Set_Game_State(eGame_State.Create_Dungeon);
        dungeon_manager.dungeon_tiles.dungeon_tiles.Value = eDungeon_Tile_State.Stone;
        Reset();
    }

    /// <summary>
    /// コマンド選択中の処理
    /// </summary>
    public void In_Decide() {
        var player = Actor_Manager.Instance.player_script;
        var base_manager_object = GameObject.Find("Base_Manager");
        var base_manager = base_manager_object.GetComponent<Base_Manager>();

        player.move_value = 0;
        base_manager.dungeon_command.Value = true;
    }

    /// <summary>
    /// 拠点を抜けたら拠点を消す
    /// </summary>
    void Reset() {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject tile in tiles) {
            Destroy(tile);
        }
        foreach (GameObject wall in walls) {
            Destroy(wall.gameObject);
        }
    }
}
