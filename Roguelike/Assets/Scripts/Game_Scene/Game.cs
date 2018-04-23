using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ゲームシーンに必要なユニーククラス //TODO:Titleから遷移すると継承元のInstanceがnullになる
/// </summary>
public class Game : Unique_Component<Game> {
    /// <summary>
    /// 入力されたキーを流すクラス
    /// </summary>
    public Key_Observer key_observer;
    /// <summary>
    /// ダメージ計算を行うクラス
    /// </summary>
    public Damage_Calculation damage_calculation;
    /// <summary>
    /// csvファイルの読み込みクラス
    /// </summary>
    public csv_Reader reader;

    /// <summary>
    /// インスタンス生成を行う
    /// </summary>
    void Awake() {
        reader = new csv_Reader();
        key_observer = new Key_Observer();
        damage_calculation = new Damage_Calculation();
    }

    void Start() {
        // Spaceキーを押すとメニュー画面が開く
        key_observer.On_Key_Down_AsObservable()
            .Where(key_code => key_code == KeyCode.M)
            .Subscribe(_ =>
                Debug.Log("メニュー画面を表示")
           ).AddTo(this);
    }

    void Update() {
        // 毎フレームキーが押されていないかチェックする
        key_observer.Key_Check();
    }
}


