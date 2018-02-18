﻿/*
制作者 アントニオ

最終編集日 01/18
 */

using UnityEngine;
using System.Collections;

/// 2次元レイヤー
public class Layer2D {

    int width; // 幅
    int height; // 高さ
    int out_of_range = -1; // 領域外を指定した時の値
    int[] values = null; // マップデータ

    /// 幅
    public int Width {
        get {
            return width;
        }
    }

    /// 高さ
    public int Height {
        get {
            return height;
        }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Layer2D(int width = 0, int height = 0) {
        if (width > 0 && height > 0) {
            Create(width, height);
        }
    }

    /// <summary>
    /// 大きさを設定する
    /// </summary>
    /// <param name = "set_width" >横幅</param>
    /// <param name = "set_height">縦幅</param>
    public void Create(int set_width, int set_height) {
        width = set_width;
        height = set_height;
        values = new int[Width * Height];
    }

    /// 座標をインデックスに変換する
    public int To_Index(int x, int y) {
        return x + (y * Width);
    }

    /// <summary>
    /// 領域内であるかを判断する
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <returns>領域内であればfalse</returns>
    public bool Is_Out_Of_Range(int x, int y) {
        if (x < 0 || x >= Width) { return true; }
        if (y < 0 || y >= Height) { return true; }

        return false;
    }

    /// <summary>
    /// 値の取得
    /// </summary>
    /// <param name="x">X座標</param>
    /// <param name="y">Y座標</param>
    /// <returns>領域外を指定したらout_of_rangeを返す</returns>
    public int Get(int x, int y) {
        if (Is_Out_Of_Range(x, y)) {
            return out_of_range;
        }

        return values[y * Width + x];
    }

    /// <summary>
    /// 値の設定
    /// </summary>
    /// <param name = "x">x座標</param>
    /// <param name = "y">y座標</param>
    /// <param name = "v"></param>
    public void Set(int x, int y, int v) {
        if (Is_Out_Of_Range(x, y)) {
            // 領域外を指定した
            return;
        }

        values[y * Width + x] = v;
    }

    /// <summary>
    /// すべてのセルを特定の値で埋める
    /// </summary>
    /// <param name="val">埋める値</param>
    public void Fill(int val) {
        for (int j = 0; j < Height; j++) {
            for (int i = 0; i < Width; i++) {
                Set(i, j, val);
            }
        }
    }

    /// <summary>
    /// 床をセットする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="val"></param>
    public void Fill_Tile(int x, int y, int val = 0) {
        Set(x, y, val);
    }

    /// <summary>
    /// 矩形領域を指定の値で埋める
    /// </summary>
    /// <param name="x">  矩形の左上(X座標)</param>
    /// <param name="y">  矩形の左上(Y座標)</param>
    /// <param name="w">  矩形の幅</param>
    /// <param name="h">  矩形の高さ</param>
    /// <param name="val">埋める値</param>
    public void Fill_Rectangle(int x, int y, int w, int h, int val) {
        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {
                int px = x + i;
                int py = y + j;
                Set(px, py, val);
            }
        }
    }

    /// <summary>
    /// 矩形領域を指定の値で埋める（4点指定)
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="top">上</param>
    /// <param name="right">右</param>
    /// <param name="bottom">下</param>
    /// <param name="val">埋める値</param>
    public void Fill_Rectangle_LTRB(int left, int top, int right, int bottom, int val) {
        Fill_Rectangle(left, top, right - left, bottom - top, val);
    }

    /// デバッグ出力
    public void Dump() {
        Debug.Log("[Layer2D] (w,h)=(" + Width + "," + Height + ")");
        for (int y = 0; y < Height; y++) {
            string s = "";
            for (int x = 0; x < Width; x++) {
                s += Get(x, y) + ",";
            }
            Debug.Log(s);
        }
    }
}
