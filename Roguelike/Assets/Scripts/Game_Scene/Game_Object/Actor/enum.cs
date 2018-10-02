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
    Up
  , Upright
  , Right
  , Downright
  , Down
  , Downleft
  , Left
  , Upleft
  , Finish // 乱数生成に使用
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
  , Non_Active     // アナウンスや、会話シーンなど移動が不可の時
  , Game_Over      // ゲームオーバー
  , Action_Max     // Player_Actionで宣言されているActionの最大値
};

/// <summary
/// プレイヤーの現在のモード
/// </summary>
public enum ePlayer_Mode {
    Nomal_Mode            // 通常モード。
  , Change_Direction_Mode // 方向転換モード。キー入力した方向を向き直る(移動は無し)
};

/// <summary>
/// 今現在どこにいるか
/// </summary>
public enum eNow_Place {
    Safety_Zone
  , Dungeon
}

/// <summary>
/// プレイヤーがどこを移動しているか
/// </summary>
public enum ePlayer_Where_Move {
    Room_Move // 部屋の中を移動している状態
  , Road_Move // 通路を移動している状態
}

/// <summary>
/// 敵の現在のモード
/// </summary>
public enum eEnemy_Mode {
    Move_Room_Mode // 部屋移動モード
  , Move_Road_Mode // 通路移動モード
  , Encounter_Mode // エンカウントモード
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
    Arts_Card
  , Quick_Card
  , Buster_Card
}

/// <summary>
/// エネミーの行動パターン
/// </summary>
public enum eEnemy_AI_Pattern {
    Pattern_01
  , Pattern_02
}

/// <summary>
/// ダンジョンの難易度
/// </summary>
public enum eDungeon_Level {
    Easy
  , Nomal
  , Hard
}

/// <summary>
/// ダンジョンの種類
/// </summary>
public enum eDungeon_Type {
    Beginning_Grass // 始まりの草原(仮)
  , Dim_Cave        // うすぐら洞窟(仮)
  , Test_Dungoen    // テストダンジョン(途中で地形が変化する者のテスト)
}

/// <summary>
/// 床の状態(これに合わせて画像を変更する)
/// </summary>
public enum eTile_State {
    Grass
  , Stone
  , Finish // 種類数の取得に使用
}

/// <summary>
/// 壁の状態(これに合わせて画像を変更する)
/// </summary>
public enum eWall_State {
    Tree
  , Stone
  , Finish // 種類数の取得に使用
}

/// <summary>
/// クラス
/// </summary>
public enum eClass_Type {
    Saber
  , Archer
  , Lancer
  , Rider
  , Caster
  , Assasin
  , Ruler
  , Berserker
  , Avenger
  , Shielder
}

/// <summary>
/// 敵のIDと合わせた列挙
/// </summary>
public enum eEnemy_ID {
    Wyvern
  , Wyvern_Doread
  , Wyvern_Origin
  , Golem
}


/// <summary>
/// ノードの状態 エネミーの移動AIに使用
/// </summary>
enum eNode_Status {
    None   // 触れてないノード
  , Open   // 開いたのノード
  , Closed // 閉じたノード
}

/// <summary>
/// 立ち絵キャラクターの表情。これに合わせて表示する画像を変える
/// </summary>
public enum eCharacter_Expression1 {
    Angry   // 怒り
  , Smile1  // 笑顔1
  , Neutral // 普通
  , Smile2  // 笑顔2
  , Finish  // 初期化用
}

/// <summary>
/// 喋るメッセージの種類
/// </summary>
public enum eMessage_Type {
    Start // 開始時
  , end   // 終了時
}
