using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// キャラクターの現在の向きを割り当てる
/// </summary>
public enum Direction {
    Invalid    // 0を格納
  , Down       // 下向き(正面)
  , Downleft   // 左下
  , Left       // 左
  , Upleft     // 左上
  , Up         // 上
  , Upright    // 右上
  , Right      // 右
  , Dowunright // 右下
}
