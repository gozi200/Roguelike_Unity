using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミー本体のクラス
/// </summary>
public class Enemy : MonoBehaviour {

    /// <summary>
    /// 死亡判定 死んでいたらtrue
    /// </summary>
    bool is_dead { get; set; }

    /// <summary>
    /// 自分のいる座標
    /// </summary>
    Vector2 position;

    /// <summary>
    /// 表示する画像を編集する
    /// </summary>
    SpriteRenderer sprite_renderer;
    /// <summary>
    /// 表示する画像を格納
    /// </summary>
    Sprite sprite;
    
    #region csvから読み込む変数
    /// <summary>
    /// 番号
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

    /// <summary>
    /// 種類ごとにエネミーを格納する
    /// </summary>
    [SerializeField]
    public List<Enemy> enemy_type = new List<Enemy>();

    void Start() {
        is_dead = false;

        gameObject.AddComponent<SpriteRenderer>();
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sortingOrder = Define_Value.ENEMY_LAYER_NUMBER;
        sprite = Resources.Load<Sprite>("Chip2/Wyvern");

        position = transform.position;
    }

    /// <summary>
    /// 自分の初期座標をマス情報を管理するクラスに知らせる
    /// </summary>
    /// <param name="width">スポーン座標(x座標)</param>
    /// <param name="height">スポーン座標(y座標)</param>
    public void Set_Initialize_Position(int width, int height) {
        position.x = width;
        position.y = height;
        gameObject.transform.position = position;
    }

    /// <summary>
    /// 現在のポジションを取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 Get_Position() {
        return position;
    }
}
