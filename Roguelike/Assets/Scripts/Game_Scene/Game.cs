using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ゲームシーンに必要なユニーククラス
/// </summary>
public class Game : Unique_Component<Game> {
    /// <summary>
    /// 入力されたキーを流すクラス
    /// </summary>
    public Key_Observer key_observer;
    /// <summary>
    /// マップを２次元配列で管理するクラス
    /// </summary>
    public Map_Layer_2D map_layer_2D;
    /// <summary>
    /// ダメージ計算を行うクラス
    /// </summary>
    public Damage_Calculation damage_calculation;
    /// <summary>
    /// インスタンス生成を行う
    /// </summary>
    void Awake() {
        key_observer = new Key_Observer();
        map_layer_2D = new Map_Layer_2D();
        damage_calculation = new Damage_Calculation();
    }

    void Start() {
        // Spaceキーを押すとメニュー画面が開く
        key_observer.On_Key_Down_AsObservable()
            .Where(key_code => key_code == KeyCode.Space)
            .Subscribe(_ =>
                Debug.Log("メニュー画面を表示")
           ).AddTo(this);
    }

    void Update() {
        // 毎フレームキーが押されていないかチェックする
        key_observer.Key_Check();
    }
}
    /// <summary>
    ///  キー入力を受け付けるクラス
    /// </summary>
    public class Key_Observer {
        Subject<KeyCode> on_key_down_sbject = new Subject<KeyCode>();

        /// <summary>
        /// 押されたキーを流す
        /// </summary>
        public void Key_Check() {
            for (KeyCode key = 0; (int)key < (int)KeyCode.Menu; ++key) {
                if (Input.GetKeyDown(key)) {
                    on_key_down_sbject.OnNext(key);
                    Debug.Log(key);
                }
            }
        }

        /// <summary>
        /// on_key_don_subjectを返す
        /// </summary>
        /// <returns></returns>
        public IObservable<KeyCode> On_Key_Down_AsObservable() {
            return on_key_down_sbject;
        }
    }

/// <summary>
/// マップを２次元配列で管理するクラス
/// </summary>
public class Map_Layer_2D {
    /// <summary>
    /// 領域外を指定したらこれを返す
    /// </summary>
    int out_of_range = -1;
    /// <summary>
    /// マップデータを配列で扱う
    /// </summary>
    int[] coordinates = null;

    /// <summary>
    /// 幅
    /// </summary>
    int width = Define_Value.WIDTH;
    public int Width {
        get {
            return width;
        }
    }

    /// <summary>
    /// 高さ
    /// </summary>
    int height = Define_Value.HEIGHT;
    public int Height {
        get {
            return height;
        }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void  Initialise(int width = 0, int height = 0) {
        if (width > 0 && height > 0) {
            Create(width, height);
        }
    }

    /// <summary>
    /// 大きさを設定する
    /// </summary>
    /// <param name = "set_width" >横幅</param>
    /// <param name = "set_height">縦幅</param>
    public void Create(int set_width, int set_height) {
        width = set_width;
        height = set_height;
        coordinates = new int[Width * Height];
    }

    /// 座標をインデックスに変換する
    public int To_Index(int x, int y) {
        return x + (y * Width);
    }

    /// <summary>
    /// 領域内であるかを判断する
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <returns>領域内であればfalse</returns>
    public bool Is_Out_Of_Range(int x, int y) {
        if (x < 0 || x >= Width) { return true; }
        if (y < 0 || y >= Height) { return true; }

        return false;
    }

    /// <summary>
    /// 値の取得
    /// </summary>
    /// <param name="x">X座標</param>
    /// <param name="y">Y座標</param>
    /// <returns>領域外を指定したらout_of_rangeを返す</returns>
    public int Get(int x, int y) {
        if (Is_Out_Of_Range(x, y)) {
            return out_of_range;
        }

        return coordinates[y * Width + x];
    }

    /// <summary>
    /// 移動や死亡判定、アイテム取得などががなされたときにレイヤーの番号を置き換える
    /// </summary>
    /// <param name="old_coordinates_x">古い座標</param>
    /// <param name="old_coordinates_y">古い座標</param>
    /// <param name="new_coordinates_x">新しい座標</param>
    /// <param name="new_coordinates_y">新しい座標</param>
    public void Repainted_Tile(int old_coordinates_x, int old_coordinates_y,
                               int new_coordinates_x, int new_coordinates_y) {
        Player player = Player.Instance.player;
        Set(old_coordinates_x, old_coordinates_y, player.Get_Feet());
        Set(new_coordinates_x, new_coordinates_y, Define_Value.PLAYER_LAYER_NUMBER);
    }

    /// <summary>
    /// 値の設定
    /// </summary>
    /// <param name = "x">x座標</param>
    /// <param name = "y">y座標</param>
    /// <param name = "v"></param>
    public void Set(int x, int y, int layer_number) {
        if (Is_Out_Of_Range(x, y)) {
            // 領域外を指定した
            return;
        }

        coordinates[y * Width + x] = layer_number;
    }

    /// <summary>
    /// すべてのセルを特定の値で埋める
    /// </summary>
    /// <param name="val">埋める値</param>
    public void Fill(int val) {
        for (int j = 0; j < Height; j++) {
            for (int i = 0; i < Width; i++) {
                Set(i, j, val);
            }
        }
    }

    /// <summary>
    /// 床をセットする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="val"></param>
    public void Fill_Tile(int x, int y, int val = 0) {
        Set(x, y, val);
    }

    /// <summary>
    /// 矩形領域を指定の値で埋める
    /// </summary>
    /// <param name="x">  矩形の左上(X座標)</param>
    /// <param name="y">  矩形の左上(Y座標)</param>
    /// <param name="w">  矩形の幅</param>
    /// <param name="h">  矩形の高さ</param>
    /// <param name="val">埋める値</param>
    public void Fill_Rectangle(int x, int y, int w, int h, int val) {
        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {
                int px = x + i;
                int py = y + j;
                Set(px, py, val);
            }
        }
    }

    /// <summary>
    /// 矩形領域を指定の値で埋める（4点指定)
    /// </summary>
    /// <param name="left">左</param>
    /// <param name="top">上</param>
    /// <param name="right">右</param>
    /// <param name="bottom">下</param>
    /// <param name="val">埋める値</param>
    public void Fill_Rectangle_LTRB(int left, int top, int right, int bottom, int val) {
        Fill_Rectangle(left, top, right - left, bottom - top, val);
    }

    public void Tile_Swap_Before(int new_layer_number, int old_layer_number) {
        new_layer_number = old_layer_number;
    }

    /// <summary>
    /// 移動後やアイテム取得後にレイヤー番号を変更する
    /// </summary>
    /// <param name="layer_nuber">変更をかける座標</param>
    /// <param name="layer_number">変更後のレイヤーナンバー</param>
    public void Tile_Swap_After(int old_layer_number, int new_layer_number) {
        old_layer_number = new_layer_number;
    }
}

/// <summary>
/// ダメージ計算をするクラス プレイヤー、パートナー、エネミーとすべてのアクターが使用する
/// </summary>
public class Damage_Calculation {
    /// <summary>
    /// ダメージ計算をする。自分、敵、味方で共有
    /// </summary>
    /// <param name = "attack">       攻撃する側攻撃力</param>
    /// <param name = "random_number">乱数</param>
    /// <param name = "defence">      攻撃を受ける側の防御力</param>
    /// <returns>実際にHPから減らされる値</returns>
    public float Damage(int attack, int random_number, float defence) {
        float damage;

        damage = attack * random_number / 100 - defence;

        return damage;
    }
}