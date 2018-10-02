using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンの自動生成モジュール 
/// </summary>
public class Dungeon_Generator : MonoBehaviour {
    /// <summary>
    /// プレイヤー
    /// /// </summary>
    Player player;
    /// <summary>
    /// マップを２次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;
    /// <summary>
    /// エネミーのマネージャークラス
    /// </summary>
    Enemy_Manager enemy_manager;
    /// <summary>
    /// ミニマップを管理するクラス
    /// </summary>
    Mini_Map mini_map;

    /// <summary>
    /// 床オブジェクト
    /// </summary>
    [SerializeField]
    GameObject tile_object;
    /// <summary>
    /// 壁オブジェクト
    /// </summary>
    [SerializeField]
    GameObject wall_object;
    /// <summary>
    /// 階段オブジェクト
    /// </summary>
    [SerializeField]
    GameObject stair_object;
    /// <summary>
    /// アイテムオブジェクト
    /// </summary>
    [SerializeField]
    GameObject item_object;
    /// <summary>
    /// 罠オブジェクト
    /// </summary>
    [SerializeField]
    GameObject trap_object;

    /// <summary>
    /// ダンジョンのオブジェクトを格納用のオブジェクト
    /// </summary>
    [SerializeField]
    GameObject dungeon_object_container;

    /// <summary>
    /// ターンを数える
    /// </summary>
    int turn_count = 0;
    /// <summary>
    /// 部屋番号を数える
    /// </summary>
    int room_counter = 0;

    /// <summary>
    /// 区画リスト
    /// </summary>
    public List<Dungeon_Division> division_list = null;
    
    /// <summary>
    /// 格納用変数。これに入れた値をentrance_positionsに格納していく
    /// </summary>
    Vector2Int entrance_position;

    /// <summary>
    /// 部屋の区画が格納されているリスト
    /// </summary>
    List<Map_Layer_2D> room_list;
    /// <summary>
    /// 部屋ごとの入口の座標。要素数 == ルームナンバー
    /// </summary>
    List<Vector2Int> room_entrance = new List<Vector2Int>();
    /// <summary>
    /// 入口の座標リスト
    /// </summary>
    public List<Vector2Int> entrance_positions = new List<Vector2Int>();

    void Awake() {
        division_list = new List<Dungeon_Division>();
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
    }

    void Start() {
        room_entrance = new List<Vector2Int>();
        enemy_manager = Enemy_Manager.Instance;
        player = Player_Manager.Instance.player_script;
        mini_map = UI_Manager.Instance.mini_map;
    }

