using System.Linq;
using UnityEngine;

/// <summary>
/// マップを２次元配列で管理
/// </summary>
public class Map_Layer_2D {
    /// <summary>
    /// 領域外を指定したらこれを返す
    /// </summary>
    int out_of_range = -1;
    /// <summary>
    /// マップデータを配列で扱う
    /// </summary>
    public int[] coordinates = null;

    /// <summary>
    /// 幅
    /// </summary>
    int width;
    public int Width { get { return width; } }

    /// <summary>
    /// 高さ
    /// </summary>
    int height;
    public int Height { get { return height; } }

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

    /// 座標をインデックスに変換する
    public int To_Index(int x, int y) {
        return y * width + x;
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
    /// 領域内であるかを判断する
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <returns>領域内であればfalse</returns>
    public bool Is_Out_Of_Range(int x, int y) {
        return (x < 0 || x >= width) || (y < 0 || y >= height);
    }
    public bool Is_Out_Of_Range(int index_count) {
        return coordinates.Count() < index_count;
    }

    /// <summary>
    /// 値の取得
    /// </summary>
    /// <param name="x">X座標</param>
    /// <param name="y">Y座標</param>
    /// <returns>領域外を指定したらout_of_rangeを返す</returns>
    public int Get_Layer_Number(int x, int y) {
        if (Is_Out_Of_Range(x, y)) {
            return out_of_range;
        }
        return coordinates[y * width + x];
    }

    /// <summary>
    /// 指定された座標が壁かどうかを調べる
    /// </summary>
    /// <param name="coordinates">調べる座標</param>
    /// <returns>壁であればtrue</returns>
    public bool Is_Wall(Vector2Int coordinates) {
        return Get_Layer_Number(coordinates.x, coordinates.y) == Define_Value.WALL_LAYER_NUMBER;
    }
    public bool Is_Wall(int coordinates_x, int coordinates_y) {
        return Get_Layer_Number(coordinates_x, coordinates_y) == Define_Value.WALL_LAYER_NUMBER;
    }

    /// <summary>
    /// 移動可能地帯であるかを調べる(壁以下であれば移動は可能)
    /// </summary>
    /// <returns>壁以下(移動可能地帯)であればtrue</returns>
    public bool Is_Less_Wall(int coordinates_x, int coordinates_y) {
        return Get_Layer_Number(coordinates_x, coordinates_y) < Define_Value.WALL_LAYER_NUMBER;
    }

    /// <summary>
    /// エネミーの移動が可能か調べる
    /// </summary>
    /// <returns>移動可能であればtrue</returns>
    public bool Is_Move_Enemy(int coordinates_x, int coordinates_y) {
        return Get_Layer_Number(coordinates_x, coordinates_y) <  Define_Value.WALL_LAYER_NUMBER &&
               Get_Layer_Number(coordinates_x, coordinates_y) != Define_Value.ENEMY_LAYER_NUMBER;
    }

    /// <summary>
    /// 指定された座標がプレイヤーかどうかを調べる
    /// </summary>
    /// <param name="coordinates">調べる座標</param>
    /// <returns>プレイヤーであればtrue</returns>
    public bool Is_Player(int coordinates_x, int coordinates_y) {
        return Get_Layer_Number(coordinates_x, coordinates_y) == Define_Value.PLAYER_LAYER_NUMBER;
    }

    /// <summary>
    /// 指定された座標が壁かどうかを調べる
    /// </summary>
    /// <param name="coordinates">調べる座標</param>
    /// <returns>敵であればtrue</returns>
    public bool Is_Enemy(int coordinates_x, int coordinates_y) {
        return Get_Layer_Number(coordinates_x, coordinates_y) == Define_Value.ENEMY_LAYER_NUMBER;
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
    /// 値の設定
    /// </summary>
    /// <param name = "layer_number">レイヤー番号</param>
    public void Set(int index_count, int layer_number) {
        if (Is_Out_Of_Range(index_count)) {
            // 領域外を指定した
            return;
        }
        coordinates[index_count] = layer_number;
    }

    /// <summary>
    /// 2次元座標から1次元配列上の座標をに直したものを返す
    /// </summary>
    /// <param name="x">2次元座標のx</param>
    /// <param name="y">2次元座標のy</param>
    /// <returns>1次元配列の座標</returns>
    public int Get(float x, float y) {
        float one_dimension_coordinates = y * Width + x;

        return (int)one_dimension_coordinates;
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
    /// <param name="left">  左</param>
    /// <param name="top">   上</param>
    /// <param name="right"> 右</param>
    /// <param name="bottom">下</param>
    /// <param name="val">埋める値</param>
    public void Fill_Rectangle_LTRB(int left, int top, int right, int bottom, int val) {
        Fill_Rectangle(left, top, right - left, bottom - top, val);
    }

    /// <summary>
    /// 移動後やアイテム取得後にレイヤー番号を変更する
    /// </summary>
    /// <param name="actor_position">変更をかける座標</param>
    /// <param name="new_layer_number">変更後のレイヤーナンバー</param>
    public void Tile_Swap(Vector2Int actor_position, int new_layer_number) {
        Set(actor_position.x, actor_position.y, new_layer_number);
    }
}