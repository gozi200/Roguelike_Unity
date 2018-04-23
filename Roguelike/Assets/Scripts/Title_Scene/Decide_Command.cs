using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 何を選んで何を決定したかを通知する
/// </summary>
public class Decide_Command : MonoBehaviour {
    /// <summary>
    /// 初めから開始
    /// </summary>
    public void Decide_Start() {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// セーブデータをロードして開始
    /// </summary>
    //TODO:まだセーブはないのではじめから
    public void Decide_Load() {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// コンフィグ画面を表示
    /// </summary>
    public void Decide_Config()　{
     //TODO: コンフィグはまだない
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void Decide_Exit() {
        UnityEditor.EditorApplication.isPlaying = false;
    } 
}
