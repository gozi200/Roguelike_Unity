using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// エネミーの行動を制御するクラス
/// </summary>
public class Enemy_Move {
    /// <summary>
    /// エネミーのマネージャクラス
    /// </summary>
    Enemy_Manager enemy_manager;
    /// <summary>
    /// 区画リスト
    /// </summary>
    List<Dungeon_Division> division_list;
    /// <summary>
    /// 自分がアタッチされているオブジェクト
    /// </summary>
    GameObject enemy_object;

    /// <summary>
    /// 入口(出口)の座標を知っておく
    /// </summary>
    Vector2Int goal_position = new Vector2Int();
    /// <summary>
    /// 目標地点の１つ前の座標を格納
    /// </summary>
    List<Point2> goal_before_one;

    /// <summary>
    /// アタッチしているオブジェクトが現在いる部屋の番号
    /// </summary>
    int now_room_number;
    public int Now_Room_Number {
        get { return now_room_number; }
    }

    /// <summary>
    /// アタッチされているエネミーの番号
    /// </summary>
    int enemy_number;
    /// <summary>
    /// 移動が終了したかを判断
    /// </summary>
    bool move_end;

    /// <summary>
    /// 座標を扱う構造体を格納
    /// </summary>
    public List<Point2> point_list;
    public struct Point2 {
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

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(GameObject enemy_object, int enemy_number) {
        enemy_manager = Enemy_Manager.Instance;
        Set_Enemy_Object(enemy_object);
        Set_Enemy_Number(enemy_number);
        now_room_number = enemy_object.GetComponent<Enemy_Controller>().enemy_status.Now_Room;
        division_list = Dungeon_Manager.Instance.dungeon_generator.division_list;
    }

    /// <summary>
    /// 自分がアタッチされている敵をセットする
    /// </summary>
    /// <param name="set_enemy_object">自分がアタッチされているオブジェクト</param>
    void Set_Enemy_Object(GameObject set_enemy_object) {
        enemy_object = set_enemy_object;
    }

    /// <summary>
    /// 動かす敵の番号を知っておく
    /// </summary>
    /// <param name="set_enemy_number">何番目の敵か</param>
    public void Set_Enemy_Number(int set_enemy_number) {
        enemy_number = set_enemy_number;
    }

    /// <summary>
    /// ノード(マス)を制作するクラス
    /// </summary>
    class A_Node {
        /// <summary>
        /// 親ノード
        /// </summary>
        A_Node parent = null;

        /// <summary>
        /// 実コスト
        /// </summary>
        int cost = 0;
        /// <summary>
        /// ヒューリスティックコスト
        /// </summary>
        int heuristic = 0;
        /// x座標
        /// </summary>
        int x = 0;
        /// <summary>
        /// y座標
        /// </summary>
        int y = 0;
        /// <summary>
        /// x座標のプロパティ
        /// </summary>
        public int X {
            get { return x; }
        }
        /// <summary>
        /// y座標のプロパティ
        /// </summary>
        public int Y {
            get { return y; }
        }
        /// <summary>
        /// コストのプロパティ
        /// </summary>
        public int Cost {
            get { return cost; }
        }

        /// <summary>
        /// <summary>
        /// ノードの状態
        /// </summary>
        eNode_Status status = eNode_Status.None;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">ノードのx座標</param>
        /// <param name="y">ノードのy座標</param>
        public A_Node(int set_x, int set_y) {
            x = set_x;
            y = set_y;
        }

        /// <summary>
        /// スコアを計算する
        /// </summary>
        /// <returns>計算後のスコア</returns>
        public int GetScore() {
            return cost + heuristic;
        }

        /// <summary>
        /// ヒューリスティック・コストの計算
        /// </summary>
        /// <param name="allowdiag">ななめ移動が可能かどうかのフラグ</param>
        /// <param name="goal_x">目標地点のx座標</param>
        /// <param name="goal_y">目標地点のy座標</param>
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

        /// <summary>
        /// ステータスがNoneかどうか
        /// </summary>
        /// <returns></returns>
        public bool Is_None() {
            return status == eNode_Status.None;
        }

