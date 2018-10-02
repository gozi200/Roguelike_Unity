/// <summary>
/// 定数を宣言しておく
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
    /// <summary>
    /// マップの端(左下)
    /// </summary>
    public const float END_MAP = -0.5f;

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

    // ダンジョンの枠部分の為、左右端に+-5、上下に+-3した値(元サイズはそれを引いた値)
    /// <summary>
    /// イージーダンジョンの横幅
    /// </summary>
    public const int EASY_DUNGEON_WIDTH = 35;
    /// <summary>
    /// イージーダンジョンの縦幅
    /// </summary>
    public const int EASY_DUNGEON_HEIGHT = 31;
    /// <summary>
    /// ノーマルダンジョンの横幅
    /// </summary>
    public const int NOMAL_DUNGEON_WIDTH = 48;
    /// <summary>
    /// ノーマルダンジョンの縦幅
    /// </summary>
    public const int NOMAL_DUNGEON_HEIGHT = 44;
    /// <summary>
    /// ハードダンジョンの横幅
    /// </summary>
    public const int HARD_DUNGEON_WIDTH = 55;
    /// <summary>
    /// ハードダンジョンの縦幅
    /// </summary>
    public const int HARD_DUNGEON_HEIGHT = 46;
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
    /// 部屋配置の余白サイズ 横
    /// </summary>
    public const int POSITION_MERGIN_X = 3;
    /// <summary>
    /// 部屋配置の余白サイズ 縦
    /// </summary>
    public const int POSITION_MERGIN_Y = 3;
    /// <summary>
    /// エネミーがスポーンする周期(ターン)
    /// </summary>
    public const int SPAWN_INTERVAL = 20;
    /// <summary>
    /// ダンジョン進入時の階層
    /// </summary>
    public const int INITIAL_FLOOR = 1;

    #endregion

    #region ダンジョンの種類(読み込みに使う)

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
    public const int MOVE_VALUE = 1;

    /// <summary>
    /// 1キャラに必要な画像の枚数
    /// </summary>
    //TODO:今は方向の差異だけなので8だけ 状態異常なども追加
    public const int ACTOR_SPRITE_NUMBER = 8;

    /// <summary>
    /// 方向の数(上、右上、右、右下、下、左下、左、左上の8つ)
    /// </summary>
    public const int DIRECTION_NUMBER = 8;

    #endregion

    #region プレイヤーに関する定数

    /// <summary>
    /// プレイアブルキャラクターの人数
    /// </summary>
    //TODO:まだ起きた沖田だけなので１
    public const int PLAYER_NUMBER = 1;
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
    public const int ROOM_LAYER_NUMBER = 1;
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
    /// csvの不要な行 プレイヤー、エネミーなどのステータス読み込みの時に使用
    /// </summary>
    public const int UNNECESSARY_COLUMN_3 = 3;

    /// <summary>
    /// プレイヤーを中心にカメラが映す横の範囲。数値はマス分(つまり左右5マス分)
    /// </summary>
    public const int CAMERA_IMAGE_WIDTH = 5;
    /// <summary>
    /// プレイヤーを中心にカメラが映す縦の範囲。数値はマス分(つまり上下3マス分)
    /// </summary>
    public const int CAMERA_IMAGE_HEIGHT = 3;


    #endregion
}
