using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 場所に合った画像を壁に貼る
/// </summary>
public class Wall_Sprite_Changer : Sprite_Changer {
    /// <summary>
    /// 壁に貼る画像を格納
    /// </summary>
    [SerializeField]
    Sprite[] wall_sprites = new Sprite[(int)eWall_State.Finish];

	void Start () {
        var dungeon_manager = Dungeon_Manager.Instance;

        // 木の壁
        dungeon_manager.wall_state.Where(tile_type => tile_type == eWall_State.Tree).Subscribe(tile_type =>
            Set_Sprite((int)tile_type)
        ).AddTo(this);
        // 石の壁
        dungeon_manager.wall_state.Where(tile_type => tile_type == eWall_State.Stone).Subscribe(tile_type =>
            Set_Sprite((int)tile_type)
        ).AddTo(this);
    }

    /// <summary>
    /// 画像をセット
    /// </summary>
    /// <param name="type">spritesの要素数</param>
    protected override void Set_Sprite(int type) {
        SpriteRenderer sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = wall_sprites[type];
    }
}
