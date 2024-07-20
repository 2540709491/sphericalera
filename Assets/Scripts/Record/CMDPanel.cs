using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Recorder;

public class CMDPanel : MonoBehaviour
{
    
    public Image PlayButton;
    public InputField NameInput;
    public Text InfoText;
    public ScriptBind scriptBind;
    public Recorder recorder;
    public string UUID;
    public bool IsRunning;
    public List<CMD> CMDs = new();
    List<CMD> CMDCopy = new();
    public bool IsLoop;
    public float recordTimer = 0;
    float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (IsLoop)
        {
            InfoText.text = $"循环  {recorder.GetHMSTime(recordTimer)}  共计{CMDs.Count}条指令";

        }
        else
        {
            InfoText.text = $"{recorder.GetHMSTime(recordTimer)}  共计{CMDs.Count}条指令";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            PlayButton.sprite = Resources.Load<Sprite>("Images/Stop");

            currentTime += Time.deltaTime;

            if (CMDCopy.Count > 0)
            {
                if (currentTime > CMDCopy[0].Time)
                {

                    recorder.danmaku.SendDanmaku(CMDCopy[0].Command);
                    CMDCopy.RemoveAt(0);
                }


            }

            //最后一个指令执行完可能还有时间
            if (currentTime > recordTimer)
            {
                if (IsLoop)//如果是加载，需要加载时设置Toggle状态
                {
                    currentTime = 0;

                    CMDCopy = new(CMDs);
                }
                else
                {
                    IsRunning = false;
                }
            }
        }
        else
        {
            PlayButton.sprite = Resources.Load<Sprite>("Images/Play");
        }
    }


    public void PlayThis()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            CMDCopy = new(CMDs);
            currentTime = 0;
        }
        else
        {
            IsRunning = false;
        }
    }

    public void RenameThis()
    {
        for (int i = recorder.SavedRecordData.RecordList.Count - 1; i >= 0; i--)
        {
            if (recorder.SavedRecordData.RecordList[i].UUID == UUID)
            {
                recorder.SavedRecordData.RecordList[i].RecordName = NameInput.text;
            }
        }
        //重名名脚本
        for (int i = scriptBind.cmds.Count - 1; i >= 0; i--)
        {
            if(scriptBind.cmds[i].UUID == UUID)
            {
                scriptBind.rename(i,NameInput.text);
            }
        } 
        PlayerPrefs.SetString("SavedRecordData", JsonConvert.SerializeObject(recorder.SavedRecordData));
        PlayerPrefs.Save();
    }



    public void DeleteThis()
    {
        for (int i = recorder.SavedRecordData.RecordList.Count - 1; i >= 0; i--)
        {
            if(recorder.SavedRecordData.RecordList[i].UUID == UUID)
            {
                recorder.SavedRecordData.RecordList.RemoveAt(i);
            }
        }    
        //删除绑定列表中的
        for (int i = scriptBind.cmds.Count - 1; i >= 0; i--)
        {
            if(scriptBind.cmds[i].UUID == UUID)
            {
                scriptBind.cmds.RemoveAt(i);
                scriptBind.removeOption(i);
            }
        } 
        PlayerPrefs.SetString("SavedRecordData", JsonConvert.SerializeObject(recorder.SavedRecordData));
        PlayerPrefs.Save();
    
        GameObject.Destroy(this.gameObject);

    }
}
