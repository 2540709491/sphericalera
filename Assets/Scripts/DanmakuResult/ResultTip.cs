using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultTip : MonoBehaviour
{
    public Color SuccessColor;
    public Color FailColor;
    public SpriteRenderer Background;
    public TextMesh RefToContent;
    public TextMesh RefToTip;
    public bool IsSuccess;
    public string Content;
    float timer;
    public float ApperaingTime;
    public float StayTime;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        RefToContent.text = Content;
        if (IsSuccess)
        {
            RefToTip.text = "发送成功";
        }
        else
        {
            RefToTip.text = "发送频率过高";
        }
        this.transform.localPosition = new Vector3(-2.5f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        this.transform.localPosition = Vector3.Lerp(new Vector3(-2.5f, 0, 0), Vector3.zero, timer / ApperaingTime);
        if (IsSuccess)
        {
            Background.color = Color.Lerp(new Color(SuccessColor.r, SuccessColor.g, SuccessColor.b, 0f), SuccessColor, timer / ApperaingTime);
           
        }
        else
        {
            Background.color = Color.Lerp(new Color(FailColor.r, FailColor.g, FailColor.b, 0f), FailColor, timer / ApperaingTime);

        }
        RefToContent.color = Color.Lerp(new Color(1, 1, 1, 0f), new Color(1, 1, 1, 1f), timer / ApperaingTime);
        RefToTip.color = Color.Lerp(new Color(1, 1, 1, 0f), new Color(1, 1, 1, 1f), timer / ApperaingTime);

        if (timer > ApperaingTime)
        {
            if (IsSuccess)
            {
                Background.color = Color.Lerp(new Color(SuccessColor.r, SuccessColor.g, SuccessColor.b, 0f), SuccessColor, (StayTime -timer + ApperaingTime) / StayTime);

            }
            else
            {
                Background.color = Color.Lerp(new Color(FailColor.r, FailColor.g, FailColor.b, 0f), FailColor, (StayTime - timer + ApperaingTime) / StayTime);

            }
            RefToContent.color = Color.Lerp(new Color(1, 1, 1, 0f), new Color(1, 1, 1, 1f), (StayTime - timer + ApperaingTime) / StayTime);
            RefToTip.color = Color.Lerp(new Color(1, 1, 1, 0f), new Color(1, 1, 1, 1f), (StayTime - timer + ApperaingTime) / StayTime);
        }


        if (timer > ApperaingTime+ StayTime)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
