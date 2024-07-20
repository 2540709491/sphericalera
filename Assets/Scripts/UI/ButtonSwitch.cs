using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitch : MonoBehaviour
{
    public bool IsOn;
    Image buttonImage;
    // Start is called before the first frame update
    void Start()
    {
        buttonImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        SetColor();
    }

    public void SwitchThis()
    {
        IsOn = !IsOn;

        

    }

    void SetColor()
    {
        if (IsOn)
        {
            buttonImage.color = new Color(1, 0.2966457f, 0.2966457f);
        }
        else
        {
            buttonImage.color = new Color(1, 1, 1);

        }


    }
}
