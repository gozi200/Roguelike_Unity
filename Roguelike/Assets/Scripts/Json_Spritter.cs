using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  受け取ったJsonファイルをインデックスごとに分割する
/// </summary>
public class Json_Splitter {
    /// <summary>
    /// インデックスごとに分割する
    /// </summary>
    /// <typeparam name="T">どのクラスか</typeparam>
    /// <param name="json">分割するJsonのファイル</param>
    /// <returns>分割されたもの</returns>
    public static List<T> List_From_Json<T>(string json) {
        string new_json = "{ \"list\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(new_json);
        return wrapper.list;
    }

    /// <summary>
    /// 受け取り用クラス 分割したものを受け取り、返す
    /// </summary>
    /// <typeparam name="T">Spritterで受け取ったクラス</typeparam>
    [Serializable]
    class Wrapper<T> {
        public List<T> list;
    }
}
