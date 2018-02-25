/*
制作者 石倉

最終編集日 02/22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  TODO: 分かりやすい名前に変える
/// </summary>
public class Dungeon_Base : MonoBehaviour {
    void Start() {
        Player_Move.Set_Dungeon_Base(this);
        Enemy_Action.Set_Dungeon_Base(this);
    }

    // TODO: 攻撃と移動一緒くたにできないか 判断基準にlayerの番号もあると特定の条件下で移動可能になった時に対応できない？(浮遊時には水場の上に行けるなど)
    /// <summary>
    /// 移動の時に呼ばれる処理 移動先が移動可能地形であるかを判断する
    /// </summary>
    /// <param name = "height"      >縦座標</param>
    /// <param name = "width"       >横座標</param>
    /// <param name = "layer_number">レイヤーの番号</param> 
    /// <returns>移動可能、攻撃発生であればtrue</returns>
    public static bool Is_Check_Move(float coordinates1, float coordinates2, int layer_number) {
        if (Dungeon_Map.Get_Map_Layer(coordinates1, coordinates2) <= layer_number) {
                return true;
            }
        return false;
    }

    /// <summary>
    ///  移動を壁越しに行っていないかを判断する
    /// </summary>
    /// <param name = "coodinates1" >判断するマス1の縦座標</param>
    /// <param name = "coodinates2" >判断するマス1の横座標</param>
    /// <param name = "coodinates3" >判断するマス2の縦座標</param>
    /// <param name = "coodinates4" >判断するマス2の横座標</param>
    /// <param name = "layer_number">レイヤーの番号</param>
    /// <returns>壁越しであればtrue</returns>
    public static bool Is_Check_Slant_Move(int coordinates1, int coordinates2, int coordinates3, int coordinates4, int layer_number) {
        if (Dungeon_Map.Get_Map_Layer(coordinates1, coordinates2) <= layer_number ||
            Dungeon_Map.Get_Map_Layer(coordinates3, coordinates4) <= layer_number) {
            return true;
        }
        return false;
    }
}
