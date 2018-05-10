using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
/// A-star algorithm
public class AStar : MonoBehaviour {
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

    /// A-starノード.
    class ANode {
        enum eStatus {
            None,
            Open,
            Closed,
        }
        /// ステータス
        eStatus _status = eStatus.None;
        /// 実コスト
        int _cost = 0;
        /// ヒューリスティック・コスト
        int _heuristic = 0;
        /// 親ノード
        ANode _parent = null;
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
            get { return _cost; }
        }

        /// コンストラクタ.
        public ANode(int x, int y) {
            _x = x;
            _y = y;
        }
        /// スコアを計算する.
        public int GetScore() {
            return _cost + _heuristic;
        }
        /// ヒューリスティック・コストの計算.
        public void CalcHeuristic(bool allowdiag, int xgoal, int ygoal) {

            if (allowdiag) {
                // 斜め移動あり
                var dx = (int)Mathf.Abs(xgoal - X);
                var dy = (int)Mathf.Abs(ygoal - Y);
                // 大きい方をコストにする
                _heuristic = dx > dy ? dx : dy;
            }
            else {
                // 縦横移動のみ
                var dx = Mathf.Abs(xgoal - X);
                var dy = Mathf.Abs(ygoal - Y);
                _heuristic = (int)(dx + dy);
            }
            Dump();
        }
        /// ステータスがNoneかどうか.
        public bool IsNone() {
            return _status == eStatus.None;
        }
        /// ステータスをOpenにする.
        public void Open(ANode parent, int cost) {
            Debug.Log(string.Format("Open: ({0},{1})", X, Y));
            _status = eStatus.Open;
            _cost = cost;
            _parent = parent;
        }
        /// ステータスをClosedにする.
        public void Close() {
            Debug.Log(string.Format("Closed: ({0},{1})", X, Y));
            _status = eStatus.Closed;
        }
        /// パスを取得する
        public void GetPath(List<Point2> pList) {
            pList.Add(new Point2(X, Y));
            if (_parent != null) {
                _parent.GetPath(pList);
            }
        }
        public void Dump() {
            Debug.Log(string.Format("({0},{1})[{2}] cost={3} heuris={4} score={5}", X, Y, _status, _cost, _heuristic, GetScore()));
        }
        public void DumpRecursive() {
            Dump();
            if (_parent != null) {
                // 再帰的にダンプする.
                _parent.DumpRecursive();
            }
        }
    }

    /// A-starノード管理.
    class ANodeMgr {
        /// 地形レイヤー.
        Map_Layer_2D _layer;
        /// 斜め移動を許可するかどうか.
        bool _allowdiag = true;
        /// オープンリスト.
        List<ANode> _openList = null;
        /// ノードインスタンス管理.
        Dictionary<int, ANode> _pool = null;
        /// ゴール座標.
        int _xgoal = 0;
        int _ygoal = 0;
        public ANodeMgr(
            Map_Layer_2D layer, int xgoal, int ygoal, bool allowdiag = true) {
            _layer = layer;
            _allowdiag = allowdiag;
            _openList = new List<ANode>();
            _pool = new Dictionary<int, ANode>();
            _xgoal = xgoal;
            _ygoal = ygoal;
        }
        /// ノード生成する.
        public ANode GetNode(int x, int y) {
            var idx = _layer.To_Index(x, y);
            if (_pool.ContainsKey(idx)) {
                // 既に存在しているのでプーリングから取得.
                return _pool[idx];
            }

            // ないので新規作成.
            var node = new ANode(x, y);
            _pool[idx] = node;
            // ヒューリスティック・コストを計算する.
            node.CalcHeuristic(_allowdiag, _xgoal, _ygoal);
            return node;
        }
        /// ノードをオープンリストに追加する.
        public void AddOpenList(ANode node) {
            _openList.Add(node);
        }
        /// ノードをオープンリストから削除する.
        public void RemoveOpenList(ANode node) {
            _openList.Remove(node);
        }
        /// 指定の座標にあるノードをオープンする.
        public ANode OpenNode(int x, int y, int cost, ANode parent) {
            // 座標をチェック.
            if (_layer.Is_Out_Of_Range(x, y)) {
                // 領域外.
                return null;
            }
            if (_layer.Get(x, y) > 1) {
                // 通過できない.
                return null;
            }
            // ノードを取得する.
            var node = GetNode(x, y);
            if (node.IsNone() == false) {
                // 既にOpenしているので何もしない
                return null;
            }

            // Openする.
            node.Open(parent, cost);
            AddOpenList(node);

            return node;
        }

        /// 周りをOpenする.
        public void OpenAround(ANode parent) {
            var xbase = parent.X; // 基準座標(X).
            var ybase = parent.Y; // 基準座標(Y).
            var cost = parent.Cost; // コスト.
            cost += 1; // 一歩進むので+1する.
            if (_allowdiag) {
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
        public ANode SearchMinScoreNodeFromOpenList() {
            // 最小スコア
            int min = 9999;
            // 最小実コスト
            int minCost = 9999;
            ANode minNode = null;
            foreach (ANode node in _openList) {
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

    // 状態.
    enum eState {
        Exec, // 実行中.
        Walk, // 移動中.
        End,  // おしまい.
    }
    eState _state = eState.Exec;
    IEnumerator Start() {

///////////var layer = 部屋の大きさ;
        // ゴール.
///////////Point2 start = Get_Now_Position(layer);
///////////Point2 goal = 部屋の出口;

        var pList = new List<Point2>();        

        // A-star実行.
        {
            // 斜め移動を許可
            var allowdiag = true;
            var mgr = new ANodeMgr(layer, goal.x, goal.y, allowdiag);

            // スタート地点のノード取得
            // スタート地点なのでコストは「0」
            ANode node = mgr.OpenNode(start.x, start.y, 0, null);
            mgr.AddOpenList(node);

            // 試行回数。10回超えたら強制中断
            int cnt = 0;
            while (cnt < 10) {
                mgr.RemoveOpenList(node);
                // 周囲を開く
                mgr.OpenAround(node);
                // 最小スコアのノードを探す.
                node = mgr.SearchMinScoreNodeFromOpenList();
                if (node == null) {
                    // 袋小路なのでおしまい.
                    Debug.Log("Not found path.");
                    break;
                }
                if (node.X == goal.x && node.Y == goal.y) {
                    // ゴールにたどり着いた.
                    Debug.Log("Success.");
                    mgr.RemoveOpenList(node);
                    node.DumpRecursive();
                    // パスを取得する
                    node.GetPath(pList);
                    // 反転する
                    pList.Reverse();
                    break;
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

        _state = eState.Walk;

        // おしまい
        _state = eState.End;
    }

    Point2 Get_Now_Position(Map_Layer_2D layer) {
        Point2 player_position;
        var player = Actor_Manager.Instance.player_script;
        player_position.x = (int)player.transform.position.x;
        player_position.y = (int)player.transform.position.y;
        return player_position;
    }
}
*/