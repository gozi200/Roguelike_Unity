using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// プレイヤー本体のクラス
/// </summary>
public class Player : Unique_Component<Player> {
    /// <summary>
    /// プレイヤーのスピード(移動量)
    /// </summary>
    public Vector2 move_value = new Vector2(5f, 5f);
    /// <summary>
    /// ２次元配列上の移動距離
    /// </summary>
    public int move_value_on_array;
    /// <summary>
    /// 死亡フラグ 死んでいたらtrue
    /// </summary>
    public bool is_dead;
    /// <summary>
    /// 使用中のキャラクター
    /// </summary>
    int using_charactor;
    /// <summary>
    /// 現在の座標の足元に何があるもの
    /// </summary>
    int feet;

    public ePlayer_Mode mode;

    public ePlayer_State state;

    public eDirection direction;

    public Player player;
    public Player_Move move;
    public Player_Status status;

    SpriteRenderer sprite_renderer;

    Sprite sprite;

    public Vector3 position;

    #region 変数(csvからの読み込み)
    /// <summary>
    /// 番号
    /// </summary>
    [System.NonSerialized]
    public int ID;
    /// <summary>
    /// 名前
    /// </summary>
    [System.NonSerialized]
    public string name;
    /// クラス
    /// </summary>
    [System.NonSerialized]
    public int class_type;
    /// <summary>
    /// 再臨状態
    /// </summary>
    [System.NonSerialized]
    public int saint_graph;
    /// <summary>
    /// レベル
    /// </summary>
    [System.NonSerialized]
    public int level;
    /// <summary>
    /// 現在の体力
    /// </summary>
    [System.NonSerialized]
    public int hit_point;
    /// <summary>
    /// 体力の最大値
    /// </summary>
    [System.NonSerialized]
    public int max_hit_point;
    /// <summary>
    /// 現在の力
    /// </summary>
    [System.NonSerialized]
    public int power;
    /// <summary>
    /// ちからの最大値
    /// </summary>
    [System.NonSerialized]
    public int max_power;
    /// 行動力(行動解数)
    /// </summary>
    [System.NonSerialized]
    public int activity;
    /// <summary>
    /// 攻撃力
    /// </summary>
    [System.NonSerialized]
    public int attack;
    /// <summary>
    /// 防御力
    /// </summary>
    [System.NonSerialized]
    public int defence;
    /// <summary>
    /// はらへりポイント
    /// </summary>
    [System.NonSerialized]
    public int hunger_point;
    /// <summary>
    /// スキル種類
    /// </summary>
    [System.NonSerialized]
    public int skill;
    /// <summary>
    /// スター発生率
    /// </summary>
    [System.NonSerialized]
    public int star_generate;
    /// <summary>
    /// スター保持数
    /// </summary>
    [System.NonSerialized]
    public int keep_star;
    /// <summary>
    /// コマンドカード種類
    /// </summary>
    [System.NonSerialized]
    public int command_card;
    /// <summary>
    /// アーツカード枚数
    /// </summary>
    [System.NonSerialized]
    public int arts_card;
    /// <summary>
    /// アーツカードでのヒット回数
    /// </summary>
    [System.NonSerialized]
    public int arts_hit_conut;
    /// <summary>
    /// クイックカード
    /// </summary>
    [System.NonSerialized]
    public int quick_card;
    /// <summary>
    /// クイックカードでの攻撃ヒット回数
    /// </summary>
    [System.NonSerialized]
    public int quick_hit_attack;
    /// <summary>
    /// バスターカード
    /// </summary>
    [System.NonSerialized]
    public int buster_card;
    /// <summary>
    /// バスターカードで攻撃ヒット回数
    /// </summary>
    [System.NonSerialized]
    public int buster_hit_count;
    /// <summary>
    /// 宝具
    /// </summary>
    [System.NonSerialized]
    public int noble_weapon;
    /// <summary>
    /// エクストラアタック
    /// </summary>
    [System.NonSerialized]
    public int extra_attack;
    /// <summary>
    /// 宝具を撃つためのポイント
    /// </summary>
    [System.NonSerialized]
    public int noble_phantasm;
    /// <summary>
    /// NPの最大数
    /// </summary>
    [System.NonSerialized]
    public int max_noble_phantasm;
    /// <summary>
    /// 攻撃時のNP上昇率
    /// </summary>
    [System.NonSerialized]
    public int attack_rise_NP;
    /// <summary>
    /// 被ダメージ時のNP上昇率
    /// </summary>
    [System.NonSerialized]
    public int defence_rise_NP;
    /// <summary>
    /// 取得経験値量
    /// </summary>
    [System.NonSerialized]
    public int experience_point;
    /// <summary>
    /// 経過ターンをカウント
    /// </summary>
    [System.NonSerialized]
    public int turn_count;

