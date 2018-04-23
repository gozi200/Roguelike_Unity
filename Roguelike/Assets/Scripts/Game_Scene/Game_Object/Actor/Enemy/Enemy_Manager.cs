using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : Unique_Component<Enemy_Manager> {
    /// <summary>
    /// 自分自身のインスタンス
    /// </summary>
    public Enemy_Manager manager;
    /// <summary>
    /// エネミースクリプト
    /// </summary>
    public Enemy enemy_script;
    /// <summary>
    /// エネミーの行動を管理するクラス
    /// </summary>
    public Enemy_Action action;
    /// <summary>
    /// エネミーのステータス関係を管理するクラス
    /// </summary>
    public Enemy_Status status;

    void Awake() {
        manager = GetComponent<Enemy_Manager>();
        enemy_script = gameObject.AddComponent<Enemy>();
        action = gameObject.AddComponent<Enemy_Action>();
    }

    void Start() {
        status = gameObject.AddComponent<Enemy_Status>();
    }
}
