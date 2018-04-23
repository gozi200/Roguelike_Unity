using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.U2D;
using System.Collections;
using UnityEngine.Tilemaps;

/// <summary>
/// ステージの範囲を設定するクラス
/// </summary>
public class Decide_Rectangle : MonoBehaviour {
    /// <summary>
    /// ステージの範囲を設定
    /// </summary>
    public Rect stage_rect;
    /// <summary>
    /// ステージ範囲の左下の頂点座標
    /// </summary>
    public Vector2 bottom_left;
    /// <summary>
    /// ステージ範囲の左上の頂点座標
    /// </summary>
    public Vector2 top_left;
    /// <summary>
    /// ステージ範囲の右下の頂点座標
    /// </summary>
    public Vector2 bottom_right;
    /// <summary>
    /// ステージ範囲の右上の頂点座標
    /// </summary>
    public Vector2 top_right;

    /// <summary>
    /// ステージ範囲を可視化するためにGizumoを表示
    /// </summary>
    void OnDrawGizmos() {
        // 矩形の四隅の座標を取得
        bottom_left = new Vector2(stage_rect.xMin, stage_rect.yMin);
        top_left = new Vector2(stage_rect.xMin, stage_rect.yMax);
        bottom_right = new Vector2(stage_rect.xMax, stage_rect.yMin);
        top_right = new Vector2(stage_rect.xMax, stage_rect.yMax);

        // 四隅の座標を基準に矩形を赤色の線で描画
        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottom_left,  top_left);
        Gizmos.DrawLine(top_left,  top_right);
        Gizmos.DrawLine(top_right, bottom_right);
        Gizmos.DrawLine(bottom_right, bottom_left);
    }
}
