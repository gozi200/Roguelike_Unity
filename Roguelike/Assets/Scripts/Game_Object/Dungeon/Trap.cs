/*
制作者 アントニオ

最終編集日 01/16
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour {
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 2;
        }
    }
}
