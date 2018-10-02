using UnityEngine;

/// <summary>
/// 破棄されたくないものをまとめておく
/// </summary>
public class Do_Not_Destroy : MonoBehaviour {
    /// <summary>
    /// 最初の会話
    /// </summary>
    static bool first_talk = true;
    public static bool First_Talk { set { first_talk = value; } get { return first_talk; } }

    /// <summary>
    /// 初期化 シーンの移動で破棄されないようにする
    /// </summary>
    public void Initialize() {
        DontDestroyOnLoad(this);
    }
}
