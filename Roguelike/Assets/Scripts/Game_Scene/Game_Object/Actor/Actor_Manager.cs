using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターを共通の動きを管理するクラス
/// </summary>
public class Actor_Manager : Unique_Component<Actor_Manager> {
    /// <summary>
    /// 自身のインスタンス
    /// </summary>
    public Actor_Manager actor_manager;
    /// <summary>
    /// アクター共通のステータスを関係の処理を管理するクラス
    /// </summary>
    public Actor_Status actor_status;
    /// <summary>
    /// アクター共通の行動を管理するクラス
    /// </summary>
    public Actor_Action actor_action;

    void Start() {
        actor_manager = gameObject.GetComponent<Actor_Manager>();
        actor_action = gameObject.GetComponent<Actor_Action>();
        actor_status = new Actor_Status();
    }
}
