using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ダンジョンの自動生成モジュール
/// </summary>
public class Dungeon_Generator : MonoBehaviour {
    /// <summary>
    /// 区画リスト
    /// </summary>
    [SerializeField]
    public List<Dungeon_Division> divition_List = null;
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
    /// プレイヤーオブジェクト
    /// </summary>
    public GameObject player_object;
    /// <summary>
    /// エネミーオブジェクト
    /// </summary>
    public GameObject enemy_object;

    public List<GameObject> enemy_list = new List<GameObject>();

    Enemy enemy;
    Player player;
    Player_Status player_status;
    Map_Layer_2D map_layer;

    #region 変数

    /// <summary>
    /// １フロアに存在している敵の数を数える
    /// </summary>
    int enemy_count;
    /// <summary>
    /// ターンを数える
    /// </summary>
    public int turn_count = 0;
    /// <summary>
    /// マップ全体の幅
    /// </summary>
    public int dungeon_width = 30;
    /// <summary>
    /// マップ全体の高さ
    /// </summary>
    public int dungeon_height = 30;
    /// <summary>
    /// 最小の部屋サイズ
    /// </summary>
    const int MIN_ROOM = 3;
    /// <summary>
    /// 最大の部屋サイズ
    /// </summary>
    const int MAX_ROOM = 5;
    /// <summary>
    /// 区画と部屋の余白サイズ
    /// </summary>
    const int MERGIN_SIZE = 3;
    /// <summary>
    /// 部屋配置の余白サイズ
    /// </summary>
    const int POSITION_MERGIN = 2;

#endregion

    void Start() {
        enemy = Enemy.Instance.enemy;
        player = Player.Instance.player;
        player_status = Player.Instance.status;
        map_layer = Game.Instance.map_layer_2D;
    }

    /// <summary>
    /// チップ上のX座標を取得する.
    /// </summary>
    /// <param name="x">チップの座標(x座標)</param>
    /// <returns></returns>
    public float Get_Chip_X(int x) {
        var spr = Util.Get_Sprite("Chip1/Grass_Wall_", "");
        var sprW = spr.bounds.size.x;

        return (sprW * x);
    }

    /// <summary>
    /// チップ上のy座標を取得する.
    /// </summary>
    /// <param name ="y"></param>
    /// <returns></returns>
    public float Get_Chip_Y(int y) {
        var spr = Util.Get_Sprite("Chip1/Grass_Wall_", "");
        var sprH = spr.bounds.size.y;

        return (sprH * y);
    }

