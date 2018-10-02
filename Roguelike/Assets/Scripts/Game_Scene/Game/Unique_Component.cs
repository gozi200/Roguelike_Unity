using UnityEngine;

/// <summary>
/// シングルトンとして動的生成するクラス
/// </summary>
/// <typeparam name="T">シングルトンにするクラス</typeparam>
public class Dynamic_Unique_Component<T> : MonoBehaviour where T : MonoBehaviour {
    /// <summary>
    /// 指定したクラスのインスタンス
    /// </summary>
    static T instance;

    /// <summary>
    /// インスタンス生成を行う
    /// </summary>
    public static T Instance {
        get {
            if (instance == null) {
                GameObject game_object = new GameObject(typeof(T).ToString());
                instance = game_object.AddComponent<T>();
            }
            return instance;
        }
    }

    //TODO:Startは１周後なので、作られたときに初期化する
    public void Initialized() {
        
    }
}