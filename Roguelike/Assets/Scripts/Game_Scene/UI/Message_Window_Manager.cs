using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ログの表示を管理
/// </summary>
public class Message_Window_Manager : MonoBehaviour {
    /// <summary>
    /// UIのマネージャオブジェクト
    /// </summary>
    GameObject UI_manager_obj;
    /// <summary>
    /// UIのマネージャクラス
    /// </summary>
    static UI_Manager UI_manager;

    /// <summary>
    /// ScrollViewに表示するメッセージ
    /// </summary>
    public static ReactiveProperty<string> message;

    /// <summary>
    /// ScrollViewの大きさ
    /// </summary>
    ScrollRect scroll_rectangle;

    /// <summary>
    /// ScrollViewが持ってるText
    /// </summary>
    Text text_message;

    void Awake() {
        UI_manager_obj = GameObject.Find("UI_Manager");
        UI_manager = UI_manager_obj.GetComponent<UI_Manager>();

        scroll_rectangle = this.gameObject.GetComponent<ScrollRect>();
        text_message = scroll_rectangle.content.GetComponentInChildren<Text>();

        message = new ReactiveProperty<string>();
        // ログへの書き込みがあったら表示する
        message.Subscribe(message => {
            text_message.text = message;
        });
    }

    /// <summary>
    /// プレイヤーが攻撃時のダメージログ
    /// </summary>
    /// <param name="player_status">プレイヤーのステータス</param>
    /// <param name="enemy">攻撃されるエネミーオブジェクト</param>
    /// <param name="damage">実際にHPから減らされる数字</param>
    public static void Player_Attack_Log(Player_Status player_status, GameObject enemy, int damage) {
        var target_enemy = enemy.GetComponent<Enemy_Controller>().enemy_status.my_status;

        message.Value += (player_status.Name + "は" + target_enemy.Name + "に" + damage + "のダメージをあたえた。\n");
        UI_manager.Is_Said_Scroll_View.Value = true;
    }

    /// <summary>
    /// エネミーが攻撃時のダメージログ
    /// </summary>
    /// <param name="enemy_status">攻撃するエネミのステータス</param>
    /// <param name="player_status">攻撃を受けるプレイヤーのステータス</param>
    /// <param name="damage">実際にHPから減らされる数字</param>
    public static void Enemy_Attack_Log(Enemy_Status_Base enemy_status, Player_Status player_status, int damage) {
        message.Value += (enemy_status.Name + "は" + player_status.Name + "に" + damage + "のダメージをあたえた。\n");
        UI_manager.Is_Said_Scroll_View.Value = true;
    }

    /// <summary>
    /// 経験値取得時のログ
    /// </summary>
    /// <param name="player_status">プレイヤーのステータス</param>
    /// <param name="enemy">倒された敵</param>
    /// <param name="Exp">その敵が倒されたことによって得られる経験値</param>
    public static void Get_Experience_Point(Player_Status player_status, GameObject enemy, int Exp) {
        var target_enemy = enemy.GetComponent<Enemy_Controller>().enemy_status.my_status;

        message.Value += (target_enemy.Name  + "をたおした。\n");
        message.Value += (player_status.Name + "は" + Exp + "ポイントの経験値をえた。\n");

        // 強制的にストリームを流す
        UI_manager.Is_Said_Scroll_View.SetValueAndForceNotify(true);
    }

    /// <summary>
    /// 現在の満腹値によってログを出す
    /// </summary>
    /// <param name="hunger_point">今の万プフ値</param>
    public static void Hunger_Log(int hunger_point) {
        var player_status = Player_Manager.Instance.player_status;
       
        // 一定量を下回ったらログで空腹を知らせる
        if (hunger_point == 50) {
            Generic_Log("おなかがへってきた");
        }
        if (hunger_point == 20) {
            Generic_Log("おなかがへりすぎて目が回ってきた");
        }
        // 2になったらログで知らせる
        if (hunger_point == 2) {
           Generic_Log("ダメだ、もうたおれそうだ！");
        }
        // 1になったらログで知らせる
        if (hunger_point == 1) {
            Generic_Log("なにかたべないと！");
        }
    }

    /// <summary>
    /// 文字列のみのログに使用
    /// </summary>
    /// <param name="set_message">表示する文字列</param>
    public static void Generic_Log(string set_message) {
        message.Value += (set_message + "\n");
        UI_manager.Is_Said_Scroll_View.Value = true;
    }
}