    public void Load_Dungeon(int level) {
        // 1. 初期化
        // 2次元配列初期化
        map_layer.Initialise(dungeon_width, dungeon_height);

        // 区画リスト作成
        divition_List = new List<Dungeon_Division>();

        // 2. すべてを壁にする
        map_layer.Fill(Define_Value.WALL_LAYER_NUMBER);

        // 3. 最初の区画を作る
        Create_Division(0, 0, dungeon_width - 1, dungeon_height - 1);

        // 4. 区画を分割する
        // 垂直 or 水平分割フラグの決定
        bool bVertical = (Random.Range(0, 2) == 0);
        Split_Divison(bVertical);

        // 5. 区画に部屋を作る
        Create_Room();

        // 6. 部屋同士をつなぐ
        Connect_Rooms();

        // ７．プレイヤー、敵、アイテム、階段、などの位置
        Random_Actor(Define_Value.PLAYER_LAYER_NUMBER, Define_Value.STAIR_LAYER_NUMBER);
        Random_Object(Define_Value.ITEM_LAYER_NUMBER, Define_Value.ENEMY_LAYER_NUMBER, Define_Value.TRAP_LAYER_NUMBER);

        Vector3 instance_pos = Vector3.zero;
        // タイルを配置
        for (int x = 0; x < map_layer.Width; ++x) {
            for (int y = 0; y < map_layer.Height; ++y) {
                // 壁以外であれば床用の画像を配置
                if (map_layer.Get(x, y) != Define_Value.WALL_LAYER_NUMBER) {
                    GameObject instance_cell = Instantiate(tile_object, instance_pos,Quaternion.identity);
                }

                // 壁であれば壁用の画像を配置
                if (map_layer.Get(x, y) == Define_Value.WALL_LAYER_NUMBER) {
                    GameObject instance_wall = Instantiate(wall_object, instance_pos, Quaternion.identity);
                    map_layer.Set(x, y, Define_Value.WALL_LAYER_NUMBER);
                }
                // プレイヤーであればプレイヤー用の画像を配置
                else if (map_layer.Get(x, y) == Define_Value.PLAYER_LAYER_NUMBER) {
                    //TODO: 画像サイズを合わせる
                    player.Set_Initialize_Position(x * Define_Value.SPRITE_SIZE, y * Define_Value.SPRITE_SIZE);
                    player_status.Set_Parameter(Define_Value.OKITA);
                    map_layer.Set(x, y, Define_Value.PLAYER_LAYER_NUMBER);
                    player.Set_Feet(map_layer.Get(x, y));
                }
                // 階段であれば階段用の画像を配置
                else if (map_layer.Get(x, y) == Define_Value.STAIR_LAYER_NUMBER) {
                    GameObject instance_stair = Instantiate(stair_object, instance_pos, Quaternion.identity);
                    map_layer.Set(x, y, Define_Value.STAIR_LAYER_NUMBER);
                }
                // アイテムであればアイテム用の画像を配置
                //TODO: アイテムはレイヤーではなくboolか何かにするか？
                else if (map_layer.Get(x, y) == Define_Value.ITEM_LAYER_NUMBER) {
                    GameObject instance_item = Instantiate(item_object, instance_pos, Quaternion.identity);
                    map_layer.Set(x, y, Define_Value.ITEM_LAYER_NUMBER);
                }
                // 敵であれば敵用の画像を配置
                else if (map_layer.Get(x, y) == Define_Value.ENEMY_LAYER_NUMBER) {
                    Instantiate(enemy_object, instance_pos, Quaternion.identity);
                    enemy.Set_Initialize_Position(x, y);
                    map_layer.Set(x, y, Define_Value.ENEMY_LAYER_NUMBER);
                    enemy_list.Add(enemy_object);
                }
                // 罠であれば罠用の画像を配置
                else if (map_layer.Get(x, y) == Define_Value.TRAP_LAYER_NUMBER) {
                    GameObject instance_cell = Instantiate(trap_object, instance_pos, Quaternion.identity);
                }
                // TODO:マジックナンバー
                instance_pos.y += Define_Value.SPRITE_SIZE;
            }
            instance_pos.y = 0.0f;
            instance_pos.x += Define_Value.SPRITE_SIZE;
        }
        // TODO: 合ってる？
        foreach (GameObject enemy in enemy_list) {
            Enemy_Action.Set_Dungeon_Generator(this);
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
            // 縦方向に分割する
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
            // 横方向に分割する
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
    /// プレイヤーと階段のランダムの位置
    /// </summary>
    /// <param name = "set_player">プレイヤー</param>
    /// <param name = "set_stair"> 階段</param>
    public void Random_Actor(int set_player, int set_stair) {
        // プレイヤーのランダム部屋号
        int random_div1 = Random.Range(0, divition_List.Count);
        // 階段のランダム部屋号（プレイヤーと違う）
        int random_div2;
            random_div2 = Random.Range(0, divition_List.Count);
        // 部屋号を探す
        for (int i = 0; i < divition_List.Count; i++) {
            if (i == random_div1) {
                // 部屋内のプレイヤーのランダムのタイル
                int x = Random.Range(divition_List[i].Room.Left, divition_List[i].Room.Right);
                int y = Random.Range(divition_List[i].Room.Top, divition_List[i].Room.Bottom);
                map_layer.Fill_Tile(x, y, set_player);
            }
            else if (i == random_div2) {
                // 部屋内の階段のランダムのタイル
                int x = Random.Range(divition_List[i].Room.Left, divition_List[i].Room.Right);
                int y = Random.Range(divition_List[i].Room.Top, divition_List[i].Room.Bottom);
                map_layer.Fill_Tile(x, y, set_stair);
            }
        }
    }

    /// <summary>
    ///他のオブジェクトのランダムの位置
    /// </summary>
    // TODO: マジックナンバー
    public void Random_Object(int val1, int val2, int val3) {
        //各オブジェクト
        for (int k = val1; k <= val3; k++) {
            int total;
            //オブジェクト数
            if (k == 6)
                if (dungeon_width < 45 && dungeon_height < 45) {
                    total = Random.Range(3, 6);
                    enemy_count = total;
                }
                else {
                    total = Random.Range(5, 8);
                    enemy_count = total;
                }
            else {
                total = Random.Range(0, divition_List.Count - 1);
            }
               if (total != 0) {
                    for (int j = 1; j <= total; j++) {
                    //ランダム部屋号
                    int random_div = Random.Range(0, divition_List.Count);
                    for (int i = 0; i < divition_List.Count; i++) {
                        //部屋号を探す
                        if (i == random_div) {
                            int x, y;
                                x = Random.Range(divition_List[i].Room.Left, divition_List[i].Room.Right);
                                y = Random.Range(divition_List[i].Room.Top, divition_List[i].Room.Bottom);
                            map_layer.Fill_Tile(x, y, k);
                        }
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
        map_layer.Fill_Rectangle_LTRB(r.Left, r.Top, r.Right, r.Bottom, Define_Value.TILE_LAYER_NUMBER);
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
    /// <param name="divA"       >部屋1</param>
    /// <param name="divB"       >部屋2</param>
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
        map_layer.Fill_Rectangle_LTRB(left, y, right + 1, y + 1, Define_Value.TILE_LAYER_NUMBER);
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
        map_layer.Fill_Rectangle_LTRB(x, top, x + 1, bottom + 1, Define_Value.TILE_LAYER_NUMBER);
    }

    //public void Turn_Tick() {
    //    ++turn_count;
    //
        ////20ターンごとに敵をスポーンさせる
        //if ((turn_count % 20) == 0) {
        //    //ランダム部屋号
        //    int random_div = Random.Range(0, divition_List.Count);
        //    for (int i = 0; i < divition_List.Count; i++) {
        //        //部屋号を探す
        //        if (i == random_div) {
        //            int x, y;
        //            float posx, posy;
        //            //他のオブジェクトの位置と同じだったら繰り返し
        //            do {
        //                x = Random.Range(divition_List[i].Room.Left, divition_List[i].Room.Right);
        //                y = Random.Range(divition_List[i].Room.Top, divition_List[i].Room.Bottom);
        //                posx = Get_Chip_X(Random.Range(divition_List[i].Room.Left, divition_List[i].Room.Right));
        //                posy = Get_Chip_Y(Random.Range(divition_List[i].Room.Top, divition_List[i].Room.Bottom));
        //            } while ((player.transform.position.x == posx && player.transform.position.y == posy) || map_layer.Get(x, y) == CHIP_UP_STAIR || map_layer.Get(x, y) == CHIP_FOOD || map_layer.Get(x, y) == CHIP_ITEM);
        //            enemy.transform.position = new Vector3(posx, posy, 0);
        //            GameObject instance_obj = Instantiate(enemy, enemy.transform.position, Quaternion.identity);
        //            enemy_script = instance_obj.GetComponent<Enemy>(); // TODO: ここのGetComponentどうする
        //            //Enemy_Manager.Set_Enemy(enemy_script);
        //            enemy_list.Add(instance_obj);
        //        }
        //    }
        //}
   // }
}
