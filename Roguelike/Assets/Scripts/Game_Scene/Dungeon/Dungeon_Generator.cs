using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// ダンジョンの自動生成モジュール 
/// </summary>
/// TODO:ダンジョンに必要という事で敵もここで作っている。分ける？
public class Dungeon_Generator : MonoBehaviour {
    /// <summary>
    /// エネミー本体のクラス
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// プレイヤー
    /// /// </summary>
    Player player;
    /// <summary>
    /// マップを２次元配列で管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;
    /// <summary>
    /// アクターのマネージャークラス
    /// </summary>
    Actor_Manager actor_manager;

    /// <summary>
    /// 床オブジェクト
    /// </summary>
    public GameObject tile_object;
    /// <summary>
    /// 壁オブジェクト
    /// </summary>
    public GameObject wall_object;
    /// <summary>
    /// 階段オブジェクト
    /// </summary>
    public GameObject stair_object;
    /// <summary>
    /// アイテムオブジェクト
    /// </summary>
    public GameObject item_object;
    /// <summary>
    /// 罠オブジェクト
    /// </summary>
    public GameObject trap_object;
    /// <summary>
    /// エネミーオブジェクト
    /// </summary>
    public GameObject enemy_object;

    /// <summary>
    /// １フロアに存在している敵の数を数える
    /// </summary>
    int enemy_count;
    /// <summary>
    /// ターンを数える
    /// </summary>
    public int turn_count = 0;
    // 部屋番号を数える
    int room_counter = 0;
    /// <summary>
    /// ダンジョンの横幅
    /// </summary>
    public int dungeon_width;
    /// <summary>
    /// ダンジョンの縦幅
    /// </summary>
    public int dungeon_height;
    /// <summary>
    /// 乱数を格納。(スポーンの時に使用)
    /// </summary>
    int spawn_random_enemy;

    /// <summary>
    /// 区画リスト
    /// </summary>
    [SerializeField]
    public List<Dungeon_Division> division_list = null;

    void Awake() {
        enemy = Actor_Manager.Instance.enemy_script;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;
    }

    void Start() {
        player = Actor_Manager.Instance.player_script;
        actor_manager = Actor_Manager.Instance;
    }

