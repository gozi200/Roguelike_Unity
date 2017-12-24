using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ダンジョンの自動生成モジュール
/// </summary>
public class Dungeon_Generator : MonoBehaviour {
    /// <summary>
    /// マップ全体の幅
    /// </summary>
    const int WIDTH = 38; // TODO: 58に

    /// <summary>
    /// マップ全体の高さ
    /// </summary>
    const int HEIGHT = 38; // TODO: 38に

    /// <summary>
    /// 区画と部屋の余白サイズ
    /// </summary>
    const int MERGIN_SIZE = 4;

    /// <summary>
    /// 部屋配置の余白サイズ
    /// </summary>
    const int POSITION_MERGIN = 2;

    /// <summary>
    /// 最小の部屋サイズ
    /// </summary>
    const int MIN_ROOM = 3;

    /// <summary>
    /// 最大の部屋サイズ
    /// </summary>
    const int MAX_ROOM = 20; // TODO: 8より大きい数になると通路の位置がずれる(直った)

    /// <summary>
    /// 通路
    /// </summary>
    const int CHIP_NONE = 0;

    /// <summary>
    /// 壁
    /// </summary>
    const int CHIP_WALL = 1;

    /// <summary>
    /// 階段
    /// </summary>
    const int CHIP_UP_STAIR = 2;

    /// <summary>
    /// 2次元配列情報
    /// </summary>
    Layer2D layer = null;

    /// <summary>
    /// 区画リスト
    /// </summary>
    List<Dungeon_Division> divition_List = null;

    /// チップ上のX座標を取得する.
    float Get_Chip_X(int i) {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        var spr = Util.Get_Sprite("Chip1/Grass_Wall_", "");
        var sprW = spr.bounds.size.x;

        return min.x + (sprW * i) + sprW / 2;
    }

    /// チップ上のy座標を取得する.
    float Get_Chip_Y(int j) {
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        var spr = Util.Get_Sprite("Chip1/Grass_Wall_", "");
        var sprH = spr.bounds.size.y;

        return max.y - (sprH * j) - sprH / 2;
    }

    void Start() {
        // ■1. 初期化
        // 2次元配列初期化
        layer = new Layer2D(WIDTH, HEIGHT);

        // 区画リスト作成
        divition_List = new List<Dungeon_Division>();

        // ■2. すべてを壁にする
        layer.Fill(CHIP_WALL);

        // ■3. 最初の区画を作る
        Create_Division(0, 0, WIDTH - 1, HEIGHT - 1);

        // ■4. 区画を分割する
        // 垂直 or 水平分割フラグの決定
        bool bVertical = (Random.Range(0, 2) == 0);
        Split_Divison(bVertical);

        // ■5. 区画に部屋を作る
        Create_Room();

        // ■6. 部屋同士をつなぐ
        Connect_Rooms();

        // デバッグ出力
        //foreach (var div in divition_List)
        //{
        //    div.Dump();
        //}
        //layer.Dump();

        // タイルを配置
        for (int i = 0; i < layer.Height; i++) {
            for (int j = 0; j < layer.Width; j++) {
                float x = Get_Chip_X(i);
                float y = Get_Chip_Y(j);
                Util.Create_Token(x, y, "Chip1/Grass_Tile", "", "Tile");

                // 壁であれば壁用の画像を配置
                if (layer.Get(i, j) == CHIP_WALL) {
                    x = Get_Chip_X(i);
                    y = Get_Chip_Y(j);
                    Util.Create_Token(x, y, "Chip1/Grass_Wall_", "", "Wall");
                }
            }
        }
    }

    /// <summary>
    /// 最初の区画を作る
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="top">上</param>
    /// <param name="right">右</param>
    /// <param name="bottom">下</param>
    void Create_Division(int left, int top, int right, int bottom) {
        Dungeon_Division div = new Dungeon_Division();
        div.Outer.Set_Position(left, top, right, bottom);
        divition_List.Add(div);
    }

