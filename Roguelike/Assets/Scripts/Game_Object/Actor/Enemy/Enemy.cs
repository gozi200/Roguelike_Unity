using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Enemy_Data enemy_data;
    public Enemy_Status enemy_status;

    [SerializeField]
    public List<Enemy_Status> enemys = new List<Enemy_Status>();

	// Use this for initialization
	void Start () {
        enemy_data = GetComponent<Enemy_Data>();
        enemy_data.Set_Parameter();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
