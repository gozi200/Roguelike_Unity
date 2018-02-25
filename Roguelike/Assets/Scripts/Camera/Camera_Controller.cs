using UnityEngine;
using System.Collections;

/// <summary>
/// カメラの倍率、移動限界域を設定するクラス
/// </summary>
public class Camera_Controller : MonoBehaviour
{
    public GameObject target;
    //private Vector2 offsetPosition;
    private Decide_Rectangle decide_rectangle;

    /// <summary>
    /// プレイヤーキャラまでの距離。固定で設定
    /// </summary>
    [SerializeField]
    private float distance = 5f;

    /// <summary>
    /// カメラの表示領域表示用
    /// </summary>
    private Vector2 cameraBottomLeft;
    private Vector2 cameraTopLeft;
    private Vector2 cameraBottomRight;
    private Vector2 cameraTopRight;
    public float cameraRangeWidth;
    public float cameraRangeHeight;

    /// <summary>
    /// カメラの表示領域を緑ラインで表示
    /// </summary>
    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(cameraBottomLeft, cameraTopLeft);
        Gizmos.DrawLine(cameraTopLeft, cameraTopRight);
        Gizmos.DrawLine(cameraTopRight, cameraBottomRight);
        Gizmos.DrawLine(cameraBottomRight, cameraBottomLeft);
    }

    void Start() {
       target = transform.parent.gameObject;

        /// <summary>
        /// ステージコントローラーを取得
        /// </summary>
        decide_rectangle = GameObject.Find("Map").GetComponent<Decide_Rectangle>();
    }

    void Update()
    {
        /// <summary>
        /// カメラの目標座標
        /// </summary>
        Vector2 newPosition;

        /// <summary>
        /// 制限されたカメラの目標座標
        /// </summary>
        Vector3 limitPosition;

        float newX = 0f;
        float newY = 0f;

        // プレイヤーキャラの位置にカメラの座標を設定する。(キャラのちょっと上にする?)
        newPosition = target.transform.position;// + offsetPosition;

        // ビューポート座標をワールド座標に変換
        cameraBottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        cameraTopLeft = new Vector2(cameraBottomLeft.x, cameraTopRight.y);
        cameraBottomRight = new Vector2(cameraTopRight.x, cameraBottomLeft.y);
        cameraRangeWidth = Vector2.Distance(cameraBottomLeft, cameraBottomRight);
        cameraRangeHeight = Vector2.Distance(cameraBottomLeft, cameraTopLeft);

        newX = Mathf.Clamp(newPosition.x, decide_rectangle.StageRect.xMin + cameraRangeWidth / 2, decide_rectangle.StageRect.xMax - cameraRangeWidth / 2);
        newY = Mathf.Clamp(newPosition.y, 0, decide_rectangle.StageRect.yMax - cameraRangeHeight / 2);

        limitPosition = new Vector3(newX, newY, -distance);
        transform.position = limitPosition;
    }
}