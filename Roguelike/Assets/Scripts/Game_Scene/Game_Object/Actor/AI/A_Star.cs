using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// A-star algorithm
public class A_Star : MonoBehaviour {
    /// <summary>
    /// 区画リスト
    /// </summary>
    List<Dungeon_Division> division_list;
    /// <summary>
    /// アタッチしているオブジェクトが現在いる部屋の番号
    /// </summary>
    int now_room_number;

    /// <summary>
    /// 座標を扱う構造体を格納
    /// </summary>
    List<Point2> point_list;
    struct Point2 {
        public int x;
        public int y;
        public Point2(int x = 0, int y = 0) {
            this.x = x;
            this.y = y;
        }

        public void Set(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    void Start() {
        division_list = Dungeon_Manager.Instance.dungeon_generator.division_list;
        now_room_number = gameObject.GetComponent<Enemy_Status>().now_room;
    }

    /// A-starノード.
    class A_Node {
        enum eStatus {
            None,
            Open,
            Closed,
        }
        /// ステータス
        eStatus status = eStatus.None;
        /// 実コスト
        int cost = 0;
        /// ヒューリスティック・コスト
        int heuristic = 0;
        /// 親ノード
        A_Node parent = null;
        /// 座標
        int _x = 0;
        int _y = 0;
        public int X {
            get { return _x; }
        }
        public int Y {
            get { return _y; }
        }
        public int Cost {
            get { return cost; }
        }

        /// コンストラクタ.
        public A_Node(int x, int y) {
            _x = x;
            _y = y;
        }
        /// スコアを計算する.
        public int GetScore() {
            return cost + heuristic;
        }
        /// ヒューリスティック・コストの計算.
        public void Calc_Heuristic(bool allowdiag, int goal_x, int goal_y) {

            if (allowdiag) {
                // 斜め移動あり
                var dx = (int)Mathf.Abs(goal_x - X);
                var dy = (int)Mathf.Abs(goal_y - Y);
                // 大きい方をコストにする
                heuristic = dx > dy ? dx : dy;
            }
            else {
                // 縦横移動のみ
                var dx = Mathf.Abs(goal_x - X);
                var dy = Mathf.Abs(goal_y - Y);
                heuristic = (int)(dx + dy);
            }
        }
        /// ステータスがNoneかどうか.
        public bool Is_None() {
            return status == eStatus.None;
        }
        /// ステータスをOpenにする.
        public void Open(A_Node parent_, int cost_) {
            status = eStatus.Open;
            cost = cost_;
            parent = parent_;
        }
        /// ステータスをClosedにする.
        public void Close() {
            status = eStatus.Closed;
        }
        /// パスを取得する
        public void GetPath(List<Point2> pList) {
            pList.Add(new Point2(X, Y));
            if (parent != null) {
                parent.GetPath(pList);
            }
        }
    }

    /// A-starノード管理.
    class A_Node_Manager {
        /// 地形レイヤー.
        Map_Layer_2D layer;
        /// 斜め移動を許可するかどうか.
        bool allowdiag = true;
        /// オープンリスト.
        List<A_Node> open_list = null;
        /// ノードインスタンス管理.
        Dictionary<int, A_Node> pool = null;
        /// ゴール座標.
        int goal_x = 0;
        int goal_y = 0;

        // コンストラクタ
        public A_Node_Manager(int room_number, int goal_X, int goal_Y, bool allowdiag_ = true) {
            layer = new Map_Layer_2D();
            allowdiag = allowdiag_;
            open_list = new List<A_Node>();
            pool = new Dictionary<int, A_Node>();
            goal_x = goal_X;
            goal_y = goal_Y;

            Set_Rectangle(room_number);
        }

        /// <summary>
        /// 区画の範囲を設定する
        /// </summary>
        /// <param name="room_number">今いる部屋の番号</param>
        void Set_Rectangle(int room_number) {
            var division_list = Dungeon_Manager.Instance.dungeon_generator.division_list;

            // フレーム(出入り口)部分も含めるため１を足し引きして調整する
            var width = (division_list[room_number].Room.Right + 1) - (division_list[room_number].Room.Left - 1);
            var height = (division_list[room_number].Room.Bottom + 1) - (division_list[room_number].Room.Top -1);
            layer.Initialise(width, height);
        }

        /// ノード生成する.
        public A_Node GetNode(int x, int y) {
            int idx = layer.To_Index(x, y);
            if (pool.ContainsKey(idx)) {
                // 既に存在しているのでプーリングから取得.
                return pool[idx];
            }

            // ないので新規作成.
            var node = new A_Node(x, y);
            pool[idx] = node;
            // ヒューリスティック・コストを計算する.
            node.Calc_Heuristic(allowdiag, goal_x, goal_y);
            return node;
        }
        /// ノードをオープンリストに追加する.
        public void AddOpenList(A_Node node) {
            open_list.Add(node);
        }
        /// ノードをオープンリストから削除する.
        public void RemoveOpenList(A_Node node) {
            open_list.Remove(node);
        }
        /// 指定の座標にあるノードをオープンする.
        public A_Node OpenNode(int x, int y, int cost, A_Node parent) {
            // 座標をチェック.
            if (layer.Is_Out_Of_Range(x, y)) {
                // 領域外.
                return null;
            }
            if (layer.Get(x, y) > 1) {
                // 通過できない.
                return null;
            }
            // ノードを取得する.
            var node = GetNode(x, y);
            if (node.Is_None() == false) {
                // 既にOpenしているので何もしない
                return null;
            }

            // Openする.
            node.Open(parent, cost);
            AddOpenList(node);

            return node;
        }

        /// 周りをOpenする.
        public void OpenAround(A_Node parent) {
            var xbase = parent.X; // 基準座標(X).
            var ybase = parent.Y; // 基準座標(Y).
            var cost = parent.Cost; // コスト.
            cost += 1; // 一歩進むので+1する.
            if (allowdiag) {
                // 8方向を開く.
                for (int j = 0; j < 3; j++) {
                    for (int i = 0; i < 3; i++) {
                        var x = xbase + i - 1; // -1～1
                        var y = ybase + j - 1; // -1～1
                        OpenNode(x, y, cost, parent);
                    }
                }
            }
            else {
                // 4方向を開く.
                var x = xbase;
                var y = ybase;
                OpenNode(x - 1, y, cost, parent); // 右.
                OpenNode(x, y - 1, cost, parent); // 上.
                OpenNode(x + 1, y, cost, parent); // 左.
                OpenNode(x, y + 1, cost, parent); // 下.
            }
        }

        /// 最小スコアのノードを取得する.
        public A_Node SearchMinScoreNodeFromOpenList() {
            // 最小スコア
            int min = 9999;
            // 最小実コスト
            int minCost = 9999;
            A_Node minNode = null;
            foreach (A_Node node in open_list) {
                int score = node.GetScore();
                if (score > min) {
                    // スコアが大きい
                    continue;
                }
                if (score == min && node.Cost >= minCost) {
                    // スコアが同じときは実コストも比較する
                    continue;
                }
                // 最小値更新.
                min = score;
                minCost = node.Cost;
                minNode = node;
            }
            return minNode;
        }
    }

    /// <summary>
    /// 行動
    /// </summary>
    public void Action() {
        // 現在位置を取得
        Point2 start = Get_Now_Position(gameObject);
        // 指定の部屋の入口(出口)を探す
        Point2 goal = Get_Goal_Position(now_room_number);
        // そのフロアのではなく、その部屋のどこにあるかで出す
        var start_position = new Vector2 {
            x = start.x - division_list[now_room_number].Room.Left,
            y = start.y - division_list[now_room_number].Room.Top
        };
        var goal_position = new Vector2 {
            x = goal.x - division_list[now_room_number].Room.Left,
            y = goal.y - division_list[now_room_number].Room.Top
        };
        // 座標を知ってる構造体のリスト
        var point_list = new List<Point2>();

        // A-star実行.
        {
            // 斜め移動を許可
            var allowdiag = true;
            var mgr = new A_Node_Manager(now_room_number, goal.x, goal.y, allowdiag);

            // スタート地点のノード取得
            // スタート地点なのでコストは「0」
            A_Node node = mgr.OpenNode((int)start_position.x, (int)start_position.y, 0, null);
            mgr.AddOpenList(node);
            // 試行回数。100回超えたら強制中断
            int cnt = 0;
            while (cnt < 100) {
                mgr.RemoveOpenList(node);
                // 周囲を開く
                mgr.OpenAround(node);
                // 最小スコアのノードを探す.
                node = mgr.SearchMinScoreNodeFromOpenList();
                if (node == null) {
                    // 袋小路なのでおしまい.
                    break;
                }
                if (node.X == goal_position.x && node.Y == goal_position.y) {
                    // ゴールにたどり着いた.
                    mgr.RemoveOpenList(node);
                    // パスを取得する
                    node.GetPath(point_list);
                    // 反転する
                    point_list.Reverse();
                    break;
                }
            }
        }
        StartCoroutine(Move(point_list));
    }

    /// <summary>
    /// エネミーを実際に動かす
    /// </summary>
    /// <param name="point_list">移動先をためているリスト</param>
    /// <returns>nullを必ず返し、１フレームに２巡以上しないように</returns>
    IEnumerator Move(List<Point2> point_list) {
        foreach (var p in point_list) {
            // 移動後の座標 部屋のではなく、フロアのものに直して代入
            Vector2 after_position = new Vector2 {
                x = p.x + division_list[now_room_number].Room.Left,
                y = p.y + division_list[now_room_number].Room.Top
            };
            gameObject.transform.position = after_position;
            yield return null;
        }
    }

    /// <summary>
    /// 現在の座標を取得
    /// </summary>
    /// <param name="actor">動かすもの</param>
    /// <returns>現在の座標</returns>
    Point2 Get_Now_Position(GameObject actor) {
        Point2 self_position;
        self_position.x = (int)actor.transform.position.x;
        self_position.y = (int)actor.transform.position.y;
        return self_position;
    }

    /// <summary>
    /// 目標地点の座標
    /// </summary>
    /// <returns>目標地点の座標</returns>
    Point2 Get_Goal_Position(int now_room) {
        Point2 goal_position;
        List<Room_Detail> room_list = Dungeon_Manager.Instance.room_list;
        List<Vector2> entrance_position = room_list[now_room].entrance;
        // 入口の中から１つを選ぶ
        int random = Random.Range(0, entrance_position.Count);

        goal_position.x = (int)entrance_position[0].x;
        goal_position.y = (int)entrance_position[0].y;

        return goal_position;
    }
}