        /// <summary>
        /// ステータスをOpenにする
        /// </summary>
        /// <param name="parent_">親ノード</param>
        /// <param name="cost_">次開けるもののコスト</param>
        public void Open(A_Node set_parent, int set_cost) {
            status = eNode_Status.Open;
            cost = set_cost;
            parent = set_parent;
        }
        /// <summary>
        /// ステータスをClosedにする
        /// </summary>
        public void Close() {
            status = eNode_Status.Closed;
        }

        /// <summary>
        /// パスを取得する
        /// </summary>
        /// <param name="point_list">移動先の座標を格納するリスト</param>
        public void GetPath(List<Point2> point_list) {
            // これはA_NodeのX,Y
            point_list.Add(new Point2(X, Y));
            if (parent != null) {
                parent.GetPath(point_list);
            }
        }
    }

    /// <summary>
    /// ノードを管理するクラス
    /// </summary>
    class A_Node_Manager {
        /// <summary>
        /// 地形レイヤー
        /// </summary>
        Map_Layer_2D layer;
        /// <summary>
        /// 斜め移動を許可するかどうか
        /// </summary>
        bool allowdiag = true;
        /// <summary>
        /// 目標地点の１つ前の座標を格納
        /// </summary>
        List<Point2> goal_before_one = new List<Point2>();
        /// <summary>
        /// オープンしてあるノードを格納
        /// </summary>
        List<A_Node> open_list = null;
        /// <summary>
        /// ノードインスタンス管理
        /// </summary>
        Dictionary<int, A_Node> pool = null;

        /// <summary>
        /// 目標地点のx座標
        /// </summary>
        int goal_x = 0;
        /// <summary>
        /// 目標地点のy座標
        /// </summary>
        int goal_y = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="room_number"          >今いる部屋の番号</param>
        /// <param name="set_goal_x"           >目標地点のx座標</param>
        /// <param name="set_goal_y"           >目標地点のy座標</param>
        /// <param name="set_goal_before_one_x">目標地点の１つ前のx座標</param>
        /// <param name="set_goal_before_one_y">目標地点の１つ前のy座標</param>
        /// <param name="set_allowdiag"        >ななめ移動が可能かどうかのフラグ</param>
        public A_Node_Manager(int room_number, int set_goal_x, int set_goal_y, List<Point2> set_goal_before_one, bool set_allowdiag = true) {
            layer = new Map_Layer_2D();
            allowdiag = set_allowdiag;
            open_list = new List<A_Node>();
            pool = new Dictionary<int, A_Node>();
            goal_x = set_goal_x;
            goal_y = set_goal_y;
            goal_before_one = set_goal_before_one;
            Set_Rectangle(room_number);
        }

        /// <summary>
        /// 区画の範囲を設定する
        /// </summary>
        /// <param name="room_number">今いる部屋の番号</param>
        void Set_Rectangle(int room_number) {
            List<Dungeon_Division> division_list = Dungeon_Manager.Instance.dungeon_generator.division_list;

            // フレーム(出入り口)部分も含めるため１を足し引きして調整する
            int width =  (division_list[room_number].Room.Right + Define_Value.ROOM_FLAME) -
                         (division_list[room_number].Room.Left);
            int height = (division_list[room_number].Room.Bottom) -
                         (division_list[room_number].Room.Top  - Define_Value.ROOM_FLAME);
            layer.Initialise(width, height);
        }

        /// <summary>
        /// ノード生成する
        /// </summary>
        /// <param name="x">そのノードのx座標</param>
        /// <param name="y">そのノードのy座標</param>
        /// <returns></returns>
        public A_Node Get_Node(int x, int y) {
            // 目的地の１つ手前の座標を探す
            foreach (var before_one in goal_before_one) {
                if (x == goal_x || y == goal_y) {
                    allowdiag = false;
                }
            }

            int idx = layer.To_Index(x, y);
            if (pool.ContainsKey(idx)) {
                // 既に存在しているのでプーリングから取得
                return pool[idx];
            }

            // ないので新規作成
            var node = new A_Node(x, y);
            pool[idx] = node;
            // ヒューリスティック・コストを計算する
            node.Calc_Heuristic(allowdiag, goal_x, goal_y);
            return node;
        }

        /// <summary>
        /// ノードをオープンリストに追加する
        /// </summary>
        /// <param name="node">オープンにしたノード</param>
        public void Add_Open_List(A_Node node) {
            open_list.Add(node);
        }

