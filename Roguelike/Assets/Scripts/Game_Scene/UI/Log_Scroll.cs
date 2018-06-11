using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ログをスクロールする
/// </summary>
public class Log_Scroll : MonoBehaviour {
    /// <summary>
    /// ScrollViewに表示するログ
    /// </summary>
    public static ReactiveProperty<string> log = new ReactiveProperty<string>("");
    /// <summary>
    /// ScrollViewの大きさ
    /// </summary>
    ScrollRect scroll_rectangle;
    /// <summary>
    /// ScrollViewが持ってるText
    /// </summary>
    private Text text_log;

    void Start() {
        scroll_rectangle = this.gameObject.GetComponent<ScrollRect>();
        text_log = scroll_rectangle.content.GetComponentInChildren<Text>();

        log.Subscribe(log => {
            text_log.text = log;
            // Textが追加されたときに５フレーム後に自動でScrollViewの下に移動するようにする。
            StartCoroutine(Delay_Method(5, () => {
                scroll_rectangle.verticalNormalizedPosition = 0;
            }));
        });
    }

    /// <summary>
    /// プレイヤーの攻撃処理時のログ
    /// </summary>
    /// <param name="player_status">プレイヤーのステータス</param>
    /// <param name="enemy">攻撃されるエネミーオブジェクト</param>
    /// <param name="damage">実際にHPから減らされる数字</param>
    public static void Player_Attack_Log(Player_Status player_status, GameObject enemy, int damage) {
        var enemy_status = enemy.GetComponent<Enemy_Status>();

        log.Value += (player_status.name + "は" + enemy_status.name + "に" + damage + "ダメージ与えた" + "\n");
    }

    /// <summary>
    /// 経験値取得時のログ
    /// </summary>
    /// <param name="player_status">プレイヤーのステータス</param>
    /// <param name="enemy">倒された敵</param>
    /// <param name="Exp">その敵が倒されたことによって得られる経験値</param>
    public static void Get_Experience_Point(Player_Status player_status, GameObject enemy, int Exp) {
        var enemy_status = enemy.GetComponent<Enemy_Status>();

        log.Value += (enemy_status.name + "をたおした。\n");
        log.Value += (player_status.name + "は" + enemy_status.experience_point + "ポイントの経験値をえた。" + "\n");
    }

    /// <summary>
    /// 文字列のみのログに使用
    /// </summary>
    public static void Generic_Log(string set_log) {
        log.Value += (set_log + "\n");
    }

    public static void Enemy_Attack_Log() {

    }

    /// <summary>
    /// 指定したフレーム数後にActionが実行される
    /// </summary>
    /// <param name="delay_framec_ount">ディレイをかけるフレーム数</param>
    /// <param name="action"></param>
    IEnumerator Delay_Method(int delay_frame_count, Action action) {
        for (var i = 0; i < delay_frame_count; i++) {
            yield return null;
        }
        action();
    }
}
