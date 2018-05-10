using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// IDに合った画像をエネミーに貼る //現在制作中 2018/05/10
/// </summary>
public class Enemy_Sprite_Changer : Sprite_Changer {
    /// <summary>
    /// エネミーに貼る画像を格納
    /// </summary>
    [SerializeField]
    Sprite[] enemy_sprite = new Sprite[Define_Value.ENEMY_NUMBER];

    void Start() {
        
    }

    /// <summary>
    /// 画像をセット
    /// </summary>
    /// <param name="type">spritesの要素数</param>
    protected override void Set_Sprite(int type) {
        SpriteRenderer sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = enemy_sprite[type];
    }
}
