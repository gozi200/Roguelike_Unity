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
    Create_Base    // 拠点を作る
  , Create_Dungeon // ダンジョンを作る
  , Player_Turn    // プレイヤーのターン
  , Partner_Turn   // パートナーのターン
  , Enemy_Trun     // エネミーのターン
  , Dungeon_Turn   // ダンジョンのターン(経過カウントなど)
}

/// <summary>
/// キャラクターの現在の向きを割り当てる
/// </summary>
public enum eDirection {
    Down      // 下向き(正面)
  , Downleft  // 左下
  , Left      // 左
  , Upleft    // 左上
  , Up        // 上
  , Upright   // 右上
  , Right     // 右
  , Downright // 右下
  , Finish    // 乱数生成に使用
};

/// <summary>
/// 現在行っている状態を割り当てる
/// </summary>
public enum ePlayer_State {
    Move           // 移動処理
  , Attack         // 攻撃処理
  , Battle_Menu    // バトルメニューを開く
  , On_Stair       // 階段に乗っているとき
  , Decide_Command // コマンド選択中(ダンジョン選択や、店で話しているとき等)
  , Game_Over      // ゲームオーバー
  , Action_Max     // Player_Actionで宣言されているActionの最大値
};

/// <summary>
/// Move状態に入っているときのプレイヤーの行動を割り当てる
/// </summary>
public enum ePlayer_Move {

}

/// <summary>
/// プレイヤーの現在のモード
/// </summary>
public enum ePlayer_Mode {
    Nomal_Mode            // 通常モード。
  , Change_Direction_Mode // 方向転換モード。キー入力した方向を向き直る(移動は無し)
};

/// <summary>
/// 敵の現在のモード
/// </summary>
public enum eEnemy_Mode {
    Move_Mode      // 移動モード
  , Encounter_Mode // エンカウントモード
  , Attack_Mode    // 攻撃モード
}

/// <summary>
/// バトルメニューを開いているときの行動
/// </summary>
public enum eBattle_Menu {
    Item           // アイテム欄を表示
  , Foot_Step      // 足元コマンド
  , Details_Status // ステータス詳細
}

/// <summary>
/// 使用するコマンドカード
/// </summary>
public enum eCommand_Card {
    Arts_Card   // アーツカード
  , Quick_Card  // クイックカード
  , Buster_Card // バスターカード
}

/// <summary>
/// エネミーの行動パターン
/// </summary>
public enum eEnemy_AI_Pattern {
    Pattern_01 // パターン01
  , Pattern_02 // パターン02
}

/// <summary>
/// ダンジョンの難易度
/// </summary>
public enum eDungeon_Level {
    Easy  // イージーモード
  , Nomal // ノーマルモード
  , Hard  // ハードモード
}

/// <summary>
/// ダンジョンの種類
/// </summary>
public enum eDungeon_Type {
    Beginning_Grass
  , Dim_Cave
}

/// <summary>
/// 床の状態(これに合わせて画像を変更する)
/// </summary>
public enum eTile_State {
    Grass // 草原
  , Stone // 石
  , Finish // 種類数の取得に使用
}

/// <summary>
/// 壁の状態(これに合わせて画像を変更する)
/// </summary>
public enum eWall_State {
    Tree // 木
  , Stone // 石
  , Finish // 種類数の取得に使用
}


