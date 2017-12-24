using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Player_Data player_data;

    [SerializeField]
    public List<PLAYER_DATA_BASE> players = new List<PLAYER_DATA_BASE>();

    public bool is_dead = false;

    private float speed = 0.5f;

    // Use this for initialization
    void Start () {
        player_data = GetComponent<Player_Data>();
        player_data.Set_Parameter();
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0);
    }
}
