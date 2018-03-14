//制作者 石倉

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ユニーククラスの基底クラス シングルトンパターンにするときはそれをここで生成する
/// </summary>
/// <typeparam name="T">新しく作るシングルトンクラス</typeparam>
public class Unique_Component<T> : MonoBehaviour where T : MonoBehaviour {
    static bool applicationIsQuitting = false;

    static T _instance;

    public static T Instance {
        get {
            if (applicationIsQuitting) {
                return null;
            }

            if (_instance == null) {
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null) {
                    GameObject game_object = new GameObject(typeof(T).ToString());
                    _instance = game_object.AddComponent<T>();
                    // シーンが変わってもDestroyされないようにする
                    DontDestroyOnLoad(game_object);
                }
            }
            return _instance;
        }
    }

    public void OnDestroy() {
        applicationIsQuitting = true;
    }
}