    /// <summary>
    /// 区画を分割する
    /// </summary>
    /// <param name="is_vertical">垂直分割するかどうか</param>
    void Split_Divison(bool is_vertical) {

        // 末尾の要素を取り出し
        Dungeon_Division parent = divition_List[divition_List.Count - 1];
        divition_List.Remove(parent);

        // 子となる区画を生成
        Dungeon_Division child = new Dungeon_Division();

        if (is_vertical) {
            // ▼縦方向に分割する
            if (Check_Division_Size(parent.Outer.Height) == false) {
                // 縦の高さが足りない
                // 親区画を戻しておしまい
                divition_List.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Outer.Top + (MIN_ROOM + MERGIN_SIZE);
            int b = parent.Outer.Bottom - (MIN_ROOM + MERGIN_SIZE);

            // AB間の距離を求める
            int ab = b - a;

            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, MAX_ROOM);

            // 分割点を決める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set_Position(
                parent.Outer.Left, p, parent.Outer.Right, parent.Outer.Bottom);

            // 親の下側をp地点まで縮める
            parent.Outer.Bottom = child.Outer.Top;
        }
        else {
            // ▼横方向に分割する
            if (Check_Division_Size(parent.Outer.Width) == false) {
                // 横幅が足りない
                // 親区画を戻しておしまい
                divition_List.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Outer.Left + (MIN_ROOM + MERGIN_SIZE);
            int b = parent.Outer.Right - (MIN_ROOM + MERGIN_SIZE);

            // AB間の距離を求める
            int ab = b - a;

            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, MAX_ROOM);

            // 分割点を求める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set_Position(
                p, parent.Outer.Top, parent.Outer.Right, parent.Outer.Bottom);

