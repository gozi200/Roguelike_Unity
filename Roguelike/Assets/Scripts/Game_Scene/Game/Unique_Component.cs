using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ユニーククラスの基底クラス シングルトンにするときはそれをここで生成する
/// </summary>
/// <typeparam name="T">新しく作るシングルトンクラス</typeparam>
public class Unique_Component<T> : MonoBehaviour where T : MonoBehaviour {
    /// <summary>
    /// 指定したクラスのインスタンス
    /// </summary>
    static T _instance;

    /// <summary>
    /// インスタンス生成を行う
    /// </summary>
    public static T Instance {
        get {
            if (_instance == null) {
                GameObject game_object = new GameObject(typeof(T).ToString());
                _instance = game_object.AddComponent<T>();
                // シーンの移行で破棄されないようにする
                //DontDestroyOnLoad(game_object);
            }
            return _instance;
        }
    }

    // Startは1フレ後なので、作られたときに初期化する
    public void Initialized() {
        
    }
}