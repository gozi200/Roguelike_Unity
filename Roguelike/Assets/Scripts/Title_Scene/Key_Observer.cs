using UnityEngine;
using UniRx;
using System;

/// <summary>
///  キー入力を受け付けるクラス
/// </summary>
public class Key_Observer {
    // 発行、購読を行えるように
    Subject<KeyCode> on_key_down_sbject = new Subject<KeyCode>();

    /// <summary>
    /// 押されたキーを流す
    /// </summary>
    public void Key_Check() {
        for (KeyCode key = 0; (int)key < (int)KeyCode.Menu; ++key) {
            if (Input.GetKeyDown(key)) {
                on_key_down_sbject.OnNext(key);
            }
        }
    }

    /// <summary>
    /// 押されたキーを返す
    /// </summary>
    /// <returns>押されたキー</returns>
    public IObservable<KeyCode> On_Key_Down_AsObservable() {
        return on_key_down_sbject;
    }
}
