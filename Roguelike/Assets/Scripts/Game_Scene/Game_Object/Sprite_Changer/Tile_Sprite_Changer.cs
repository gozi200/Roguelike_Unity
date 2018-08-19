using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 地形に合わせた画像を床に貼る
/// </summary>
public class Tile_Sprite_Changer : MonoBehaviour {
    /// <summary>
    /// 床に貼る画像を格納
    /// </summary>
    [SerializeField]
    Sprite[] tile_sprite = new Sprite[(int)eTile_State.Finish];

    void Start() {
        var dungeon_manager = Dungeon_Manager.Instance;

        // 草原の床
        dungeon_manager.Tile_State.Where(tile_type => tile_type == eTile_State.Grass).Subscribe(tile_type =>
            Set_Sprite((int)tile_type)
        ).AddTo(this);
        // 石の床
        dungeon_manager.Tile_State.Where(tile_type => tile_type == eTile_State.Stone).Subscribe(tile_type =>
            Set_Sprite((int)tile_type)
        ).AddTo(this);
    }

    /// <summary>
    /// 画像をセット
    /// </summary>
    /// <param name="type">spritesの要素数</param>
    void Set_Sprite(int type) {
        var sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = tile_sprite[type];
    }
}
