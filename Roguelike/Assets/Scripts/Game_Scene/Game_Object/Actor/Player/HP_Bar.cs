using System.Collections;
using System.Collections.Generic;
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
    /// 表示座標を指定する
    /// </summary>
    void Handle_Bar() {
        var player_status = Actor_Manager.Instance.player_status;
        health.fillAmount = Map(player_status.hit_point, 0, player_status.max_hit_point, 0, 1);
    }

    /// <summary>
    /// 座標を設定する
    /// </summary>
    /// <param name="value"></param>
    /// <param name="inMin"></param>
    /// <param name="inMax"></param>
    /// <param name="outMin"></param>
    /// <param name="outMax"></param>
    /// <returns></returns>
    float Map(float value, float inMin, float inMax, float outMin, float outMax) {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
