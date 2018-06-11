using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// エネミーのステータス関係を管理するクラス
/// </summary>
[System.Serializable]
public class Enemy_Status : Actor_Status {
    /// <summary>
    /// エネミーのマネージャクラス
    /// </summary>
    Enemy_Manager enemy_manager;


    #region csvから読み込む変数
    //TODO:publicは使わない
    /// <summary>
    /// 番号 TODO:Reactiveじゃなくてもいい？
    /// </summary>
    public int ID;
    /// <summary>
    /// 名前
    /// </summary>
    public new string name;
    /// <summary>
    /// クラス
    /// </summary>
    public int class_type;
    /// <summary>
    /// レベル
    /// </summary>
    public int level;
    /// <summary>
    /// 体力
    /// </summary>
    public int hit_point;
    /// <summary>
    /// 最大体力
    /// </summary>
    public int max_hitpoint;
    /// <summary>
    /// 攻撃力
    /// </summary>
    public int attack;
    /// <summary>
    /// 防御力
    /// </summary>
    public int defence;
    /// <summary>
    /// 行動力(1ターンに動ける回数)
    /// </summary>
    public int activity;
    /// <summary>
    /// クリティカルの出やすさ
    /// </summary>
    public int critical;
    /// <summary>
    /// 倒されたときにプレイヤーに与える経験値量
    /// </summary>
    public int experience_point;
    /// <summary>
    /// スキル構成(タイプ)
    /// </summary>
    public int skill;
    /// <summary>
    /// 行動パターン
    /// </summary>
    public int AI_pattern;
    /// <summary>
    /// 出現開始階層
    /// </summary>
    public int first_floor;
    /// <summary>
    /// 出現終了階層
    /// </summary>
    public int last_floor;
    /// <summary>
    /// 出現後からの経過ターン
    /// </summary>
    public int turn_count;

    #endregion

    ///// <summary>
    ///// 自分のいる部屋番号
    ///// </summary>
    //[SerializeField]
    //int now_room;
    //public int Now_Room { set; get; }

    /// <summary>
    /// １つ前にいた座標
    /// </summary>
    int before_coordinate;
    public int Before_Coodinater { set; get; }
    /// <summary>
    /// ダンジョンに出現する敵の種類ごとにステータスを設定し、リストに格納する
    /// </summary>
    public void Create_Enemy() {
        // csv読み込みクラス
        csv_Reader reader;
        reader = Game.Instance.reader;
        enemy_manager = Enemy_Manager.Instance;
        var enemy_status = reader.Load_csv("csv/Actor/Enemy/Enemy_csv", Define_Value.UNNECESSARY_COLUMN);

        for (int enemy_type = 0; enemy_type < Define_Value.ENEMY_NUMBER; ++enemy_type) {
            var enemy = new Enemy_Status {
                ID               = int.Parse(enemy_status[enemy_type][0]),  // 番号
                name             = enemy_status[enemy_type][1],  // 名前
                class_type       = int.Parse(enemy_status[enemy_type][2]),  // クラス
                level            = int.Parse(enemy_status[enemy_type][3]),  // レベル
                hit_point        = int.Parse(enemy_status[enemy_type][4]),  // 体力
                max_hitpoint     = int.Parse(enemy_status[enemy_type][5]),  // 最大体力
                attack           = int.Parse(enemy_status[enemy_type][6]),  // 攻撃力
                defence          = int.Parse(enemy_status[enemy_type][7]),  // 防御力
                activity         = int.Parse(enemy_status[enemy_type][8]),  // 行動力
                critical         = int.Parse(enemy_status[enemy_type][9]),  // クリティカル
                experience_point = int.Parse(enemy_status[enemy_type][10]), // 経験値
                skill            = int.Parse(enemy_status[enemy_type][11]), // スキル(種類)
                AI_pattern       = int.Parse(enemy_status[enemy_type][12]), // AI
                first_floor      = int.Parse(enemy_status[enemy_type][13]), // 出開始階層
                last_floor       = int.Parse(enemy_status[enemy_type][14]), // 出現終了階層
                turn_count       = int.Parse(enemy_status[enemy_type][15]) // 経過ターンをカウント
            };

            enemy_manager.enemy_type.Add(enemy);
        }
    }

    /// <summary>
    /// スポーンされたエネミーに登録しておいた種類別のステータスをセットする
    /// </summary>
    /// <param name="enemy_object">生成されたエネミー</param>
    /// <param name="type">敵の種類</param>
    public void Set_Parameter(GameObject enemy_object, int type) {
        enemy_manager = Enemy_Manager.Instance;
        Enemy_Status enemy_status = enemy_object.GetComponent<Enemy_Controller>().enemy_status;

        enemy_status.ID               = enemy_manager.enemy_type[type].ID;               // ID
        enemy_status.name             = enemy_manager.enemy_type[type].name;             // 名前
        enemy_status.class_type       = enemy_manager.enemy_type[type].class_type;       // クラス
        enemy_status.level            = enemy_manager.enemy_type[type].level;            // レベル
        enemy_status.hit_point        = enemy_manager.enemy_type[type].hit_point;        // 体力
        enemy_status.max_hitpoint     = enemy_manager.enemy_type[type].max_hitpoint;     // 最大体力
        enemy_status.attack           = enemy_manager.enemy_type[type].attack;           // 攻撃力
        enemy_status.defence          = enemy_manager.enemy_type[type].defence;          // 防御力
        enemy_status.activity         = enemy_manager.enemy_type[type].activity;         // 行動力
        enemy_status.critical         = enemy_manager.enemy_type[type].critical;         // クリティカル発生率
        enemy_status.experience_point = enemy_manager.enemy_type[type].experience_point; // 経験値
        enemy_status.skill            = enemy_manager.enemy_type[type].skill;            // スキル
        enemy_status.AI_pattern       = enemy_manager.enemy_type[type].AI_pattern;       // 行動パターン
        enemy_status.first_floor      = enemy_manager.enemy_type[type].first_floor;      // 出現開始階層
        enemy_status.last_floor       = enemy_manager.enemy_type[type].last_floor;       // 出現終了階層
        enemy_status.turn_count       = enemy_manager.enemy_type[type].turn_count;       // 経過ターン
    }

    /// <summary>
    /// ターンを終える
    /// </summary>
    public void End_Turn() {
        var game_manager = GameManager.Instance;

        game_manager.Set_Game_State(eGame_State.Player_Turn);
    }

    /// <summary>
    /// 死亡したかを判定する 毎アクターのターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns>死亡していたらtrue</returns>
    public override bool Is_Dead(int now_HP, int index) {
        if (now_HP <= 0) {
            now_HP = 0;
            enemy_manager.Dead_Enemy(index);
        }
        return false;
    }

    /// <summary>
    /// 自分がどこの部屋にいるのかを探す
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public override void Where_Floor(int x, int y) {
        var dungeon_generator = Dungeon_Manager.Instance.dungeon_generator;
        for (int i = 0; i < dungeon_generator.division_list.Count; ++i) {
            if (x > dungeon_generator.division_list[i].Room.Left - Define_Value.ROOM_FLAME &&
                x < dungeon_generator.division_list[i].Room.Right &&
                y < dungeon_generator.division_list[i].Room.Bottom &&
                y > dungeon_generator.division_list[i].Room.Top - Define_Value.ROOM_FLAME) {
                Now_Room = i;
            }
        }
    }
}
