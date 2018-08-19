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
    public ReactiveProperty<bool> start_talk;
    /// <summary>
    /// 話している最中かどうか
    /// </summary>
    public bool talking = false;

    /// <summary>
    /// 喋っているキャラクター。1人しかしゃべらないときはこちらを使う。
    /// </summary>
    [SerializeField]
    GameObject Standing_Caracter1;
    /// <summary>
    /// 喋っているキャラクター
    /// </summary>
    GameObject Standing_Caracter2;

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
    /// キャラクターの立ち絵
    /// </summary>
    GameObject stand_chara;

    Standing_Character_Sprite_Changer stand_chara_changer;

    void Start() {
        start_talk = new ReactiveProperty<bool>(false);
        stand_chara = UI_Manager.Instance.standing_chara_changer.parent;
        stand_chara_changer = UI_Manager.Instance.standing_chara_changer;

        player_action = Player_Manager.Instance.player_action;
        character_message_data = UI_Manager.Instance.character_message_data;

        var word_number = 0;
        start_talk.Where(flag => !!flag/* && talking*/).Subscribe(_ => {
            // 適当な速度で文字を表示する
            Observable.Interval(TimeSpan.FromMilliseconds(35)).Subscribe(flag => {
                // 規定のタグを見つけたら終わり
                if ('N' == character_message_data.character_speech[word_number]) {
                    talking = false;
                    stand_chara_changer.text_info.enabled = true;
                    return;
                }

                // 文字を１文字ずつ追加して文章を作る
                Message_Window_Manager.message.Value += character_message_data.character_speech[word_number];
                // 文字列を表示するスクロールビューを表示
                UI_manager.Is_Said_Scroll_View.Value = true;
                // 喋るキャラクターを表示
                Standing_Caracter1.SetActive(true);

                //TODO: 文で区切って改行したい。タグ文字はEで
                if ('E' == character_message_data.character_speech[word_number]) {

                }

                // 範囲外にならないように読み込む文字を進める
                if (word_number < character_message_data.character_speech.Length) {
                    ++word_number;
                }
            });
        }).AddTo(this);
    }

    void Update() {
        if (!talking) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                // スクロールビューを非表示に
                UI_manager.Is_Said_Scroll_View.Value = false;
                // プレイヤーを元の状態に(操作可能に)
                player_action.Player_State = player_action.Before_State;
                // 喋っているキャラクター(立ち絵)を非表示に
                stand_chara.SetActive(false);
            }
        }
    }
};
