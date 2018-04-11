using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 何を選んで何を決定したかを通知する
/// </summary>
public class Decide_Command : MonoBehaviour {
    public eTitle_Command title_command;
    /// <summary>
    /// カーソルが上に上がった時に列挙体の値を引く
    /// </summary>
    int up_cursor;
    /// <summary>
    /// カーソルが下がった時に列挙体の値を足す
    /// </summary>
    int down_cusor;

    void Start() {
        up_cursor = -1;
        down_cusor = 1;
        title_command = eTitle_Command.Load;
    }

    void Update() {
        Key_Check();
    }

    void Key_Check() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            title_command += up_cursor;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            title_command += down_cusor;
        }

        if (title_command < eTitle_Command.Load) {
            title_command = eTitle_Command.Load;
        }
        else if (title_command > eTitle_Command.Exit) {
            title_command = eTitle_Command.Exit;
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            switch (title_command) {
                case eTitle_Command.Load:
                    //TODO: セーブ機能はまだないのではじめからで移行
                    SceneManager.LoadScene("Game");
                    break;
                case eTitle_Command.Start:
                    SceneManager.LoadScene("Game");
                    break;
                case eTitle_Command.Config:
                    // 設定画面へ移行
                    break;
                case eTitle_Command.Exit:
                    UnityEditor.EditorApplication.isPlaying = false;
                    break;
            }
        }
    }
}
