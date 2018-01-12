using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public Player_Status player_status;

    [SerializeField]
    public List<Player_Data> players = new List<Player_Data>();

    public Vector2 SPEED = new Vector2(5f, 5f);

    public int turn_count;
    public bool is_dead;

    // Use this for initialization
    void Start() {
        player_status = GetComponent<Player_Status>();
        player_status.Set_Parameter();

        turn_count = 0;
        is_dead = false;

        SPEED.x = 5; // 移動量
        SPEED.y = 5;
    }

    // Update is called once per frame
    void Update() {
        // 向きを取得(テスト中)
        float angle_direction = transform.eulerAngles.z * (Mathf.PI / 180.0f);
        Vector3 direction = new Vector3(Mathf.Cos(angle_direction), Mathf.Sin(angle_direction), 0.0f);
    }
}