        /// <summary>
        /// ノードをオープンリストから削除する
        /// </summary>
        /// <param name="node">不要ノード</param>
        public void Remove_Open_List(A_Node node) {
            open_list.Remove(node);
        }

        /// <summary>
        /// 指定の座標にあるノードをオープンする
        /// </summary>
        /// <param name="x"     >オープンするノードの座標</param>
        /// <param name="y"     >オープンするノードの座標</param>
        /// <param name="cost"  >オープンするノードのコスト</param>
        /// <param name="parent">親ノード</param>
        /// <returns>オープンしたノード</returns>
        public A_Node Open_Node(int x, int y, int cost, A_Node parent) {
            // 座標をチェック
            if (layer.Is_Out_Of_Range(x, y)) {
                // 領域外
                return null;
            }
            if (layer.Get(x, y) > 1) {
                // 通過できない
                return null;
            }
            // ノードを取得する
            A_Node node = Get_Node(x, y);
            if (node.Is_None() == false) {
                // 既にOpenしているので何もしない
                return null;
            }

            // Openする
            node.Open(parent, cost);

            Add_Open_List(node);

            return node;
        }

        /// <summary>
        /// 周囲をOpenする
        /// </summary>
        /// <param name="parent"></param>
        public void Open_Around(A_Node parent) {
            // 基準座標(X)
            int xbase = parent.X;
            // 基準座標(Y)
            int ybase = parent.Y;
            // コスト
            int cost = parent.Cost;
            // 一歩進むので+1する
            cost += Define_Value.TILE_SCALE;
            if (allowdiag) {
                // 8方向を開く
                for (int j = 0; j <= 4; ++j) {
                    for (int i = 0; i <= 4; ++i) {
                        int x = xbase + i - Define_Value.TILE_SCALE; // -1～1
                        int y = ybase + j - Define_Value.TILE_SCALE; // -1～1
                        Open_Node(x, y, cost, parent);
                    }
                }
            }
            else {
                // 4方向を開く
                var x = xbase;
                var y = ybase;
                // 上から時計回り
                Open_Node(x, y - Define_Value.TILE_SCALE, cost, parent);
                Open_Node(x - Define_Value.TILE_SCALE, y, cost, parent);
                Open_Node(x, y + Define_Value.TILE_SCALE, cost, parent);
                Open_Node(x + Define_Value.TILE_SCALE, y, cost, parent);
            }
        }

        /// <summary>
        /// 最小スコアのノードを取得する
        /// </summary>
        /// <returns>最小スコアのノード</returns>
        public A_Node Search_Min_Score_Node() {
            // 最小スコア
            int min = 9999;
            // 最小実コスト
            int min_cost = 9999;
            A_Node min_node = null;

            foreach (A_Node node in open_list) {
                int score = node.GetScore();
                if (score > min) {
                    // スコアが大きい
                    continue;
                }
                if (score == min && node.Cost >= min_cost) {
                    // スコアが同じときは実コストも比較する
                    continue;
                }
                // 最小値更新.
                min = score;
                min_cost = node.Cost;
                min_node = node;
            }
            return min_node;
        }
    }

    /// <summary>
    /// エネミーの状態合わせた移動処理を行う
    /// </summary>
    public void Move_Action(int index) {
        switch (enemy_manager.enemies[index].GetComponent<Enemy>().mode) {
            case eEnemy_Mode.Move_Floor_Mode:
                Move();
                break;
            case eEnemy_Mode.Move_Road_Mode:
                Road_Move();
                break;
            case eEnemy_Mode.Encounter_Mode:
                Encount_Move();
                break;
        }
    }

