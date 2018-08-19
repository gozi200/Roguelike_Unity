using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Standing_Character_Sprite_Changer : MonoBehaviour {
    //TODO:キャラ毎に分ける？ 今は１種類しかいないから問題は無し
    /// <summary>
    /// 立ち絵画像をまとめたもの
    /// </summary>
    [SerializeField]
    SpriteAtlas standing_character_sprites;
    /// <summary>
    /// まとめた画像の１つずつを格納
    /// </summary>
    Sprite[] sprite_array;

    /// <summary>
    /// プレイヤーの位置を基準に出すので場所を知る
    /// </summary>
    [SerializeField]
    GameObject player;
    /// <summary>
    /// 自分がアタッチされているオブジェクト
    /// </summary>
    [SerializeField]
    public GameObject parent;
    /// <summary>
    /// 表示するテキスト(操作案内)
    /// </summary>
    public Text text_info;
    
    /// <summary>
    /// 代入用の移動後の座標
    /// </summary>
    Vector3 set_position;

    /// <summary>
    /// 表情の種類
    /// </summary>
    eMash_Expression expression;

    void Start() {
        text_info = parent.GetComponentInChildren<Text>();

        standing_character_sprites = Resources.Load<SpriteAtlas>("Standing_Character/Mash/Mash_Standing_Pose");
        sprite_array = new Sprite[standing_character_sprites.spriteCount];
        standing_character_sprites.GetSprites(sprite_array);
        parent.SetActive(false);
        // 適当なタイミング以外は隠しておく
        text_info.enabled = false;
    }

    /// <summary>
    /// 表示する画像のセッティング
    /// </summary>
    /// <param name="type">spritesの要素数</param>
    public void Set_Sprite(int type) {
        // 表示する画像
        var sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = sprite_array[type];

        // 座標
        set_position = new Vector3(player.transform.position.x, player.transform.position.y - 0.3f, 0);
        gameObject.transform.position = set_position;
    }
}
