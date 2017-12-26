using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Status : MonoBehaviour {
     GameObject player;
    
    Player_Data player_data = new Player_Data();

    const int SERVANT_NUMBER = 10; // 登場サーヴァントの騎数

    public void Set_Parameter() {
        player = GameObject.Find("Player");
        var player_status= csv_Reader.Load_csv("csv/Actor/Player/Player_csv", 3);

        for(int i = 0; i < 2; ++i) { // TODO: マジックナンバー 10騎分のデータができ次第SERVANT_NUMBERを使う
        player_data.ID                 = int.Parse(player_status[i][0]);  // 番号
        player_data.name               = player_status[i][1];             // 名前
        player_data.class_type         = int.Parse(player_status[i][2]);  // クラス
        player_data.saint_graph        = int.Parse(player_status[i][3]);  // 再臨状態
        player_data.level              = int.Parse(player_status[i][4]);  // レベル
        player_data.hit_point          = int.Parse(player_status[i][5]);  // 体力
        player_data.max_hitpoint       = int.Parse(player_status[i][6]);  // 最大体力
        player_data.power              = int.Parse(player_status[i][7]);  // ちから(攻撃力見加味するボーナス値)
        player_data.max_power          = int.Parse(player_status[i][8]);  // 力の最大値
        player_data.activity           = int.Parse(player_status[i][9]);  // 行動力
        player_data.attack             = int.Parse(player_status[i][10]); // 攻撃力
        player_data.defence            = int.Parse(player_status[i][11]); // 防御力
        player_data.hunger_point       = int.Parse(player_status[i][12]); // はらへりポイント
        player_data.skill              = int.Parse(player_status[i][13]); // スキル(種類)
        player_data.star_generate      = int.Parse(player_status[i][14]); // スター発生率
        player_data.keep_star          = int.Parse(player_status[i][15]); // スター保持数
        player_data.command_card       = int.Parse(player_status[i][16]); // コマンドカード枚数
        player_data.arts_card          = int.Parse(player_status[i][17]); // アーツカード枚数
        player_data.arts_hit_conut     = int.Parse(player_status[i][18]); // アーツでの攻撃時のヒット回数
        player_data.quick_card         = int.Parse(player_status[i][19]); // クイックのカード枚数
        player_data.quick_hit_attack   = int.Parse(player_status[i][20]); // クイックでの攻撃時のヒット回数
        player_data.buster_card        = int.Parse(player_status[i][21]); // バスターのカード枚数
        player_data.buster_hit_count   = int.Parse(player_status[i][22]); // バスターでの攻撃時のヒット回数
        player_data.extra_attack       = int.Parse(player_status[i][23]); // エクストラアタック(種類)
        player_data.noble_weapon       = int.Parse(player_status[i][24]); // 宝具(種類)
        player_data.noble_phantasm     = int.Parse(player_status[i][25]); // 宝具を撃つためのポイント(以下NP)
        player_data.max_noble_phantasm = int.Parse(player_status[i][26]); // NPの最大値
        player_data.attack_rise_NP     = int.Parse(player_status[i][27]); // 攻撃時のNPの上昇量
        player_data.defence_rise_NP    = int.Parse(player_status[i][28]); // 被ダメージ時のNPの上昇量
        player_data.experience_point   = int.Parse(player_status[i][29]); // 経験値
        player_data.turn_count         = int.Parse(player_status[i][30]); // 経過ターンをカウント

            player.GetComponent<Player>().players.Add(player_data);
        }
    }
}