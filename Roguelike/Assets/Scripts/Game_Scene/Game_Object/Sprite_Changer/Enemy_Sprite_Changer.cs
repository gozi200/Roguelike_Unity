using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.U2D;

/// <summary>
/// IDに合った画像をエネミーに貼る
/// </summary>
public class Enemy_Sprite_Changer : MonoBehaviour {
    /// <summary>
    /// 敵画像をまとめたもの
    /// </summary>
    [SerializeField]
    SpriteAtlas enemy_atlas;
    /// <summary>
    /// いくつのものをまとめたかを格納
    /// </summary>
    int sprite_count;
    /// <summary>
    /// まとめた画像の１つずつを格納
    /// </summary>
    Sprite[] sprite_array;

    void Start() {
        enemy_atlas = Resources.Load<SpriteAtlas>("Enemy\\Enemy_Atlas");
        sprite_count = enemy_atlas.spriteCount;
        sprite_array = new Sprite[sprite_count];
        enemy_atlas.GetSprites(sprite_array);
        var enemy_status = gameObject.GetComponent<Enemy_Controller>().enemy_status;
        var actor_manager = Actor_Manager.Instance;

        Set_Sprite(enemy_status.ID);
    }

    /// <summary>
    /// 画像をセット
    /// </summary>
    /// <param name="type">spritesの要素数</param>
    void Set_Sprite(int type) {
        var sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = sprite_array[type];
    }
}
