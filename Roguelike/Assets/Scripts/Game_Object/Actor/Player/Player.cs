using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public List<PLAYER_DATA_BASE> players;


    public bool is_dead = false;

    private float speed = 0.5f;

    PLAYER_DATA_BASE data_base = new PLAYER_DATA_BASE();


    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0);
    }
}
