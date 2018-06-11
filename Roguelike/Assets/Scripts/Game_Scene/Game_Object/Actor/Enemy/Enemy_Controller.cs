using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの行動やステータスを一括で管理するクラス
/// </summary>
public class Enemy_Controller : MonoBehaviour {
    /// <summary>
    /// エネミーのマネージャークラス
    /// </summary>
    public Enemy_Manager enemy_manager;
    /// <summary>
    /// エネミースクリプト
    /// </summary>
    public Enemy enemy_script;
    /// <summary>
    /// エネミーの行動を管理するクラス
    /// </summary>
    public Enemy_Action enemy_action;
    /// <summary>
    /// エネミーのステータス関係を管理するクラス
    /// </summary>
    public Enemy_Status enemy_status;
    /// <summary>
    /// エネミーの行動を制御
    /// </summary>
    public Enemy_Move enemy_move;

    public void Initialize() {
        enemy_manager = Enemy_Manager.Instance;
        enemy_script = gameObject.GetComponent<Enemy>();
        enemy_status = new Enemy_Status();
        enemy_move = new Enemy_Move();
    }
}
