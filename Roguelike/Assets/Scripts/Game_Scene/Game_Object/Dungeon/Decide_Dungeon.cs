using UnityEngine;

/// <summary>
/// 選択されたダンジョンを作る準備をする
/// </summary>
public class Decide_Dungeon : MonoBehaviour {
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
        player_action = Player_Manager.Instance.player_action;
        dungeon_data = new Dungeon_Data();
    }

    /// <summary>
    /// 草原への移動が選択されたときに呼ばれる
    /// </summary>
    public void Move_Grass() {
        var player = Player_Manager.Instance.player_script;

        // 移動先のダンジョンの情報をcsvファイルからロード
        dungeon_data.Load_Dungeon(eDungeon_Type.Beginning_Grass);
        // そのダンジョンの最終階層を設定
        dungeon_manager.Max_Floor.Value = dungeon_data.max_floor;
        // ゲームの状態をダンジョン制作のものに
        game_manager.Set_Game_State(eGame_State.Create_Dungeon);
        // 配置する床を進入ダンジョンに合ったのものに
        dungeon_manager.Tile_State.Value = eTile_State.Grass;
        // 配置する壁を進入ダンジョンに合ったのものに
        dungeon_manager.Wall_State.Value = eWall_State.Tree;
        // ダンジョン選択UIを消す
        base_manager.is_said_dungeon_command.Value = false;
        // ダンジョンの進入するので拠点を消去
        Reset();
        // 現在の位置をダンジョンに
        game_manager.Now_Place.Value = eNow_Place.Dungeon;
        // ダンジョン移動後はプレイヤーのターンから
        player_action.Player_State = ePlayer_State.Move;
        // 選択中は０なので1に戻して歩けるように
        player.Move_Value = Define_Value.MOVE_VALUE;
    }

    /// <summary>
    /// 洞窟への移動が選択されたときに呼ばれる
    /// </summary>
    public void Move_Cave() {
        var player = Player_Manager.Instance.player_script;

        // 移動先以外は草原と一緒
        dungeon_data.Load_Dungeon(eDungeon_Type.Dim_Cave);
        dungeon_manager.Max_Floor.Value = dungeon_data.max_floor;
        game_manager.Set_Game_State(eGame_State.Create_Dungeon);
        dungeon_manager.Tile_State.Value = eTile_State.Stone;
        dungeon_manager.Wall_State.Value = eWall_State.Stone;
        base_manager.is_said_dungeon_command.Value = false;
        Reset();
        game_manager.Now_Place.Value = eNow_Place.Dungeon;
        player_action.Player_State = ePlayer_State.Move;
        player.Move_Value = 1;
    }

    /// <summary>
    /// コマンド選択中の処理
    /// </summary>
    public void In_Decide() {
        var player = Player_Manager.Instance.player_script;
        var base_manager_object = GameObject.Find("Base_Manager");
        var base_manager = base_manager_object.GetComponent<Base_Manager>();

        // 歩けなくする
        player.Move_Value = 0;
        base_manager.is_said_dungeon_command.Value = true;
    }

    /// <summary>
    /// 拠点を抜けたら拠点を消す
    /// </summary>
    void Reset() {
        var map_layer = Dungeon_Manager.Instance.map_layer_2D;

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        // レイヤー番号を保持している配列を解放
        map_layer.coordinates = null;

        foreach (GameObject tile in tiles) {
            Destroy(tile);
        }
        foreach (GameObject wall in walls) {
            Destroy(wall.gameObject);
        }
    }
}
