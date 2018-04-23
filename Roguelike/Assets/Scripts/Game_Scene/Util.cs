/*
制作者 アントニオ
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// 様々なユーティリティ
/// </summary>
public class Util {
    /// Mathf.Cosの角度指定版
    public static float Cos_Ex(float Deg) {
        return Mathf.Cos(Mathf.Deg2Rad * Deg);
    }
    /// Mathf.Sinの角度指定版
    public static float Sin_Ex(float Deg) {
        return Mathf.Sin(Mathf.Deg2Rad * Deg);
    }

    /// 入力方向を取得する
    public static Vector2 Get_Input_Vector() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        return new Vector2(x, y).normalized;
    }

    /// トークンを動的生成する
    public static Token Create_Token(float x, float y, string Sprite_File, string Sprite_Name, string objName = "Token") {
        GameObject obj = new GameObject(objName);
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<Rigidbody2D>();
        obj.AddComponent<Token>();

        Token t = obj.GetComponent<Token>();

        // Wallスプライトのレイヤー
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        if (objName == "Grass_Wall" )
            sprite.sortingOrder = 1;

        // スプライト設定
        t.Set_Position_Sprite(Get_Sprite(Sprite_File, Sprite_Name));
        
        // 座標を設定
        t.X = x;
        t.Y = y;
        
        // 重力を無効にする
        t.Gravity_Scale = 0.0f;

        return t;
    }

    /// スプライトをリソースから取得する
    /// ※スプライトは「Resources/Sprites」以下に配置していなければなりません
    /// ※fileNameに空文字（""）を指定するとシングルスプライトから取得します
    public static Sprite Get_Sprite(string fileName, string spriteName) {
        if (spriteName == "") {
            // シングルスプライト
            return Resources.Load<Sprite>(fileName);
        }
        else {
            // マルチスプライト
            Sprite[] sprites = Resources.LoadAll<Sprite>(fileName);
            return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName));
        }
    }
    
}