    /// <summary>
    /// 部屋にいるときの移動を管理
    /// </summary>
    public void Stack_List() {
        // 現在の座標を取得
        Point2 start = Get_Now_Position(enemy_object.GetComponent<Enemy>());
        // 指定の部屋の入口(出口)を探す
        Point2 goal = Get_Goal_Position(now_room_number);
        // 入口(出口)の１つ前の座標を取得
        goal_before_one = new List<Point2>();
        Get_Goal_Before_One(goal);

        if (goal_before_one.Count == 0) {
            Debug.Log("入り口前取れてない");
        }

        // 今いる座標
        var start_position = new Vector2Int {
            x = start.x,
            y = start.y
        };
        // 今いる部屋の中の座標に置き換える
        start_position = In_Room_Coodinates(start_position);
        // 目的地の座標
        goal_position = new Vector2Int {
            x = goal.x,
            y = goal.y 
        };
        // 今いる部屋の中の座標に置き換える
        goal_position = In_Room_Coodinates(goal_position);

        // 座標を知ってる構造体のリスト
        point_list = new List<Point2>();

        // 斜め移動を許可
        var allowdiag = true;
        var node_manager = new A_Node_Manager(now_room_number, goal.x, goal.y, goal_before_one, allowdiag);

        // スタート地点のノード取得
        // スタート地点なのでコストは「0」
        A_Node node = node_manager.Open_Node((int)start_position.x, (int)start_position.y, 0, null);
        node_manager.Add_Open_List(node);

        // 試行回数
        int counter = 0;
        while (counter < 10) { //TODO:マジックナンバー
            node_manager.Remove_Open_List(node);
            // 周囲を開く
            node_manager.Open_Around(node);
            // 最小スコアのノードを探す.
            node = node_manager.Search_Min_Score_Node();
            if (node == null) {
                // 袋小路なのでおしまい.
                break;
            }
            // ゴールにたどり着いた // TODO:たどり着かない場合がある。
            if (node.X == goal_position.x && node.Y == goal_position.y) {
                node_manager.Remove_Open_List(node);
                // パスを取得する
                node.GetPath(point_list);
                // 反転する
                point_list.Reverse();
                break;
            }
        }
    }

