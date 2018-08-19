using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーを種類毎に分ける
/// </summary>
[CreateAssetMenu(fileName = "Enemy_Data_Base", menuName = "Create_Enemy_Data_Base")]
public class Enemy_Data_Base : ScriptableObject {
    /// <summary>
    /// これに各タイプごとに敵を分ける
    /// </summary>
    [SerializeField]
    private List<Enemy_Status_Base> enemy_list = new List<Enemy_Status_Base>();

    /// <summary>
    /// リストを返す
    /// </summary>
    /// <returns>敵の種類ごとに分割したjsonデータ</returns>
    public List<Enemy_Status_Base> Get_Enemy_List() {
        return enemy_list;
    }
}
