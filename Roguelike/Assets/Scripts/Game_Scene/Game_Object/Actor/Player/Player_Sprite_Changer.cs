using UnityEngine;
using UnityEngine.U2D;
using UniRx;
using System.Collections.Generic;
using System.Linq;

public class Player_Sprite_Changer : MonoBehaviour {
    /// <summary>
    /// 敵画像をまとめたもの
    /// </summary>
    [SerializeField]
    List<SpriteAtlas> player_sprites;
    /// <summary>
    /// いくつのものをまとめたかを格納
    /// </summary>
    int sprite_count;

    /// <summary>
    /// まとめた画像を１つずつ格納
    /// </summary>
    Sprite[] sprite_array;

    void Start() {
        player_sprites = new List<SpriteAtlas>();
        sprite_count = Define_Value.ACTOR_SPRITE_NUMBER;
        sprite_array = new Sprite[sprite_count];

        // 初期設定
        Initialize();

        // 向いている方向に合わせた画像を張る
        var player = Player_Manager.Instance.player_script;
        player.Direction.Subscribe(direction => {
            //TODO:マジックナンバー キャラ選択作ったら自分の選択中のキャラを入れるメソッドを作り直す
            Set_Sprite(0, (int)direction);
        }).AddTo(this);
    }

    /// <summary>
    /// 必要画像を格納しておく
    /// </summary>
    void Initialize() {
        player_sprites.Add(Resources.Load<SpriteAtlas>("Picture/Player/Okita_Sprite"));

        //TODO:マシュの画像はないので動かないけど、こうやって格納していく
        player_sprites.Add(Resources.Load<SpriteAtlas>("Picture/Player/Mash_Sprite"));

        for (int i = 0; i < Define_Value.PLAYER_NUMBER; ++i) {
            // キャラ毎に配列に画像を格納
            player_sprites[i].GetSprites(sprite_array);
            // 名前順でソート
            sprite_array = sprite_array.OrderBy(sprite => sprite.name).ToArray();
        }
    }

    /// <summary>
    /// 画像をセット
    /// </summary>
    /// <param name="ID">表示するキャラクターのID</param>
    /// <param name="index">spritesの要素番号</param>
    void Set_Sprite(int ID, int index) {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite_array[index];
    }
}
