using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 拠点(安全地帯)となる地点のマネージャー TODO:テスト中
/// </summary>
public class Base_Manager : MonoBehaviour {
    /// <summary>
    /// 壁用のプレハブ
    /// </summary>
    [SerializeField]
    GameObject wall_prefab;
    /// <summary>
    /// 草の床のプレハブ
    /// </summary>
    [SerializeField]
    GameObject grass_tile;
    /// <summary>
    /// 石の床のプレハブ
    /// </summary>
    [SerializeField]
    GameObject stone_tile;
    /// <summary>
    /// ゲートのプレハブ
    /// </summary>
    [SerializeField]
    GameObject right_gate;

    /// <summary>
    /// ダンジョン選択コマンドの表示非表示
    /// </summary>
    public ReactiveProperty<bool> dungeon_command;
    /// <summary>
    /// ダンジョン選択コマンド
    /// </summary>
    [SerializeField]
    GameObject dungeon_command_UI;

    /// <summary>
    /// マップの元になるデータ
    /// </summary>
    string map_matrix;

    void Start() {
        dungeon_command = new ReactiveProperty<bool>(false);

        #region マップデータ

        map_matrix = ":" +
                     "00000000:" +
                     "01112110:" +
                     "01112110:" +
                     "01114110:" +
                     "01112110:" +
                     "01112110:" +
                     "00003000";

        #endregion

        // trueで階段で進むか否かのコマンドを表示 falseで閉じる
        dungeon_command.Where(flag => !!flag)
            .Subscribe(flag =>
            dungeon_command_UI.SetActive(flag)
            ).AddTo(this);
        dungeon_command.Where(flag => !flag)
            .Subscribe(flag =>
            dungeon_command_UI.SetActive(flag)
            ).AddTo(this);
    }

    /// <summary>
    /// 拠点を作る
    /// </summary>
    /// <param name="map_matrix">拠点のデータ</param>
    public void Create_Base() {
        var map_layer = Dungeon_Manager.Instance.map_layer_2D;
        var player = Player_Manager.Instance.player_script;
        var player_status = Player_Manager.Instance.player_status;

        // 初期化
        map_layer.Initialise(Define_Value.CALDEA_SPAWN_X, Define_Value.CALDEA_SPAWN_Y);

        // ':'をデリミタとして配列に格納
        string[] map_matrix_array = map_matrix.Split(':');

        for (int x = 0; x < map_matrix_array.Length; x++) {
            string x_map = map_matrix_array[x];
            for (int y = 0; y < x_map.Length; y++) {
                // 配列から取り出した１要素から１文字づつ取り出す
                int obj = int.Parse(x_map.Substring(y, 1));

                if (obj == 0) {
                    Instantiate(wall_prefab, new Vector2(x + 1, y + 1), Quaternion.identity);
                    map_layer.Set(x + 1, y + 1, Define_Value.WALL_LAYER_NUMBER);
                }
                else if (obj == 1) {
                    Instantiate(grass_tile, new Vector2(x + 1, y + 1), Quaternion.identity);
                    map_layer.Set(x + 1, y + 1, Define_Value.TILE_LAYER_NUMBER);
                }
                else if (obj == 2) {
                    Instantiate(stone_tile, new Vector2(x + 1, y + 1), Quaternion.identity);
                    map_layer.Set(x + 1, y + 1, Define_Value.TILE_LAYER_NUMBER); 
                }
                else if (obj == 3) {
                    Instantiate(right_gate, new Vector2(x + 1, y + 1), Quaternion.identity);
                    map_layer.Set(x + 1, y + 1, Define_Value.MOVE_DUNGEON_TILE);
                }
                else if (obj == 4) {
                    Instantiate(stone_tile, new Vector2(x + 1, y + 1), Quaternion.identity);

                    player.Set_Initialize_Position(x + 1, y + 1);

                    map_layer.Set(x + 1, y + 1, Define_Value.PLAYER_LAYER_NUMBER);
                    //TODO:足元のものを取って来たい
                    player.Set_Feet(Define_Value.TILE_LAYER_NUMBER);
                    // ここでプレイヤーに命を与える
                    player.Exist = true;
                }
            }
        }
    }
}
