using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ダンジョンのマネージャー
/// </summary>
public class Dungeon_Manager : Unique_Component<Dungeon_Manager> {
    /// <summary>
    /// 自身のインスタンス
    /// </summary>
    public Dungeon_Manager dungeon_manager;
    /// <summary>
    /// ダンジョン作成クラス
    /// </summary>
    public Dungeon_Generator dungeon_generator;
    /// <summary>
    /// マップを２次元配列で管理するクラス
    /// </summary>
    public Map_Layer_2D map_layer_2D;
    /// <summary>
    /// 表示する階層
    /// </summary>
    int floor;
    /// <summary>
    /// ダンジョンの難易度
    /// </summary>
    int level;
    /// <summary>
    /// そのダンジョンの最終階層
    /// </summary>
    int Max_floor;
    /// <summary>
    /// 階層UI
    /// </summary>
    Text floor_text;

    /// <summary>
    /// ゲームの現在の状態
    /// </summary>
    eGame_State game_state;
    /// <summary>
    /// 現在のダンジョンの難易度
    /// </summary>
    eDungeon_Level dungeon_level;

    void Awake() {
        level = 1;
        floor = 1;
        Max_floor = 5;
        dungeon_manager = gameObject.GetComponent<Dungeon_Manager>();
        dungeon_generator = GameObject.FindObjectOfType<Dungeon_Generator>();
        map_layer_2D = new Map_Layer_2D();
    }

    /// <summary>
    /// 作成したダンジョンを表示
    /// </summary>
    public void Create() {
        game_state = eGame_State.Create_Dungeon;

        Initialize();
    }

    /// <summary>
    /// 次のダンジョンへの移動処理
    /// </summary>
    public void NextLevel() {
        Reset();
        ++floor;
        Initialize();
    }

    /// <summary>
    ///　プレイヤー以外全てのオブジェクトを消す
    /// </summary>
    void Reset() {
        // 全てのオブジェクトをタグで探す TODO:Findは重い？
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        GameObject[] stairs = GameObject.FindGameObjectsWithTag("Stair");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        List<GameObject> enemy_list = dungeon_generator.enemys;

        // ダンジョンを生成しなおすのに、一度オブジェクトを消す
        foreach (GameObject trap in traps) {
            Destroy(trap);
        }
        foreach (GameObject obj in stairs) {
            Destroy(obj);
        }
        foreach (GameObject tile in tiles) {
            Destroy(tile);
        }
        foreach (GameObject enemy in enemy_list) {
            Destroy(enemy);
        }
        foreach (GameObject item in items) {
            Destroy(item);
        }
        foreach (GameObject wall in walls) {
            Destroy(wall.gameObject);
        }
        enemy_list.Clear();
    }

    /// <summary>
    /// ダンジョン表示する前の準備
    /// </summary>
    void Initialize() {
        floor_text = GameObject.Find("Floor_Text").GetComponent<Text>();
        floor_text.text = string.Format("{0}階 / {1}階", new string[] { floor.ToString(), Max_floor.ToString() });
        //TODO:現在進入中のダンジョンからダンジョンの難易度を出したものをいれる
        dungeon_generator.Load_Dungeon(level);
    }
}