    /// <summary>
    /// 通路にいるときの移動
    /// </summary>
    void Road_Move() {
        var map_layer = Dungeon_Manager.Instance.map_layer_2D;
        var enemy_direction = enemy_object.GetComponent<Enemy>().direction;
        var enemy_position = new Vector2Int();
        // エネミーのx座標
        var enemy_x = enemy_object.transform.position.x;
        // エネミーのy座標
        var enemy_y = enemy_object.transform.position.y;

        switch (enemy_direction) {
            // 上方向への移動
            case eDirection.Up:
                if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                            map_layer.Get(enemy_x, enemy_y + Define_Value.MOVE_VAULE))) {
                    enemy_position.y += Define_Value.MOVE_VAULE;
                }
                else {
                    if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                                map_layer.Get(enemy_x - Define_Value.MOVE_VAULE, enemy_y))) {
                        enemy_direction = eDirection.Right;
                        enemy_position.x -= Define_Value.MOVE_VAULE;

                    }
                    else if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                                     map_layer.Get(enemy_x + Define_Value.MOVE_VAULE, enemy_y))) {
                        enemy_direction = eDirection.Left;
                        enemy_position.x += Define_Value.MOVE_VAULE;
                    }
                }
                break;
            // 右方向への移動
            case eDirection.Right:
                if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                            map_layer.Get(enemy_x + Define_Value.MOVE_VAULE, enemy_y))) {
                    enemy_position.x += Define_Value.MOVE_VAULE;
                }
                else {
                    if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                                map_layer.Get(enemy_x, enemy_y - Define_Value.MOVE_VAULE))) {
                        enemy_direction = eDirection.Right;
                        enemy_position.y -= Define_Value.MOVE_VAULE;

                    }
                    else if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                                     map_layer.Get(enemy_x, enemy_y + Define_Value.MOVE_VAULE))) {
                        enemy_direction = eDirection.Left;
                        enemy_position.y += Define_Value.MOVE_VAULE;
                    }
                }
                break;
            // 下方向への移動
            case eDirection.Down:
                if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                            map_layer.Get(enemy_x, enemy_y - Define_Value.MOVE_VAULE))) {
                    enemy_position.y -= Define_Value.MOVE_VAULE;
                }
                else {
                    if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                                map_layer.Get(enemy_x - Define_Value.MOVE_VAULE, enemy_y))) {
                        enemy_direction = eDirection.Right;
                        enemy_position.x -= Define_Value.MOVE_VAULE;

                    }
                    else if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                                     map_layer.Get(enemy_x + Define_Value.MOVE_VAULE, enemy_y))) {
                        enemy_direction = eDirection.Left;
                        enemy_position.x += Define_Value.MOVE_VAULE;
                    }
                }
                break;
            // 左方向への移動
            case eDirection.Left:
                if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                            map_layer.Get(enemy_x, enemy_y + Define_Value.MOVE_VAULE))) {
                    enemy_position.y += Define_Value.MOVE_VAULE;
                }
                else {
                    if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                                map_layer.Get(enemy_x, enemy_y + Define_Value.MOVE_VAULE))) {
                        enemy_direction = eDirection.Right;
                        enemy_position.y += Define_Value.MOVE_VAULE;

                    }
                    else if (Actor_Action.Move_Check(map_layer.Get(enemy_x, enemy_y),
                                                     map_layer.Get(enemy_x, enemy_y - Define_Value.MOVE_VAULE))) {
                        enemy_direction = eDirection.Left;
                        enemy_position.y -= Define_Value.MOVE_VAULE;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// エンカウント時の移動
    /// </summary>
    void Encount_Move() {
        
    }

    /// <summary>
    /// エネミーを実際に動かす
    /// </summary>
    /// <param name="point_list">移動先をためているリスト</param>
    public void Move() {
        bool is_update = true;
        Vector2Int enemy_position = enemy_object.GetComponent<Enemy>().position;
        var map_layer = Dungeon_Manager.Instance.map_layer_2D;
        var enemy = enemy_object.GetComponent<Enemy>();

        foreach (var p in point_list) {
            if (!is_update) {
                break;
            }

            // 移動後の座標。部屋のではなく、フロアのものに直して代入
            var after_position = new Vector2Int {
                x = p.x + division_list[now_room_number].Room.Left,
                y = p.y + division_list[now_room_number].Room.Top
            };

            // 床の座標を元のものに戻す
            map_layer.Tile_Swap(enemy_object.transform.position, enemy.Feet);

            // 元の座標を覚える
            Vector2Int before_position = enemy_position;
            if (before_position != after_position) {
                is_update = false;
            }
            else { continue; }

            // 移動後の座標に動かす
            enemy_object.transform.position = new Vector3(after_position.x, after_position.y);
            // 本体クラスの座標も動かす
            enemy.Set_Position(after_position);
            Enemy_Action enemy_action = new Enemy_Action();
            Set_Tile(enemy_object.GetComponent<Enemy>().My_Number);
        }

        // ゴールにたどり着いたらリストを破棄
        if (enemy_position == goal_position) {
            point_list.Clear();
        }
    }

    /// <summary>
    /// 現在の座標を取得
    /// </summary>
    /// <param name="actor">動かすもの</param>
    /// <returns>現在の座標</returns>
    Point2 Get_Now_Position(Enemy enemy) {
        Point2 self_position;
        self_position.x = enemy.position.x;
        self_position.y = enemy.position.y;

        return self_position;
    }

    /// <summary>
    /// 目標地点の座標を返す
    /// </summary>
    /// <returns>目標地点の座標</returns>
    Point2 Get_Goal_Position(int now_room) {
        Point2 goal_position;
        List<List<Vector2Int>> room_list_ = Dungeon_Manager.Instance.room_list;
        List<Vector2Int> entrance_position = room_list_[now_room];
        // 入口の中から１つを選ぶ
        int random = Random.Range(0, entrance_position.Count);

        goal_position.x = (int)entrance_position[random].x;
        goal_position.y = (int)entrance_position[random].y;

        return goal_position;
    }

    /// <summary>
    /// 入口の座標から入口の１つ前の座標を探す
    /// </summary>
    /// <param name="goal_position">入口の座標</param>
    void Get_Goal_Before_One(Point2 goal_position) {
        var map_layer = Dungeon_Manager.Instance.map_layer_2D;
        // 目的地の1つ手前
        Point2 goal_before;

        // 上方向に通路が伸びている場合 以下、順に時計回りに検索 // TODO:敵やプレイヤーがいた場合は分からない
        if (map_layer.Get(goal_position.x, goal_position.y - Define_Value.TILE_SCALE) == Define_Value.TILE_LAYER_NUMBER) {
            // 入口の前の右側に当たる場所の座標を代入
            goal_before.x = goal_position.x + Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y - Define_Value.TILE_SCALE;
            // 移動可能地帯でなかったらいらない
            if (map_layer.Get(goal_before.x, goal_before.y) == Define_Value.TILE_LAYER_NUMBER) {
                // 見つけたポイントをフロア全体ではなく、部屋の座標に変換する
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
            // 入口の前の左側に当たる場所の座標を代入
            goal_before.x = goal_position.x - Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y - Define_Value.TILE_SCALE;
            // 移動可能地帯でなかったらいらない
            if (map_layer.Get(goal_before.x, goal_before.y) == Define_Value.TILE_LAYER_NUMBER) {
                // 見つけたをポイントをフロア全体ではなく、部屋の座標に変換する
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
        }
        // 右方向にに通路が伸びている場合
        else if (map_layer.Get(goal_position.x - Define_Value.TILE_SCALE, goal_position.y) == Define_Value.TILE_LAYER_NUMBER) {
            goal_before.x = goal_position.x - Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y + Define_Value.TILE_SCALE;
            if (map_layer.Get(goal_before.x, goal_before.y) == Define_Value.TILE_LAYER_NUMBER) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }

            goal_before.x = goal_position.x - Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y - Define_Value.TILE_SCALE;
            if (map_layer.Get(goal_before.x, goal_before.y) == Define_Value.TILE_LAYER_NUMBER) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
        }
        // 下方向に通路が伸びている場合
        else if (map_layer.Get(goal_position.x, goal_position.y + Define_Value.TILE_SCALE) == Define_Value.TILE_LAYER_NUMBER) {
            goal_before.x = goal_position.x + Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y + Define_Value.TILE_SCALE;
            if (map_layer.Get(goal_before.x, goal_before.y) == Define_Value.TILE_LAYER_NUMBER) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
            goal_before.x = goal_position.x - Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y + Define_Value.TILE_SCALE;
            if (map_layer.Get(goal_before.x, goal_before.y) == Define_Value.TILE_LAYER_NUMBER) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
        }
        // 左方向に通路が伸びている場合
        else if (map_layer.Get(goal_position.x + Define_Value.TILE_SCALE, goal_position.y) == Define_Value.TILE_LAYER_NUMBER) {
            goal_before.x = goal_position.x + Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y + Define_Value.TILE_SCALE;
            if (map_layer.Get(goal_before.x, goal_before.y) == Define_Value.TILE_LAYER_NUMBER) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }

            goal_before.x = goal_position.x + Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y - Define_Value.TILE_SCALE;
            if (map_layer.Get(goal_before.x, goal_before.y) == Define_Value.TILE_LAYER_NUMBER) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
        }
    }

    /// <summary>
    /// フロアの座標ではなく、その部屋での座標に直す
    /// </summary>
    /// <param name="position">フロア全体での座標</param>
    /// <returns>その部屋での座標</returns>
    Vector2Int In_Room_Coodinates(Vector2Int position) {
        position.x -= division_list[now_room_number].Room.Left;
        position.y -= division_list[now_room_number].Room.Top;

        return position;
    }
    /// <summary>
    /// フロアの座標ではなく、その部屋での座標に直す
    /// </summary>
    /// <param name="position">フロア全体での座標</param>
    /// <returns>その部屋での座標</returns>
    Point2 In_Room_Coodinates(Point2 position) {
        position.x -= division_list[now_room_number].Room.Left;
        position.y -= division_list[now_room_number].Room.Top;

        return position;
    }

    /// <summary>
    /// レイヤー番号を入れ替える
    /// </summary>
    /// <param name="index">要素番号。何番目の敵なのか</param>
    public void Set_Tile(int index) {
        var enemy_manager = Enemy_Manager.Instance;
        var dungeon_manager = Dungeon_Manager.Instance;
        // 要素数に使うので0からの値に合わせる
        index -= 1;
        var enemy_position = enemy_manager.enemies[index].GetComponent<Enemy>().position;
        enemy_object.GetComponent<Enemy>().Set_Feet(dungeon_manager.map_layer_2D.Get(enemy_position.x, enemy_position.y));

        dungeon_manager.map_layer_2D.Tile_Swap(enemy_manager.enemies[index].GetComponent<Enemy>().position,
                            Define_Value.ENEMY_LAYER_NUMBER);
        move_end = true;
    }
}
