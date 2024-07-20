using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
public class Block : MonoBehaviour
{
    public int posX;
    public int posY;
    public Manager manager;

    float timer;
    float maxTimer = 0.6f;
    Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = this.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<SpriteRenderer>().bounds.Intersects(manager.mouse.GetComponent<SpriteRenderer>().bounds))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (manager.colorDetector.isWaitToClick && !EventSystem.current.IsPointerOverGameObject())
                {
                    manager.colorDetector.isWaitToClick = false;
                    manager.colorDetector.AddPoint();
                }
                else if(manager.isRightClickMenuOn != true && !EventSystem.current.IsPointerOverGameObject())
                {
                    manager.danmaku.SendDanmaku($"M{GetCode(posX)}{GetCode(posY)}" != "M64" ? $"M{GetCode(posX)}{GetCode(posY)}"+"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[UnityEngine.Random.Range(0, 62)] :"M644");
                    manager.PlayerBlock = this;

                    manager.trackManager.AddTrack(this);
                }

            }
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                if (manager.isRightClickMenuOn != true && !EventSystem.current.IsPointerOverGameObject())
                {
                    manager.danmaku.SendDanmaku($"T{GetCode(posX)}{GetCode(posY)}");
                    manager.PlayerBlock = this;

                    manager.trackManager.ResetTrack();
                    manager.trackManager.AddTrack(this);
                }

            }
        }

        if(manager.PlayerBlock == this)
        {
            timer += Time.deltaTime;
            Color newColor = new Color(1, 0, 0, 0.1f);
            Color newColor2 = new Color(1, 0, 0, 0.3f);
            if(timer < maxTimer / 2)
            {
                this.GetComponent<SpriteRenderer>().color = Color.Lerp(newColor, newColor2, timer / (maxTimer / 2));
            }else if(timer < maxTimer)
            {
                this.GetComponent<SpriteRenderer>().color = Color.Lerp(newColor2, newColor, (timer - (maxTimer / 2)) / (maxTimer / 2));

            }
            else
            {
                timer = 0;
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = originalColor;
        }
    }

    string GetCode(int id)
    {
        if(id <=9)
        {
            return id.ToString();
        }
        else
        {
            switch (id)
            {
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                case 16:
                    return "H";
                case 17:
                    return "I";
                case 18:
                    return "L";
                case 19:
                    return "P";
            }






        }

        return "0";
    }
}
