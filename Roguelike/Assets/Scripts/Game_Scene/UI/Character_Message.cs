using UnityEngine;
using UniRx;
using System;

/// <summary>
/// キャラクターのセリフを管理する
/// </summary>
public class Character_Message : MonoBehaviour {
    /// <summary>
    /// trueで会話を始める(メッセージウィンドゥの表示)
    /// </summary>
    public ReactiveProperty<bool> talk;
    /// <summary>
    /// 話している最中かどうか
    /// </summary>
    public bool talking = false;

    /// <summary>
    /// 喋っているキャラクター。
    /// </summary>
    [SerializeField]
    GameObject standing_character1;

    /// <summary>
    /// 立ち絵の画像を変えるクラス
    /// </summary>
    Standing_Character_Sprite_Changer stand_chara_changer;
    /// <summary>
    /// UI関係ののマネージャクラス
    /// </summary>
    [SerializeField]
    UI_Manager UI_manager;
    /// <summary>
    /// プレイヤーの行動を管理するクラス
    /// </summary>
    Player_Action player_action;
    /// <summary>
    /// キャラクターのセリフデータを扱うクラス
    /// </summary>
    static Character_Message_Data character_message_data;

    /// <summary>
    /// １区切り話し終えたか
    /// </summary>
    bool is_sentence = false;
    /// <summary>
    /// 喋ってる台詞の番号(char配列の番号)
    /// </summary>
    int word_number = 0;
    /// <summary>
    /// 区切りの番号(台詞リストの番号)
    /// </summary>
    int sentence = 0;

    void Start() {
        talk = new ReactiveProperty<bool>(false);
        stand_chara_changer = UI_Manager.Instance.standing_chara_changer;

        player_action = Player_Manager.Instance.player_action;
        character_message_data = UI_Manager.Instance.character_message_data;

        // 会話が始まったら入る
        talk.Where(flag => !!flag).Subscribe(_ => {
            // 適当な速度で文字を表示する
        IDisposable interval = Observable.Interval(TimeSpan.FromMilliseconds(35)).Subscribe(__ => {
                // 文章での区切り
                if (!is_sentence) {
                    // 最後までしゃべり切ったら終わり
                    if (sentence == character_message_data.character_speeches.Count - 1 &&
                        word_number == character_message_data.character_speeches[sentence].Length) {
                        talking = false;
                        stand_chara_changer.text_info.enabled = true;
                        return;
                    }
                    // 最後まで行ったらページ分け
                    else if (word_number == character_message_data.character_speeches[sentence].Length) {
                        is_sentence = true;
                        return;
                    }

                    // 文で区切って改行。
                    if ('。' == character_message_data.character_speeches[sentence][word_number]) {
                        Message_Window_Manager.message.Value += character_message_data.character_speeches[sentence][word_number];
                        Message_Window_Manager.message.Value += "\n";
                    }
                    // タグはよける
                    else {
                        // 文字を１文字ずつ追加して文章を作る
                        Message_Window_Manager.message.Value += character_message_data.character_speeches[sentence][word_number];
                    }

                    // 範囲外にならないように読み込む文字を進める
                    if (word_number < character_message_data.character_speeches[sentence].Length) {
                        ++word_number;
                    }

                }

                // １区切りしゃべり終えているか
                if (is_sentence) {
                    // 喋り終えてたら終了
                    if (sentence == character_message_data.character_speeches.Count - 1 &&
                        word_number == character_message_data.character_speeches[character_message_data.character_speeches.Count - 1].Length) {
                        talking = false;
                        stand_chara_changer.text_info.enabled = true;
                        return;
                    }
                }

                // 文字列を表示するスクロールビューを表示
                UI_manager.Is_Said_Scroll_View.Value = true;
                // 喋るキャラクターを表示
                standing_character1.SetActive(true);
            }).AddTo(this);
        }).AddTo(this);
    }

    void Update() {
        // キー入力でページ送り
        if (!talking && (player_action.Player_State == ePlayer_State.Non_Active)) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                // スクロールビューを非表示に
                UI_manager.Is_Said_Scroll_View.Value = false;
                //会話終了
                UI_manager.Talk = false;
                // 会話開始フラグもfalseに
                talk.Value = false;
                // プレイヤーを元の状態に(操作可能に)
                player_action.Player_State = player_action.Before_State;
                // 喋っているキャラクター(立ち絵)を非表示に
                standing_character1.SetActive(false);
                // 話し終えたら文字は消去
                Message_Window_Manager.message.Value = "";
            }
            return;
        }

        if (talking) {
            // キー入力でページ分のセリフを全部表示
            if (!is_sentence) {
                if (Input.GetKeyDown(KeyCode.Return)) {
                    is_sentence = true;
                    Message_Window_Manager.message.Value = character_message_data.character_speeches[sentence];
                    word_number = character_message_data.character_speeches[sentence].Length;
                }
                if (is_sentence) {
                    return;
                }
            }

            if (is_sentence) {
                if (Input.GetKeyDown(KeyCode.Return)) {
                    Message_Window_Manager.message.Value = "";
                    is_sentence = false;
                    word_number = 0;
                    ++sentence;

                    //TEST--------------------------------------------
                    var standing_chara = GameObject.Find("Standhing_Character");
                    var standing_chara_script = standing_chara.GetComponent<Standing_Character_Sprite_Changer>();
                    standing_chara_script.Set_Sprite(3);
                    //---------------------------------------------
                }
            }
        }
    }
};