    /// <summary>
    /// 新しくダンジョンを作る。フロア移動のたびに呼ばれる。
    /// </summary>
    /// <param name="level">どの難易度のダンジョンが呼ばれたか</param>
    public void Load_Dungeon(int level) {
        // レベルに合った大きさのダンジョンの生成する
        switch (level) {
            case Define_Value.EASY:
                dungeon_width = Define_Value.EASY_DUNGEON_WIDTH;
                dungeon_height = Define_Value.EASY_DUNGEON_HEIGHT;
                break;
            case Define_Value.NOMAL:
                dungeon_width = Define_Value.NOMAL_DUNGEON_WIDTH;
                dungeon_height = Define_Value.NOMAL_DUNGEON_HEIGHT;
                break;
            case Define_Value.HARD:
                dungeon_width = Define_Value.HARD_DUNGEON_WIDTH;
                dungeon_height = Define_Value.HARD_DUNGEON_HEIGHT;
                break;
        }

        // 初期化
        map_layer.Initialise(dungeon_width, dungeon_height);// 区画リスト作成
        division_list = new List<Dungeon_Division>();

        // すべてを壁にする
        map_layer.Fill(Define_Value.WALL_LAYER_NUMBER);

        // 最初の区画を作る
        Create_Division(0, 0, dungeon_width - 1, dungeon_height - 1);

        // 区画を分割する
        // 垂直 or 水平分割フラグの決定
        bool bVertical = (Random.Range(0, 2) == 0);
        Split_Divison(bVertical);

        // 区画に部屋を作る
        Create_Room();

        // 部屋同士をつなぐ
        Connect_Rooms();
        // 使ったカウンターを初期化
        room_counter = 0;

        // プレイヤー、敵、アイテム、階段の位置を決定
        Random_Actor(Define_Value.PLAYER_LAYER_NUMBER);
        //TODO:中身を作り終えたらスポーンさせる
        // 引数に入れたオブジェクトをランダムで複数個配置する
        Random_Object(Define_Value.ENEMY_LAYER_NUMBER);
        //Random_Object(Define_Value.ITEM_LAYER_NUMBER);
        //Random_Object(Define_Value.TRAP_LAYER_NUMBER);
        // 階段を配置。 階段は１つのみなので、↑とは別口で
        Create_Stair();

        Vector3 instance_position = Vector3.zero;
        // タイルを配置
        for (int x = 0; x < map_layer.Get_Width(); ++x) {
            for (int y = 0; y < map_layer.Get_Height(); ++y) {
                // 壁以外であれば床用の画像を配置
                if (map_layer.Get(x, y) != Define_Value.WALL_LAYER_NUMBER) {
                    GameObject instance_tile = Instantiate(tile_object, instance_position, Quaternion.identity);
                }

                // 壁であれば壁用の画像を配置
                if (map_layer.Get(x, y) == Define_Value.WALL_LAYER_NUMBER) {
                    GameObject instance_wall = Instantiate(wall_object, instance_position, Quaternion.identity);
                }
                // プレイヤーであればプレイヤーを配置
                else if (map_layer.Get(x, y) == Define_Value.PLAYER_LAYER_NUMBER) {
                    Vector2 player_position = new Vector2(x, y);
                    map_layer.Set(x, y, Define_Value.PLAYER_LAYER_NUMBER);
                    // TODO:足元のものを取って来たい
                    player.Set_Feet(Define_Value.TILE_LAYER_NUMBER);
                    player.Set_Position(player_position);
                }
                // 階段であれば階段用の画像を配置
                else if (map_layer.Get(x, y) == Define_Value.STAIR_LAYER_NUMBER) {
                    GameObject instance_stair = Instantiate(stair_object, instance_position, Quaternion.identity);
                }
                // アイテムであればアイテム用の画像を配置
                //TODO: アイテムはレイヤーではなく、落ちてる落ちてないでのboolか何かにするか
                else if (map_layer.Get(x, y) == Define_Value.ITEM_LAYER_NUMBER) {
                    GameObject instance_item = Instantiate(item_object, instance_position, Quaternion.identity);
                }
                // エネミーであればエネミーを生成
                else if (map_layer.Get(x, y) == Define_Value.ENEMY_LAYER_NUMBER) {
                    Create_Enemy(x, y);
                }
                // 罠であれば罠用の画像を配置
                else if (map_layer.Get(x, y) == Define_Value.TRAP_LAYER_NUMBER) {
                    GameObject instance_cell = Instantiate(trap_object, instance_position, Quaternion.identity);
                }
                instance_position.y += tile_object.GetComponent<SpriteRenderer>().bounds.size.y;
            }
            instance_position.y = 0.0f;
            instance_position.x += tile_object.GetComponent<SpriteRenderer>().bounds.size.x;
            instance_position.z -= Define_Value.CAMERA_DISTANCE;
        }
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
            int a = parent.Outer.Top + (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE);
            int b = parent.Outer.Bottom - (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE);

            // AB間の距離を求める
            int ab = b - a;

            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, Define_Value.MAX_ROOM);

