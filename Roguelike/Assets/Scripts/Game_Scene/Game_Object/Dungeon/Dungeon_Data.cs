/// <summary>
/// ダンジョンの情報を決めるクラス
/// </summary>
public class Dungeon_Data {
    #region csvから読み込み変数

    // TOOD:publicは使わない
    /// <summary>
    /// 番号
    /// </summary>
    public int ID;
    /// <summary>
    /// 名前
    /// </summary>
    public string name;
    /// <summary>
    /// ダンジョンの難易度
    /// </summary>
    public int level;
    /// <summary>
    /// 最後の階
    /// </summary>
    public int max_floor;
    /// <summary>
    /// 中間ポイント
    /// </summary>
    public int safety_zone;
    /// <summary>
    /// ボスのいる階層
    /// </summary>
    public int boss_floor;
    /// <summary>
    /// 出現敵タイプ
    /// </summary>
    public int appear_enemy;
    /// <summary>
    /// 地形タイプ
    /// </summary>
    public int terrain_type;

    #endregion

    /// <summary>
    /// 進入するダンジョンの情報を読み込む
    /// </summary>
    /// <param name="set_dungeon_type">ダンジョンの種類</param>
    public void Load_Dungeon(eDungeon_Type set_dungeon_type) {
        eDungeon_Type dungeon_type = set_dungeon_type;
        var reader = Game.Instance.csv_reader;
        var dungeon_data = reader.Load_csv("csv/Dungeon/Dungeon_csv", Define_Value.UNNECESSARY_COLUMN_3);
        ID           = int.Parse(dungeon_data[(int)dungeon_type][0]); // 番号
        name         = dungeon_data          [(int)dungeon_type][1];  // 名前
        level        = int.Parse(dungeon_data[(int)dungeon_type][2]); // 難易度
        max_floor    = int.Parse(dungeon_data[(int)dungeon_type][3]); // 最大階層
        safety_zone  = int.Parse(dungeon_data[(int)dungeon_type][4]); // 中間ポイント
        boss_floor   = int.Parse(dungeon_data[(int)dungeon_type][5]); // ボスフロア
        appear_enemy = int.Parse(dungeon_data[(int)dungeon_type][6]); // 出現敵タイプ
        terrain_type = int.Parse(dungeon_data[(int)dungeon_type][7]); // ダンジョン進入時の地形タイプ
    }
}
