using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// プレイヤー本体のクラス
/// </summary>
public class Player : MonoBehaviour {
    /// <summary>
    /// 自分自身のインスタンス
    /// </summary>
    Player player;
    /// <summary>
    /// プレイヤーのステータス関係を管理するクラス
    /// </summary>
    public Player_Status status;
    /// <summary>
    /// 自分のモードを認識する
    /// </summary>
    public ePlayer_Mode mode;
    /// <summary>
    /// 自分の状態を認識する
    /// </summary>
    public ePlayer_State state;
    /// <summary>
    /// 自分の向いている方向を認識する
    /// </summary>
    public eDirection direction;
    /// <summary>
    /// ２次元配列上の移動距離
    /// </summary>
    public int move_value { get; set; }
    /// <summary>
    /// 死亡フラグ 死んでいたらtrue
    /// </summary>
    public bool is_dead { get; set; }
    /// <summary>
    /// 現在の座標の足元にあるもの
    /// </summary>
    public int feet { get; set; }

    /// <summary>
    /// 自分のいる座標
    /// </summary>
    public Vector2 position;

    #region 変数(csvからの読み込み)
    /// <summary>
    /// 番号
    /// </summary>
    public int ID;
    /// <summary>
    /// 名前
    /// </summary>
    public new string name;
    /// クラス
    /// </summary>
    public int class_type;
    /// <summary>
    /// 再臨状態
    /// </summary>
    public int saint_graph;
    /// <summary>
    /// レベル
    /// </summary>
    public int level;
    /// <summary>
    /// 現在の体力
    /// </summary>
    public int hit_point;
    /// <summary>
    /// 体力の最大値
    /// </summary>
    public int max_hit_point;
    /// <summary>
    /// 現在の力
    /// </summary>
    public int power;
    /// <summary>
    /// ちからの最大値
    /// </summary>
    public int max_power;
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
    public int hunger_point;
    /// <summary>
    /// スキル種類
    /// </summary>
    public int skill;
    /// <summary>
    /// スター発生率
    /// </summary>
    public int star_generate;
    /// <summary>
    /// スター保持数
    /// </summary>
    public int keep_star;
    /// <summary>
    /// コマンドカード種類
    /// </summary>
    public int command_card;
    /// <summary>
    /// アーツカード枚数
    /// </summary>
    public int arts_card;
    /// <summary>
    /// アーツカードでのヒット回数
    /// </summary>
    public int arts_hit_conut;
    /// <summary>
    /// クイックカード
    /// </summary>
    public int quick_card;
    /// <summary>
    /// クイックカードでの攻撃ヒット回数
    /// </summary>
    public int quick_hit_attack;
    /// <summary>
    /// バスターカード
    /// </summary>
    public int buster_card;
    /// <summary>
    /// バスターカードで攻撃ヒット回数
    /// </summary>
    public int buster_hit_count;
    /// <summary>
    /// 宝具
    /// </summary>
    public int noble_weapon;
    /// <summary>
    /// エクストラアタック
    /// </summary>
    public int extra_attack;
    /// <summary>
    /// 宝具を撃つためのポイント
    /// </summary>
    public int noble_phantasm;
    /// <summary>
    /// NPの最大数
    /// </summary>
    public int max_noble_phantasm;
    /// <summary>
    /// 攻撃時のNP上昇率
    /// </summary>
    public int attack_rise_NP;
    /// <summary>
    /// 被ダメージ時のNP上昇率
    /// </summary>
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

    void Awake() {
        status = new Player_Status();
        player = gameObject.GetComponent<Player>();
    }

    void Start() {
        is_dead = false;

        gameObject.transform.localScale = new Vector2(0.4f, 0.4f);
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
    public void Set_Position(Vector2 new_position) {
        gameObject.transform.position = new_position;
    }

    /// <summary>
    /// 現在の座標を取得
    /// </summary>
    /// <returns></returns>
    public Vector3 Get_Position() {
        return position;
    }

    /// <summary>
    /// 足元にあるものを設定する
    /// </summary>
    /// <param name="layer_number">今のいる場所のレイヤー番号</param>
    public void Set_Feet(int layer_number) {
        feet = layer_number;
    }
}
