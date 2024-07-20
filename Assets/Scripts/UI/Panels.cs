using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panels : MonoBehaviour
{
    public List<GameObject> UIPanels;
    public List<Image> PageButtons;

    private Vector3 startpos;
    // Start is called before the first frame update
    void Start()
    {
        UIPanelSwitch(0);
        PageButtons[1].color = new Color(1, 0.2966457f, 0.2966457f);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UIPanelSwitch(int id)
    {
        for(int i = 0; i < UIPanels.Count; i++)
        {
            if(i == id)
            {
                UIPanels[i].SetActive(true);
                PageButtons[i].color = new Color(1, 0.2966457f, 0.2966457f);

            }
            else
            {
                if (i!=1)
                {
                    UIPanels[i].SetActive(false); 
                    PageButtons[i].color = new Color(1, 1, 1);
                }
               
                

            }
        }

    }

    public void changeToolPanel()
    {
        if (UIPanels[1].GetComponent<CanvasGroup>().alpha!=0f)
        {
            UIPanels[1].GetComponent<CanvasGroup>().alpha=0f;
            PageButtons[1].color = new Color(1f, 1f, 1f);
            
        }
        else
        {
            UIPanels[1].GetComponent<CanvasGroup>().alpha=0.55f;
            PageButtons[1].color = new Color(1, 0.2966457f, 0.2966457f);
        }
    }
}
