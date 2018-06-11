using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class Enemy_Manager : Unique_Component<Enemy_Manager> {
    /// <summary>
    /// エネミーオブジェクト
    /// </summary>
    GameObject enemy_object;
    /// <summary>
    /// エネミーステータス
    /// </summary>
    Enemy_Status enemy_status;
    /// <summary>
    /// エネミーの行動を決めるクラス
    /// </summary>
    public Enemy_Action enemy_action;

    /// <summary>
    /// フロア内に存在しているエネミーを格納する
    /// </summary>
    public List<GameObject> enemies;
    /// <summary>
    /// 種類ごとにエネミーを格納する
    /// </summary>
    public List<Enemy_Status> enemy_type;

    /// <summary>
    /// 乱数を格納。(スポーンの時に使用)
    /// </summary>
    int spawn_random_enemy;

    /// <summary>
    /// １フロアに存在している敵の数を数える
    /// </summary>
    int enemy_counter;
    public int Enemy_Counter {
        set { enemy_counter = value; }
        get { return enemy_counter; }
    }

    void Start() {
        enemy_counter = 0;
        enemy_status = new Enemy_Status();

        // 行動決定するクラスの初期化を行う
        enemy_action = new Enemy_Action();
        enemy_action.Initialize();

        enemies = new List<GameObject>();
        enemy_type = new List<Enemy_Status>();
    }

    /// <summary>
    /// 座標で敵を探す
    /// </summary>
    /// <param name="x">探す座標</param>
    /// <param name="y">探す座標</param>
    /// <returns>そこにいる敵</returns>
    public GameObject Find_Enemy(float x, float y) {
        for (int i = 0; i < enemies.Count; ++i) {
            if (enemies[i].transform.position.x == x &&
                enemies[i].transform.position.y == y) {
                return enemies[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 死んでいたらオブジェクトとリストから消す
    /// </summary>
    /// <param name="index">エネミーリストの要素数</param>
    public void Dead_Enemy(int index) {
        Map_Layer_2D map_layer = Dungeon_Manager.Instance.map_layer_2D;
        var enemy_manager = Enemy_Manager.Instance;

        // 死んだら消して、床のレイヤー番号を元のものに戻す
        if (enemy_status.Is_Dead(enemy_status.hit_point)) {
            // 足元のレイヤーを元に戻す
            map_layer.Tile_Swap(enemy_manager.enemies[index].transform.position,
                                enemy_manager.enemies[index].GetComponent<Enemy>().Feet);
            // ゲームオブジェクトの解放
            Destroy(enemy_manager.enemies[index]);
            // リストの要素の解放
            enemy_manager.enemies.RemoveAt(index);
        }
    }

    /// <summary>
    /// ダンジョンに出現する敵の中からスポーンさせるエネミーを乱数で決める
    /// </summary>
    /// <returns>スポーンさせるエネミーのID</returns>
    void Random_Enemy_Type() {
        List<Enemy_Status> appear_enemy_list = Enemy_Manager.Instance.enemy_type;
        ReactiveProperty<int> now_floor = Dungeon_Manager.Instance.floor;
        int[] lottery_enemy = new int[appear_enemy_list.Count];

        // 現在の階層から出現階層を調べ、満たしているものを抽出する
        for (int i = 0; i < appear_enemy_list.Count; ++i) {
            if (appear_enemy_list[i].first_floor <= now_floor.Value &&
                appear_enemy_list[i].last_floor >= now_floor.Value) {

                lottery_enemy[i] = appear_enemy_list[i].ID;
            }
        }
        // その階層に出現する敵を乱数で選出
        spawn_random_enemy = Random.Range(lottery_enemy.Min(), lottery_enemy.Max() + 1);
    }

    /// <summary>
    /// ダンジョンに出現させるエネミーを創る
    /// </summary>
    /// <param name="x">座標(座標のレイヤー番号を見るのに使用)</param>
    /// <param name="y">座標(座標のレイヤー番号を見るのに使用)</param>
    public void Create_Enemy(int x, int y) {
        // リストにするので１つずつインスタンスを作る
        enemy_object = new GameObject("Enemy") {
            tag = "Enemy"
        };

        // どの敵をスポーンさせるか乱数で決める
        Random_Enemy_Type();

        // 本体のスクリプト追加
        enemy_object.AddComponent<Enemy>();
        // GetComponentがかさむので１時変数に
        var enemy_script = enemy_object.GetComponent<Enemy>();

        // エネミーの動きやステータスを管理するクラスを追加
        enemy_object.AddComponent<Enemy_Controller>();
        var enemy_contoroller = enemy_object.GetComponent<Enemy_Controller>();
        enemy_contoroller.Initialize();

        // スポーンするエネミーのステータスを設定する
        enemy_object.GetComponent<Enemy_Controller>().enemy_status.Set_Parameter(enemy_object, spawn_random_enemy);
        enemy_script.My_Number = Enemy_Manager.Instance.Enemy_Counter;
        enemy_script.Set_Initialize_Position(x, y);
        // スポーン時の向きは乱数で決める
        enemy_script.direction = (eDirection)Random.Range(0.0f, (int)eDirection.Finish);
        //TODO:足元のものを取って来たい
        enemy_script.Set_Feet(Define_Value.TILE_LAYER_NUMBER);
        // 今いる部屋番号を取得
        enemy_object.GetComponent<Enemy_Controller>().enemy_status.Where_Floor(x, y);

        // 移動に必要なものを初期化
        enemy_contoroller.enemy_move.Initialize(enemy_object, Enemy_Counter);
        // 移動先を決めておく
        enemy_contoroller.enemy_move.Stack_List();

        // 敵の種類に合わせた画像を張るクラスを追加
        enemy_object.AddComponent<Enemy_Sprite_Changer>();
        // ダンジョンに出現しているエネミーを格納するものに追加
        enemies.Add(enemy_object);
        Debug.Log("enemiesの数" + enemies.Count);
        // フロア内のエネミーを数えるカウンタを増やす
        ++Enemy_Counter;
    }
}
