using UnityEngine;
using System;

/// <summary>
/// エネミーのステータス関係を管理
/// </summary>
[Serializable]
public class Enemy_Status : Actor_Status {
    /// <summary>
    /// エネミーの種類ごとに分割したステータスを格納
    /// </summary>
    [SerializeField]
    public Enemy_Status_Base my_status;

    /// <summary>
    /// jsonから読み取るエネミーステータス
    /// </summary>
    //List<Enemy_Load_Variable> enemy_base;
    /// <summary>
    /// 外部から読み込むjsonファイル
    /// </summary>
    //string enemy_file;

    /// <summary>
    /// レベル
    /// </summary>
    int level;
    public int Level { set { level = value; } get { return level; } }
    /// <summary>
    /// 最大体力
    /// </summary>
    int hit_point;
    public int Hit_Point { set { level = value; } get { return level; } }
    /// <summary>
    /// 行動力(1ターンに動ける回数)
    /// </summary>
    int activity;
    public int Activity { set { level = value; } get { return level; } }

    /// <summary>
    /// 現在いる部屋の番号
    /// </summary>
    public override int Now_Room { set { now_room = value; } get { return now_room; } }

    public void Set_Parameter(int type) {
        //TODO:ダンジョンによってレベルは変わる セット時ではまだ作られていないためひとまずは１で
        level = 1;
        // 基本は1ターンに1度の行動
        activity = 1;
        // 初期HPはもちろん最大値
        Hit_Point = Enemy_Manager.Instance.appear_enemy_type[type].Max_HP;

        my_status = Enemy_Manager.Instance.appear_enemy_type[type];
    }

    ////Jsonを使う場合はこれで使える
    ///// <summary>
    ///// 初期化 使う場合はEnemy_Controller:cs33で呼び出しを行う
    ///// </summary>
    //public void Initialize() {
    //    enemy_base = new List<Enemy_Load_Variable>();
    //    enemy_file = File.ReadAllText("Assets/Resources/Json/Enemy_Json.json");
    //    // jsonデータを敵毎に分割
    //    enemy_base = Json_Splitter.List_From_Json<Enemy_Load_Variable>(enemy_file);
    //}
    
    ///// <summary>
    ///// スポーンされたエネミーに登録しておいた種類別のステータスをセットする
    ///// </summary>
    ///// <param name="enemy_object">生成されたエネミー</param>
    ///// <param name="type">敵の種類</param>
    //public void Set_Parameter(GameObject enemy_object, int type) {
    //    level     = enemy_base[type].level;     // レベル
    //    activity  = enemy_base[type].activity;  // 行動力
    //    hit_point = enemy_base[type].hit_point; // 現在の体力
    
    //    my_status = Enemy_Manager.Instance.appear_enemy_type[type];
    //}

    /// <summary>
    /// ターンを終える
    /// </summary>
    public void End_Turn() {
        var game_manager = GameManager.Instance;

        game_manager.Set_Game_State(eGame_State.Player_Turn);
    }

    /// <summary>
    /// 死亡したかを判定する 毎ターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns>死亡していたらtrue</returns>
    public override bool Is_Dead(int now_HP) {
        if (now_HP <= 0) {
            now_HP = 0;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 自分がどこの部屋にいるのかを探す
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public override void Where_Room(int x, int y) {
        var dungeon_generator = Dungeon_Manager.Instance.dungeon_generator;
        for (int i = 0; i < dungeon_generator.division_list.Count; ++i) {
            if (x >= dungeon_generator.division_list[i].Room.Left - Define_Value.ROOM_FLAME &&
                x <= dungeon_generator.division_list[i].Room.Right &&
                y <= dungeon_generator.division_list[i].Room.Bottom &&
                y >= dungeon_generator.division_list[i].Room.Top - Define_Value.ROOM_FLAME) {
                Now_Room = i;
            }
        }
    }
}
