/// <summary>
/// 定数を宣言するクラス
/// </summary>
public static class Define_Value {
    #region マップに関する定数

    /// <summary>
    /// プレイヤーからカメラまでの距離
    /// </summary>
    public const int CAMERA_DISTANCE = -5;
    /// <summary>
    /// 1マスの大きさ
    /// </summary>
    public const int TILE_SCALE = 1;
    /// <summary>
    /// 部屋の枠部分(入口がある囲い)
    /// </summary>
    public const int ROOM_FLAME = 1;

    #endregion

    #region ダンジョンに関する定義

    /// <summary>
    /// 難易度優しい
    /// </summary>
    public const int EASY = 1;
    /// <summary>
    /// 難易度普通
    /// </summary>
    public const int NOMAL = 2;
    /// <summary>
    /// 難易度難しい
    /// </summary>
    public const int HARD = 3;
    /// <summary>
    /// イージーダンジョンの横幅
    /// </summary>
    public const int EASY_DUNGEON_WIDTH = 30;
    /// <summary>
    /// イージーダンジョンの縦幅
    /// </summary>
    public const int EASY_DUNGEON_HEIGHT = 30;
    /// <summary>
    /// ノーマルダンジョンの横幅
    /// </summary>
    public const int NOMAL_DUNGEON_WIDTH = 45;
    /// <summary>
    /// ノーマルダンジョンの縦幅
    /// </summary>
    public const int NOMAL_DUNGEON_HEIGHT = 45;
    /// <summary>
    /// ハードダンジョンの横幅
    /// </summary>
    public const int HARD_DUNGEON_WIDTH = 60;
    /// <summary>
    /// ハードダンジョンの縦幅
    /// </summary>
    public const int HARD_DUNGEON_HEIGHT = 60;
    /// <summary>
    /// 最小の部屋サイズ
    /// </summary>
    public const int MIN_ROOM = 3;
    /// <summary>
    /// 最大の部屋サイズ
    /// </summary>
    public const int MAX_ROOM = 6;
    /// <summary>
    /// 区画と部屋の余白サイズ
    /// </summary>
    public const int MERGIN_SIZE = 3;
    /// <summary>
    /// 部屋配置の余白サイズ
    /// </summary>
    public const int POSITION_MERGIN = 2;
    /// <summary>
    /// エネミーがスポーンする周期(ターン)
    /// </summary>
    public const int SPAWN_INTERVAL = 20;
    /// <summary>
    /// ダンジョン進入時の階層
    /// </summary>
    public const int INITIAL_FLOOR = 1;

    #endregion

    #region ダンジョンの種類(csvの読み込みに使う)
 
    /// <summary>
    /// 名前が決まったらそれ
    /// </summary>
    public const int GRASS = 1;
    public const int CAVE = 2;


    #endregion

    #region 拠点に関する定義

    /// <summary>
    /// カルデアでのスポーンポイント(x軸)
    /// </summary>
    public const int CALDEA_SPAWN_X = 15;
    /// <summary>
    /// カルデアでのスポーンポイント(y軸)
    /// </summary>
    public const int CALDEA_SPAWN_Y = 15;

    #endregion

    #region アクターの関する定数

    /// <summary>
    /// 1回で動ける移動距離
    /// </summary>
    public const int MOVE_VAULE = 1;

    #endregion

    #region プレイヤーに関する定数

    /// <summary>
    /// プレイアブルキャラクターの人数
    /// </summary>
    public const int PLAYER_NUMBER = 10;
    /// <summary>
    /// プレイヤーキャラクター沖田のID
    /// </summary>
    public const int OKITA = 0;
    /// <summary>
    /// プレイヤーキャラクターマシュのID
    /// </summary>
    public const int MASH = 1;

    /// <summary>
    /// レベルの最大値(仮)
    /// </summary>
    public const int MAX_LV = 999;
    /// <summary>
    /// 経験値の最大値(仮)
    /// </summary>
    public const int MAX_EXP = 999999999;

    #endregion

    #region エネミーに関する定数

    /// <summary>
    /// 登場エネミーの種類(ひとまずの値)
    /// </summary>
    public const int ENEMY_NUMBER = 2;

    #endregion

    #region アイテムに関する定数


    #endregion

    #region レイヤー番号

    /// <summary>
    /// 床のレイヤー番号
    /// </summary>
    public const int TILE_LAYER_NUMBER = 1;
    /// <summary>
    /// 通路の床番号
    /// </summary>
    public const int ROAD_LAYER_NUMBER = 2;
    /// <summary>
    /// 入口の床番号
    /// </summary>
    public const int ENTRANCE_LAYER_NUMBER = 3;
    /// <summary>
    /// 階段のレイヤーナンバー
    /// </summary>
    public const int STAIR_LAYER_NUMBER = 4;
    /// <summary>
    /// 罠のレイヤーナンバー
    /// </summary>
    public const int TRAP_LAYER_NUMBER = 5;
    /// <summary>
    /// アイテムのレイヤーナンバー
    /// </summary>
    public const int ITEM_LAYER_NUMBER = 6;
    /// <summary>
    /// 踏むとダンジョン移動コマンドを出現させる
    /// </summary>
    public const int MOVE_DUNGEON_TILE = 7;

    /// <summary>
    /// プレイヤーのレイヤーナンバー
    /// </summary>
    public const int PLAYER_LAYER_NUMBER = 101;
    /// <summary>
    /// エネミーのレイヤーナンバー
    /// </summary>
    public const int ENEMY_LAYER_NUMBER = 102;

// これ以降は進入不可地帯(特定の条件下では進入可能)
    /// <summary>
    /// 壁のレイヤー番号
    /// </summary>
    public const int WALL_LAYER_NUMBER = 1001;

    #endregion

    #region その他

    /// <summary>
    /// csvの不要な行
    /// </summary>
    public const int UNNECESSARY_COLUMN = 3;

#endregion
}
