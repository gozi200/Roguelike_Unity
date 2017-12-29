using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Base : MonoBehaviour {
    Dungeon_Generator dungeon_generator = new Dungeon_Generator();


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool Is_Move(int x, int y) {
        // 範囲外は移動不可
       //if (x < 0 || x >= WIDTH || y < 0 || y >= m_height)
            return true;
    
        // 壁があるか？
        //if (getTile(x, y)->isWall)
            return true;
    
        // 移動可能
        return false;
    }

    bool Check_Move(int ax, int ay, int bx, int by) {
        // Ａ・Ｂが同一だったりしないか？
        if (ax == bx && ay == by)
            return false;   // 同一は周囲８マスではない

        // Ａ・Ｂは周囲８マスにいないか？
        if (System.Math.Abs(ax - bx) > 1 || System.Math.Abs(ay - by) > 1)
            return false;   // 離れている

        // Ｂは移動可能地形か？
        if (Is_Move(bx, by))
            return false;

        // Ａからみて、Ｂは左上？
        if (ax - 1 == bx && ay - 1 == by) {
            // 左と上が移動不能地形か？
            if (Is_Move(ax - 1, ay) && Is_Move(ax, ay - 1))
                return false;

            return true;
        }
        // Ａからみて、Ｂは右上？
        if (ax + 1 == bx && ay - 1 == by) {
            // 右と上が移動不能地形か？
            if (Is_Move(ax + 1, ay) && Is_Move(ax, ay - 1))
                return false;

            return true;
        }
        // Ａからみて、Ｂは左下？
        if (ax - 1 == bx && ay + 1 == by) {
            // 右と下が移動不能地形か？
            if (Is_Move(ax - 1, ay) && Is_Move(ax, ay + 1))
                return false;

            return true;
        }
        // Ａからみて、Ｂは右下？
        if (ax + 1 == bx && ay + 1 == by) {
            // 右と下が移動不能地形か？
            if (Is_Move(ax + 1, ay) && Is_Move(ax, ay + 1))
                return false;

            return true;
        }

        return true;
    }
}
