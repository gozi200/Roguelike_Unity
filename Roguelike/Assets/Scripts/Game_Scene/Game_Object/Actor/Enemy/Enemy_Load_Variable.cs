/// <summary>
/// 外部ファイルから値を読み込んで使う敵の変数
/// </summary>
[System.Serializable]
public class Enemy_Load_Variable {
    /// <summary>
    /// レベル
    /// </summary>
    public int level;
    /// <summary>
    /// 最大体力
    /// </summary>
    public int hit_point;
    /// <summary>
    /// 行動力(1ターンに動ける回数)
    /// </summary>
    public int activity;
}
