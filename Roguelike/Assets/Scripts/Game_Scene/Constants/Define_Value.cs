/// <summary>
/// 定数を宣言するクラス
/// </summary>
public static class Define_Value {
    #region マップに関する定数

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
    public const int MAX_ROOM = 5;
    /// <summary>
    /// 区画と部屋の余白サイズ
    /// </summary>
    public const int MERGIN_SIZE = 3;
    /// <summary>
    /// 部屋配置の余白サイズ
    /// </summary>
    public const int POSITION_MERGIN = 2;
    /// <summary>
    /// プレイヤーからカメラまでの距離
    /// </summary>
    public const int CAMERA_DISTANCE = -5;
    /// <summary>
    /// 1マスの大きさ
    /// </summary>
    public const int TILE_SCALE = 1;

    #endregion

    #region アクターの関する定数

    /// <summary>
    /// 1階で動ける移動距離
    /// </summary>
    public const int MOVE_VAULE = 1;

    #endregion

    #region プレイヤーに関する定数

    /// <summary>
    /// プレイアブルキャラクターの人数
    /// </summary>
    public const int PLAYER_NUMBER = 10;
    /// <summary>
    /// キャラクターID
    /// </summary>
    public const int OKITA = 0;

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
    /// 階段のレイヤーナンバー
    /// </summary>
    public const int STAIR_LAYER_NUMBER = 2;
    /// <summary>
    /// 罠のレイヤーナンバー
    /// </summary>
    public const int TRAP_LAYER_NUMBER = 3;
    /// <summary>
    /// アイテムのレイヤーナンバー
    /// </summary>
    public const int ITEM_LAYER_NUMBER = 4;

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
}
