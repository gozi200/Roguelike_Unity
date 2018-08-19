using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

/// <summary>
/// エネミーのマネージャクラス
/// </summary>
public class Enemy_Manager : MonoBehaviour {
    /// <summary>
    /// エネミーオブジェクト
    /// </summary>
    GameObject enemy_object;
    /// <summary>
    /// エネミーのステータス関係のクラス
    /// </summary>
    Enemy_Status enemy_status;
    /// <summary>
    /// エネミーの行動を決めるクラス
    /// </summary>
    public Enemy_Action enemy_action;
    /// <summary>
    /// jsonから読み取ったエネミーのステータスを保持
    /// </summary>
    public Enemy_Data_Base enemy_data_base;

    /// <summary>
    /// フロア内に存在しているエネミーを格納する
    /// </summary>
    public List<GameObject> enemies;
    /// <summary>
    /// ダンジョンに出現するエネミーを種類ごとに格納
    /// </summary>
    public List<Enemy_Status_Base> appear_enemy_type;

    /// <summary>
    /// 乱数を格納。(スポーンの時に使用)
    /// </summary>
    int spawn_random_enemy;

    /// <summary>
    /// １フロアに存在している敵の数を数える
    /// </summary>
    int enemy_counter;
    public int Enemy_Counter { set { enemy_counter = value; } get { return enemy_counter; } }

    /// <summary>
    ///  自身のインスタンスを生成 //TODO:オブジェクトを作らないシングルトン生成クラスを作る
    /// </summary>
    public static Enemy_Manager Instance;
    void Awake() {
        // 作られていなかったら自身のデータを入れる
        if (Instance == null) {
            Instance = this;
        }
        // ２個以上作られないようにする
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        enemy_counter = 0;
        enemy_status = new Enemy_Status();

        // 行動決定するクラスの初期化を行う
        enemy_action = new Enemy_Action();
        enemy_action.Initialize();

        enemies = new List<GameObject>();
        appear_enemy_type = enemy_data_base.Get_Enemy_List();
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
        var enemy = enemies[index].GetComponent<Enemy>();

        // 足元のレイヤーを元に戻す
        map_layer.Tile_Swap(enemy.Position,enemy.Feet);
        // ゲームオブジェクトの破棄
        Destroy(enemies[index]);

        // 死んでしまったのリストを前詰めにする
        for(int i = index; i < enemies.Count; ++i) {
            // 範囲外を選択しないようにする
            if(i == enemies.Count - 1) {
                break;
            }
            // 要素番号に合わせるための+1
            enemies[i] = enemies[i + 1];

            // 要素番号と合わせるための+1
            enemies[i].GetComponent<Enemy>().My_Number = i + 1;
        }

        // 死んだ分は減らす
        Enemy_Counter -= 1;

        // 前詰めでケツ２つが同一になっているので、最後のリストを解放
        enemies.RemoveAt(enemies.Count - 1);
    }

    /// <summary>
    /// ダンジョンに出現する敵の中からスポーンさせるエネミーを乱数で決める
    /// </summary>
    /// <returns>スポーンさせるエネミーのID</returns>
    void Random_Enemy_Type() {
        List<Enemy_Status_Base> appear_enemy_list = appear_enemy_type;
        ReactiveProperty<int> now_floor = Dungeon_Manager.Instance.Floor;
        int[] lottery_enemy = new int[appear_enemy_list.Count];

        // 現在の階層から出現階層を調べ、満たしているものを抽出する
        for (int i = 0; i < appear_enemy_list.Count; ++i) {
            var my_status = appear_enemy_list[i];
            if (my_status.First_Floor <= now_floor.Value &&
                my_status.Last_Floor  >= now_floor.Value) {

                lottery_enemy[i] = my_status.ID;
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

        // 本体のスクリプト追加
        enemy_object.AddComponent<Enemy>();
        // エネミーの動きやステータスを管理するクラスを追加
        enemy_object.AddComponent<Enemy_Controller>();
        // 敵の種類に合わせた画像を張るクラスを追加
        enemy_object.AddComponent<Enemy_Sprite_Changer>();

        // 一時変数
        var enemy_script = enemy_object.GetComponent<Enemy>();
        var enemy_contoroller = enemy_object.GetComponent<Enemy_Controller>();
        
        enemy_contoroller.Initialize();
        // どの敵をスポーンさせるか乱数で決める
        Random_Enemy_Type();

        // スポーンするエネミーのステータスを設定する
        enemy_object.GetComponent<Enemy_Controller>().enemy_status.Set_Parameter(spawn_random_enemy);

        enemy_script.My_Number = Enemy_Counter;
        enemy_script.Set_Initialize_Position(x, y);
        // スポーン時の向きは乱数で決める
        enemy_script.Direction = (eDirection)Random.Range(0.0f, (int)eDirection.Finish);
        //TODO:足元のものを取って来たい
        enemy_script.Set_Feet(Define_Value.ROOM_LAYER_NUMBER);
        // 今いる部屋番号を取得
        enemy_object.GetComponent<Enemy_Controller>().enemy_status.Where_Room(x, y);

        // 移動に必要なものを初期化
        enemy_contoroller.enemy_move.Initialize(enemy_object, Enemy_Counter);
        // 移動先を決めておく
        enemy_contoroller.enemy_move.Stack_Route_Until_Entrance();

        // ダンジョンに出現しているエネミーを格納するものに追加
        enemies.Add(enemy_object);
        // フロア内のエネミーを数えるカウンタを増やす
        ++Enemy_Counter;
    }
}
