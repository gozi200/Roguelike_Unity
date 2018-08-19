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
    /// Character_Mesasgeクラスの取得に使用
    /// </summary>
    GameObject chara_message_obj;
    /// <summary>
    /// キャラクターのセリフを管理するクラス
    /// </summary>
    Character_Message character_message;

    //json用
    //string character_message_file;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize() {
        chara_message_obj = GameObject.Find("Character_Message");
        character_message = chara_message_obj.GetComponent<Character_Message>();
        

        //json使う場合に
        //character_message_file = File.ReadAllText("Assets/Resources/Json/Enemy_Json.json");
        //// jsonデータを敵毎に分割
        //character_messages = Json_Splitter.List_From_Json<string>(character_message_file);
    }

    /// <summary>
    /// 次にしゃべるセリフをセットする
    /// </summary>
    public void Set_Event_Talk() {
        var reader = Game.Instance.csv_reader;
        var character_message_data = reader.Load_csv("csv/Message/", 1);
        character_speech = character_message_data[0][0];
    }

    /// <summary>
    /// ゲーム開始時のセリフを喋らせる
    /// </summary>
    public void Start_Talk() {
        var reader = Game.Instance.csv_reader;
        var character_message_data = reader.Load_csv("csv/Message/Repeat_Character_Message", 1);
        var player_action = Player_Manager.Instance.player_action;
        player_action.Player_State = ePlayer_State.Non_Active;
        character_speech = character_message_data[0][0];
        character_message.talking = true;
        character_message.start_talk.Value = true;
    }

    /// <summary>
    /// ゲーム終了時のセリフをセットする
    /// </summary>
    void Ser_End_Talk() {

    }
}
