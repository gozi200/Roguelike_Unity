using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

/// <summary>
/// ダンジョンのマネージャー
/// </summary>
public class Dungeon_Manager : Unique_Component<Dungeon_Manager> {
    /// <summary>
    /// アクターのマネージャー
    /// </summary>
    Actor_Manager actor_manager;
    /// <summary>
    /// 部屋の情報を設定するクラス
    /// </summary>
    public Room_Detail room_detail;
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
    public ReactiveProperty<int> floor;
    /// <summary>
    /// そのダンジョンの最終階層
    /// </summary>
    public ReactiveProperty<int> max_floor;

    /// <summary>
    /// 生成するダンジョンの種類
    /// </summary>
    public eDungeon_Type dungeon_type;
    /// <summary>
    /// 配置するタイルの状態
    /// </summary>
    public ReactiveProperty<eTile_State> tile_state;
    /// <summary>
    /// 配置する壁の状態
    /// </summary>
    public ReactiveProperty<eWall_State> wall_state;

    /// <summary>
    /// 部屋ごとに情報を格納する
    /// </summary>
    public List<Room_Detail> room_list;

    void Awake() {
        dungeon_generator = GameObject.Find("Dungeon_Generator").GetComponent<Dungeon_Generator>();
        map_layer_2D = new Map_Layer_2D();
    }

    void Start() {
        room_list = new List<Room_Detail>();

        room_detail = new Room_Detail();
        actor_manager = Actor_Manager.Instance;
        floor = new ReactiveProperty<int>(Define_Value.INITIAL_FLOOR);
        max_floor = new ReactiveProperty<int>();
        tile_state = new ReactiveProperty<eTile_State>(eTile_State.Grass);
        wall_state = new ReactiveProperty<eWall_State>(eWall_State.Tree);
    }

    /// <summary>
    /// 次のダンジョンへの移動処理
    /// </summary>
    public void Next_Level(int level) {
        // 最終フロアーを越したらリザルト画面へ
        if (floor.Value < max_floor.Value) {
            Reset();
            ++floor.Value;
            dungeon_generator.Load_Dungeon(level);
        }
        else if (floor.Value >= max_floor.Value) {
            SceneManager.LoadScene("Result");
        }
    }

    /// <summary>
    ///　プレイヤー以外全てのオブジェクトを消す
    /// </summary>
    void Reset() {
        // 全てのオブジェクトをタグで探す
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        GameObject[] stairs = GameObject.FindGameObjectsWithTag("Stair");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> enemy_list = actor_manager.enemys;

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
        foreach (GameObject enemy in enemys) {
            Destroy(enemy);
        }
        foreach (GameObject item in items) {
            Destroy(item);
        }
        foreach (GameObject wall in walls) {
            Destroy(wall.gameObject);
        }
        enemy_list.Clear();
        room_list.Clear();
    }
}
