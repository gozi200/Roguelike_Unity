/*
    制作者 石倉

    最終更新日 2018/02/08
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのマネージャークラス
/// </summary>
public class Player_Manager : MonoBehaviour {
    [SerializeField]
    static Player player;
    public static Player Player_Data {
        get {
            return player;
        }

        private set {
            player = value;
        }
    }

    void Start() {
        GameManager.Set_Player(this);
    }
}
