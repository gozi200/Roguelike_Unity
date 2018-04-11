/// <summary>
/// 定数を宣言するクラス
/// </summary>
public static class Define_Value{

    #region マップに関する定数

    /// <summary>
    /// ダンジョンの横幅
    /// </summary>
    public const int WIDTH = 30;
    /// <summary>
    /// ダンジョンの縦幅
    /// </summary>
    public const int HEIGHT = 30;
    /// <summary>
    /// 床のレイヤー番号
    /// </summary>
    public const int TILE_LAYER_NUMBER = 1;
    public const int STAIR_LAYER_NUMBER = 2;
    /// <summary>
    /// 壁のレイヤー番号
    /// </summary>
    public const int WALL_LAYER_NUMBER = 1001;

    /// <summary>
    /// 1マス分のサイズ //TODO: 画像サイズを調整してあわせる
    /// </summary>
    public const int SPRITE_SIZE = 5;

    #endregion

    #region アクターの関する定数

    /// <summary>
    /// ワールド座標上での移動距離
    /// TODO:ややこしいので画像サイズを調整して１にする
    /// </summary>
    public const int MOVE_VAULE = 5;

    #endregion

    #region プレイヤーに関する定数

    /// <summary>
    /// プレイヤーのレイヤーナンバー
    /// </summary>
    public const int PLAYER_LAYER_NUMBER = 101;
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
    /// 敵のレイヤーナンバー
    /// </summary>
    public const int ENEMY_LAYER_NUMBER = 102;

    /// <summary>
    /// 登場エネミーの種類(ひとまずの値)
    /// </summary>
    const int ENEMY_NUMBER = 2;


    #endregion

    #region 罠に関する定数
    //TODO: 少ないようならマップと統一する？

    public const int TRAP_LAYER_NUMBER = 3;

    #endregion

    #region アイテムに関する定数

    public const int ITEM_LAYER_NUMBER = 4;

    #endregion
}
