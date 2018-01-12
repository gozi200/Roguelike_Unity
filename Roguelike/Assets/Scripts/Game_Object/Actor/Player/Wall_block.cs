using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_block : MonoBehaviour
{

    static bool northbool = true;
    static bool eastbool = true;
    static bool southbool = true;
    static bool westbool = true;

    void Start()
    {
    }

    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D Tree)
    {
        Debug.Log("Function");
        switch (gameObject.name)
        {
            case "N":
                {
                    if (Tree.tag == "Wall")
                    {
                        northbool = false;
                    }
                    else if (Tree.tag != "wall")
                    {
                        northbool = true;
                    }
                }
                break;
            case "S":
                {
                    if (Tree.tag == "Wall")
                    {
                        southbool = false;
                    }
                    else if (Tree.tag != "wall")
                    {
                        southbool = true;
                    }
                }
                break;
            case "E":
                {
                    if (Tree.tag == "Wall")
                    {
                        eastbool = false;
                    }
                    else if (Tree.tag != "wall")
                    {
                        eastbool = true;
                    }
                }
                break;
            case "W":
                {

                    if (Tree.tag == "Wall")
                    {
                        westbool = false;
                    }
                    else if (Tree.tag != "wall")
                    {
                        westbool = true;
                    }
                    break;
                }
        }
    }

        public bool canmove(string k)
        {
            Debug.Log(k);
            if (k == "N")
            {
                return northbool;
            }
            else if (k == "E")
            {
                return eastbool;
            }
            else if (k == "S")
            {
                return southbool;
            }
            else if (k == "W")
            {
                return westbool;
            }
            return true;
        }
        void OnTriggerExit2D(Collider2D Tree)
        {
            switch (gameObject.name)
            {
                case "N": northbool = true; break;
                case "S": southbool = true; break;
                case "E": eastbool = true; break;
                case "W": westbool = true; break;
            }

        }
    }
