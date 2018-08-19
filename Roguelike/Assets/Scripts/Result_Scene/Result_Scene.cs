using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// リザルトシーンの進行を管理
/// </summary>
public class Result_Scene : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene("Title");
        }
    }
}
