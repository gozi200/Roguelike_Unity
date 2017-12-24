using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Enemy_Data enemy_data;
    public ENEMY_STATUS enemy_status;

    [SerializeField]
    public List<ENEMY_STATUS> enemys = new List<ENEMY_STATUS>();

    bool is_dead = false;

	// Use this for initialization
	void Start () {
        enemy_data = GetComponent<Enemy_Data>();
        enemy_data.Set_Parameter();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