    #endregion

    void Awake() {
        status = new Player_Status();
        player = gameObject.GetComponent<Player>();
    }

    void Start() {
        move_value_on_array = 1;

        feet = 0;

        using_charactor = Define_Value.OKITA;

        move = gameObject.AddComponent<Player_Move>();
        gameObject.AddComponent<SpriteRenderer>();
        gameObject.AddComponent<Player_Action>();

        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        //TODO: ここがNULL
        sprite = Resources.Load<Sprite>("Resources/Chip2/OKITA");

        is_dead = false;

        move_value.x = 5;
        move_value.y = 5;

        position = gameObject.transform.position;
    }

    /// <summary>
    /// TODO: テスト用
    /// </summary>
    private void OnGUI() {
        sprite_renderer.sprite = sprite;
    }

    /// <summary>
    /// 自分の初期座標を設定
    /// </summary>
    /// <param name="width">スポーン座標(x座標)</param>
    /// <param name="height">スポーン座標(y座標)</param>
    public void Set_Initialize_Position(int width, int height) {
        position.x = width;
        position.y = height;
        gameObject.transform.position = position;
    }

    /// <summary>
    /// 変更したポジションをtransform.positionに合わせる
    /// </summary>
    /// <param name="new_position">変更後の座標</param>
    public void Set_Position(Vector3 new_position) {
        gameObject.transform.position = new_position;
    }

    /// <summary>
    /// 足元にある地形を返す
    /// </summary>
    /// <returns>足元にあるもの(レイヤナンバー)</returns>
    public int Get_Feet() {
        return feet;
    }
    /// <summary>
    /// 足元にあるものを設定する
    /// </summary>
    /// <param name="layer_number"></param>
    public void Set_Feet(int layer_number) {
        feet = layer_number;
    }
}

