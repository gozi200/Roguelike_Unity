using UnityEngine;
using UnityEditor;

/// <summary>
/// エディタ上から新しいwindowを作る
/// </summary>
public class Editor_Window : EditorWindow {
    /// <summary>
    /// ScriptableObjectSampleの変数
    /// </summary>
    private Enemy_Status_Base enemy_status_base;

    /// <summary>
    /// 読み込み、書き込み、新規作成先のパス(入力させて使う)
    /// </summary>
    private string asset_path;

    /// <summary>
    /// windowを生成
    /// </summary>
    [MenuItem("Editor/SetEnemyStatus")]
    private static void Create() {
        // 生成
        Editor_Window window = GetWindow<Editor_Window>("EnemyStatus");
        // 最小サイズ設定
        window.minSize = new Vector2Int(500, 300);
    }

    // 項目の生成
    private void OnGUI() {
        if (enemy_status_base == null) {
            // 読み込み
            Import();
        }

        // ここから項目の配置　扱いやすいような配置に(独断＆偏見)
        // 見出し
        EditorGUILayout.LabelField("アセットパスの拡張子(.asset)を忘れずに");

        GUILayout.Space(15);

        // パス指定
        GUILayout.Label("読み込み、書き込み、新規作成するアセットのパス");
        asset_path = EditorGUILayout.TextField("", asset_path);

        // 列挙体の設定
        enemy_status_base.Class_Type = (eClass_Type)EditorGUILayout.EnumPopup("クラス", enemy_status_base.Class_Type);

        // ステータス入力
        using (new GUILayout.HorizontalScope()) {
            using (new GUILayout.VerticalScope()) {
                enemy_status_base.ID           = EditorGUILayout.IntField("ID", enemy_status_base.ID);
                enemy_status_base.Max_HP       = EditorGUILayout.IntField("最大体力", enemy_status_base.Max_HP);
                enemy_status_base.Defence      = EditorGUILayout.IntField("防御力", enemy_status_base.Defence);
                enemy_status_base.Critical     = EditorGUILayout.IntField("クリティカル率", enemy_status_base.Critical);
                enemy_status_base.First_Floor  = EditorGUILayout.IntField("出現開始階層", enemy_status_base.First_Floor);
                enemy_status_base.Skill        = EditorGUILayout.IntField("使用スキル", enemy_status_base.Skill);
            }
            using (new GUILayout.VerticalScope()) {
                enemy_status_base.Name             = EditorGUILayout.TextField("名前", enemy_status_base.Name);
                enemy_status_base.Attack           = EditorGUILayout.IntField("攻撃力", enemy_status_base.Attack);
                enemy_status_base.Experience_Point = EditorGUILayout.IntField("撃破時獲得経験値", enemy_status_base.Experience_Point);
                enemy_status_base.Finish_Move      = EditorGUILayout.IntField("必殺技", enemy_status_base.Finish_Move);
                enemy_status_base.Last_Floor       = EditorGUILayout.IntField("出現終了階層", enemy_status_base.Last_Floor);
                enemy_status_base.AI_Pattern       = EditorGUILayout.IntField("行動パターン", enemy_status_base.AI_Pattern);
            }
        }

        // ボタンの配置
        using (new GUILayout.HorizontalScope()) {
            if (GUILayout.Button("読み込み")) {
                Import();
            }
            if (GUILayout.Button("書き込み")) {
                Export();
            }
            if(GUILayout.Button("新規生成")) {
                Create_Asset();
            }
        }
    }

    /// <summary>
    /// 読み込み
    /// </summary>
    void Import() {
        if (enemy_status_base == null) {
            enemy_status_base = ScriptableObject.CreateInstance<Enemy_Status_Base>();
        }

        Enemy_Status_Base enemy_status_base_ = AssetDatabase.LoadAssetAtPath<Enemy_Status_Base>(asset_path);
        if (enemy_status_base_ == null) {
            return;
        }

        // コピーする
        enemy_status_base.Copy(enemy_status_base_);
    }

    /// <summary>
    /// 書き込み
    /// </summary>
    void Export() {
        // 書き込み先の読み込み
        Enemy_Status_Base enemy_status_base_ = AssetDatabase.LoadAssetAtPath<Enemy_Status_Base>(asset_path);
        if (enemy_status_base_ == null) {
            enemy_status_base_ = ScriptableObject.CreateInstance<Enemy_Status_Base>();
        }

        // コピー
        enemy_status_base_.Copy(enemy_status_base);

        // Inspectorkaから編集できないようにする
        enemy_status_base_.hideFlags = HideFlags.NotEditable;
        // 更新通知
        EditorUtility.SetDirty(enemy_status_base_);
        // 保存
        AssetDatabase.SaveAssets();
        // エディタを最新の状態にする
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 新しいアセットを生成する
    /// </summary>
    void Create_Asset() {
        // 書き込み先の読み込み
        Enemy_Status_Base enemy_status_base_ = AssetDatabase.LoadAssetAtPath<Enemy_Status_Base>(asset_path);
        if (enemy_status_base_ == null) {
            enemy_status_base_ = ScriptableObject.CreateInstance<Enemy_Status_Base>();
        }

        // アセット作成
        AssetDatabase.CreateAsset(enemy_status_base_, asset_path);

        // コピー生成したものを元データにコピー
        enemy_status_base_.Copy(enemy_status_base);

        // Inspectorkaから編集できないようにする
        enemy_status_base_.hideFlags = HideFlags.NotEditable;
        // 保存
        AssetDatabase.SaveAssets();
        // エディタを最新の状態にする
        AssetDatabase.Refresh();
    }
}