            // 親の右側をp地点まで縮める
            parent.Outer.Right = child.Outer.Left;
        }

        // 次に分割する区画をランダムで決める
        if (Random.Range(0, 2) == 0) {
            // 子を分割する
            divition_List.Add(parent);
            divition_List.Add(child);
        }
        else {
            // 親を分割する
            divition_List.Add(child);
            divition_List.Add(parent);
        }

        // 分割処理を再帰呼び出し (分割方向は縦横交互にする)
        Split_Divison(!is_vertical);
    }

    /// <summary>
    /// 指定のサイズを持つ区画を分割できるかどうか
    /// </summary>
    /// <param name="size">チェックする区画のサイズ</param>
    /// <returns>分割できればtrue</returns>
    bool Check_Division_Size(int size) {
        // (最小の部屋サイズ + 余白)
        // 2分割なので x2 する
        // +1 して連絡通路用のサイズも残す
        int min = (MIN_ROOM + MERGIN_SIZE) * 2 + 1;

        return size >= min;
    }

    /// <summary>
    /// 区画に部屋を作る
    /// </summary>
    void Create_Room() {
        foreach (Dungeon_Division div in divition_List) {
            // 基準サイズを決める
            int dw = div.Outer.Width - MERGIN_SIZE;
            int dh = div.Outer.Height - MERGIN_SIZE;

            // 大きさをランダムに決める
            int sw = Random.Range(MIN_ROOM, dw);
            int sh = Random.Range(MIN_ROOM, dh);

            // 最大サイズを超えないようにする
            sw = Mathf.Min(sw, MAX_ROOM);
            sh = Mathf.Min(sh, MAX_ROOM);

            // 空きサイズを計算 (区画 - 部屋)
            int rw = (dw - sw);
            int rh = (dh - sh);

            // 部屋の左上位置を決める
            int rx = Random.Range(0, rw) + POSITION_MERGIN;
            int ry = Random.Range(0, rh) + POSITION_MERGIN;

            int left = div.Outer.Left + rx;
            int right = left + sw;
            int top = div.Outer.Top + ry;
            int bottom = top + sh;

            // 部屋のサイズを設定
            div.Room.Set_Position(left, top, right, bottom);

            // 部屋を通路にする
            Fill_Dungeon_Rectangle(div.Room);
        }
    }

    /// <summary>
	/// Dungeon_Rectangleの範囲を塗りつぶす
    /// </summary>
    /// <param name="rect">矩形情報</param>
    void Fill_Dungeon_Rectangle(Dungeon_Division.Dungeon_Rectangle r) {
        layer.Fill_Rectangle_LTRB(r.Left, r.Top, r.Right, r.Bottom, CHIP_NONE);
    }

    /// <summary>
    /// 部屋同士を通路でつなぐ
    /// </summary>
    void Connect_Rooms() {
        for (int i = 0; i < divition_List.Count - 1; i++) {
            // リストの前後の区画は必ず接続できる
            Dungeon_Division a = divition_List[i];
            Dungeon_Division b = divition_List[i + 1];

            // 2つの部屋をつなぐ通路を作成
            Create_Road(a, b);

            // 孫にも接続する
            for (int j = i + 2; j < divition_List.Count; j++) {
                Dungeon_Division c = divition_List[j];
                if (Create_Road(a, c, true)) {
                    // 孫に接続できたらおしまい
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 指定した部屋の間を通路でつなぐ
    /// </summary>
    /// <param name="divA">部屋1</param>
    /// <param name="divB">部屋2</param>
	/// <param name="bGrandChild">孫チェックするかどうか</param>
    /// <returns>つなぐことができたらtrue</returns>
	bool Create_Road(Dungeon_Division divA, Dungeon_Division divB, bool bGrandChild = false) {
        if (divA.Outer.Bottom == divB.Outer.Top || divA.Outer.Top == divB.Outer.Bottom) {
            // 上下でつながっている
            // 部屋から伸ばす通路の開始位置を決める
            int x1 = Random.Range(divA.Room.Left, divA.Room.Right);
            int x2 = Random.Range(divB.Room.Left, divB.Room.Right);
            int y = 0;

            if (bGrandChild) {
                // すでに通路が存在していたらその情報を使用する
                if (divA.HasRoad()) { x1 = divA.Road.Left; }
                if (divB.HasRoad()) { x2 = divB.Road.Left; }
            }

            if (divA.Outer.Top > divB.Outer.Top) {
                // B - A (Bが上側)
                y = divA.Outer.Top;
                // 通路を作成
                divA.CreateRoad(x1, y + 1, x1 + 1, divA.Room.Top);
                divB.CreateRoad(x2, divB.Room.Bottom, x2 + 1, y);
            }
            else {
                // A - B (Aが上側)
                y = divB.Outer.Top;
                // 通路を作成
                divA.CreateRoad(x1, divA.Room.Bottom, x1 + 1, y);
                divB.CreateRoad(x2, y, x2 + 1, divB.Room.Top);
            }
            Fill_Dungeon_Rectangle(divA.Road);
            Fill_Dungeon_Rectangle(divB.Road);

            // 通路同士を接続する
            FillHLine(x1, x2, y);

            // 通路を作れた
            return true;
        }

        if (divA.Outer.Left == divB.Outer.Right || divA.Outer.Right == divB.Outer.Left) {
            // 左右でつながっている
            // 部屋から伸ばす通路の開始位置を決める
            int y1 = Random.Range(divA.Room.Top, divA.Room.Bottom);
            int y2 = Random.Range(divB.Room.Top, divB.Room.Bottom);
            int x = 0;

            if (bGrandChild) {
                // すでに通路が存在していたらその情報を使う
                if (divA.HasRoad()) { y1 = divA.Road.Top; }
                if (divB.HasRoad()) { y2 = divB.Road.Top; }
            }

            if (divA.Outer.Left > divB.Outer.Left) {
                // B - A (Bが左側)
                x = divA.Outer.Left;
                // 通路を作成
                divB.CreateRoad(divB.Room.Right, y2, x, y2 + 1);
                divA.CreateRoad(x + 1, y1, divA.Room.Left, y1 + 1);
            }
            else {
                // A - B (Aが左側)
                x = divB.Outer.Left;
                divA.CreateRoad(divA.Room.Right, y1, x, y1 + 1);
                divB.CreateRoad(x, y2, divB.Room.Left, y2 + 1);
            }
            Fill_Dungeon_Rectangle(divA.Road);
            Fill_Dungeon_Rectangle(divB.Road);

            // 通路同士を接続する
            FillVLine(y1, y2, x);

            // 通路を作れた
            return true;
        }

        // つなげなかった
        return false;
    }

    /// <summary>
    /// 水平方向に線を引く (左と右の位置は自動で反転する)
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="right">右</param>
    /// <param name="y">Y座標</param>
    void FillHLine(int left, int right, int y) {
        if (left > right) {
            // 左右の位置関係が逆なので値をスワップする
            int tmp = left;
            left = right;
            right = tmp;
        }
        layer.Fill_Rectangle_LTRB(left, y, right + 1, y + 1, CHIP_NONE);
    }

    /// <summary>
    /// 垂直方向に線を引く (上と下の位置は自動で反転する)
    /// </summary>
    /// <param name="top">上</param>
    /// <param name="bottom">下</param>
    /// <param name="x">X座標</param>
    void FillVLine(int top, int bottom, int x) {
        if (top > bottom) {
            // 上下の位置関係が逆なので値をスワップする
            int tmp = top;
            top = bottom;
            bottom = tmp;
        }
        layer.Fill_Rectangle_LTRB(x, top, x + 1, bottom + 1, CHIP_NONE);
    }

    void OnGUI() {
        if (GUI.Button(new Rect(320, 160, 128, 32), "もう１回")) {
            Application.LoadLevel("Main");
        }
    }
}
