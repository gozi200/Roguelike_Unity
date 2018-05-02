using UnityEngine;
using System.Collections;

/// <summary>
/// カメラの倍率、移動限界域を設定するクラス
/// </summary>
public class Camera_Controller : MonoBehaviour {
    /// <summary>
    /// カメラの表示する範囲を決定するクラス
    /// </summary>
    Decide_Rectangle decide_rectangle;

    /// <summary>
    /// カメラの表示領域の左下頂点
    /// </summary>
    Vector2 bottom_left;
    /// <summary>
    /// カメラの表示領域の左上頂点
    /// </summary>
    Vector2 top_left;
    /// <summary>
    /// カメラの表示領域の右下頂点
    /// </summary>
    Vector2 bottom_right;
    /// <summary>
    /// カメラの表示領域の右上頂点
    /// </summary>
    Vector2 top_right;
    /// <summary>
    /// カメラの目標座標
    /// </summary>
    Vector2 new_position;
    /// <summary>
    /// 制限されたカメラの目標座標
    /// </summary>
    Vector3 limit_position;
    /// <summary>
    /// 親のゲームオブジェクト(プレイヤー)
    /// </summary>
    GameObject target;
    /// <summary>
    /// カメラの表示範囲の高さ
    /// </summary>
    public float camera_range_width;
    /// <summary>
    /// カメラの表示範囲の幅
    /// </summary>
    public float camera_range_height;

    void Start() {
        target = GameObject.Find("Player");
        // ステージの範囲
        decide_rectangle = GameObject.Find("Map").GetComponent<Decide_Rectangle>();
    }

    void Update() {
        float newX = 0f;
        float newY = 0f;

        // プレイヤーキャラの位置にカメラの座標を設定する。
        new_position = target.transform.position;
        gameObject.transform.position = target.transform.position;

        // ビューポート座標をワールド座標に変換
        bottom_left = Camera.main.ViewportToWorldPoint(new Vector2(0, 0f));
        top_right = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        top_left = new Vector2(bottom_left.x, top_right.y);
        bottom_right = new Vector2(top_right.x, bottom_left.y);
        camera_range_width = Vector2.Distance(bottom_left, bottom_right);
        camera_range_height = Vector2.Distance(bottom_left, top_left);
        newX = Mathf.Clamp(new_position.x, decide_rectangle.stage_rect.xMin + camera_range_width / 2, decide_rectangle.stage_rect.xMax - camera_range_width / 2);
        newY = Mathf.Clamp(new_position.y, 2.5f, decide_rectangle.stage_rect.yMax - camera_range_height / 2);
        limit_position = new Vector3(newX, newY, Define_Value.CAMERA_DISTANCE);
        transform.position = limit_position;
    }

    /// <summary>
    /// カメラの描画範囲を緑の線で表示
    /// </summary>
    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(bottom_left, top_left);
        Gizmos.DrawLine(top_left, top_right);
        Gizmos.DrawLine(top_right, bottom_right);
        Gizmos.DrawLine(bottom_right, bottom_left);
    }
}
