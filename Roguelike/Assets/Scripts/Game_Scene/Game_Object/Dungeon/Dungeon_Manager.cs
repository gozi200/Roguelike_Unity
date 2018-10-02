using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

/// <summary>
/// ダンジョンのマネージャー
/// </summary>
public class Dungeon_Manager : Dynamic_Unique_Component<Dungeon_Manager> {
    /// <summary>
    /// アクターのマネージャクラス
    /// </summary>
    Enemy_Manager enemy_manager;
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
    ReactiveProperty<int> floor;
    public ReactiveProperty<int> Floor { set { floor = value; } get { return floor; } }
    /// <summary>
    /// そのダンジョンの最終階層
    /// </summary>
    ReactiveProperty<int> max_floor;
    public ReactiveProperty<int> Max_Floor { set { max_floor = value; } get{ return max_floor;} }
    /// <summary>
    /// 配置するタイル種類
    /// </summary>
    ReactiveProperty<eTile_State> tile_state;
    public ReactiveProperty<eTile_State> Tile_State { set { tile_state = value; } get{ return tile_state;} }
    /// <summary>
    /// 配置する壁の種類
    /// </summary>
    ReactiveProperty<eWall_State> wall_state;
    public ReactiveProperty<eWall_State> Wall_State { set { wall_state = value; }　get{ return wall_state;} }

    /// <summary>
    /// 生成、進入しているダンジョンの種類。
    /// </summary>
    public eDungeon_Type dungeon_type;

    /// <summary>
    /// ダンジョン生存フラグ
    /// </summary>
    bool is_exist;
    public bool Is_Exist { set { is_exist = value; } get { return is_exist; } }

    /// <summary>
    /// 部屋ごとの入口の座標を格納する
    /// </summary>
    public List<List<Vector2Int>> entrance_list;
    /// <summary>
    /// フロア内の部屋を格納しておく
    /// </summary>
    public List<Map_Layer_2D> room_list;

    void Awake() {
        dungeon_generator = GameObject.Find("Dungeon_Generator").GetComponent<Dungeon_Generator>();
        map_layer_2D = new Map_Layer_2D();

        // 拠点でも使うので早めに初期化
        max_floor = new ReactiveProperty<int>();
        floor = new ReactiveProperty<int>(Define_Value.INITIAL_FLOOR);
    }

    void Start() {
        entrance_list = new List<List<Vector2Int>>();
        room_list = new List<Map_Layer_2D>();

        enemy_manager = Enemy_Manager.Instance;
        tile_state = new ReactiveProperty<eTile_State>(eTile_State.Grass);
        wall_state = new ReactiveProperty<eWall_State>(eWall_State.Tree);

        // DEBUG--------------------------------
        Key_Observer key_observer;
        key_observer = Game.Instance.key_observer;
        key_observer.On_Key_Down_AsObservable()
            .Where(key => key == KeyCode.Y)
            .Subscribe(_ => {
                max_floor.Value = 1;
            });
        //------------------------------------------
    }

    /// <summary>
    /// 次のフロアへの移動処理
    /// </summary>
    public void Next_Level(int level) {
        if (floor.Value < max_floor.Value) {
            Reset();
            ++floor.Value;
            enemy_manager.enemies.Clear();
            dungeon_generator.Load_Dungeon(level);
        }
        // 最終フロアーを越したらリザルト画面へ
        else if (floor.Value >= max_floor.Value) {
            var game_manager = GameManager.Instance;

            SceneManager.LoadScene("Result");
            // ダンジョンから抜けたら拠点から
            game_manager.Now_Place.Value = eNow_Place.Safety_Zone;
            Enemy_Manager.Instance.enemies.Clear();
            Enemy_Manager.Instance.enemies = null;
        }
    }

    /// <summary>
    ///　プレイヤー以外全てのオブジェクトを消す
    /// </summary>
    void Reset() {
        // 全てのオブジェクトをタグで探す
        GameObject[] traps   = GameObject.FindGameObjectsWithTag("Trap");
        GameObject[] stairs  = GameObject.FindGameObjectsWithTag("Stair");
        GameObject[] tiles   = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] items   = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] walls   = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] mini_map_tiles = GameObject.FindGameObjectsWithTag("Mini_Map");

        List<GameObject> enemy_list = enemy_manager.enemies;

        // ダンジョンを生成しなおすのに、一度オブジェクトを消す
        foreach(GameObject trap in traps) {
            Destroy(trap);
        }
        foreach(GameObject obj in stairs) {
            Destroy(obj);
        }
        foreach(GameObject tile in tiles) {
            Destroy(tile);
        }
        foreach(GameObject enemy in enemies) {
            Destroy(enemy);
        }
        foreach(GameObject item in items) {
            Destroy(item);
        }
        foreach(GameObject wall in walls) {
            Destroy(wall);
        }
        foreach(GameObject mini_map_tile in mini_map_tiles) {
            Destroy(mini_map_tile);
            //mini_map_tile.SetActive(false);
        }

        enemy_list.Clear();
        entrance_list.Clear();
        room_list.Clear();
        dungeon_generator.entrance_positions.Clear();
    }
}
