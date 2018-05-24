using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;

/// <summary>
/// ローカルストレージにファイルとして、シリアライズしたデータを保存できるシングルトンです（iOSの場合、該当ファイルはiCloudバックアップ対象から除外します）
/// </summary>
abstract public class Savable_Singleton_Base<T> where T : Savable_Singleton_Base<T>, new() {
    protected static T instance;
    bool isLoaded = false;
    protected bool isSaving = false;

    public static T Instance {
        get {
            if (null == instance) {
                var json = File.Exists(GetSavePath()) ? File.ReadAllText(GetSavePath()) : "";
                if (json.Length > 0) {
                    LoadFromJSON(json);
                }
                else {
                    instance = new T();
                    instance.isLoaded = true;
                    instance.PostLoad();
                }
            }
            return instance;
        }
    }

    protected virtual void PostLoad() {
    }

    public void Save() {
        if (isLoaded) {
            isSaving = true;
            var path = GetSavePath();
            File.WriteAllText(path, JsonUtility.ToJson(this));
#if UNITY_IOS
            // iOSでデータをiCloudにバックアップさせない設定
            UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
            isSaving = false;
        }
    }

    public void Reset() {
        instance = null;
    }

    public void Clear() {
        if (File.Exists(GetSavePath())) {
            File.Delete(GetSavePath());
        }
        instance = null;
    }

    public static void LoadFromJSON(string json) {
        try {
            instance = JsonUtility.FromJson<T>(json);
            instance.isLoaded = true;
            instance.PostLoad();
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }

    static string GetSavePath() {
        return string.Format("{0}/{1}", Application.persistentDataPath, GetSaveKey());
    }

    static string GetSaveKey() {
        var provider = new SHA1CryptoServiceProvider();
        var hash = provider.ComputeHash(System.Text.Encoding.ASCII.GetBytes(typeof(T).FullName));
        return BitConverter.ToString(hash);
    }
}