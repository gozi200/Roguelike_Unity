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
        int left = 0;
        public int Left { set { left = value; } get { return left; } }
        /// <summary>
        /// 区画の上辺
        /// </summary>
        int top = 0;
        public int Top { set { top = value; } get { return top; } }
        /// <summary>
        /// 区画の右辺
        /// </summary>
        int right = 0;
        public int Right { set { right = value; } get { return right; } }
        /// <summary>
        /// 区画の底辺
        /// </summary>
        int bottom = 0;
        public int Bottom { set { bottom = value; } get { return bottom; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Dungeon_Rectangle(int left = 0, int top = 0, int right = 0, int bottom = 0) {
            Set_Position(left, top, right, bottom);
        }

        /// <summary>
        /// 各区の各辺の座標をセットする
        /// </summary>
        /// <param name = "set_left"   >左</param>
        /// <param name = "set_top"    >上</param>
        /// <param name = "set_right"  >右</param>
        /// <param name = "set_bottom" >左</param>
        public void Set_Position(int set_left, int set_top, int set_right, int set_bottom) {
            left   = set_left;
            top    = set_top;
            right  = set_right;
            bottom = set_bottom;
        }

        /// <summary>
        /// 幅を返す
        /// </summary>
        public int Width {
            get { return right - left; }
        }

        /// <summary>
        /// 高さを返す
        /// </summary>
        public int Height {
            get { return bottom - top; }
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
