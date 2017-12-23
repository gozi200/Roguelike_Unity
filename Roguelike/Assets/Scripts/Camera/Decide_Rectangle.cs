using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.U2D;
using System.Collections;
using UnityEngine.Tilemaps;

/// <summary>
/// ステージの範囲を設定するクラス
/// </summary>
public class Decide_Rectangle : MonoBehaviour
{
   // [SerializeField]
   // GameObject Dungeon_Generator;

    //private Transform floor;

    /// <summary>
    /// ステージの高さを設定
    /// </summary>
    public float StageHeight = 30f;

    /// <summary>
    /// ステージの範囲を設定
    /// </summary>
    public Rect StageRect;
    [SerializeField]
    public Vector2 LowerLeft;
    public Vector2 UpperLeft;
    public Vector2 LowerRight;
    public Vector2 UpperRight;

    /// <summary>
    /// ステージ範囲の範囲を可視化するためにGizumoを表示
    /// </summary>
    void OnDrawGizmos() {
        // 矩形の四隅の座用を取得
        LowerLeft = new Vector2(StageRect.xMin, StageRect.yMax);
        UpperLeft = new Vector2(StageRect.xMin, StageRect.yMin);
        LowerRight = new Vector2(StageRect.xMax, StageRect.yMax);
        UpperRight = new Vector2(StageRect.xMax, StageRect.yMin);

        // 四隅の座標を基準に矩形を赤色の線で描画
        Gizmos.color = Color.red;
        Gizmos.DrawLine(LowerLeft, UpperLeft);
        Gizmos.DrawLine(UpperLeft, UpperRight);
        Gizmos.DrawLine(UpperRight, LowerRight);
        Gizmos.DrawLine(LowerRight, LowerLeft);
    }

    void Start() {

    }

    void Update() {
    }
}