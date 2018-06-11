using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Battle_Menu : MonoBehaviour {
    /// <summary>
    /// バトルメニューを表示
    /// </summary>
    ReactiveProperty<bool> is_battle_menu;
    /// <summary>
    /// バトルメニューのUI
    /// </summary>
    GameObject battle_menu_UI;
    /// <summary>
    /// プレイヤースクリプト
    /// </summary>
    Player player;
    /// <summary>
    /// キーの入力を流すクラス
    /// </summary>
    Key_Observer key_observer;

	void Start () {
        player = Player_Manager.Instance.player_script;
        key_observer = Game.Instance.key_observer;
        battle_menu_UI = GameObject.Find("Battle_Menu");

        is_battle_menu = new ReactiveProperty<bool>(false);

        // Tabが押されたときにバトルメニュー画面を表示する
        key_observer.On_Key_Down_AsObservable()
            .Where(key_code => key_code == KeyCode.Tab)
            .Subscribe(_ => {
                player.player_state = ePlayer_State.Battle_Menu;
                is_battle_menu.Value = true;
            }).AddTo(this);

        // trueでバトルメニューUIを表示 falseで閉じる
        is_battle_menu.Where(flag => !!flag)
            .Subscribe(flag => {
                battle_menu_UI.SetActive(flag);
            }).AddTo(this);
        is_battle_menu.Where(flag => !flag)
            .Subscribe(flag => {
                battle_menu_UI.SetActive(flag);
            }).AddTo(this);
    }

    void Update() {
        // TODO:テスト中 思い通りには動くが、Escapeには重複して処理を入れるためこのままではダメ
        if (Input.GetKeyDown(KeyCode.Escape)) {
            is_battle_menu.Value = false;
            player.player_state = ePlayer_State.Move;
        }
    }
}
