using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// エネミーの行動を制御
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
    /// エネミーのステータスクラス
    /// </summary>
    Enemy_Status enemy_status;

    /// <summary>
    /// 目標地点の１つ前の座標を格納
    /// </summary>
    List<Vector2Int> goal_before_one;

    /// <summary>
    /// 動く敵が生まれたてかどうか。生まれた時だけ入れない処理がある
    /// </summary>
    bool freshly = true;
    /// <summary>
    /// 部屋から出て、最初の通路移動時のみtrue。1回のみではなく、部屋から出るたびにtrue。
    /// </summary>
    bool first_move_road = true;

    /// <summary>
    /// Moveメソッドで移動先の決定に使用
    /// </summary>
    int counter = 1;

    /// <summary>
    /// 座標を扱う構造体を格納
    /// </summary>
    public List<Vector2Int> point_list;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(GameObject enemy_object) {
        enemy_manager = Enemy_Manager.Instance;
        Set_Enemy_Object(enemy_object);
        enemy_status = enemy_object.GetComponent<Enemy_Controller>().enemy_status;
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
    /// ノード(マス)を制作
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
        public int X {
            get { return x; }
        }
        /// <summary>
        /// y座標
        /// </summary>
        int y = 0;
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
        /// <param name="set_parent">親ノード</param>
        /// <param name="set_cost">次開けるもののコスト</param>
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
        public void GetPath(List<Vector2Int> point_list) {
            // これはA_NodeのX,Y
            point_list.Add(new Vector2Int(X, Y));
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
        /// 目標地点の１つ前の座標を格納
        /// </summary>
        List<Vector2Int> goal_before_one = new List<Vector2Int>();
        /// <summary>
        /// オープンしてあるノードを格納
        /// </summary>
        List<A_Node> open_list = null;
        /// <summary>
        /// ノードインスタンス管理
        /// </summary>
        Dictionary<int, A_Node> pool = null;

        /// <summary>
        /// 斜め移動を許可するかどうか
        /// </summary>
        bool allowdiag;
        /// <summary>
        /// 生まれたて(生成されたターン)かどうか
        /// </summary>
        bool is_freshly;
        /// <summary>
        /// 1回目に入れたくない処理を避ける
        /// </summary>
        bool is_first = true;
        /// <summary>
        /// 目標地点の座標
        /// </summary>
        Vector2Int goal_position = new Vector2Int();
        /// <summary>
        /// 開始地点の座標
        /// </summary>
        Vector2Int start_position = new Vector2Int();

        /// <summary>
        /// 部屋ごとのマップのレイヤー
        /// </summary>
        List<Map_Layer_2D> room_layer = Dungeon_Manager.Instance.room_list;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="room_number"        >今いる部屋の番号</param>
        /// <param name="set_start_position" >開始地点の座標</param>
        /// <param name="set_goal_position"  >目標地点の座標</param>
        /// <param name="set_goal_before_one">目標地点の１つ斜めの座標(通路側ではなく部屋側に１)</param>
        /// <param name="set_allowdiag"      >斜め移動可能かどうかのフラグ</param>
        public A_Node_Manager(
            int room_number,
            bool set_freshly,
            bool set_allowdiag,
            Vector2Int set_start_position,
            Vector2Int set_goal_position,
            List<Vector2Int> set_goal_before_one
            ) {
            open_list = new List<A_Node>();
            pool = new Dictionary<int, A_Node>();

            is_freshly = set_freshly;
            allowdiag = set_allowdiag;
            start_position = set_start_position;
            goal_position = set_goal_position;
            goal_before_one = set_goal_before_one;

            // 入口の場所の番号を変える
            Set_Entrance(room_number);
            // 部屋内にいるエネミーの場所を設定する
            Set_Enemy(room_number);
        }

        /// <summary>
        /// 部屋単位で見たときの入口を設定する
        /// </summary>
        void Set_Entrance(int room_number) {
            room_layer[room_number].Set(start_position.x, start_position.y, Define_Value.ENTRANCE_LAYER_NUMBER);
            room_layer[room_number].Set(goal_position.x,  goal_position.y,  Define_Value.ENTRANCE_LAYER_NUMBER);
        }

        /// <summary>
        /// 部屋単位で見たときのエネミーの場所を設定する
        /// </summary>
        void Set_Enemy(int room_number) {
            room_layer[room_number].Set(start_position.x, start_position.y, Define_Value.ENEMY_LAYER_NUMBER);
        }

        /// <summary>
        /// ノード生成する
        /// </summary>
        /// <param name="x">そのノードのx座標</param>
        /// <param name="y">そのノードのy座標</param>
        /// <returns></returns>
        public A_Node Get_Node(int x, int y, int now_room) {
            int index = room_layer[now_room].To_Index(x, y);
            if (pool.ContainsKey(index)) {
                // 既に存在しているのでプーリングから取得
                return pool[index];
            }

            // ないので新規作成
            var node = new A_Node(x, y);
            pool[index] = node;
            // ヒューリスティック・コストを計算する
            node.Calc_Heuristic(allowdiag, goal_position.x, goal_position.y);
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
        public A_Node Open_Node(int x, int y, int cost, A_Node parent, int now_room) {
            // 領域外であったら移動不可
            if (room_layer[now_room].Is_Out_Of_Range(x, y)) {
                return null;
            }
            // 初回はエネミーの番号が敷かれたところに出すので無視する。部屋の中にしかスポーンしないので無視しても大丈夫
            if (!is_first) {
                // 移動不可領域であったらオープンしない
                if (!room_layer[now_room].Is_Move_Enemy(x, y)) {
                    return null;
                }
            }
            // ノードを取得する
            A_Node node = Get_Node(x, y, now_room);

            // 既にOpenしていたら何もしない
            if (node.Is_None() == false) {
                return null;
            }

            node.Open(parent, cost);

            Add_Open_List(node);

            is_first = false;

            return node;
        }

        /// <summary>
        /// 周囲をOpenする
        /// </summary>
        /// <param name="parent">親ノード</param>
        public void Open_Around(A_Node parent, int now_room) {
            // 基準座標(X)
            int x_base = parent.X;
            // 基準座標(Y)
            int y_base = parent.Y;
            // コスト
            int cost = parent.Cost;
            // 一歩進むので+1する
            cost += Define_Value.TILE_SCALE;

            // 目的地の１つ手前(斜め方向)であれば斜め移動を禁止
            // 生まれたて(生成されたターン)かどうか
            if (!is_freshly) {
                // 出入口にいるとき(進入時)、出入口の１つ前は斜め移動不可
                if (!(start_position.x == parent.X &&
                      start_position.y == parent.Y)) {
                    foreach (var before_one in goal_before_one) {
                        if (!(parent.X == before_one.x && parent.Y == before_one.y)) {
                            allowdiag = true;
                        }
                        else {
                            allowdiag = false;
                        }
                    }
                }
                else {
                    allowdiag = false;
                }
            }
            // 生まれたて(生成されたターン)は出入口に出現することはないので、１つ前のマスのみの検索
            else {
                foreach (var before_one in goal_before_one) {
                    if (!(parent.X == before_one.x && parent.Y == before_one.y)) {
                        allowdiag = true;
                    }
                    else {
                        allowdiag = false;
                    }
                }
            }
            // ななめ方向の移動が許可されていたら8方向から探す
            if (allowdiag) {
                // 8方向を開く
                for (int i = 0; i <= 2; ++i) {
                    for (int j = 0; j <= 2; ++j) {
                        int x = x_base + i - Define_Value.TILE_SCALE; // -1～1
                        int y = y_base + j - Define_Value.TILE_SCALE; // -1～1
                        Open_Node(x, y, cost, parent, now_room);
                    }
                }
            }
            // そうでなければ4方向から
            else {
                // 4方向を開く
                var x = x_base;
                var y = y_base;
                // 上から時計回り
                Open_Node(x, y - Define_Value.TILE_SCALE, cost, parent, now_room);
                Open_Node(x - Define_Value.TILE_SCALE, y, cost, parent, now_room);
                Open_Node(x, y + Define_Value.TILE_SCALE, cost, parent, now_room);
                Open_Node(x + Define_Value.TILE_SCALE, y, cost, parent, now_room);
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
                // 最小値更新
                min = score;
                min_cost = node.Cost;
                min_node = node;
            }
            return min_node;
        }
    }

    /// <summary>
    /// エネミーの状態に合わせた移動処理を行う
    /// </summary>
    public void Move_Action(int index) {
        switch (enemy_manager.enemies[index].GetComponent<Enemy>().mode) {
            case eEnemy_Mode.Move_Room_Mode:
                Room_Move();
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
    /// 出入口までの道順を１歩ずつためる。部屋進入時かつその部屋にプレイヤーがいなかったら呼ばれる
    /// </summary>
    public void Stack_Route_Until_Entrance() {
        // 斜め移動可否のフラグ 初回(部屋侵入時)は入口にいるので、ななめ移動を禁止
        bool allowdiag = false;

        // 現在の座標を取得
        Vector2Int now_position = Get_Now_Position(enemy_object.GetComponent<Enemy>());
        // 今いる部屋の中の座標に変換する
        var start_position = In_Room_Coodinates(now_position);

        // 指定の部屋の入口(出口)を探す
        Vector2Int goal = Get_Goal_Position(enemy_status.Now_Room, now_position);

        // 条件に合っていなかったら抜ける
        if (!freshly) {
            if (goal == now_position) {
                enemy_object.GetComponent<Enemy>().mode = eEnemy_Mode.Move_Road_Mode;
                Reverse_Direction(enemy_object.GetComponent<Enemy>());
                return;
            }
        }
        // ２回目以降からは生まれてでなくする
        freshly = false;

        // 目標地点を、部屋ごとの座標に変換する
        Vector2Int entrance_position = In_Room_Coodinates(goal);
 
        // 入口(出口)の１つ前の座標を取得
        goal_before_one = new List<Vector2Int>();
        Get_Goal_Before_One(goal);

        // 移動先の座標を格納しておく
        point_list = new List<Vector2Int>();

        var node_manager = new A_Node_Manager(enemy_status.Now_Room, freshly, allowdiag, start_position, entrance_position, goal_before_one);

        // スタート地点のノード取得
        // スタート地点なのでコストは「0」かつ親はなし
        A_Node node = node_manager.Open_Node(start_position.x, start_position.y, 0, null, enemy_status.Now_Room);
        node_manager.Add_Open_List(node);

        while (true) {
            node_manager.Remove_Open_List(node);
            node_manager.Open_Around(node, enemy_status.Now_Room);
            node = node_manager.Search_Min_Score_Node();

            // 出口を見つけられなかったら迷子になる
            if (node == null) {
                enemy_object.GetComponent<Enemy>().Is_Lost_Myself = true;
                break;
            }

            // ゴールにたどり着いた
            if (node.X == entrance_position.x && node.Y == entrance_position.y) {
                enemy_object.GetComponent<Enemy>().Is_Lost_Myself = false;

                node_manager.Remove_Open_List(node);
                node.GetPath(point_list);
                point_list.Reverse();
                break;
            }
        }
    }

    /// <summary>
    /// 向きを反転させる 行き止まり部屋に入った時など呼ばれる
    /// </summary>
    /// <param name="enemy">方向転換するエネミー</param>
    void Reverse_Direction(Enemy enemy) {
        switch (enemy.Direction.Value) {
            case eDirection.Up:
                enemy.Direction.Value = eDirection.Down;
                break;
            case eDirection.Right:
                enemy.Direction.Value = eDirection.Left;
                break;
            case eDirection.Down:
                enemy.Direction.Value = eDirection.Up;
                break;
            case eDirection.Left:
                enemy.Direction.Value = eDirection.Right;
                break;
        }
    }

    /// <summary>
    /// エネミーが通路にいるときの移動
    /// </summary>
    void Road_Move() {
        var map_layer = Dungeon_Manager.Instance.map_layer_2D;
        var enemy = enemy_object.GetComponent<Enemy>();
        var enemy_direction = enemy.Direction.Value;

        var after_position = enemy.Position;
        // 見やすくするために短く
        var enemy_x = enemy.Position.x;
        var enemy_y = enemy.Position.y;
        // 移動前の座標を覚えておく
        var now_position = new Vector2Int(enemy_x, enemy_y);

        switch (enemy_direction) {
            // 上方向への移動
            case eDirection.Up:
                if (map_layer.Is_Move_Enemy(enemy_x, enemy_y + Define_Value.MOVE_VALUE)) {
                    after_position += Vector2Int.up;
                }
                // 行き止まりに当たったら左右を検索し、道のあるほうへ行く(左方向優先)
                else if (map_layer.Is_Wall(enemy_x, enemy_y + Define_Value.MOVE_VALUE)) {
                    // 左に道があればそちらへ進む
                    if (map_layer.Is_Move_Enemy(enemy_x - Define_Value.MOVE_VALUE, enemy_y)) {
                        enemy_direction = eDirection.Left;
                        after_position += Vector2Int.left;

                    }
                    // 無かったら右へ 両方ないパターンは無いので最後はここへ入る
                    else if (map_layer.Is_Move_Enemy(enemy_x + Define_Value.MOVE_VALUE, enemy_y)) {
                        enemy_direction = eDirection.Right;
                        after_position += Vector2Int.right;
                    }
                }
                // 移動不可で壁でなかったら残るは敵がいる場合のみ
                else {
                    // Uターンさせる
                    Reverse_Direction(enemy);
                }
                break;
            // 右方向への移動 以下7方向も上方向と同様の処理
            case eDirection.Right:
                if (map_layer.Is_Move_Enemy(enemy_x + Define_Value.MOVE_VALUE, enemy_y)) {
                    after_position += Vector2Int.right;
                }
                else if (map_layer.Is_Wall(enemy_x + Define_Value.MOVE_VALUE, enemy_y)) {
                    if (map_layer.Is_Move_Enemy(enemy_x, enemy_y + Define_Value.MOVE_VALUE)) {
                        enemy_direction = eDirection.Up;
                        after_position += Vector2Int.up;

                    }
                    else if (map_layer.Is_Move_Enemy(enemy_x, enemy_y - Define_Value.MOVE_VALUE)) {
                        enemy_direction = eDirection.Down;
                        after_position += Vector2Int.down;
                    }
                }
                // 移動不可で壁でなかったら残るは敵がいる場合のみ
                else {
                    // Uターンさせる
                    Reverse_Direction(enemy);
                }
                break;
            // 下方向への移動
            case eDirection.Down:
                if (map_layer.Is_Move_Enemy(enemy_x, enemy_y - Define_Value.MOVE_VALUE)) {
                    after_position += Vector2Int.down;
                }
                else if (map_layer.Is_Wall(enemy_x, enemy_y - Define_Value.MOVE_VALUE)) {
                    if (map_layer.Is_Move_Enemy(enemy_x + Define_Value.MOVE_VALUE, enemy_y)) {
                        enemy_direction = eDirection.Right;
                        after_position += Vector2Int.right;

                    }
                    else if (map_layer.Is_Move_Enemy(enemy_x - Define_Value.MOVE_VALUE, enemy_y)) {
                        enemy_direction = eDirection.Left;
                        after_position += Vector2Int.left;
                    }
                }
                // 移動不可で壁でなかったら残るは敵がいる場合のみ
                else {
                    // Uターンさせる
                    Reverse_Direction(enemy);
                }
                break;
            // 左方向への移動
            case eDirection.Left:
                if (map_layer.Is_Move_Enemy(enemy_x - Define_Value.MOVE_VALUE, enemy_y)) {
                    after_position += Vector2Int.left;
                }
                else if(map_layer.Is_Wall(enemy_x - Define_Value.MOVE_VALUE, enemy_y)) {
                    if (map_layer.Is_Move_Enemy(enemy_x, enemy_y - Define_Value.MOVE_VALUE)) {
                        enemy_direction = eDirection.Down;
                        after_position += Vector2Int.down;

                    }
                    else if (map_layer.Is_Move_Enemy(enemy_x, enemy_y + Define_Value.MOVE_VALUE)) {
                        enemy_direction = eDirection.Up;
                        after_position += Vector2Int.up;
                    }
                }
                // 移動不可で壁でなかったら残るは敵がいる場合のみ
                else {
                    // Uターンさせる
                    Reverse_Direction(enemy);
                }
                break;
        }
        Where_Move(now_position, after_position, enemy);
    }

    /// <summary>
    /// エネミーのエンカウント時の移動
    /// </summary>
    void Encount_Move() {

    }

    /// <summary>
    /// エネミーの部屋での移動処理
    /// </summary>
    /// <param name="point_list">移動先をためているリスト</param>
    public void Room_Move() {
        // エネミーのデータ上の座標
        var enemy = enemy_object.GetComponent<Enemy>();
        var enemy_status = enemy_object.GetComponent<Enemy_Controller>().enemy_status;
        Vector2Int now_position = enemy.Position;
        var room_layer = Dungeon_Manager.Instance.room_list;

        if (point_list == null) {
            return;
        }
        if (point_list.Count <= counter) {
            return;
        }

        // 移動先の座標を順番に取る
        var after_point_x = point_list[counter].x;
        var after_point_y = point_list[counter].y;

        // 移動後の座標。部屋のではなく、フロアのものに直して代入
        var after_position = new Vector2Int {
            x = after_point_x + division_list[enemy_status.Now_Room].Room.Left - Define_Value.ROOM_FLAME,
            y = after_point_y + division_list[enemy_status.Now_Room].Room.Top  - Define_Value.ROOM_FLAME
        };

        if (room_layer[enemy_status.Now_Room].Is_Enemy(after_point_x, after_point_y)) {
            //TODO: 再検索する
            return;
        }

        // カウント加算
        ++counter;

        // 移動するにあたっての処理。オブジェクトの移動とか床番号を変えたり
        Where_Move(now_position, after_position, enemy);

        // ゴールにたどり着いたらカウンターを初期化
        if (counter == point_list.Count) {
            //通路に出ることになるのでフラグを立てておく
            first_move_road = true;
            counter = 1;
        }
    }

    /// <summary>
    ///  現在移動している場所に応じて処理を分ける
    /// </summary>
    void Where_Move(Vector2Int now_position, Vector2Int after_position, Enemy enemy) {
        if(enemy.mode == eEnemy_Mode.Move_Road_Mode) {
            Road_Move_Process(now_position, after_position, enemy);
        }
        else if (enemy.mode == eEnemy_Mode.Move_Room_Mode) {
            Room_Move_Process(now_position, after_position, enemy);
        }
    }

    /// <summary>
    /// エネミーのオブジェクトを動かす
    /// </summary>
    /// <param name="before_position">移動前(現在)の座標</param>
    /// <param name="after_position">移動後の座標</param>
    /// <param name="enemy">動くエネミー</param>
    void Room_Move_Process(Vector2Int now_position, Vector2Int after_position, Enemy enemy) {
        var room_layer  = Dungeon_Manager.Instance.room_list;
        var floor_layer = Dungeon_Manager.Instance.map_layer_2D;
        var enemy_status = enemy_object.GetComponent<Enemy_Controller>().enemy_status;

        // 現在の座標(部屋ごとの物)
        var room_now_position = In_Room_Coodinates(now_position);
        // 移動後の座標(部屋ごとの物)
        var room_after_position = In_Room_Coodinates(after_position);
        
        // 移動する方向に合わせて自分の向きも変える
        Direction_Change(now_position.x, now_position.y,
                         after_position.x,  after_position.y, enemy);

        // 本体クラスの座標も動かす
        enemy.Set_Position(after_position);

        // 元のレイヤー番号に戻す
        floor_layer.Tile_Swap(now_position, enemy.Feet);
        // ルーム毎のレイヤーも同様
        room_layer[enemy_status.Now_Room].Tile_Swap(room_now_position, enemy.Feet);

        // 移動後の座標のレイヤーナンバーを取得する
        enemy.Set_Feet(floor_layer.Get_Layer_Number(after_position.x, after_position.y));
        
        // 移動完了後にいる場所を自分のレイヤー番号に
        floor_layer.Tile_Swap(after_position, Define_Value.ENEMY_LAYER_NUMBER);
        // ルーム毎のレイヤーも同様
        room_layer[enemy_status.Now_Room].Tile_Swap(room_after_position, Define_Value.ENEMY_LAYER_NUMBER);
    }

    /// <summary>
    /// エネミーのオブジェクトを動かす
    /// </summary>
    /// <param name="before_position">移動前(現在)の座標</param>
    /// <param name="after_position">移動後の座標</param>
    /// <param name="enemy">移動するエネミー</param>
    void Road_Move_Process(Vector2Int now_position, Vector2Int after_position, Enemy enemy) {
        var room_layer = Dungeon_Manager.Instance.room_list;
        var floor_layer = Dungeon_Manager.Instance.map_layer_2D;
        var enemy_status = enemy_object.GetComponent<Enemy_Controller>().enemy_status;

        //TODO:もっと読みやすくできないか

        if (!first_move_road) {
            // 移動する方向に合わせて自分の向きも変える
            Direction_Change(now_position.x, now_position.y,
                             after_position.x, after_position.y, enemy);

            // 本体クラスの座標も動かす
            enemy.Set_Position(after_position);

            // 元のレイヤー番号に戻す
            floor_layer.Tile_Swap(now_position, enemy.Feet);

            // 移動後の座標のレイヤーナンバーを取得する
            enemy.Set_Feet(floor_layer.Get_Layer_Number(after_position.x, after_position.y));

            // 移動完了後は今いる場所を自分のレイヤー番号に
            floor_layer.Tile_Swap(after_position, Define_Value.ENEMY_LAYER_NUMBER);
        }
        else {
            // 現在の座標(部屋ごとの物)
            var room_now_position = In_Room_Coodinates(now_position);
            // 移動後の座標(部屋ごとの物)
            var room_after_position = In_Room_Coodinates(after_position);

            // 移動する方向に合わせて自分の向きも変える
            Direction_Change(now_position.x, now_position.y,
                             after_position.x, after_position.y, enemy);

            // 本体クラスの座標も動かす
            enemy.Set_Position(after_position);

            // 元のレイヤー番号に戻す
            floor_layer.Tile_Swap(now_position, enemy.Feet);
            // ルーム毎のレイヤーも同様
            room_layer[enemy_status.Now_Room].Tile_Swap(room_now_position, enemy.Feet);

            // 移動後の座標のレイヤーナンバーを取得する
            enemy.Set_Feet(floor_layer.Get_Layer_Number(after_position.x, after_position.y));

            // 移動完了後にいる場所を自分のレイヤー番号に。部屋ごとでは床は0でok
            floor_layer.Tile_Swap(after_position, Define_Value.ENEMY_LAYER_NUMBER);
            // ルーム毎のレイヤーも同様
            room_layer[enemy_status.Now_Room].Tile_Swap(room_after_position, Define_Value.ENEMY_LAYER_NUMBER);

            first_move_road = false;
        }
    }

    /// <summary>
    /// 移動量からどの方向に移動したかを調べる
    /// </summary>
    /// <param name="before_x">移動前のX座標</param>
    /// <param name="before_y">移動前のY座標</param>
    /// <param name="after_x" >移動後のX座標</param>
    /// <param name="after_y" >移動後のY座標</param>
    /// <param name="enemy"   >移動する敵</param>
    void Direction_Change(int before_x, int before_y, int after_x, int after_y, Enemy enemy) {
        // 上
        if (after_x == before_x && after_y == before_y + 1) {
            enemy.Direction.Value = eDirection.Up;
        }
        // 右上
        else if (after_x == before_x + 1 && after_y == before_y + 1) {
            enemy.Direction.Value = eDirection.Upright;
        }
        // 右
        else if (after_x == before_x + 1 && after_y == before_y) {
            enemy.Direction.Value = eDirection.Right;
        }
        // 右下
        else if (after_x == before_x + 1 && after_y == before_y - 1) {
            enemy.Direction.Value = eDirection.Downright;
        }
        // 下
        else if (after_x == before_x && after_y == before_y - 1) {
            enemy.Direction.Value = eDirection.Down;
        }
        // 左下
        else if (after_x == before_x - 1 && after_y == before_y - 1) {
            enemy.Direction.Value = eDirection.Downleft;
        }
        // 左
        else if (after_x == before_x - 1 && after_y == before_y) {
            enemy.Direction.Value = eDirection.Left;
        }
        // 左上
        else if (after_x == before_x - 1 && after_y == before_y + 1) {
            enemy.Direction.Value = eDirection.Upleft;
        }
    }

    /// <summary>
    /// 現在の座標を取得
    /// </summary>
    /// <param name="actor">動かすもの</param>
    /// <returns>現在の座標</returns>
    Vector2Int Get_Now_Position(Enemy enemy) {
        Vector2Int self_position = new Vector2Int {
            x = enemy.Position.x,
            y = enemy.Position.y
        };

        return self_position;
    }

    /// <summary>
    /// 目標地点の座標を返す
    /// </summary>
    /// <returns>目標地点の座標</returns>
    Vector2Int Get_Goal_Position(int now_room, Vector2Int start_position) {
        List<List<Vector2Int>> entrance_list = Dungeon_Manager.Instance.entrance_list;
        List<Vector2Int> entrance_position = entrance_list[now_room];

        // 出口の数を見てどこから出るかを決める
        // １つしかなければUターン
        if (entrance_position.Count == 1) {
            return entrance_position[0];
        }
        else {
            while (true) {
                // 入口の中から１つを選ぶ
                int random = Random.Range(0, entrance_position.Count);

                // 複数ある場合は入ってきた入口以外で検索
                if (!(start_position.x == entrance_position[random].x &&
                      start_position.y == entrance_position[random].y)) {
                    Vector2Int goal_position = new Vector2Int {
                        x = entrance_position[random].x,
                        y = entrance_position[random].y
                    };
                    return goal_position;
                }
            }
        }
    }

    /// <summary>
    /// 入口の座標から入口の１つ前の座標を探す
    /// </summary>
    /// <param name="goal_position">入口の座標</param>
    void Get_Goal_Before_One(Vector2Int goal_position) {
        var map_layer = Dungeon_Manager.Instance.map_layer_2D;
        // 目的地の1つ手前
        Vector2Int goal_before = new Vector2Int();

        // 上方向に通路が伸びている場合
        if (map_layer.Is_Move_Enemy(goal_position.x, goal_position.y - Define_Value.TILE_SCALE)) {
            // 入口の前の右側に当たる場所の座標を代入
            goal_before.x = goal_position.x + Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y - Define_Value.TILE_SCALE;
            // 移動可能地帯でなかったらいらない
            if (map_layer.Is_Move_Enemy(goal_before.x, goal_before.y)) {
                // 見つけたポイントをフロア全体ではなく、部屋の座標に変換する
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }

            // 入口の前の左側に当たる場所の座標を代入
            goal_before.x = goal_position.x - Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y - Define_Value.TILE_SCALE;
            // 移動可能地帯でなかったらいらない
            if (map_layer.Is_Less_Wall(goal_before.x, goal_before.y)) {
                // 見つけたをポイントをフロア全体ではなく、部屋の座標に変換する
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
        }
        // 右方向に通路が伸びている場合 以下、上方向と同様に時計回りで検索
        else if (map_layer.Is_Less_Wall(goal_position.x - Define_Value.TILE_SCALE, goal_position.y)) {
            goal_before.x = goal_position.x - Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y + Define_Value.TILE_SCALE;
            if (map_layer.Is_Less_Wall(goal_before.x, goal_before.y)) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }

            goal_before.x = goal_position.x - Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y - Define_Value.TILE_SCALE;
            if (map_layer.Is_Less_Wall(goal_before.x, goal_before.y)) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
        }
        // 下方向に通路が伸びている場合
        else if (map_layer.Is_Less_Wall(goal_position.x, goal_position.y + Define_Value.TILE_SCALE)) {
            goal_before.x = goal_position.x + Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y + Define_Value.TILE_SCALE;
            if (map_layer.Is_Less_Wall(goal_before.x, goal_before.y)) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }

            goal_before.x = goal_position.x - Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y + Define_Value.TILE_SCALE;
            if (map_layer.Is_Less_Wall(goal_before.x, goal_before.y)) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }
        }
        // 左方向に通路が伸びている場合
        else if (map_layer.Is_Less_Wall(goal_position.x + Define_Value.TILE_SCALE, goal_position.y)) {
            goal_before.x = goal_position.x + Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y + Define_Value.TILE_SCALE;
            if (map_layer.Is_Less_Wall(goal_before.x, goal_before.y)) {
                goal_before = In_Room_Coodinates(goal_before);
                goal_before_one.Add(goal_before);
            }

            goal_before.x = goal_position.x + Define_Value.TILE_SCALE;
            goal_before.y = goal_position.y - Define_Value.TILE_SCALE;
            if (map_layer.Is_Less_Wall(goal_before.x, goal_before.y)) {
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
        // 部屋のフレーム部分を引く
        position.x -= division_list[enemy_status.Now_Room].Room.Left - Define_Value.ROOM_FLAME;
        position.y -= division_list[enemy_status.Now_Room].Room.Top - Define_Value.ROOM_FLAME;

        return position;
    }
}
