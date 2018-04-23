using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップを２次元配列で管理するクラス
/// </summary>
public class Map_Layer_2D {
    /// <summary>
    /// 領域外を指定したらこれを返す
    /// </summary>
    int out_of_range = -1;
    /// <summary>
    /// マップデータを配列で扱う
    /// </summary>
    int[] coordinates = null;

    /// <summary>
    /// 幅
    /// </summary>
    int width;

    /// <summary>
    /// 高さ
    /// </summary>
    int height;

    /// <summary>
    /// 幅を取得
    /// </summary>
    /// <returns></returns>
    public int Get_Width() {
        return width;
    }

    /// <summary>
    /// 高さを取得
    /// </summary>
    /// <returns></returns>
    public int Get_Height() {
        return height;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void Initialise(int width = 0, int height = 0) {
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
        coordinates = new int[width * height];
    }

    /// <summary>
    /// 座標をインデックスに変換する
    /// </summary>
    /// <param name="x">座標</param>
    /// <param name="y">座標</param>
    /// <returns></returns>
    // TODO:使ってない 2018/04/21
    public int To_Index(int x, int y) {
        return x + (y * width);
    }

    /// <summary>
    /// 領域内であるかを判断する
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <returns>領域内であればfalse</returns>
    public bool Is_Out_Of_Range(int x, int y) {
        if (x < 0 || x >= width) { return true; }
        if (y < 0 || y >= height) { return true; }

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
        return coordinates[y * width + x];
    }

    /// <summary>
    /// オブジェクトの座標から取るとき //TODO:テスト中
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int Get_(float x, float y) {
        if(Is_Out_Of_Range((int)x, (int)y)) {
            return out_of_range;
        }
        return coordinates[(int)y * width + (int)x];
    }

    /// <summary>
    /// 値の設定
    /// </summary>
    /// <param name = "x">x座標</param>
    /// <param name = "y">y座標</param>
    /// <param name = "layer_number">レイヤー番号</param>
    public void Set(int x, int y, int layer_number) {
        if (Is_Out_Of_Range(x, y)) {
            // 領域外を指定した
            return;
        }
        coordinates[y * width + x] = layer_number;
    }

    /// <summary>
    /// すべてのセルを特定の値で埋める
    /// </summary>
    /// <param name="val">埋める値</param>
    public void Fill(int val) {
        for (int j = 0; j < height; j++) {
            for (int i = 0; i < width; i++) {
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

    /// <summary>
    /// 移動後やアイテム取得後にレイヤー番号を変更する
    /// </summary>
    /// <param name="layer_nuber">変更をかける座標</param>
    /// <param name="layer_number">変更後のレイヤーナンバー</param>
    public void Tile_Swap(Vector2 player_position, int new_layer_number) {
        Set((int)player_position.x, (int)player_position.y, new_layer_number);
    }
}