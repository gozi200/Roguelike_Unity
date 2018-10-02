using UnityEngine;

/// <summary>
/// ミニマップを生成、表示する
/// </summary>
public class Mini_Map : MonoBehaviour {
    /// <summary>
    /// ゲームのマネージャークラス 現在のいる場所を知るのに使用
    /// </summary>
    GameManager game_manager;
    /// <summary>
    /// ダンジョンのマップを管理するクラス
    /// </summary>
    Map_Layer_2D map_layer;

    /// <summary>
    /// ミニマップでの1マス分の画像。歩いた場所、到達した部屋を表示
    /// </summary>
    [SerializeField]
    GameObject mini_map_tile;
    /// <summary>
    /// ミニマップ上に表示するプレイヤーの場所を示す
    /// </summary>
    [SerializeField]
    GameObject player_point;
    /// <summary>
    /// ミニマップ用キャンバス。生成したものはここに格納していく
    /// </summary>
    [SerializeField]
    GameObject canvas;

    /// <summary>
    /// ミニマップのタイルのオブジェクトを格納
    /// </summary>
    GameObject[] mini_map_tiles;

    /// <summary>
    /// アクセスする要素番号
    /// </summary>
    int index = 0;

    void Start() {
        game_manager = GameManager.Instance;
        map_layer = Dungeon_Manager.Instance.map_layer_2D;

        player_point.transform.localScale  = new Vector2(0.1f, 0.1f);
        mini_map_tile.transform.localScale = new Vector2(0.1f, 0.1f);
    }

    /// <summary>
    /// ミニマップを構成するオブジェクトを配置する
    /// </summary>
    public void Arrange_Mini_Map() {
        // ミニマップのサイズを作ったダンジョンと同じに
        mini_map_tiles = new GameObject[map_layer.Width * map_layer.Height];

        // 生成の基準点
        var init_x = gameObject.transform.parent.position.x;
        var instance_position = new Vector3(gameObject.transform.parent.position.x, gameObject.transform.parent.position.y - 2, 0);

        int counter = 0;
        for (int j = 0; j < map_layer.Height; ++j) {
            for (int i = 0; i < map_layer.Width; ++i) {
                var tile_obj = mini_map_tiles[counter] = Instantiate(mini_map_tile, instance_position, Quaternion.identity);
                tile_obj.transform.parent = canvas.transform;
                instance_position.x += mini_map_tile.GetComponent<SpriteRenderer>().bounds.size.x;
                mini_map_tiles[counter].SetActive(true);
                ++counter;
            }
            instance_position.x = init_x;
            instance_position.y += mini_map_tile.GetComponent<SpriteRenderer>().bounds.size.y;
        }

        //TEST
        var hoge = Player_Manager.Instance.player_script.Position;
        var fuga = map_layer.Get(hoge.x, hoge.y);
        var piyo = player_point.transform.position = mini_map_tiles[fuga].transform.position;
        Instantiate(player_point, piyo, Quaternion.identity);
    }

    /// <summary>
    /// プレイヤーの進行方向を見てミニマップを表示する
    /// </summary>
    /// <param name="direction">プレイヤーの進行方向</param>
    public void Which_Open(Vector2Int player_position, eDirection direction) {
        switch (direction) {
            case eDirection.Up:
                When_Up_Move(player_position);
                break;
            case eDirection.Upright:
                When_Upright_Move(player_position);
                break;
            case eDirection.Right:
                When_Right_Move(player_position);
                break;
            case eDirection.Downright:
                When_Downright_Move(player_position);
                break;
            case eDirection.Down:
                When_Down_Move(player_position);
                break;
            case eDirection.Downleft:
                When_Downleft_Move(player_position);
                break;
            case eDirection.Left:
                When_Left_Move(player_position);
                break;
            case eDirection.Upleft:
                When_Upleft_Move(player_position);
                break;
        }

    }

    #region 移動方向に応じたミニマップ表示メソッド(8方向分)

