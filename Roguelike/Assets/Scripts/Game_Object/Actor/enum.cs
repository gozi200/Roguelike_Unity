using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
  , Dowunright // 右下
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
/// 現在のモードを割り当てる
/// </summary>
public enum eMode {
    Invalid               // 0を格納
  , Nomal_Mode            // 通常モード。上下左右のキー入力で入力方向に移動
  , Change_Direction_Mode // 方向転換モード。キー入力した方向を向く
  , Diagonally_Mode       // 斜め移動モード。斜めのみの移動になる
};

/// <summary>
/// バトルメニューの行動を割り当てる
/// </summary>
public enum eBattle_Menu {
    Invalid        // 0を格納
  , Item           // アイテム欄を表示
  , Foot_Step      // 足元コマンド
  , Details_Status // ステータス詳細
}