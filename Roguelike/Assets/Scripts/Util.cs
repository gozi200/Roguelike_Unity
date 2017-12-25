using UnityEngine;
using System.Collections;

/// 様々なユーティリティ
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
        if (objName == "Wall" )
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

    private static Rect GUI_Rectangle = new Rect();
    static Rect Get_GUI_Rectangle() {
        return GUI_Rectangle;
    }
    private static GUIStyle GUI_style = null;
    static GUIStyle Get_GUIStyle() {
        return GUI_style ?? (GUI_style = new GUIStyle());
    }
    /// フォントサイズを設定
    public static void SetFontSize(int size) {
        Get_GUIStyle().fontSize = size;
    }
    /// フォントカラーを設定
    public static void SetFontColor(Color color) {
        Get_GUIStyle().normal.textColor = color;
    }
    /// ラベルの描画
    public static void GUILabel(float x, float y, float w, float h, string text) {
        Rect rect = Get_GUI_Rectangle();
        rect.x = x;
        rect.y = y;
        rect.width = w;
        rect.height = h;

        GUI.Label(rect, text, Get_GUIStyle());
    }
    // ボタンの描画
    public static bool GUIButton(float x, float y, float w, float h, string text) {
        Rect rect = Get_GUI_Rectangle();
        rect.x = x;
        rect.y = y;
        rect.width = w;
        rect.height = h;

        return GUI.Button(rect, text, Get_GUIStyle());
    }
}