    /// <summary>
    /// 新しくダンジョンを作る。開始時、フロア移動時に呼ばれる。
    /// </summary>
    /// <param name="level">どの難易度のダンジョンが呼ばれたか</param>
    public void Load_Dungeon(int level) {
        // ダンジョンの横幅
        int dungeon_width = 0;
        // ダンジョンの縦幅
        int dungeon_height = 0;
        // 敵の出現数をリセット
        Enemy_Manager.Instance.Enemy_Counter = 1;

        Dungeon_Manager.Instance.Is_Exist = true;

        // レベルに合った大きさのダンジョンの生成する
        switch (level) {
            case Define_Value.EASY:
                dungeon_width  = Define_Value.EASY_DUNGEON_WIDTH;
                dungeon_height = Define_Value.EASY_DUNGEON_HEIGHT;
                break;
            case Define_Value.NOMAL:
                dungeon_width  = Define_Value.NOMAL_DUNGEON_WIDTH;
                dungeon_height = Define_Value.NOMAL_DUNGEON_HEIGHT;
                break;
            case Define_Value.HARD:
                dungeon_width  = Define_Value.HARD_DUNGEON_WIDTH;
                dungeon_height = Define_Value.HARD_DUNGEON_HEIGHT;
                break;
        }

        // 初期化
        map_layer.Initialise(dungeon_width, dungeon_height);
        division_list = new List<Dungeon_Division>();

        // すべてを壁にする
        map_layer.Fill(Define_Value.WALL_LAYER_NUMBER);

        // 最初の区画を作る。カメラが画面外を移さないように余分に作る //TODO: 作らないでも余分の壁の画像を１枚貼ってもいいか
        Create_Division(Define_Value.CAMERA_IMAGE_WIDTH, Define_Value.CAMERA_IMAGE_HEIGHT, 
                        dungeon_width - Define_Value.CAMERA_IMAGE_WIDTH, dungeon_height - Define_Value.CAMERA_IMAGE_HEIGHT);

        // 区画を分割する
        // 垂直 or 水平分割フラグの決定
        bool vertical = (Random.Range(0, 2) == 0);
        Split_Divison(vertical);

        // 区画に部屋を作る
        Create_Room();

        // 部屋同士をつなぐ
        Connect_Rooms();

        // 入口(出口)となる部分に専用のレイヤーを貼る
        Set_Entrance_Position();

        // 部屋の壁をセットする
        Room_Layer_Initialize();

        // プレイヤーをスポーンさせる部屋を乱数で決定する
        Set_Player();
        //TODO:中身を作り終えたらスポーンさせる
        // 引数に入れたオブジェクトをランダムで複数個配置する
        Set_Object(Define_Value.ENEMY_LAYER_NUMBER);
        //Random_Object(Define_Value.ITEM_LAYER_NUMBER);
        //Random_Object(Define_Value.TRAP_LAYER_NUMBER);
        // 階段を配置。 階段は１つのみなので、↑とは別口で
        Create_Stair();

        var instance_position = Vector3.zero;
        // タイルを配置
        for (int x = 0; x < map_layer.Width; ++x) {
            for (int y = 0; y < map_layer.Height; ++y) {
                // 壁以外であれば床用の画像を配置
                if (!map_layer.Is_Wall(x, y)) {
                    var obj = Instantiate(tile_object, instance_position, Quaternion.identity);
                    obj.transform.parent = dungeon_object_container.transform;
                }

                // 壁であれば壁用の画像を配置
                if (map_layer.Is_Wall(x, y)) {
                    var obj = Instantiate(wall_object, instance_position, Quaternion.identity);
                    obj.transform.parent = dungeon_object_container.transform;
                }
                // プレイヤーであればプレイヤーを配置
                else if (map_layer.Is_Player(x, y)) {
                    Vector2Int player_position = new Vector2Int(x, y);
                    map_layer.Set(x, y, Define_Value.PLAYER_LAYER_NUMBER);
                    // TODO:足元のものを取って来たい
                    player.Set_Feet(Define_Value.ROOM_LAYER_NUMBER);
                    player.Set_Position(player_position);
                    var player_status = Player_Manager.Instance.player_status;
                    player_status.Where_Room(x, y);
                }
                // 階段であれば階段用の画像を配置
                else if (map_layer.Get_Layer_Number(x, y) == Define_Value.STAIR_LAYER_NUMBER) {
                    var obj = Instantiate(stair_object, instance_position, Quaternion.identity);
                    obj.transform.parent = dungeon_object_container.transform;

                }
                // アイテムであればアイテム用の画像を配置
                //TODO: アイテムはレイヤーではなく、落ちてる落ちてないでのboolか何かにするか
                else if (map_layer.Get_Layer_Number(x, y) == Define_Value.ITEM_LAYER_NUMBER) {
                    var obj = Instantiate(item_object, instance_position, Quaternion.identity);
                    obj.transform.parent = dungeon_object_container.transform;
                }
                // エネミーであればエネミーを生成
                else if (map_layer.Is_Enemy(x, y)) {
                    enemy_manager.Create_Enemy(x, y);
                }
                // 罠であれば罠用の画像を配置
                else if (map_layer.Get_Layer_Number(x, y) == Define_Value.TRAP_LAYER_NUMBER) {
                    var obj =Instantiate(trap_object, instance_position, Quaternion.identity);
                    obj.transform.parent = dungeon_object_container.transform;
                }
                instance_position.y += tile_object.GetComponent<SpriteRenderer>().bounds.size.y;
            }
            instance_position.y = 0.0f;
            instance_position.x += tile_object.GetComponent<SpriteRenderer>().bounds.size.x;
        }

        // 配置が完了したらその通りにミニマップに反映
        mini_map.Arrange_Mini_Map();
    }