/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Status {
    /// <summary>
    /// プレイヤークラス
    /// </summary>
    Player player;

    /// <summary>
    /// レベルの最大値
    /// </summary>
    const int MAX_LV = 999;
    /// <summary>
    /// 経験値の最大値(仮)
    /// </summary>
    const int MAX_EXP = 999999999;
    /// <summary>
    /// プレイアブルキャラクターの数
    /// </summary>
    const int SERVANT_NUMBER = 10;
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
	,MAX_EXP // 25
        };

    /// <summary>
    /// プレイヤーのステータスを設定する。 スポーン時とキャラクターチェンジの時に呼ばれる
    /// </summary>
    /// <param name="use_chara">使用するキャラクターの番号</param>
    public void Set_Parameter(int use_chara) {
        player = Player.Instance.player;
        var player_status = csv_Reader.Load_csv("csv/Actor/Player/Player_csv", 3);
        player.ID                 = int.Parse(player_status[use_chara][0]);  // 番号
        player.name               = player_status[use_chara][1];   // 名前
        player.class_type         = int.Parse(player_status[use_chara][2]);  // クラス
        player.saint_graph        = int.Parse(player_status[use_chara][3]);  // 再臨状態
        player.level              = int.Parse(player_status[use_chara][4]);  // レベル
        player.hit_point          = int.Parse(player_status[use_chara][5]);  // 体力
        player.max_hit_point      = int.Parse(player_status[use_chara][6]);  // 最大体力
        player.power              = int.Parse(player_status[use_chara][7]);  // ちから(攻撃力見加味するボーナス値)
        player.max_power          = int.Parse(player_status[use_chara][8]);  // 力の最大値
        player.activity           = int.Parse(player_status[use_chara][9]);  // 行動力
        player.attack             = int.Parse(player_status[use_chara][10]); // 攻撃力
        player.defence            = int.Parse(player_status[use_chara][11]); // 防御力
        player.hunger_point       = int.Parse(player_status[use_chara][12]); // はらへりポイント
        player.skill              = int.Parse(player_status[use_chara][13]); // スキル(種類)
        player.star_generate      = int.Parse(player_status[use_chara][14]); // スター発生率
        player.keep_star          = int.Parse(player_status[use_chara][15]); // スター保持数
        player.command_card       = int.Parse(player_status[use_chara][16]); // コマンドカード枚数
        player.arts_card          = int.Parse(player_status[use_chara][17]); // アーツカード枚数
        player.arts_hit_conut     = int.Parse(player_status[use_chara][18]); // アーツでの攻撃時のヒット回数
        player.quick_card         = int.Parse(player_status[use_chara][19]); // クイックのカード枚数
        player.quick_hit_attack   = int.Parse(player_status[use_chara][20]); // クイックでの攻撃時のヒット回数
        player.buster_card        = int.Parse(player_status[use_chara][21]); // バスターのカード枚数
        player.buster_hit_count   = int.Parse(player_status[use_chara][22]); // バスターでの攻撃時のヒット回数
        player.noble_weapon       = int.Parse(player_status[use_chara][24]); // 宝具(種類)
        player.extra_attack       = int.Parse(player_status[use_chara][23]); // エクストラアタック(種類)
        player.noble_phantasm     = int.Parse(player_status[use_chara][25]); // 宝具を撃つためのポイント(以下NP)
        player.max_noble_phantasm = int.Parse(player_status[use_chara][26]); // NPの最大値
        player.attack_rise_NP     = int.Parse(player_status[use_chara][27]); // 攻撃時のNPの上昇量
        player.defence_rise_NP    = int.Parse(player_status[use_chara][28]); // 被ダメージ時のNPの上昇量
        player.experience_point   = int.Parse(player_status[use_chara][29]); // 経験値
        player.turn_count         = int.Parse(player_status[use_chara][30]); // 経過ターンをカウント
    }

    /// <summary>
    /// Playerのターン経過の処理(自動回復、はらへり) プレイヤーの行動が終了したときに呼ばれる
    /// </summary>
    public void Turn() {
        ++player.turn_count;

        // 空腹であるか
        if (player.hunger_point <= 0) {
            --player.hit_point;

            //if (Is_Dead()) {
            // ログに"餓死したと流す"
            //}
        }
        // 3ターンに一度空腹ポイントを１減らす
        else if (0 == player.turn_count % 3) {
            --player.hunger_point;
            // 満腹値20で"お腹がすいてきた"     のログを表示
            // 満腹度 0で"お腹がすいて死にそうだ"のログを表示
        }
        else {
            // TODO: 体力の自動回復
        }
    }

    /// <summary>
    /// 経験値を加算 敵を倒したときに呼ばれる プレイヤー、パートナーどちらが倒しても呼ばれる
    /// </summary>
    /// <param name="exp">加算する経験値量</param>
    void Add_Experience_Point(int exp) {
        player.experience_point += exp;

        // 上限を越しても設定した最大値を超えないようにする
        if (player.experience_point > MAX_EXP) {
            player.experience_point = MAX_EXP;
        }

        // レベルアップに必要な経験値量を超えたか確かめる
        // TODO: - 1はいらない？ 要テスト
        if (exp_data_base[player.level - 1] <= player.experience_point) {
            int new_Lv = Get_Exp_Level();

            // 一度に２レベル以上あがる場合にも対応
            for (; player.level < new_Lv; ++player.level) {
                int add_hp;
                int add_atk;
                int add_def;

                // 体力の最大値をランダム(3~5)で増やす
                add_hp = Random.Range(0, 2) + 3;

                player.max_hit_point += add_hp;

                // TODO: 攻撃力,防御力の最大値を仕様書を参考に増やす
            }
        }
    }

    /// <summary>
    /// 経験値量からレベルを算出
    /// </summary>
    /// <returns>実際に上がった後のレベル</returns>
    int Get_Exp_Level() {
        int lv;

        for (lv = 0; lv < MAX_LV; ++lv) {
            if (exp_data_base[lv] > player.experience_point) {
                break;
            }
        }
        return lv + 1;
    }

    /// <summary>
    /// 死亡したかを判定する 毎アクターのターンの終わりに確認する
    /// </summary>
    /// <param name="now_HP">現在の体力</param>
    /// <returns>死亡していたらtrue</returns>
    public bool Is_Dead(int now_HP) {
        if (now_HP <= 0) {
            now_HP = 0;
            return true;
        }
        return false;
    }
}