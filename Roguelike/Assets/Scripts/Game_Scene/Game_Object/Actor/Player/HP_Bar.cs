using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 体力ゲージの減少を目に見えるように
/// </summary>
public class HP_Bar : MonoBehaviour {
    /// <summary>
    /// 体力バーのイメージ
    /// </summary>
    [SerializeField]
    Image health;

    /// <summary>
    /// 指定した座標に表示する
    /// </summary>
    void Update() {
        Handle_Bar();
    }

    /// <summary>
    /// HPの増減に合わせてバーを伸縮させる
    /// </summary>
    void Handle_Bar() {
        var player_status = Player_Manager.Instance.player_status;
        // 現在の体力が最大体力の何割かを求める
        health.fillAmount = (float)player_status.Hit_Point.Value / (float)player_status.Max_Hit_Point.Value;
    }
}
