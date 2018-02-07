using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの行動を管理クラス
/// </summary>
public class Actor_Action : MonoBehaviour {
    [SerializeField]
    static Player_Move move;

    public static void Set_Move(Player_Move set_move) {
        move = set_move;
    }

    public static Player_Move Get_Move() {
        return move;
    }
}
