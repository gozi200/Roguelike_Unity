﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Player_Data player_data;

    private float speed = 0.5f;

    Vector3 move = Vector3.zero;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        //transform.Translate(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0);
    }

   // void Attack(Enemy* target) {

    //}
}