            // 分割点を決める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            child.Outer.Set_Position(
                parent.Outer.Left, p, parent.Outer.Right, parent.Outer.Bottom);

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
            int a = parent.Outer.Left + (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE);
            int b = parent.Outer.Right - (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE);

            // AB間の距離を求める
            int ab = b - a;

            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, Define_Value.MAX_ROOM);

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
            division_list.Add(parent);
            division_list.Add(child);
        }
        else {
            // 親を分割する
            division_list.Add(child);
            division_list.Add(parent);
        }
        // 分割処理を再帰呼び出し (分割方向は縦横交互にする)
        Split_Divison(!is_vertical);
    }

    /// <summary>
    /// プレイヤーをランダムの位置に配置
    /// </summary>
    /// <param name = "set_player">プレイヤー</param>
    public void Random_Actor(int set_player) {
        // プレイヤーのランダム部屋号
        int random_div1 = Random.Range(0, division_list.Count - 1);
        // 階段のランダム部屋号
        int random_div2 = Random.Range(0, division_list.Count - 1);
        // 部屋番号を探す
        for (int i = 0; i < division_list.Count; i++) {
            if (i == random_div1) {
                // 部屋内のプレイヤーのランダムのタイル
                int x = Random.Range(division_list[i].Room.Left, division_list[i].Room.Right);
                int y = Random.Range(division_list[i].Room.Top, division_list[i].Room.Bottom);
                map_layer.Fill_Tile(x, y, set_player);
            }
        }
    }

    /// <summary>
    /// 階段を作る 最後に生成する
    /// </summary>
    void Create_Stair() {
        //ランダム部屋号
        int random_div = Random.Range(0, division_list.Count);
        for (int i = 0; i < division_list.Count; i++) {
            //部屋番号を探す
            if (i == random_div) {
                int x, y;
                x = Random.Range(division_list[i].Room.Left, division_list[i].Room.Right);
                y = Random.Range(division_list[i].Room.Top, division_list[i].Room.Bottom);
                map_layer.Fill_Tile(x, y, Define_Value.STAIR_LAYER_NUMBER);
            }
        }
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
        int min = (Define_Value.MIN_ROOM + Define_Value.MERGIN_SIZE) * 2 + 1;

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
            int dw = div.Outer.Width - Define_Value.MERGIN_SIZE;
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

            // 部屋の左上位置を決める
            int rx = Random.Range(0, rw) + Define_Value.POSITION_MERGIN;
            int ry = Random.Range(0, rh) + Define_Value.POSITION_MERGIN;

            int left = div.Outer.Left + rx;
            int right = left + sw;
            int top = div.Outer.Top + ry;
            int bottom = top + sh;

            // 部屋のサイズを設定
            div.Room.Set_Position(left, top, right, bottom);

            // 部屋を通れるように床で埋める
            Fill_Dungeon_Rectangle(div.Room);

            // 出来た部屋に番号をつける
            div.Room.room_number = room_counter;
            ++room_counter;
        }
    }

    /// <summary>
	/// Dungeon_Rectangleの範囲を塗りつぶす
    /// </summary>
    /// <param name="rect">矩形情報</param>
    void Fill_Dungeon_Rectangle(Dungeon_Division.Dungeon_Rectangle room) {
        map_layer.Fill_Rectangle_LTRB(room.Left, room.Top, room.Right, room.Bottom, Define_Value.TILE_LAYER_NUMBER);
    }

    /// <summary>
    /// 通路には通路の番号を与える(描画自体は床)
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
    /// 指定した部屋の間を通路でつなぐ
    /// </summary>
    /// <param name="division_A">部屋1</param>
    /// <param name="division_B">部屋2</param>
	/// <param name="grand_child">孫チェックするかどうか</param>
    /// <returns>つなぐことができたらtrue</returns>
	bool Create_Road(Dungeon_Division division_A, Dungeon_Division division_B, bool grand_child = false) {
        bool A_road_exist = false;
        bool B_road_exist = false;
        var entrance_position_A = new Vector2();
        var entrance_position_B = new Vector2();

        // 上下でつながっている
        if (division_A.Outer.Bottom == division_B.Outer.Top || division_A.Outer.Top == division_B.Outer.Bottom) {
            // 部屋から伸ばす通路の開始位置を決める
            int x1 = Random.Range(division_A.Room.Left, division_A.Room.Right);
            int x2 = Random.Range(division_B.Room.Left, division_B.Room.Right);
            int y = 0;

            if (grand_child) {
                // すでに通路が存在していたらその情報を使用する
                if (division_A.HasRoad()) {
                    x1 = division_A.Road.Left;
                    A_road_exist = true;
                }
                if (division_B.HasRoad()) {
                    x2 = division_B.Road.Left;
                    B_road_exist = true;
                }
            }

            // A
            // |
            // B (Aが上側)
            if (division_A.Outer.Top > division_B.Outer.Top) {
                y = division_A.Outer.Top;
                // 通路を作成
                division_B.CreateRoad(x2, division_B.Room.Bottom + 1, x2 + 1, y);
                // 通路を作成
                division_A.CreateRoad(x1, y + 1, x1 + 1, division_A.Room.Top - 2);

                // 入口の座標を取る
                entrance_position_B = new Vector2(x2, division_B.Room.Bottom);
                entrance_position_A = new Vector2(x1, division_A.Room.Top - 1);
                // すでに入口となっているか判断
                if (map_layer.Get((int)entrance_position_B.x, (int)entrance_position_B.y) != Define_Value.ENTRANCE_LAYER_NUMBER) {
                    if (B_road_exist) {
                        for (int i = 0; i < division_list.Count; ++i) {
                            if (division_list[i].Road.Left == x2) {
                                Dungeon_Manager.Instance.room_list[i].entrance.Add(entrance_position_B);
                                map_layer.Set((int)entrance_position_B.x, (int)entrance_position_B.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            }
                        }
                    }
                    if (A_road_exist) {
                        for (int i = 0; i < division_list.Count; ++i) {
                            if (division_list[i].Road.Left == x1) {
                                Dungeon_Manager.Instance.room_list[i].entrance.Add(entrance_position_A);
                                map_layer.Set((int)entrance_position_A.x, (int)entrance_position_A.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            }
                        }
                    }
                        
                    else {
                        // 作った部屋の情報を保持
                        var room_detail = new Room_Detail();
                        // 部屋の入り口となる部分に専用のレイヤー番号を敷く
                        map_layer.Set((int)entrance_position_B.x, (int)entrance_position_B.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                        room_detail.entrance.Add(entrance_position_B);
                        Dungeon_Manager.Instance.room_list.Add(room_detail);

                        if (map_layer.Get((int)entrance_position_A.x, (int)entrance_position_A.y) != Define_Value.ENTRANCE_LAYER_NUMBER) {
                            // 作った部屋の情報を保持
                            var room_detail2 = new Room_Detail();
                            // 部屋の入り口となる部分に専用のレイヤー番号を敷く
                            map_layer.Set((int)entrance_position_A.x, (int)entrance_position_A.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            room_detail2.entrance.Add(entrance_position_A);
                            Dungeon_Manager.Instance.room_list.Add(room_detail2);
                        }
                    }
                }
                A_road_exist = false;
                B_road_exist = false;
            }
            // B 
            // | 
            // A (Bが上側)
            else {
                y = division_B.Outer.Top;
                // 通路を作成
                division_A.CreateRoad(x1, division_A.Room.Bottom + 1, x1 + 1, y);
                // 通路を作成
                division_B.CreateRoad(x2, y, x2 + 1, division_B.Room.Top - 2);

                entrance_position_A = new Vector2(x1, division_A.Room.Bottom);
                entrance_position_B = new Vector2(x2, division_B.Room.Top - 1);
                if (map_layer.Get((int)entrance_position_A.x, (int)entrance_position_A.y) != Define_Value.ENTRANCE_LAYER_NUMBER) {
                    if (A_road_exist) {
                        for (int i = 0; i < division_list.Count; ++i) {
                            if (division_list[i].Road.Left == x1) {
                                Dungeon_Manager.Instance.room_list[i].entrance.Add(entrance_position_A);
                                map_layer.Set((int)entrance_position_A.x, (int)entrance_position_A.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            }
                        }
                    }
                    if (B_road_exist) {
                        for (int i = 0; i < division_list.Count; ++i) {
                            if (division_list[i].Road.Left == x2) {
                                Dungeon_Manager.Instance.room_list[i].entrance.Add(entrance_position_B);
                                map_layer.Set((int)entrance_position_B.x, (int)entrance_position_B.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            }
                        }
                    }
                    else {
                        // 作った部屋の情報を保持
                        var room_detail = new Room_Detail();
                        // 部屋の入り口となる部分に専用のレイヤー番号を敷く
                        map_layer.Set((int)entrance_position_A.x, (int)entrance_position_A.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                        room_detail.entrance.Add(entrance_position_A);
                        Dungeon_Manager.Instance.room_list.Add(room_detail);

                        if (map_layer.Get((int)entrance_position_B.x, (int)entrance_position_B.y) != Define_Value.ENTRANCE_LAYER_NUMBER) {
                            var room_detail2 = new Room_Detail();
                            // 部屋の入り口となる部分に専用のレイヤー番号を敷く
                            map_layer.Set((int)entrance_position_B.x, (int)entrance_position_B.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            room_detail2.entrance.Add(entrance_position_B);
                            Dungeon_Manager.Instance.room_list.Add(room_detail2);
                        }
                    }
                }
                A_road_exist = false;
                B_road_exist = false;
            }
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
                if (division_A.HasRoad()) {
                    y1 = division_A.Road.Top;
                    A_road_exist= true;
                }
                if (division_B.HasRoad()) {
                    y2 = division_B.Road.Top;
                    B_road_exist = true;
                }
            }

            // B - A (Bが左側)
            if (division_A.Outer.Left > division_B.Outer.Left) {
                x = division_A.Outer.Left;
                // 通路を作成
                division_B.CreateRoad(division_B.Room.Right + 1, y2, x, y2 + 1);
                // 通路を作成
                division_A.CreateRoad(x + 1, y1, division_A.Room.Left - 2, y1 + 1);

                // 作った部屋の情報を保持
                var room_detail = new Room_Detail();

                entrance_position_A = new Vector2(division_B.Room.Right, y2);
                entrance_position_B= new Vector2(division_A.Room.Left - 1, y1);
                if (map_layer.Get((int)entrance_position_A.x, (int)entrance_position_A.y) != Define_Value.ENTRANCE_LAYER_NUMBER) {
                    if (A_road_exist) {
                        for (int i = 0; i < division_list.Count; ++i) {
                            if (division_list[i].Road.Left == y2) {
                                Dungeon_Manager.Instance.room_list[i].entrance.Add(entrance_position_A);
                                map_layer.Set((int)entrance_position_A.x, (int)entrance_position_A.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            }
                        }
                    }
                    if (B_road_exist) {
                        for (int i = 0; i < division_list.Count; ++i) {
                            if (division_list[i].Road.Top == y1) {
                                Dungeon_Manager.Instance.room_list[i].entrance.Add(entrance_position_B);
                                map_layer.Set((int)entrance_position_B.x, (int)entrance_position_B.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            }
                        }
                    }
                    else {
                        // 部屋の入り口となる部分に専用のレイヤー番号を敷く
                        map_layer.Set((int)entrance_position_A.x, (int)entrance_position_A.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                        room_detail.entrance.Add(entrance_position_A);
                        Dungeon_Manager.Instance.room_list.Add(room_detail);

                        if (map_layer.Get((int)entrance_position_B.x, (int)entrance_position_B.y) != Define_Value.ENTRANCE_LAYER_NUMBER) {
                            var room_detail2 = new Room_Detail();
                            // 部屋の入り口となる部分に専用のレイヤー番号を敷く
                            map_layer.Set((int)entrance_position_B.x, (int)entrance_position_B.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            room_detail2.entrance.Add(entrance_position_B);
                            Dungeon_Manager.Instance.room_list.Add(room_detail2);
                        }
                    }
                }
                A_road_exist = false;
                B_road_exist = false;
            }
            // A - B (Aが左側)
            else {
                x = division_B.Outer.Left;
                // 通路を作成
                division_A.CreateRoad(division_A.Room.Right + 1, y1, x, y1 + 1);
                // 通路を作成
                division_B.CreateRoad(x, y2, division_B.Room.Left - 2, y2 + 1);

                // 作った部屋の情報を保持
                var room_detail = new Room_Detail();

                entrance_position_A = new Vector2(division_A.Room.Right, y1);
                entrance_position_B = new Vector2(division_B.Room.Left - 1, y2);
                if (map_layer.Get((int)entrance_position_A.x, (int)entrance_position_A.y) != Define_Value.ENTRANCE_LAYER_NUMBER) {
                    if (A_road_exist) {
                        for (int i = 0; i < division_list.Count; ++i) {
                            if (division_list[i].Road.Left == y1) {
                                Dungeon_Manager.Instance.room_list[i].entrance.Add(entrance_position_A);
                                map_layer.Set((int)entrance_position_A.x, (int)entrance_position_A.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            }
                        }
                    }
                    if (B_road_exist) {
                        for (int i = 0; i < division_list.Count; ++i) {
                            if (division_list[i].Road.Top == y1) {
                                Dungeon_Manager.Instance.room_list[i].entrance.Add(entrance_position_B);
                                map_layer.Set((int)entrance_position_B.x, (int)entrance_position_B.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            }
                        }
                    }
                    else {
                        // 部屋の入り口となる部分に専用のレイヤー番号を敷く
                        map_layer.Set((int)entrance_position_A.x, (int)entrance_position_A.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                        room_detail.entrance.Add(entrance_position_A);
                        Dungeon_Manager.Instance.room_list.Add(room_detail);

                        if (map_layer.Get((int)entrance_position_B.x, (int)entrance_position_B.y) != Define_Value.ENTRANCE_LAYER_NUMBER) {
                            var room_detail2 = new Room_Detail();
                            // 部屋の入り口となる部分に専用のレイヤー番号を敷く
                            map_layer.Set((int)entrance_position_B.x, (int)entrance_position_B.y, Define_Value.ENTRANCE_LAYER_NUMBER);
                            room_detail2.entrance.Add(entrance_position_B);
                            Dungeon_Manager.Instance.room_list.Add(room_detail2);
                        }
                    }
                }
                A_road_exist = false;
                B_road_exist = false;
            }
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
        map_layer.Fill_Rectangle_LTRB(left, y, right + 1, y + 1, Define_Value.ROAD_LAYER_NUMBER);
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
        map_layer.Fill_Rectangle_LTRB(x, top, x + 1, bottom + 1, Define_Value.ROAD_LAYER_NUMBER);
    }

    /// <summary>
    ///他のオブジェクトのランダムの位置
    /// </summary>
    public void Random_Object(int set_object) {
        //各オブジェクト
        int total;
        //オブジェクト数
        if (Define_Value.EASY_DUNGEON_WIDTH < Define_Value.NOMAL_DUNGEON_WIDTH &&
            Define_Value.EASY_DUNGEON_HEIGHT < Define_Value.NOMAL_DUNGEON_HEIGHT) {
            total = Random.Range(2, 5);
        }
        else if (Define_Value.NOMAL_DUNGEON_WIDTH <= Define_Value.HARD_DUNGEON_WIDTH &&
                 Define_Value.NOMAL_DUNGEON_HEIGHT <= Define_Value.HARD_DUNGEON_HEIGHT) {
            total = Random.Range(5, 8);
        }
        else {
            total = Random.Range(0, division_list.Count - 1);
        }

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
                        map_layer.Fill_Tile(x, y, set_object);
                    }
                }
            }
        }
        // エネミー生成時はエネミーの数を数える
        if(set_object == Define_Value.ENEMY_LAYER_NUMBER) {
            enemy_count = total;
        }
    }

    /// <summary>
    /// ダンジョンに出現する敵の中からスポーンさせるエネミーを乱数で決める
    /// </summary>
    /// <returns>スポーンさせるエネミーのID</returns>
    void Random_Enemy_Type() {
        var appear_enemy_list = Actor_Manager.Instance.enemy_script.enemy_type;
        var now_floor = Dungeon_Manager.Instance.floor;
        int[] lottery_enemy = new int[appear_enemy_list.Count];

        // 現在の階層から出現階層を調べ、満たしているものを抽出する
        for (int i = 0; i < appear_enemy_list.Count; ++i) {
            if (appear_enemy_list[i].first_floor <= now_floor.Value &&
                appear_enemy_list[i].last_floor >= now_floor.Value) {

                lottery_enemy[i] = appear_enemy_list[i].ID.Value;
            }
        }
        // その階層に出現する敵を乱数で選出
        spawn_random_enemy = Random.Range(lottery_enemy.Min(), lottery_enemy.Max() + 1);
    }

    /// <summary>
    /// エネミーを創る
    /// </summary>
    /// <param name="x">座標(座標のレイヤー番号を見るのに使用)</param>
    /// <param name="y">座標(座標のレイヤー番号を見るのに使用)</param>
    void Create_Enemy(int x, int y) {
        // リストにするので１つずつインスタンスを作る
        enemy_object = new GameObject("Enemy");

        enemy_object.tag = "Enemy";
        enemy_object.AddComponent<Enemy_Status>();
        // どの敵をスポーンさせるか乱数で決める
        Random_Enemy_Type();
        // スポーンするエネミーのステータスを設定する
        enemy_object.GetComponent<Enemy_Status>().Set_Parameter(enemy_object, spawn_random_enemy);

        enemy_object.AddComponent<Enemy>();
        // GetComponentがかさむので１時変数に
        var enemy_script = enemy_object.GetComponent<Enemy>();
        enemy_script.Set_Initialize_Position(x, y);

        //TODO:足元のものを取って来たい
        enemy_script.Set_Feet(Define_Value.TILE_LAYER_NUMBER);
        // 今いる部屋番号を取得
        enemy_object.GetComponent<Enemy_Status>().Where_Floor(x, y);

        enemy_object.AddComponent<A_Star>();
        enemy_object.AddComponent<Enemy_Sprite_Changer>();
        // ダンジョンに出現しているエネミーを格納するものに追加
        actor_manager.enemys.Add(enemy_object);
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
                    } while (player.transform.position.x == position_x && player.transform.position.y == position_y); 
                    // スポーンさせる場所が分かったので、その場所に産む
                    Create_Enemy((int)position_x, (int)position_y);
                }
            }
        }
    }
}