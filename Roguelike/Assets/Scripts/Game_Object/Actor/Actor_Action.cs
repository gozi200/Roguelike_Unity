using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの行動を管理クラス
/// </summary>
public class Actor_Action : MonoBehaviour {
    [SerializeField]
    static Move move;

    public static void Set_Move(Move set_move) {
        move = set_move;
    }

    public static Move Get_Move() {
        return move;
    }
}
