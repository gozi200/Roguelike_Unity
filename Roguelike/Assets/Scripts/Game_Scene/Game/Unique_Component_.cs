using UnityEngine;

/// <summary>
/// シングルトンとして静的生成するクラス
/// </summary>
/// <typeparam name="T">シングルトンにするクラス</typeparam>
public class Static_Unique_Component<T> : MonoBehaviour where T : MonoBehaviour {
    /// <summary>
    /// 指定したクラスのインスタンス
    /// </summary>
    static T instance;

    /// <summary>
    /// インスタンス生成を行う
    /// </summary>
    public static T Instance {
        get {
            if(instance == null) {
                instance = (T)FindObjectOfType(typeof(T));
            }
            return instance;
        }
    }
}
