using UnityEngine;

/// <summary>
/// エフェクトの位置を変える
/// </summary>
public class Position_Changer : MonoBehaviour {
    // 現在何が選ばれているのかを知る
    enum eNow_Decide {
        Start
      , Load
      , Exit
    }

    /// <summary>
    /// 押されたら最初からゲームを開始するスタートのボタン
    /// </summary>
    [SerializeField]
    GameObject start_button;
    /// <summary>
    /// 押されたら途中からゲーム化開始するロードのボタン
    /// </summary>
    [SerializeField]
    GameObject load_button;
    /// <summary>
    /// 押されたらゲームを終了させる終了ボタン
    /// </summary>
    [SerializeField]
    GameObject exit_button;

    /// <summary>
    /// 選択されているボタンと連動させる
    /// </summary>
    eNow_Decide now_decide;

    void Start() {
        // 最初はスタートボタンから //TODO:ロードができ次第、初期配置をロードに変更する
        now_decide = eNow_Decide.Start;
    }

    void Update() {
        // 選択されているボタンに連動させる 一番上、下のときは何もしない
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            switch (now_decide) {
                case eNow_Decide.Start:
                    break;
                case eNow_Decide.Load:
                    now_decide = eNow_Decide.Start;
                    break;
                case eNow_Decide.Exit:
                    now_decide = eNow_Decide.Load;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            switch (now_decide) {
                case eNow_Decide.Start:
                    now_decide = eNow_Decide.Load;
                    break;
                case eNow_Decide.Load:
                    now_decide = eNow_Decide.Exit;
                    break;
                case eNow_Decide.Exit:
                    break;
            }
        }

        // 状態に応じて選ばれているものの座標に飛ぶ
        switch (now_decide) {
            case eNow_Decide.Start:
                gameObject.transform.position = start_button.transform.position;
                break;
            case eNow_Decide.Load:
                gameObject.transform.position = load_button.transform.position;
                break;
            case eNow_Decide.Exit:
                gameObject.transform.position = exit_button.transform.position;
                break;
        }

    }
}