    /// <summary>
    /// 上方向に移動した場合のミニマップの表示
    /// </summary>
    /// <param name="player_position">プレイヤーの座標</param>
    void When_Up_Move(Vector2Int player_position) {
        //検索する座標を格納
        var search_coordinates = new Vector2Int[3];
        // 自分から見て上
        search_coordinates[0] = new Vector2Int(player_position.x,     player_position.y + 1);
        // 自分から見て左上
        search_coordinates[1] = new Vector2Int(player_position.x - 1, player_position.y + 1);
        // 自分から見て右上
        search_coordinates[2] = new Vector2Int(player_position.x + 1, player_position.y + 1);

        // 壁でなければミニマップに表示
        foreach (var search_coordinate in search_coordinates) {
            if (!map_layer.Is_Wall(search_coordinate)) {
                index = search_coordinate.y * map_layer.Width + search_coordinate.x;
                Debug.Log(index);
                mini_map_tiles[index].SetActive(true);
            }
        }
    }
    /// <summary>
    /// 右上に移動した場合のミニマップの表示
    /// </summary>
    /// <param name="player_position">プレイヤーの座標</param>
    void When_Upright_Move(Vector2Int player_position) {
        // 検索する座標を格納
        var search_coordinates = new Vector2Int[5];
        // 自分から見て上
        search_coordinates[0] = new Vector2Int(player_position.x, player_position.y + 1);
        // 自分から見て右上
        search_coordinates[1] = new Vector2Int(player_position.x + 1, player_position.y + 1);
        // 自分から見て右
        search_coordinates[2] = new Vector2Int(player_position.x - 1, player_position.y);
        // 自分から見て左下
        search_coordinates[3] = new Vector2Int(player_position.x - 1, player_position.y - 1);
        // 自分から見て左上
        search_coordinates[4] = new Vector2Int(player_position.x - 1, player_position.y + 1);

        int index = 0;
        // 壁でなければミニマップに表示
        foreach (var search_coordinate in search_coordinates) {
            if (!map_layer.Is_Wall(search_coordinate)) {
                Debug.Log(index);
                index = search_coordinate.y * map_layer.Width + search_coordinate.x;
                mini_map_tiles[index].SetActive(true);
            }
        }
    }
    /// <summary>
    /// 右に移動した場合のミニマップの表示
    /// </summary>
    /// <param name="player_position">プレイヤーの座標</param>
    void When_Right_Move(Vector2Int player_position) {
        //検索する座標を格納
        var search_coordinates = new Vector2Int[3];
        // 自分から見て右上
        search_coordinates[0] = new Vector2Int(player_position.x + 1, player_position.y + 1);
        // 自分から見て右
        search_coordinates[1] = new Vector2Int(player_position.x + 1, player_position.y);
        // 自分から見て右下
        search_coordinates[2] = new Vector2Int(player_position.x + 1, player_position.y - 1);

        int index = 0;
        // 壁でなければミニマップに表示
        foreach (var search_coordinate in search_coordinates) {
            if (!map_layer.Is_Wall(search_coordinate)) {
                index = search_coordinate.y * map_layer.Width + search_coordinate.x;
                Debug.Log(index);
                mini_map_tiles[index].SetActive(true);
            }
        }
    }
    /// <summary>
    /// 右下に移動した場合のミニマップの表示
    /// </summary>
    /// <param name="player_position">プレイヤーの座標</param>
    void When_Downright_Move(Vector2Int player_position) {
        // 検索する座標を格納
        var search_coordinates = new Vector2Int[5];
        // 自分から見て右上
        search_coordinates[0] = new Vector2Int(player_position.x + 1, player_position.y + 1);
        // 自分から見て右
        search_coordinates[1] = new Vector2Int(player_position.x + 1, player_position.y);
        // 自分から見て右下
        search_coordinates[2] = new Vector2Int(player_position.x + 1, player_position.y - 1);
        // 自分から見て下
        search_coordinates[3] = new Vector2Int(player_position.x, player_position.y - 1);
        // 自分から見て左下
        search_coordinates[4] = new Vector2Int(player_position.x - 1, player_position.y - 1);

        int index = 0;
        // 壁でなければミニマップに表示
        foreach (var search_coordinate in search_coordinates) {
            if (!map_layer.Is_Wall(search_coordinate)) {
                index = search_coordinate.y * map_layer.Width + search_coordinate.x;
                Debug.Log(index);
                mini_map_tiles[index].SetActive(true);
            }
        }
    }
    /// <summary>
    /// 下に移動した場合のミニマップの表示
    /// </summary>
    /// <param name="player_position">プレイヤーの座標</param>
    void When_Down_Move(Vector2Int player_position) {
        //検索する座標を格納
        var search_coordinates = new Vector2Int[3];
        // 自分から見て右下
        search_coordinates[0] = new Vector2Int(player_position.x + 1, player_position.y - 1);
        // 自分から見て下
        search_coordinates[1] = new Vector2Int(player_position.x, player_position.y - 1);
        // 自分から見て左下
        search_coordinates[2] = new Vector2Int(player_position.x - 1, player_position.y - 1);

        int index = 0;
        // 壁でなければミニマップに表示
        foreach (var search_coordinate in search_coordinates) {
            if (!map_layer.Is_Wall(search_coordinate)) {
                index = search_coordinate.y * map_layer.Width + search_coordinate.x;
                Debug.Log(index);
                mini_map_tiles[index].SetActive(true);
            }
        }
    }
    /// <summary>
    /// 左下に移動した場合のミニマップの表示
    /// </summary>
    /// <param name="player_position">プレイヤーの座標</param>
    void When_Downleft_Move(Vector2Int player_position) {
        // 検索する座標を格納
        var search_coordinates = new Vector2Int[5];
        // 自分から見て左下
        search_coordinates[0] = new Vector2Int(player_position.x + 1, player_position.y - 1);
        // 自分から見て下
        search_coordinates[1] = new Vector2Int(player_position.x, player_position.y - 1);
        // 自分から見て右下
        search_coordinates[2] = new Vector2Int(player_position.x - 1, player_position.y - 1);
        // 自分から見て右
        search_coordinates[3] = new Vector2Int(player_position.x - 1, player_position.y);
        // 自分から見て右上
        search_coordinates[4] = new Vector2Int(player_position.x - 1, player_position.y + 1);

        int index = 0;
        // 壁でなければミニマップに表示
        foreach (var search_coordinate in search_coordinates) {
            if (!map_layer.Is_Wall(search_coordinate)) {
                index = search_coordinate.y * map_layer.Width + search_coordinate.x;
                Debug.Log(index);
                mini_map_tiles[index].SetActive(true);
            }
        }
    }
    /// <summary>
    /// 左方向に移動した場合のミニマップの表示
    /// </summary>
    /// <param name="player_position">プレイヤーの座標</param>
    void When_Left_Move(Vector2Int player_position) {
        // 検索する座標を格納
        var search_coordinates = new Vector2Int[3];
        // 自分から見て左下
        search_coordinates[0] = new Vector2Int(player_position.x - 1, player_position.y - 1);
        // 自分から見て左
        search_coordinates[1] = new Vector2Int(player_position.x - 1, player_position.y);
        // 自分からみて左上
        search_coordinates[2] = new Vector2Int(player_position.x - 1, player_position.y + 1);

        int index = 0;
        // 壁でなければミニマップに表示
        foreach (var search_coordinate in search_coordinates) {
            if (!map_layer.Is_Wall(search_coordinate)) {
                index = search_coordinate.y * map_layer.Width + search_coordinate.x;
                Debug.Log(index);
                mini_map_tiles[index].SetActive(true);
            }
        }
    }
    /// <summary>
    /// 左上に移動した場合のミニマップの表示
    /// </summary>
    /// <param name="player_position">プレイヤーの座標</param>
    void When_Upleft_Move(Vector2Int player_position) {
        // 検索する座標を格納
        var search_coordinates = new Vector2Int[5];
        // 自分から見て左下
        search_coordinates[0] = new Vector2Int(player_position.x - 1, player_position.y - 1);
        // 自分から見て左
        search_coordinates[1] = new Vector2Int(player_position.x - 1, player_position.y);
        // 自分から見て左上
        search_coordinates[2] = new Vector2Int(player_position.x - 1, player_position.y + 1);
        // 自分から見て上
        search_coordinates[3] = new Vector2Int(player_position.x,     player_position.y + 1);
        // 自分から見て右上
        search_coordinates[4] = new Vector2Int(player_position.x + 1, player_position.y + 1);

        int index = 0;
        // 壁でなければミニマップに表示
        foreach (var search_coordinate in search_coordinates) {
            if (!map_layer.Is_Wall(search_coordinate)) {
                index = search_coordinate.y * map_layer.Width + search_coordinate.x;
                Debug.Log(index);
                mini_map_tiles[index].SetActive(true);
            }
        }
    }
    #endregion
}
