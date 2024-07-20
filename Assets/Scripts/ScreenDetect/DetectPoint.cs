using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPoint : MonoBehaviour
{
    public int RealPosX;
    public int RealPosY;
    public Color SavedColor;
    public Color NewColor;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    public ChatManager chatManager;
    public Manager manager;
    public CMDPanel CmdPanel;//绑定的脚本
    float timer;
    float shakeTimer;
    public bool isWarningOn;
    int WarningTimes = 0;//超过指定时长才报警

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!chatManager.IsOn && !manager.isRightClickMenuOn)//聊天窗打开时不工作
        {
            timer += Time.deltaTime;
        }
        if (timer > 0.4f)
        {
            timer = 0;
            NewColor = ColorDetector.GetPixelColor(RealPosX, RealPosY);
            if (!isWarningOn)
            {
                if (Mathf.Abs(NewColor.r - SavedColor.r) > 0.2 || Mathf.Abs(NewColor.g - SavedColor.g) > 0.2 ||Mathf.Abs(NewColor.b - SavedColor.b) > 0.2)
                {
                    WarningTimes++;

                    if(WarningTimes > 8)
                    {
                        audioSource.Play();
                        isWarningOn = true;
                        if (!CmdPanel.IsRunning)
                        {
                            CmdPanel.PlayThis();
                        }
                    }

                }
                else
                {
                    WarningTimes = 0;
                }
            }


        }
        if (isWarningOn)
        {
            float maxTimer = 0.4f;
            shakeTimer += Time.deltaTime;
            Color newColor = new Color(1, 1, 1, 1f);
            Color newColor2 = new Color(1, 0, 0, 1f);
            if (shakeTimer < maxTimer / 2)
            {
                spriteRenderer.color = Color.Lerp(newColor, newColor2, shakeTimer / (maxTimer / 2));
            }
            else if (shakeTimer < maxTimer)
            {
                spriteRenderer.color = Color.Lerp(newColor2, newColor, (shakeTimer - (maxTimer / 2)) / (maxTimer / 2));

            }
            else
            {
                shakeTimer = 0;
            }
        }
    }
}
