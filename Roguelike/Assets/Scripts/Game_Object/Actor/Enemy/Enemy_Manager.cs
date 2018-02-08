/*
    制作者 石倉

    最終更新日 2018/02/08
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーのマネージャークラス
/// </summary>
public class Enemy_Manager : MonoBehaviour {
    [SerializeField]
    static Enemy enemy;
    public static Enemy Enemy_Data {
        get {
            return enemy;
        }

        private set {
            enemy = value;
        }
    }
}
