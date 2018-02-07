using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // 現在の状態
    eGame_State game_state;

    [SerializeField]
    Player_Manager player_manager;

    [SerializeField]
    Enemy_Manager enemy_manager;

    [SerializeField]
    public Dungeon_Generator dungeon_generator;

    public static GameManager Instance;

    private Text level_text;
    private int level = 1;
    bool button_pressed = false;
    Player player_script = null;
    GameObject player;
    GameObject stair;

    [SerializeField]
    Enemy enemy;

    // Use this for initialization
    void Start () {
        Instance = this;
        game_state = eGame_State.Dungeon_Create;

        dungeon_generator = GetComponent<Dungeon_Generator>();
        Initialize();
        player = dungeon_generator.player;
        /*player_script = player.GetComponent<Player>();
        Player_Manager.Set_Player(player_script);*/

    }

    void NextLevel()
    {
        //stair = generator.stair;
        //if (player.transform.position == stair.transform.position)
        {
            Reset();
            level++;
            Initialize();
            //player.transform.position = dungeon_generator.player.transform.position;
        }
    }

    void Reset()
    {

        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        GameObject[] stairs = GameObject.FindGameObjectsWithTag("Stair");
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        List<GameObject> walls = dungeon_generator.walls;
        //List<Enemy_Data> enemys = enemy.enemys;
        foreach (GameObject trap in traps)
        {
            Destroy(trap);
        }
        foreach (GameObject obj in stairs)
        {
            Destroy(obj);
        }
        foreach (GameObject food in foods)
        {
            Destroy(food);
        }
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
        walls.Clear();
        //enemys.Clear();

    }

    void Initialize()
    {
        level_text = GameObject.Find("LevelText").GetComponent<Text>();
        level_text.text = "Level " + level;
        dungeon_generator.Load_Dungeon(level);
        //player.transform.position = generator.player.transform.position;
    }

    public void Set_Game_State(eGame_State set_state) {
        game_state = set_state;
        Game_Loop(set_state);
    }

    void Game_Loop(eGame_State state) {
        switch (state) {
            case eGame_State.Dungeon_Create:
                // ダンジョンを作る
                break;

            case eGame_State.Player_Turn:
                // プレイヤーの行動
                break;

            case eGame_State.Partner_Turn:
                // パートナーの行動
                break;

            case eGame_State.Enemy_Trun:
                // エネミーの行動
                break;

            case eGame_State.Dungeon_Turn:
                // ダンジョンのターン(敵のスポーンなど)
                break;
        }
    }

    void OnGUI()
    {
        stair = dungeon_generator.stair;
        if (GUI.Button(new Rect(565, 365, 128, 32), "もう１回"))
        {
            NextLevel();
        }

        if (!button_pressed)
        {
            if (player.transform.position == stair.transform.position)
            {
                player.GetComponent<Player>().speed = new Vector2(0, 0);
                if (GUI.Button(new Rect(225, 120, 128, 32), "次のレベル進む？"))
                {
                    NextLevel();
                    player.GetComponent<Player>().speed = new Vector2(5f, 5f);
                }

                else if (GUI.Button(new Rect(385, 120, 128, 32), "探索を続く？"))
                {
                    button_pressed = true;
                    player.GetComponent<Player>().speed = new Vector2(5f, 5f);
                }
            }
        }
        if (player.transform.position != stair.transform.position)
            button_pressed = false;
    }
}
