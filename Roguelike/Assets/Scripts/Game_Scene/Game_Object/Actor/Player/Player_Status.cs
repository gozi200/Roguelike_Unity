using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Status : Actor_Status {
    #region 変数(csvからの読み込み)
    //TODO:publicは使わない
    /// <summary>
    /// 番号
    /// </summary>
    [HideInInspector]
    public int ID;
    /// <summary>
    /// 名前
    /// </summary>
    public new string name;
    /// クラス
    /// </summary>
    [HideInInspector]
    public int class_type;
    /// <summary>
    /// 再臨状態
    /// </summary>
    public int saint_graph;
    /// <summary>
    /// レベル
    /// </summary>
    public ReactiveProperty<int> level;
    /// <summary>
    /// 現在の体力
    /// </summary>
    public ReactiveProperty<int> hit_point;
    /// <summary>
    /// 体力の最大値
    /// </summary>
    public ReactiveProperty<int> max_hit_point;
    /// <summary>
    /// 現在の力
    /// </summary>
    public ReactiveProperty<int> power;
    /// <summary>
    /// ちからの最大値
    /// </summary>
    public ReactiveProperty<int> max_power;
    /// 行動力(行動解数)
    /// </summary>
    public int activity;
    /// <summary>
    /// 攻撃力
    /// </summary>
    public int attack;
    /// <summary>
    /// 防御力
    /// </summary>
    public int defence;
    /// <summary>
    /// はらへりポイント
    /// </summary>
    public ReactiveProperty<int> hunger_point;
    /// <summary>
    /// スキル種類
    /// </summary>
    [HideInInspector]
    public int skill;
    /// <summary>
    /// スター発生率
    /// </summary>
    [HideInInspector]
    public int star_generate;
    /// <summary>
    /// スター保持数
    /// </summary>
    public ReactiveProperty<int> keep_star;
    /// <summary>
    /// コマンドカード種類
    /// </summary>
    [HideInInspector]
    public int command_card;
    /// <summary>
    /// アーツカード枚数
    /// </summary>
    public int arts_card;
    /// <summary>
    /// アーツカードでのヒット回数
    /// </summary>
    [HideInInspector]
    public int arts_hit_conut;
    /// <summary>
    /// クイックカード
    /// </summary>
    public int quick_card;
    /// <summary>
    /// クイックカードでの攻撃ヒット回数
    /// </summary>
    [HideInInspector]
    public int quick_hit_attack;
    /// <summary>
    /// バスターカード
    /// </summary>
    public int buster_card;
    /// <summary>
    /// バスターカードで攻撃ヒット回数
    /// </summary>
    [HideInInspector]
    public int buster_hit_count;
    /// <summary>
    /// 宝具
    /// </summary>
    [HideInInspector]
    public int noble_weapon;
    /// <summary>
    /// エクストラアタック
    /// </summary>
    [HideInInspector]
    public int extra_attack;
    /// <summary>
    /// 宝具を撃つためのポイント
    /// </summary>
    public ReactiveProperty<int> noble_phantasm;
    /// <summary>
    /// NPの最大数
    /// </summary>
    [HideInInspector]
    public int max_noble_phantasm;
    /// <summary>
    /// 攻撃時のNP上昇率
    /// </summary>
    [HideInInspector]
    public int attack_rise_NP;
    /// <summary>
    /// 被ダメージ時のNP上昇率
    /// </summary>
    [HideInInspector]
    public int defence_rise_NP;
    /// <summary>
    /// 取得経験値量
    /// </summary>
    public int experience_point;
    /// <summary>
    /// 経過ターンをカウント
    /// </summary>
    public int turn_count;

    #endregion
    /// <summary>
    /// レベルアップに必要な経験値量を格納する配列
    /// </summary>
    public int[] exp_data_base = new int[25] {
           5 // 1 から次のレベルに必要な経験値(仮)
	,     10 // 2 以下略
	,     20 // 3
	,     40 // 4
	,     80 // 5
	,    160 // 6
	,    320 // 7
	,    640 // 8
	,   1300 // 9
	,   2600 // 10
	,   5200 // 11
	,  10000 // 12
	,  20000 // 13
	,  39000 // 14
	,  68000 // 15
	, 100000 // 16
	, 200000 // 17
	, 400000 // 18
	, 800000 // 19
	,1600000 // 20
	,2500000 // 21
	,4600000 // 22
	,6700000 // 23
	,8000000 // 24 ひとまずはここまで
	,Define_Value.MAX_EXP // 25
        };

    // DEBUG--------------------------------
    Key_Observer key;
    //------------------------------------------

    public void Initialize() {
        level = new ReactiveProperty<int>();
        hit_point = new ReactiveProperty<int>();
        max_hit_point = new ReactiveProperty<int>();
        power = new ReactiveProperty<int>();
        max_power = new ReactiveProperty<int>();
        hunger_point = new ReactiveProperty<int>();
        keep_star = new ReactiveProperty<int>();
        noble_phantasm = new ReactiveProperty<int>();

        // DEBUG------------------------------------
        key = Game.Instance.key_observer;
        key.On_Key_Down_AsObservable()
            .Where(key => key == KeyCode.Backspace)
            .Subscribe(_ =>
             hit_point.Value = 10000);
        //------------------------------------------
    }

    /// <summary>
    /// プレイヤーのステータスを設定する。 ゲーム開始時とキャラクターチェンジの時に呼ばれる
    /// </summary>
    /// <param name="use_chara">使用するキャラクターの番号</param>
    public void Set_Parameter(int use_chara) {
        var reader = Game.Instance.reader;
        var player_status = reader.Load_csv("csv/Actor/Player/Player_csv", Define_Value.UNNECESSARY_COLUMN);

        ID                   = int.Parse(player_status[use_chara][0]);  // 番号
        name                 = player_status          [use_chara][1];   // 名前
        class_type           = int.Parse(player_status[use_chara][2]);  // クラス
        saint_graph          = int.Parse(player_status[use_chara][3]);  // 再臨状態
        level.Value          = int.Parse(player_status[use_chara][4]);  // レベル
        hit_point.Value      = int.Parse(player_status[use_chara][5]);  // 体力
        max_hit_point.Value  = int.Parse(player_status[use_chara][6]);  // 最大体力
        power.Value          = int.Parse(player_status[use_chara][7]);  // ちから(攻撃力に加味するボーナス値)
        max_power.Value      = int.Parse(player_status[use_chara][8]);  // 力の最大値
        activity             = int.Parse(player_status[use_chara][9]);  // 行動力
        attack               = int.Parse(player_status[use_chara][10]); // 攻撃力
        defence              = int.Parse(player_status[use_chara][11]); // 防御力
        hunger_point.Value   = int.Parse(player_status[use_chara][12]); // はらへりポイント
        skill                = int.Parse(player_status[use_chara][13]); // スキル(種類)
        star_generate        = int.Parse(player_status[use_chara][14]); // スター発生率
        keep_star.Value      = int.Parse(player_status[use_chara][15]); // スター保持数
        command_card         = int.Parse(player_status[use_chara][16]); // コマンドカード枚数
        arts_card            = int.Parse(player_status[use_chara][17]); // アーツカード枚数
        arts_hit_conut       = int.Parse(player_status[use_chara][18]); // アーツでの攻撃時のヒット回数
        quick_card           = int.Parse(player_status[use_chara][19]); // クイックのカード枚数
        quick_hit_attack     = int.Parse(player_status[use_chara][20]); // クイックでの攻撃時のヒット回数
        buster_card          = int.Parse(player_status[use_chara][21]); // バスターのカード枚数
        buster_hit_count     = int.Parse(player_status[use_chara][22]); // バスターでの攻撃時のヒット回数
        noble_weapon         = int.Parse(player_status[use_chara][24]); // 宝具(種類)
        extra_attack         = int.Parse(player_status[use_chara][23]); // エクストラアタック(種類)
        noble_phantasm.Value = int.Parse(player_status[use_chara][25]); // 宝具を撃つためのポイント(以下NP)
        max_noble_phantasm   = int.Parse(player_status[use_chara][26]); // NPの最大値
        attack_rise_NP       = int.Parse(player_status[use_chara][27]); // 攻撃時のNPの上昇量
        defence_rise_NP      = int.Parse(player_status[use_chara][28]); // 被ダメージ時のNPの上昇量
        experience_point     = int.Parse(player_status[use_chara][29]); // 経験値
        turn_count           = int.Parse(player_status[use_chara][30]); // 経過ターンをカウント
    }

    /// <summary>
    /// Playerのターン経過の処理(自動回復、はらへり) プレイヤーの行動が終了したときに呼ばれる
    /// </summary>
    public void Add_Turn() {
        ++turn_count;
        var game_manager = GameManager.Instance;

        //TODO:ひとまず
        if(hit_point.Value < max_hit_point.Value &&
           hunger_point.Value > 0) {
            ++hit_point.Value;
        }

        // 空腹であればHPを1減らす
        if (hunger_point.Value <= 0) {
            --hit_point.Value;
        }

        // 3ターンに1度空腹ポイントを1減らす
        else if (0 == turn_count % 3) {
            --hunger_point.Value;

            // 一定量を下回ったらログで空腹を知らせる
            if(hunger_point.Value == 50) {
                Log_Scroll.Generic_Log("おなかがへってきた");
            }
            if (hunger_point.Value == 20) {
                Log_Scroll.Generic_Log("おなかがへりすぎて目が回ってきた");
            }
            // 2になったらログで知らせる
            if(hunger_point.Value == 2) {
                Log_Scroll.Generic_Log("ダメだ、もうたおれそうだ！");
            }
            // 1になったらログで知らせる
            if(hunger_point.Value == 1) {
                Log_Scroll.Generic_Log("なにかたべないと！");
            }
        }

        if (Enemy_Manager.Instance.enemies.Count > 0) {
            game_manager.Set_Game_State(eGame_State.Enemy_Trun);
        }
        else if (Dungeon_Manager.Instance.Is_Exist) {
            game_manager.Set_Game_State(eGame_State.Dungeon_Turn);
        }
        //TODO:ほんとはパートナーの番に
        else {
            //TODO:パートナーターン
        }
    }

/// <summary>
/// 経験値を加算 敵を倒したときに呼ばれる プレイヤー、パートナーどちらが倒しても呼ばれる
/// </summary>
/// <param name="exp">加算する経験値量</param>
public void Add_Experience_Point(int exp) {
        experience_point += exp;

        // 上限を越しても設定した最大値を超えないようにする
        if (experience_point > Define_Value.MAX_EXP) {
            experience_point = Define_Value.MAX_EXP;
        }

        // レベルアップに必要な経験値量を超えたか確かめる 
        // 要素数に合わせるために-1
        if (exp_data_base[level.Value - 1] <= experience_point) {
            int new_Lv = GetExp_Level();

            // 一度に２レベル以上あがる場合にも対応
            for (; level.Value < new_Lv; ++level.Value) {
                int add_hp;
                int add_atk;
                int add_def;

                // 体力の最大値をランダム(3~5)で増やす
                add_hp = Random.Range(0, 2) + 3;

                max_hit_point.Value += add_hp;

                // TODO: 攻撃力,防御力の最大値を仕様書を参考に増やす
            }
        }
    }

    /// <summary>
    /// 経験値量からレベルを算出
    /// </summary>
    /// <returns>実際に上がった後のレベル</returns>
    int GetExp_Level() {
        int level;

        for (level = 0; level < Define_Value.MAX_LV; ++level) {
            if (exp_data_base[level] > experience_point) {
                break;
            }
        }
        return level + 1;
    }

    /// <summary>
    /// 死亡したかを判定する 毎アクターのターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns>死亡していたらtrue</returns>
    public override bool Is_Dead(int now_HP) {
        if (now_HP <= 0) {
            var player_status = Player_Manager.Instance.player_status;
            player_status.hit_point.Value = 0;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 自分がどこの部屋にいるのかを探す 
    /// </summary>
    /// <param name="x">自分のx座標</param>
    /// <param name="y">自分のy座標</param>
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
