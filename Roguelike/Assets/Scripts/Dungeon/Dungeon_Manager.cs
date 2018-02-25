/*
制作者 アントニオ

最終編集日 02/08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ダンジョンのマネージャー
/// </summary>
public class Dungeon_Manager : MonoBehaviour {
    /// <summary>
    /// 現在の状態
    /// </summary>
    eGame_State game_state;

    /// <summary>
    /// マネージャーのインスタンス
    /// </summary>
    public static Dungeon_Manager Instance;

    /// <summary>
    /// ダンジョン作成
    /// </summary>
    [SerializeField]
    public Dungeon_Generator dungeon_generator;

    /// <summary>
    /// レベルUI
    /// </summary>
    private Text level_text;
    private int level = 1;

    /// <summary>
    /// GUIボタン、True＝押した、False=押してない
    /// </summary>
    bool button_pressed = false;

    /// <summary>
    /// ゲームオブジェクト
    /// </summary>
    Player player_script = null;

    GameObject player_object;

    GameObject stair;

    Vector2 player_speed;

    [SerializeField]
    Player player;

    void Start() {
        player_speed = player.GetComponent<Player>().speed;
    }

    /// <summary>
    /// 作成したダンジョンを表示
    /// </summary>
    public void Create() {
        Instance = this;
        game_state = eGame_State.Create_Dungeon;

        dungeon_generator = GetComponent<Dungeon_Generator>();

        player_object = dungeon_generator.player_object;

        Initialize();
    }

    /// <summary>
    /// 次のダンジョンへの移動処理
    /// </summary>
    public void NextLevel() {
        Reset();
        ++level;
        Initialize();
    }

    /// <summary>
    ///　プレイヤー以外全てのオブジェクトを消す
    /// </summary>
    void Reset() {
        // 全てのオブジェクトをタグで探す
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        GameObject[] stairs = GameObject.FindGameObjectsWithTag("Stair");
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        List<Cell> walls = dungeon_generator.walls;
        List<GameObject> enemy_list = dungeon_generator.enemy_list;


        // ダンジョンを生成しなおすのに、一度オブジェクトを消す
        foreach (GameObject trap in traps) {
            Destroy(trap);
        }
        foreach (GameObject obj in stairs) {
            Destroy(obj);
        }
        foreach (GameObject food in foods) {
            Destroy(food);
        }
        foreach (GameObject tile in tiles) {
            Destroy(tile);
        }
        foreach (GameObject enemy in enemy_list) {
            Destroy(enemy);
        }
        foreach (GameObject item in items) {
            Destroy(item);
        }
        foreach (Cell wall in walls) {
            Destroy(wall.gameObject);
        }
        walls.Clear();
        enemy_list.Clear();
    }

    /// <summary>
    /// ダンジョン表示する前の準備；
    /// </summary>
    void Initialize() {
        level_text = GameObject.Find("LevelText").GetComponent<Text>();
        level_text.text = "Level " + level;
        dungeon_generator.Load_Dungeon(level);
    }

    /// <summary>
    /// 階段にいる時の選択
    /// </summary>
    public void OnGUI() {
        stair = dungeon_generator.stair_object;
        //デバッグ用
        if (GUI.Button(new Rect(565, 365, 128, 32), "もう１回")) {
            NextLevel();
        }

        if (!button_pressed) {
            if (player.transform.position == stair.transform.position) {
                //選ぶまで動けなくなる
                player_speed = new Vector2(0, 0);

                //次のレベルへ行く
                if (GUI.Button(new Rect(225, 120, 128, 32), "次のレベル進む？")) {
                    NextLevel();
                    player_speed = new Vector2(5f, 5f);
                }

                //探索を続く
                else if (GUI.Button(new Rect(385, 120, 128, 32), "探索を続く？")) {
                    button_pressed = true;
                    player_speed = new Vector2(5f, 5f);
                }
            }
        }
        if (player.transform.position != stair.transform.position)
            button_pressed = false;
    }
}
