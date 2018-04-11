/*
制作者 アントニオ
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour {

    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image health;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        HandleBar();
    }

    private void HandleBar() {
        health.fillAmount = Map(10, 0, 20, 0, 1);
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax) {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
