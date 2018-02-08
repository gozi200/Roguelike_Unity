/*
制作者　石倉 

最終編集日 2018/02/07
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------
enumを定義しておくスクリプト
---------------------------*/

/// <summary>
/// ゲームの進行を割り当てる
/// </summary>
public enum eGame_State {
    Invalid    // 0を格納
  , Create_Base
  , Create_Dungeon
  , Player_Turn
  , Partner_Turn
  , Enemy_Trun
  , Dungeon_Turn
}

/// <summary>
/// キャラクターの現在の向きを割り当てる
/// </summary>
public enum eDirection {
    Invalid    // 0を格納
  , Down       // 下向き(正面)
  , Downleft   // 左下
  , Left       // 左
  , Upleft     // 左上
  , Up         // 上
  , Upright    // 右上
  , Right      // 右
  , Downright // 右下
};

/// <summary>
/// 現在行っているアクションを割り当てる
/// </summary>
public enum ePlayer_Action {
    Invalid               // 0を格納
  , Move                  // 移動処理
  , Attack                // 攻撃処理
  , Battle_Menu           // バトルメニューを開く
  , Game_Over             // ゲームオーバー
  , Action_Max            // Player_Actionで宣言されているActionの最大値
};

/// <summary>
/// Move状態に入っているときのプレイヤーの行動を割り当てる
/// </summary>
public enum ePlayer_Move {
    Invalid
  , 
}

/// <summary>
/// プレイヤーの現在のモードを割り当てる
/// </summary>
public enum ePlayer_Mode {
    Invalid               // 0を格納
  , Nomal_Mode            // 通常モード。上下左右のキー入力で入力方向に移動
  , Change_Direction_Mode // 方向転換モード。キー入力した方向を向く
  , Diagonally_Mode       // 斜め移動モード。斜めのみの移動になる
};

/// <summary>
/// 敵の現在のモードを割り当てる
/// </summary>
public enum eEnemy_Mode {
    Invalid        // 0を格納
  , Move_Mode      // 移動モード
  , Encounter_Mode // エンカウントモード
  , Attack_Mode    // 攻撃モード
   
}

/// <summary>
/// バトルメニューの行動を割り当てる
/// </summary>
public enum eBattle_Menu {
    Invalid        // 0を格納
  , Item           // アイテム欄を表示
  , Foot_Step      // 足元コマンド
  , Details_Status // ステータス詳細
}

/// <summary>
/// 使用するコマンドカードを割り当てる
/// </summary>
public enum eCommand_Card {
    Invalid     // 0を格納
  , Arts_Card   // アーツカード
  , Quick_Card  // クイックカード
  , Buster_Card // バスターカード

}

/// <summary>
/// エネミーの行動パターンを割り当てる。
/// </summary>
public enum eEnemy_AI_Pattern {
    Invalid  // 0を格納
, Pattern_01 // パターン01
, Pattern_02 // パターン02
}
