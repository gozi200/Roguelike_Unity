using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画像変更を行うクラスの親
/// </summary>
public class Sprite_Changer : MonoBehaviour {
    /// <summary>
    /// 画像をセットする
    /// </summary>
    /// <param name="type">spritesの要素数</param>
    virtual protected void Set_Sprite(int type) { }
}
