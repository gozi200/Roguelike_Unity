using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// キャラクターのセリフのデータを管理
/// </summary>
public class Character_Message_Data {
    /// <summary>
    /// キャラクターが喋る台詞
    /// </summary>
    public string character_speech;
    /// <summary>
    /// 喋る台詞を区切ったものを格納する(ウィンドウのページ毎)
    /// </summary>
    public List<string> character_speeches;

    /// <summary>
    /// Character_Mesasgeクラスの取得に使用
    /// </summary>
    GameObject chara_message_obj;
    /// <summary>
    /// キャラクターのセリフを管理するクラス
    /// </summary>
    Character_Message character_message;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize() {
        character_speeches = new List<string>();

        chara_message_obj = GameObject.Find("Character_Message");
        character_message = chara_message_obj.GetComponent<Character_Message>();
    }

    /// <summary>
    /// 次にしゃべるセリフをセットする
    /// </summary>
    /// <param name="speech_number">喋る台詞の番号</param>
    public void Set_Event_Talk(int message_type) {
        var reader = Game.Instance.csv_reader;
        var message_data = reader.Load_csv("csv/Message/Repeat_Character_Message", 1);

        for (int i = 0; i < message_data[message_type].Length; ++i) {
            character_speeches.Add(message_data[message_type][i]);
        }
    }

    /// <summary>
    /// ゲーム開始時のセリフを喋らせる
    /// </summary>
    public void Start_Talk() {
        var player_action = Player_Manager.Instance.player_action;
        player_action.Player_State = ePlayer_State.Non_Active;
        Set_Event_Talk((int)eMessage_Type.Start);
        character_message.talking = true;
        character_message.talk.Value = true;
    }

    /// <summary>
    /// ゲーム終了時のセリフをセットする
    /// </summary>
    void Ser_End_Talk() {

    }
}
