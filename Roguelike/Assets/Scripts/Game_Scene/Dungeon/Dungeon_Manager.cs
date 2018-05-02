﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

/// <summary>
/// ダンジョンのマネージャー
/// </summary>
public class Dungeon_Manager : Unique_Component<Dungeon_Manager> {
    /// <summary>
    /// アクターのマネージャー
    /// </summary>
    Actor_Manager actor_manager;
    /// <summary>
    /// ダンジョン作成クラス
    /// </summary>
    public Dungeon_Generator generator;
    /// <summary>
    /// マップを２次元配列で管理するクラス
    /// </summary>
    public Map_Layer_2D map_layer_2D;

    /// <summary>
    /// 表示する階層
    /// </summary>
    public ReactiveProperty<int> floor;
    /// <summary>
    /// そのダンジョンの最終階層
    /// </summary>
    public int max_floor { set; get; }
    /// <summary>
    /// ダンジョンの難易度
    /// </summary>
    int level;

    /// <summary>
    /// 現在のダンジョンの難易度
    /// </summary>
    eDungeon_Level dungeon_level;

    void Awake() {
        level = 1;
        floor = new ReactiveProperty<int>();
        floor.Value = 1;
        max_floor = 2;
        generator = GameObject.FindObjectOfType<Dungeon_Generator>();
        map_layer_2D = new Map_Layer_2D();
    }

    void Start() {
        actor_manager = Actor_Manager.Instance;
    }

    /// <summary>
    /// ダンジョンを作る準備
    /// </summary>
    public void Create() {
        Initialize();
    }

    /// <summary>
    /// 次のダンジョンへの移動処理
    /// </summary>
    public void NextLevel() {
        // 最終フロアーを越したらリザルト画面へ
        if (floor.Value >= max_floor) {
            SceneManager.LoadScene("Result");
        }
        else {
            Reset();
            ++floor.Value;
            Initialize();
        }
    }

    /// <summary>
    ///　プレイヤー以外全てのオブジェクトを消す
    /// </summary>
    void Reset() {
        // 全てのオブジェクトをタグで探す TODO:Findは重い？
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        GameObject[] stairs = GameObject.FindGameObjectsWithTag("Stair");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> enemy_list = actor_manager.enemys;

        // ダンジョンを生成しなおすのに、一度オブジェクトを消す
        foreach (GameObject trap in traps) {
            Destroy(trap);
        }
        foreach (GameObject obj in stairs) {
            Destroy(obj);
        }
        foreach (GameObject tile in tiles) {
            Destroy(tile);
        }
        foreach (GameObject enemy in enemys) {
            Destroy(enemy);
        }
        foreach (GameObject item in items) {
            Destroy(item);
        }
        foreach (GameObject wall in walls) {
            Destroy(wall.gameObject);
        }
        enemy_list.Clear();
    }

    /// <summary>
    /// ダンジョンの初期化
    /// </summary>
    void Initialize() {
        //TODO:現在進入中のダンジョンからダンジョンの難易度を出したものをいれる
        generator.Load_Dungeon(level);
    }
}