    /// <summary>
    /// 最初の区画を作る
    /// </summary>
    /// <param name = "left"  >左</param>
    /// <param name = "top"   >上</param>
    /// <param name = "right" >右</param>
    /// <param name = "bottom">下</param>
    void Create_Division(int left, int top, int right, int bottom) {
        Dungeon_Division div = new Dungeon_Division();
        div.Outer.Set_Position(left, top, right, bottom);
        division_list.Add(div);
    }

    /// <summary>
    /// 区画を分割する
    /// </summary>
    /// <param name="is_vertical">垂直分割するかどうか</param>
    void Split_Divison(bool is_vertical) {
        // 末尾の要素を取り出し
        Dungeon_Division parent = division_list[division_list.Count - 1];
        division_list.Remove(parent);

        // 子となる区画を生成
        Dungeon_Division child = new Dungeon_Division();

        if (is_vertical) {
            // 縦方向に分割する
            if (Check_Division_Size(parent.Outer.Height) == false) {
                // 縦の高さが足りない
                // 親区画を戻しておしまい
                division_list.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Outer.Top    + (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE);
            int b = parent.Outer.Bottom - (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE);

            // AB間の距離を求める
            int ab = b - a;

            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, Define_Value.MAX_ROOM);

            // 分割点を決める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set_Position(parent.Outer.Left, p, parent.Outer.Right, parent.Outer.Bottom);

            // 親の下側をp地点まで縮める
            parent.Outer.Bottom = child.Outer.Top;
        }
        else {
            // 横方向に分割する
            if (Check_Division_Size(parent.Outer.Width) == false) {
                // 横幅が足りない
                // 親区画を戻しておしまい
                division_list.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Outer.Left  + (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE);
            int b = parent.Outer.Right - (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE);

            // AB間の距離を求める
            int ab = b - a;

            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, Define_Value.MAX_ROOM);

            // 分割点を求める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set_Position(p, parent.Outer.Top, parent.Outer.Right, parent.Outer.Bottom);

            // 親の右側をp地点まで縮める
            parent.Outer.Right = child.Outer.Left;
        }

        // 次に分割する区画をランダムで決める
        if (Random.Range(0, 2) == 0) {
            // 子を分割する
            division_list.Add(parent);
            division_list.Add(child);
        }
        else {
            // 親を分割する
            division_list.Add(child);
            division_list.Add(parent);
        }
        // 分割処理を再帰呼び出し(分割方向は縦横交互にする)
        Split_Divison(!is_vertical);
    }

    /// <summary>
    /// プレイヤーをランダムの位置に配置
    /// </summary>
    public void Set_Player() {
        // プレイヤーのランダム部屋号
        int random_division = Random.Range(0, division_list.Count);

        for (int i = 0; i < division_list.Count; i++) {
            if (i == random_division) {
                int x = Random.Range(division_list[i].Room.Left, division_list[i].Room.Right);
                int y = Random.Range(division_list[i].Room.Top,  division_list[i].Room.Bottom);
                map_layer.Fill_Tile(x, y, Define_Value.PLAYER_LAYER_NUMBER);
            }
        }
    }

