using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ダンジョン区画情報
/// </summary>
public class Dungeon_Division {
    /// <summary>
    /// 矩形管理
    /// </summary>
    public class Dungeon_Rectangle {
        /// <summary>
        /// 区画の左辺
        /// </summary>
        public int Left   = 0;
        /// <summary>
        /// 区画の上辺
        /// </summary>
        public int Top    = 0;
        /// <summary>
        /// 区画の右辺
        /// </summary>
        public int Right  = 0;
        /// <summary>
        /// 区画の底辺
        /// </summary>
        public int Bottom = 0;

        /// <summary>
        /// その区画にある部屋に番号を与える
        /// </summary>
        /// TODO:使ってる？
        public int room_number = 0;

        /// <summary>
        /// その部屋の入口(出口)の座標を覚えておく
        /// </summary>
        public List<Vector2> entrance_position = new List<Vector2>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
		public Dungeon_Rectangle(int left = 0, int top = 0, int right = 0, int bottom = 0) {
            Set_Position(left, top, right, bottom);
        }

        /// <summary>
        /// 値をまとめて設定する
        /// </summary>
        /// <param name = "left"   >左</param>
        /// <param name = "top"    >上</param>
        /// <param name = "right"  >右</param>
        /// <param name = "bottom" >左</param>
        public void Set_Position(int left, int top, int right, int bottom) {
            Left   = left;
            Top    = top;
            Right  = right;
            Bottom = bottom;
        }

        /// <summary>
        /// 幅を返す
        /// </summary>
        public int Width {
            get { return Right - Left; }
        }

        /// <summary>
        /// 高さを返す
        /// </summary>
        public int Height {
            get { return Bottom - Top; }
        }

        /// <summary>
        /// 面積 (幅 x 高さ)を返す
        /// </summary>
        public int Measure {
            get { return Width * Height; }
        }

        /// <summary>
        /// 矩形情報をコピーする
        /// </summary>
        /// <param name="rect">コピー元の矩形情報</param>
        public void Copy(Dungeon_Rectangle rect) {
            Left   = rect.Left;
            Top    = rect.Top;
            Right  = rect.Right;
            Bottom = rect.Bottom;
        }
    }

    /// <summary>
    /// 外周の矩形情報
    /// </summary>
    public Dungeon_Rectangle Outer;

    /// <summary>
    /// 区画内に作ったルーム情報
    /// </summary>
    public Dungeon_Rectangle Room;

    /// <summary>
    /// 通路情報
    /// </summary>
    public Dungeon_Rectangle Road;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Dungeon_Division() {
        Outer = new Dungeon_Rectangle();
        Room  = new Dungeon_Rectangle();
        Road  = null;
    }

    /// <summary>
    /// 通路が存在するかどうか
    /// </summary>
    /// <returns>通路が存在すればtrue</returns>
    public bool Has_Road() {
        return Road != null;
    }

    /// <summary>
    /// 通路を作成する
    /// </summary>
    /// <param name = "left"   >左</param>
    /// <param name = "top"    >上</param>
    /// <param name = "right"  >右</param>
    /// <param name = "bottom" >下</param>
    public void Create_Road(int left, int top, int right, int bottom) {
        Road = new Dungeon_Rectangle(left, top, right, bottom);
    }
}
