using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 状態に合わせた画像を床に貼る
/// </summary>
public class Dungeon_Tiles : MonoBehaviour {
    /// <summary>
    /// ダンジョンのタイルの状態
    /// </summary>
    public ReactiveProperty<eDungeon_Tile_State> dungeon_tiles;

    /// <summary>
    /// ダンジョンに使う床用の画像を格納
    /// </summary>
    [SerializeField]
    Sprite[] tile_sprite = new Sprite[(int)eDungeon_Tile_State.Finish];

    void Start() {
        var dungeon_manager = Dungeon_Manager.Instance;
        dungeon_tiles = dungeon_manager.tile_state;

        // 草原のタイル
        dungeon_tiles.Where(tile_type => tile_type == eDungeon_Tile_State.Grass).Subscribe(tile_type =>
            Set_Tile_Sprite((int)tile_type)
        ).AddTo(this);
        // 石のタイル
        dungeon_tiles.Where(tile_type => tile_type == eDungeon_Tile_State.Stone).Subscribe(tile_type =>
            Set_Tile_Sprite((int)tile_type)
        ).AddTo(this);
    }

    /// <summary>
    /// 画像をセット
    /// </summary>
    /// <param name="type"></param>
    public void Set_Tile_Sprite(int type) {
        SpriteRenderer sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = tile_sprite[type];
    }
}
