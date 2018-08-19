using UnityEngine;

/// <summary>
/// gameObjectのSpriteのα値を調整し、緩やかに光っているように見せる
/// </summary>
public class Change_Alpha : MonoBehaviour {
    /// <summary>
    /// 表示している画像のカラーを入れる
    /// </summary>
    Color color;
    /// <summary>
    /// 表示している画像のSpriteRenderer
    /// </summary>
    SpriteRenderer sprite_renderer;
    /// <summary>
    /// α値を増減させる
    /// </summary>
    float value_changer;
    /// <summary>
    /// これが表示している画像のα値になる
    /// </summary>
    float alpha_value;


	void Start () {
        alpha_value = 0.2f;
        value_changer = 0.0045f;

        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        color = sprite_renderer.color;
	}
	
	void Update () {
        // 規定値より薄くなったら濃く、濃くなったら薄くなるようにする
        if(alpha_value < 0.1f) {
            value_changer = -value_changer;
        }
        else if (alpha_value > 0.75f) {
            value_changer = -value_changer;
        }

        alpha_value += value_changer;

        color.a = alpha_value;
        sprite_renderer.color = color;
	}
}
