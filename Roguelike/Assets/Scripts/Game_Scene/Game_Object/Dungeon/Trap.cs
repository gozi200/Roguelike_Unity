/*
 制作者 アントニオ
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 2;
        }
    }
}
