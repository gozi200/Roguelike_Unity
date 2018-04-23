using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
///  キー入力を受け付けるクラス
/// </summary>
public class Key_Observer {
    Subject<KeyCode> on_key_down_sbject = new Subject<KeyCode>();

    /// <summary>
    /// 押されたキーを流す
    /// </summary>
    public void Key_Check() {
        for (KeyCode key = 0; (int)key < (int)KeyCode.Menu; ++key) {
            if (Input.GetKeyDown(key)) {
                on_key_down_sbject.OnNext(key);
                Debug.Log(key);
            }
        }
    }

    /// <summary>
    /// on_key_don_subjectを返す
    /// </summary>
    /// <returns></returns>
    public IObservable<KeyCode> On_Key_Down_AsObservable() {
        return on_key_down_sbject;
    }
}
