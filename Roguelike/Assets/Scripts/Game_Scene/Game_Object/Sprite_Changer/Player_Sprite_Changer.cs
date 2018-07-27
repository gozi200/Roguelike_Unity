using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Player_Sprite_Changer : MonoBehaviour {
    /// <summary>
    /// 床画像をまとめたもの
    /// </summary>
    [SerializeField]
    SpriteAtlas Player_atlas;
    /// <summary>
    /// いくつのものをまとめたかを格納
    /// </summary>
    int sprite_count;
    /// <summary>
    /// まとめた画像の１つずつを格納
    /// </summary>
    Sprite[] sprite_array;


    void Start()
    {
        Player_atlas = Resources.Load<SpriteAtlas>("Player\\Players_Atlas");
        sprite_count = Player_atlas.spriteCount;
        sprite_array = new Sprite[sprite_count];
        Player_atlas.GetSprites(sprite_array);
        var player_status = gameObject.GetComponent<Player_Status>();
        Set_Sprite(player_status.ID);

    }

    /// <summary>
    /// 画像をセット
    /// </summary>
    /// <param name="type">spritesの要素数</param>
    void Set_Sprite(int type)
    {
        var sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = sprite_array[type];
    }
}
