using UnityEngine;
using UniRx;

/// <summary>
/// ゲームシーンに必要なユニーククラス
/// </summary>
public class Game : Dynamic_Unique_Component<Game> {
    /// <summary>
    /// 入力されたキーを流すクラス
    /// </summary>
    public Key_Observer key_observer;
    /// <summary>
    /// csvファイルの読み込みクラス
    /// </summary>
    public csv_Reader csv_reader;
    /// <summary>
    /// シーン異動で破棄されないものを保持するクラス
    /// </summary>
    Do_Not_Destroy do_not_destroy;

    void Awake() {
        do_not_destroy = GameObject.Find("Do_Not_Destroy").GetComponent<Do_Not_Destroy>();
        do_not_destroy.Initialize();
        csv_reader = new csv_Reader();
        key_observer = new Key_Observer();
    }

    void Start() {
        // Mキーを押すとメニュー画面が開く
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

    /// <summary>
    /// 引数で受け取ったものをマイナス1して返す 関数にすることで役割を明示化する
    /// </summary>
    public int Decrement(int value) {
        return --value;
    }
}
