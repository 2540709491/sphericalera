using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Manager : MonoBehaviour
{
    public Danmaku danmaku;
    public Mouse mouse;
    public TrackManager trackManager;
    public ColorDetector colorDetector;
    public Block PlayerBlock;
    public Block MainRoom = null;
    public bool isRightClickMenuOn;
    bool shouldTurnRightMenuOff;
    public GameObject RightClickMenu;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldTurnRightMenuOff)
        {
            shouldTurnRightMenuOff = false;
            isRightClickMenuOn = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && PlayerBlock != null && !EventSystem.current.IsPointerOverGameObject())
        {
            isRightClickMenuOn = true;

        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            shouldTurnRightMenuOff = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse2) && !EventSystem.current.IsPointerOverGameObject())
        {
            shouldTurnRightMenuOff = true;
        }

        if (isRightClickMenuOn)
        {
            RightClickMenu.SetActive(true);
            RightClickMenu.transform.position = PlayerBlock.transform.position;
        }
        else
        {
            RightClickMenu.SetActive(false);
        }
    }


    void Init()
    {
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                Block block = ((GameObject)GameObject.Instantiate(Resources.Load("Block"), this.transform)).GetComponent<Block>();
                block.posX = x;
                block.posY = y;
                block.manager = this;
                float step = 0.416f;
                block.transform.localPosition = new Vector3(step * x, -step * y, 0);
                block.transform.localScale = new Vector3(step, step, 1);
                if ((x + y) % 2 != 0)
                {
                    block.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                }
            }



        }






    }

    public void ReturnHome()
    {
        if(MainRoom != null)
        {
            PlayerBlock = MainRoom;
        }
    }


}
