using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {
    public Player_Data player_data;
    public PLAYER_STATUS player_status;

    [SerializeField]
    public List<PLAYER_STATUS> players = new List<PLAYER_STATUS>();

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
