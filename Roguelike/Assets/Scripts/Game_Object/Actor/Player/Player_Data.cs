using UnityEngine;
using System.Collections;
using System.Linq;



/// <summary>
/// プレイヤーのステータスを設定する
/// </summary>
public class Player_Data : MonoBehaviour {
    Player player = new Player();

    const int SERVANT_NUMBER = 10; // 登場サーヴァントの騎数

    public void Set_Parameter() {
        var player_data = csv_Reader.Load_csv("csv/Actor/Player/Player_csv", 3);
        PLAYER_STATUS player_status;

        for(int i = 0; i < 2; ++i) { // TODO: マジックナンバー 10騎分のデータができ次第SERVANT_NUMBERを使う
        player_status.ID                 = int.Parse(player_data[i][0]);  // 番号
        player_status.name               = player_data[i][1];            // 名前
        player_status.class_type         = int.Parse(player_data[i][2]);  // クラス
        player_status.saint_graph        = int.Parse(player_data[i][3]);  // 再臨状態
        player_status.level              = int.Parse(player_data[i][4]);  // レベル
        player_status.hit_point          = int.Parse(player_data[i][5]);  // 体力
        player_status.max_hitpoint       = int.Parse(player_data[i][6]);  // 最大体力
        player_status.power              = int.Parse(player_data[i][7]);  // ちから(攻撃力見加味するボーナス値)
        player_status.max_power          = int.Parse(player_data[i][8]);  // 力の最大値
        player_status.activity           = int.Parse(player_data[i][9]);  // 行動力
        player_status.attack             = int.Parse(player_data[i][10]); // 攻撃力
        player_status.defence            = int.Parse(player_data[i][11]); // 防御力
        player_status.hunger_point       = int.Parse(player_data[i][12]); // はらへりポイント
        player_status.skill              = int.Parse(player_data[i][13]); // スキル(種類)
        player_status.star_generate      = int.Parse(player_data[i][14]); // スター発生率
        player_status.keep_star          = int.Parse(player_data[i][15]); // スター保持数
        player_status.command_card       = int.Parse(player_data[i][16]); // コマンドカード枚数
        player_status.arts_card          = int.Parse(player_data[i][17]); // アーツカード枚数
        player_status.arts_hit_conut     = int.Parse(player_data[i][18]); // アーツでの攻撃時のヒット回数
        player_status.quick_card         = int.Parse(player_data[i][19]); // クイックのカード枚数
        player_status.quick_hit_attack   = int.Parse(player_data[i][20]); // クイックでの攻撃時のヒット回数
        player_status.buster_card        = int.Parse(player_data[i][21]); // バスターのカード枚数
        player_status.buster_hit_count   = int.Parse(player_data[i][22]); // バスターでの攻撃時のヒット回数
        player_status.extra_attack       = int.Parse(player_data[i][23]); // エクストラアタック(種類)
        player_status.noble_weapon       = int.Parse(player_data[i][24]); // 宝具(種類)
        player_status.noble_phantasm     = int.Parse(player_data[i][25]); // 宝具を撃つためのポイント(以下NP)
        player_status.max_noble_phantasm = int.Parse(player_data[i][26]); // NPの最大値
        player_status.attack_rise_NP     = int.Parse(player_data[i][27]); // 攻撃時のNPの上昇量
        player_status.defence_rise_NP    = int.Parse(player_data[i][28]); // 被ダメージ時のNPの上昇量
        player_status.experience_point   = int.Parse(player_data[i][29]); // 経験値
        player_status.turn_count         = int.Parse(player_data[i][30]); // 経過ターンをカウント

            player.players.Add(player_status);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}