    /// <summary>
    /// 階段を作る 最後に生成する
    /// </summary>
    void Create_Stair() {
        bool is_found = false;

        while (!is_found) {
            //ランダム部屋号
            int random_div = Random.Range(0, division_list.Count);
            int x = 0; 
            int y = 0;

            for (int i = 0; i < division_list.Count; i++) {
                //部屋番号を探す
                if (i == random_div) {
                    x = Random.Range(division_list[i].Room.Left, division_list[i].Room.Right);
                    y = Random.Range(division_list[i].Room.Top, division_list[i].Room.Bottom);
                    map_layer.Fill_Tile(x, y, Define_Value.STAIR_LAYER_NUMBER);
                    is_found = true;
                }
            }
            // プレイヤーを上書きしてしまわないようにする
            if (map_layer.Get_Layer_Number(x, y) == Define_Value.PLAYER_LAYER_NUMBER) {
                continue;
            }
        }
    }

    /// <summary>
    /// 複数生成するものの数を乱数で決め、配置する
    /// </summary>
    /// <param name="set_object">生成するオブジェクトの番号</param>
    public void Set_Object(int set_object) {
        var total = 1;
        // ダンジョンの大きさに合わせてオブジェクト数を調整する
        if (Define_Value.EASY_DUNGEON_WIDTH < Define_Value.NOMAL_DUNGEON_WIDTH &&
            Define_Value.EASY_DUNGEON_HEIGHT < Define_Value.NOMAL_DUNGEON_HEIGHT) {
            total = Random.Range(1, 3);
        }
        if (Define_Value.NOMAL_DUNGEON_WIDTH <= Define_Value.HARD_DUNGEON_WIDTH &&
            Define_Value.NOMAL_DUNGEON_HEIGHT <= Define_Value.HARD_DUNGEON_HEIGHT) {
            total = Random.Range(3, 5);
        }

        // １つ以上生成する場合
        if (total != 0) {
            for (int j = 1; j <= total; j++) {
                //ランダム部屋号
                int random_div = Random.Range(0, division_list.Count);
                for (int i = 0; i < division_list.Count; i++) {
                    //部屋番号を探す
                    if (i == random_div) {
                        int x, y;
                        x = Random.Range(division_list[i].Room.Left, division_list[i].Room.Right);
                        y = Random.Range(division_list[i].Room.Top, division_list[i].Room.Bottom);
                        // プレイヤーを上書きしてしまわないようにする
                        if (map_layer.Get_Layer_Number(x, y) == Define_Value.PLAYER_LAYER_NUMBER) {
                            continue;
                        }
                        map_layer.Fill_Tile(x, y, set_object);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 指定のサイズを持つ区画を分割できるかどうか
    /// </summary>
    /// <param name="size">チェックする区画のサイズ</param>
    /// <returns>分割できればtrue</returns>
    bool Check_Division_Size(int size) {
        // (最小の部屋サイズ + 余白)  2分割なので x2 する  +1 して連絡通路用のサイズも残す
        int min = (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE) * 2 + Define_Value.TILE_SCALE;

        return size >= min;
    }

    /// <summary>
    /// 区画に部屋を作る
    /// </summary>
    void Create_Room() {
        // カウンターを初期化
        room_counter = 0;
        foreach (Dungeon_Division div in division_list) {
            // 基準サイズを決める
            int dw = div.Outer.Width  - Define_Value.MERGIN_SIZE;
            int dh = div.Outer.Height - Define_Value.MERGIN_SIZE;

            // 大きさをランダムに決める
            int sw = Random.Range(Define_Value.MIN_ROOM, dw);
            int sh = Random.Range(Define_Value.MIN_ROOM, dh);

            // 最大サイズを超えないようにする
            sw = Mathf.Min(sw, Define_Value.MAX_ROOM);
            sh = Mathf.Min(sh, Define_Value.MAX_ROOM);

            // 空きサイズを計算 (区画 - 部屋)
            int rw = (dw - sw);
            int rh = (dh - sh);

            // 部屋の左下位置を決める
            int rx = Random.Range(0, rw) + Define_Value.POSITION_MERGIN_X;
            int ry = Random.Range(0, rh) + Define_Value.POSITION_MERGIN_Y;

            int left   = div.Outer.Left + rx;
            int right  = left + sw;
            int top    = div.Outer.Top + ry;
            int bottom = top + sh;

            // 部屋のサイズを設定
            div.Room.Set_Position(left, top, right, bottom);

            // 部屋を個別に分けてリストへ格納
            Stack_Room(left, top, right, bottom);

            // 部屋を通れるように床で埋める
            Fill_Dungeon_Rectangle(div.Room);

            ++room_counter;
        }
    }

    /// <summary>
    /// 部屋の座標を格納しておく
    /// </summary>
    /// <param name="left  ">部屋の左辺</param>
    /// <param name="top   ">部屋の城辺</param>
    /// <param name="right ">部屋の右辺</param>
    /// <param name="bottom">部屋の下辺</param>
    void Stack_Room(int left, int top, int right, int bottom) {
        // 部屋の幅を求める 足し引きは部屋のフレーム部
        int width = (right + Define_Value.ROOM_FLAME) - (left - Define_Value.ROOM_FLAME);
        // 同じく高さを求める
        int height = (bottom + Define_Value.ROOM_FLAME) - (top - Define_Value.ROOM_FLAME);

        // 格納する部屋を作る
        Map_Layer_2D layer = new Map_Layer_2D();
        layer.Initialise(width, height);

        Dungeon_Manager.Instance.room_list.Add(layer);
    }

    /// <summary>
	/// Dungeon_Rectangleの範囲を塗りつぶす
    /// </summary>
    /// <param name="rect">矩形情報</param>
    void Fill_Dungeon_Rectangle(Dungeon_Division.Dungeon_Rectangle room) {
        map_layer.Fill_Rectangle_LTRB(room.Left, room.Top, room.Right, room.Bottom, Define_Value.ROOM_LAYER_NUMBER);
    }

    /// <summary>
    /// 通路の番号で埋める(描画自体は床)
    /// </summary>
    /// <param name="road">矩形の通路</param>
    void Fill_Dungeon_Rectangle_Road(Dungeon_Division.Dungeon_Rectangle road) {
        map_layer.Fill_Rectangle_LTRB(road.Left, road.Top, road.Right, road.Bottom, Define_Value.ROAD_LAYER_NUMBER);
    }

    /// <summary>
    /// 部屋同士を通路でつなぐ
    /// </summary>
    void Connect_Rooms() {
        for (int i = 0; i < division_list.Count - 1; i++) {
            // リストの前後の区画は必ず接続できる
            Dungeon_Division a = division_list[i];
            Dungeon_Division b = division_list[i + 1];

            // 2つの部屋をつなぐ通路を作成
            Create_Road(a, b);

            // 孫にも接続する
            for (int j = i + 2; j < division_list.Count; j++) {
                Dungeon_Division c = division_list[j];
                if (Create_Road(a, c, true)) {
                    // 孫に接続できたらおしまい
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 指定した部屋の間を通路でつなぎ、部屋に隣接している通路を入口とする
    /// </summary>
    /// <param name="division_A">部屋1</param>
    /// <param name="division_B">部屋2</param>
	/// <param name="grand_child">孫チェックするかどうか</param>
    /// <returns>繋ぐことができたらtrue</returns>
	bool Create_Road(Dungeon_Division division_A, Dungeon_Division division_B, bool grand_child = false) {
        // 上下でつながっている
        if (division_A.Outer.Bottom == division_B.Outer.Top || division_A.Outer.Top == division_B.Outer.Bottom) {
            // 部屋から伸ばす通路の開始位置を決める
            int x1 = Random.Range(division_A.Room.Left, division_A.Room.Right);
            int x2 = Random.Range(division_B.Room.Left, division_B.Room.Right);
            int y = 0;

            if (grand_child) {
                // すでに通路が存在していたらその情報を使用する
                if (division_A.Has_Road()) {
                    x1 = division_A.Road.Left;
                }
                if (division_B.Has_Road()) {
                    x2 = division_B.Road.Left;
                }
            }

            // A
            // |
            // B (Aが上側)
            if (division_A.Outer.Top > division_B.Outer.Top) {
                y = division_A.Outer.Top + 1;
                // 通路を作成
                division_B.Create_Road(x2, division_B.Room.Bottom + Define_Value.TILE_SCALE, x2 + Define_Value.TILE_SCALE, y);
                // 通路を作成
                division_A.Create_Road(x1, y + Define_Value.TILE_SCALE, x1 + Define_Value.TILE_SCALE, division_A.Room.Top - Define_Value.TILE_SCALE);

                // 入口の座標を保持
                if (map_layer.Get_Layer_Number(x1, division_A.Room.Top) == Define_Value.ROOM_LAYER_NUMBER) {
                    entrance_position = new Vector2Int(x1, division_A.Room.Top - Define_Value.ROOM_FLAME);
                    entrance_positions.Add(entrance_position);
                }
                if (map_layer.Get_Layer_Number(x2, division_B.Room.Bottom - 1) == Define_Value.ROOM_LAYER_NUMBER) {
                    entrance_position = new Vector2Int(x2, division_B.Room.Bottom);
                    entrance_positions.Add(entrance_position);
                }
            }
            // B 
            // | 
            // A (Bが上側)
            else {
                y = division_B.Outer.Top + 1;
                // 通路を作成
                division_A.Create_Road(x1, division_A.Room.Bottom + Define_Value.TILE_SCALE, x1 + Define_Value.TILE_SCALE, y);
                // 通路を作成
                division_B.Create_Road(x2, y, x2 + Define_Value.TILE_SCALE, division_B.Room.Top - Define_Value.TILE_SCALE);

                // 入口の座標を保持
                if (map_layer.Get_Layer_Number(x2, division_B.Room.Top) == Define_Value.ROOM_LAYER_NUMBER) {
                    entrance_position = new Vector2Int(x2, division_B.Room.Top - Define_Value.ROOM_FLAME);
                    entrance_positions.Add(entrance_position);
                }
                if (map_layer.Get_Layer_Number(x1, division_A.Room.Bottom - 1) == Define_Value.ROOM_LAYER_NUMBER) {
                    entrance_position = new Vector2Int(x1, division_A.Room.Bottom);
                    entrance_positions.Add(entrance_position);
                }
            }

            // 通路部分に通路の番号を与える
            Fill_Dungeon_Rectangle_Road(division_A.Road);
            Fill_Dungeon_Rectangle_Road(division_B.Road);

            // 通路同士を接続する
            Fill_Horizontal_Line(x1, x2, y);

            // 通路を作れた
            return true;
        }

        // 左右でつながっている
        if (division_A.Outer.Left == division_B.Outer.Right || division_A.Outer.Right == division_B.Outer.Left) {
            // 部屋から伸ばす通路の開始位置を決める
            int y1 = Random.Range(division_A.Room.Top, division_A.Room.Bottom);
            int y2 = Random.Range(division_B.Room.Top, division_B.Room.Bottom);
            int x = 0;

            if (grand_child) {
                // すでに通路が存在していたらその情報を使う
                if (division_A.Has_Road()) {
                    y1 = division_A.Road.Top;
                }
                if (division_B.Has_Road()) {
                    y2 = division_B.Road.Top;
                }
            }

            // B - A (Bが左側)
            if (division_A.Outer.Left > division_B.Outer.Left) {
                x = division_A.Outer.Left + 1;
                // 通路を作成
                division_B.Create_Road(division_B.Room.Right + Define_Value.TILE_SCALE, y2, x, y2 + Define_Value.TILE_SCALE);
                // 通路を作成
                division_A.Create_Road(x + Define_Value.TILE_SCALE, y1, division_A.Room.Left - Define_Value.TILE_SCALE, y1 + Define_Value.TILE_SCALE);

                // 入口の座標を保持
                if (map_layer.Get_Layer_Number(division_B.Room.Right - 1, y2) == Define_Value.ROOM_LAYER_NUMBER) {
                    entrance_position = new Vector2Int(division_B.Room.Right, y2);
                    entrance_positions.Add(entrance_position);
                }
                if (map_layer.Get_Layer_Number(division_A.Room.Left, y1) == Define_Value.ROOM_LAYER_NUMBER) {
                    entrance_position = new Vector2Int(division_A.Room.Left - Define_Value.ROOM_FLAME, y1);
                    entrance_positions.Add(entrance_position);
                }
            }
            // A - B (Aが左側)
            else {
                x = division_B.Outer.Left + 1;
                // 通路を作成
                division_A.Create_Road(division_A.Room.Right + Define_Value.TILE_SCALE, y1, x, y1 + Define_Value.TILE_SCALE);
                // 通路を作成
                division_B.Create_Road(x, y2, division_B.Room.Left - Define_Value.TILE_SCALE, y2 + Define_Value.TILE_SCALE);

                // 入口の座標を保持
                if (map_layer.Get_Layer_Number(division_A.Room.Right - 1, y1) == Define_Value.ROOM_LAYER_NUMBER) {
                    entrance_position = new Vector2Int(division_A.Room.Right, y1);
                    entrance_positions.Add(entrance_position);
                }
                if (map_layer.Get_Layer_Number(division_B.Room.Left, y2) == Define_Value.ROOM_LAYER_NUMBER) {
                    entrance_position = new Vector2Int(division_B.Room.Left - Define_Value.ROOM_FLAME, y2);
                    entrance_positions.Add(entrance_position);
                }
            }

            // 通路部分に通路の番号を与える
            Fill_Dungeon_Rectangle_Road(division_A.Road);
            Fill_Dungeon_Rectangle_Road(division_B.Road);

            // 通路同士を接続する
            Fill_Virtical_Line(y1, y2, x);

            // 通路を作れた
            return true;
        }
        // つなげなかった
        return false;
    }

    /// <summary>
    /// 入口(出口)を各部屋に割り当てる
    /// </summary>
    void Set_Entrance_Position() {
        // 座標が被っている入口は削除
        for (int i = 0; i < entrance_positions.Count; ++i) {
            for (int j = 1; j < entrance_positions.Count; ++j) {
                // 範囲外にならないように
                if ((i + j) < entrance_positions.Count) {
                    if (entrance_positions[i].x == entrance_positions[i + j].x &&
                        entrance_positions[i].y == entrance_positions[i + j].y) {
                        entrance_positions.RemoveAt(i);
                    }
                }
            }
        }
        // 区画番号 == 部屋番号なので、すべての区画(部屋)を回す
        for (int i = 0; i < division_list.Count; ++i) {
            room_entrance = new List<Vector2Int>();
 
            // 全ての入口の数分回す
            for (int j = 0; j < entrance_positions.Count; ++j) {
                if (Is_Witch_Room(i,j)) {  
                    room_entrance.Add(entrance_positions[j]);
                }
            }
            Dungeon_Manager.Instance.entrance_list.Add(room_entrance);
        }
        // 格納しておいた座標を入口とする
        for (int i = 0; i < entrance_positions.Count; ++i) {
            map_layer.Set((int)entrance_positions[i].x, (int)entrance_positions[i].y, Define_Value.ENTRANCE_LAYER_NUMBER);
        }
    }
    /// <summary>
    /// その入口がどの部屋に隣接しているかを調べる
    /// </summary>
    /// <param name="i">ダンジョンの区画の要素番号</param>
    /// <param name="j">入口を格納してるリストの要素番号</param>
    /// <returns>区画番号に合った入口を見つけたらtrue</returns>
    bool Is_Witch_Room(int i, int j) {
        // 部屋の枠部分(出入り口のある周)も見るため -1 で調整
        return (division_list[i].Room.Left - Define_Value.ROOM_FLAME <= entrance_positions[j].x && entrance_positions[j].x <= division_list[i].Room.Right) &&
               (division_list[i].Room.Top  - Define_Value.ROOM_FLAME <= entrance_positions[j].y && entrance_positions[j].y <= division_list[i].Room.Bottom);
    }
    /// <summary>
    /// 水平方向に線を引く (左と右の位置は自動で反転する)
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="right">右</param>
    /// <param name="y">Y座標</param>
    void Fill_Horizontal_Line(int left, int right, int y) {
        if (left > right) {
            // 左右の位置関係が逆なので値をスワップする
            int tmp = left;
            left = right;
            right = tmp;
        }
        map_layer.Fill_Rectangle_LTRB(left, y, right + Define_Value.TILE_SCALE, y + Define_Value.TILE_SCALE, Define_Value.ROAD_LAYER_NUMBER);
    }

    /// <summary>
    /// 垂直方向に線を引く (上と下の位置は自動で反転する)
    /// </summary>
    /// <param name="top">上</param>
    /// <param name="bottom">下</param>
    /// <param name="x">X座標</param>
    void Fill_Virtical_Line(int top, int bottom, int x) {
        if (top > bottom) {
            // 上下の位置関係が逆なので値をスワップする
            int tmp = top;
            top = bottom;
            bottom = tmp;
        }
        map_layer.Fill_Rectangle_LTRB(x, top, x + Define_Value.TILE_SCALE, bottom + Define_Value.TILE_SCALE, Define_Value.ROAD_LAYER_NUMBER);
    }

    /// <summary>
    /// 部屋のレイヤー番号を初期設定する(変動の無い床と壁)
    /// </summary>
    void Room_Layer_Initialize() {
        room_list = Dungeon_Manager.Instance.room_list;
        // リストの数だけ回す
        for (int i = 0; i < room_list.Count; ++i) {
            // マスをすべて回す
            for (int j = 0; j < room_list[i].coordinates.Length; ++j) {
                // 外周を壁に
                if (j <   room_list[i].Width                                || // 下辺を壁に
                    j %   room_list[i].Width == 0                           || // 右辺を壁に
                    j >= (room_list[i].Height - 1) * room_list[i].Width     || // 上辺を壁に
                    j %   room_list[i].Width ==      room_list[i].Width - 1    /* 左辺を壁に*/) {
                    room_list[i].Set(j, Define_Value.WALL_LAYER_NUMBER);
                }
                // それ以外は床を張る
                else {
                    room_list[i].Set(j, Define_Value.ROOM_LAYER_NUMBER);
                }
            }
        }
    }

    /// <summary>
    /// ダンジョンのターンを進める
    /// </summary>
    public void Turn_Tick() {
        ++turn_count;
        // 20ターンごとに敵をスポーンさせる
        if ((turn_count % Define_Value.SPAWN_INTERVAL) == 0) {
            // ランダム部屋号
            var random_div = Random.Range(0, division_list.Count);
            for (int i = 0; i < division_list.Count; i++) {
                // 部屋号を探す
                if (i == random_div) {
                    int x, y;
                    float position_x = 0, position_y = 0;
        
                    do { // プレイヤーと同じ場所だったらやり直し
                        x = Random.Range(division_list[i].Room.Left, division_list[i].Room.Right);
                        y = Random.Range(division_list[i].Room.Top, division_list[i].Room.Bottom);
                        position_x = Random.Range(division_list[i].Room.Left, division_list[i].Room.Right);
                        position_y = Random.Range(division_list[i].Room.Top, division_list[i].Room.Bottom);
                    } while (transform.position.x == position_x && player.transform.position.y == position_y);

                    enemy_manager.Create_Enemy((int)position_x, (int)position_y);
                }
            }
        }
    }
}
