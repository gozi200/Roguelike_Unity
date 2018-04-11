using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カーソルの位置を調整する
/// </summary>
public class CurSor_Controller : MonoBehaviour {
    /// <summary>
    /// Decideクラスで何が選ばれているのかを知る
    /// </summary>
    [SerializeField]
    Decide_Command decide_command;
    /// <summary>
    /// 現在の座標
    /// </summary>
    public Vector2 position;

    Vector2 load_position;
    Vector2 start_position;
    Vector2 config_position;
    Vector2 exit_position;

    void Start() {
        position = gameObject.transform.position;
        load_position = new Vector2(3.5f, -0.1f);
        start_position = new Vector2(3.5f, -1f);
        config_position = new Vector2(3.5f, -2);
        exit_position = new Vector2(3.5f, -3);
    }

    void Update() {
        Move_Cursor();
    }

    void Move_Cursor() {
        switch (decide.title_command) {
            case eTitle_Command.Load:
                position = load_position;
                Set_Posiiton(position);
                break;
            case eTitle_Command.Start:
                position = start_position;
                Set_Posiiton(position);
                break;
            case eTitle_Command.Config:
                position = config_position;
                Set_Posiiton(position);
                break;
            case eTitle_Command.Exit:
                position = exit_position;
                Set_Posiiton(position);
                break;
        }
    }

    void Set_Posiiton(Vector2 set_position) {
        gameObject.transform.position = set_position;
    }

}