using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutobyKey : MonoBehaviour
{
    public Danmaku danmaku;

    private bool Apressed = true;
    private bool Zpressed = true;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (getDirection() == null)
        {
            Apressed = true;
        }
        if (getDirection() == null)
        {
            Zpressed = true;
        }

        if (Apressed)
        {
            //建造
            if (Input.GetKey(KeyCode.A))
            {
                Debug.Log("A pressed");
                var walltype = getWallType();
                if (walltype != null)
                {
                    Debug.Log("walltype pressed");
                    var direction = getDirection();
                    if (direction != null && direction != "9")
                    {
                        danmaku.SendDanmaku("B" + walltype + direction);
                        Apressed = false;
                    }
                }
            }
        }




        if (Zpressed)
        {
            //拆除
            if (Input.GetKey(KeyCode.Z))
            {
                Debug.Log("Z pressed");
                var walltype = getWallType();

                var direction = getDirection();
                if (direction != null)
                {
                    danmaku.SendDanmaku("C" + walltype + direction);
                    Zpressed = false;
                }
            } 
        }

        

    }

    private string getDirection()
    {
        if (Input.GetKey(KeyCode.R))
        {
            return "7";
        }

        if (Input.GetKey(KeyCode.T))
        {
            return "0";
        }

        if (Input.GetKey(KeyCode.Y))
        {
            return "1";
        }

        if (Input.GetKey(KeyCode.H))
        {
            return "2";
        }

        if (Input.GetKey(KeyCode.N))
        {
            return "3";
        }

        if (Input.GetKey(KeyCode.B))
        {
            return "4";
        }

        if (Input.GetKey(KeyCode.V))
        {
            return "5";
        }

        if (Input.GetKey(KeyCode.F))
        {
            return "6";
        }

        if (Input.GetKey(KeyCode.G))
        {
            return "9";
        }

        return null;
    }

    private string getWallType()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            return "1";
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            return "2";
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            return "3";
        }
        else if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5))
        {
            return "5";
        }

        return null;
    }
}