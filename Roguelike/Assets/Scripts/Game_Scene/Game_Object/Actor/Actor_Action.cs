using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの行動を管理クラス TODO: 移動など共有できるものを制作
/// </summary>
public class Actor_Action : MonoBehaviour {
    /// <summary>
    /// ダメージ計算を行うクラス
    /// </summary>
    Damage_Calculation damage_calculation;

    void Start() {
        damage_calculation = gameObject.GetComponent<Damage_Calculation>();
    }
}
