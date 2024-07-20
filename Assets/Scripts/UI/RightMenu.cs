using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightMenu : MonoBehaviour
{
    public List<Image> OperationButtons;
    public Danmaku danmaku;
    public Button CurrentButton;
    public string Operation;
    public Manager manager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TriggerPos(int thisPos)
    {
        

        danmaku.SendDanmaku(Operation + thisPos);

        manager.isRightClickMenuOn = false;
    }

    public void TriggerOperation(Image button, string operation)
    {
        foreach(Image b in OperationButtons)
        {
            if (b.GetInstanceID() == button.GetInstanceID())
            {
                b.color = new Color(b.color.r, b.color.g, b.color.b, 1f);              
            }
            else
            {
                b.color = new Color(b.color.r, b.color.g, b.color.b, 0.3f);
            }
        }

        Operation = operation;


    }
}
