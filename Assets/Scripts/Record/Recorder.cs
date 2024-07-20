using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Recorder : MonoBehaviour
{
    public Toggle LoopToggle;
    public TextMeshProUGUI TimerText;
    public Danmaku danmaku;
    public Button RecordButton;
    public Button RunButton;
    public Button SaveButton;
    public TrackManager trackManager;
    public ScriptBind scriptBind;

    float recordTimer = 0;

    public RecordInfo CurrentRecord = null;
    public List<CMD> CMDCopy = new();//用于播放

    public enum RecorderState { Default, Recording, Running };
    public RecorderState CurrentState;

    public SavedData SavedRecordData = new SavedData();

    //保存的数据
    public class SavedData
    {
        public List<RecordInfo> RecordList = new List<RecordInfo>();
    }

    public class RecordInfo
    {
        public string UUID;
        public string RecordName;
        public float RecordTime;
        public bool isLoop;
        public List<CMD> CMDs = new();
    }

    public class CMD
    {
        public float Time;
        public string Command;
    }

    //UI
    public Transform ScrollContent;


    // Start is called before the first frame update
    void Start()
    {
        string SavedRecordDataTxt = PlayerPrefs.GetString("SavedRecordData", null);
        try
        {
            if (SavedRecordDataTxt != null)
            {
                SavedRecordData = JsonConvert.DeserializeObject<SavedData>(SavedRecordDataTxt);

                foreach (RecordInfo record in SavedRecordData.RecordList)
                {
                    CreateNewPanel(record);
                }

            }
        }catch(Exception e)
        {
            Debug.LogError("错误" + e.Message);
            SavedRecordData = new();
        }



    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                    RecordButton.onClick.Invoke();
            }
        }
        



        if (CurrentState == RecorderState.Recording)
        {
            recordTimer += Time.deltaTime;
            RecordButton.interactable = true;
            RunButton.interactable = false;
            RunButton.GetComponent<Image>().color = new Color(0.4117647f, 0.8980392f, 1f);
        }
        else if (CurrentState == RecorderState.Running)
        {
            recordTimer += Time.deltaTime;
            RecordButton.interactable = false;
            RunButton.interactable = true;
            RunButton.GetComponent<Image>().color = new Color(1, 0.2966457f, 0.2966457f);

            if (CMDCopy.Count > 0)
            {
                if(recordTimer > CMDCopy[0].Time)
                {

                    danmaku.SendDanmaku(CMDCopy[0].Command);
                    CMDCopy.RemoveAt(0);
                }

           
            }

            //最后一个指令执行完可能还有时间
            if (recordTimer > CurrentRecord.RecordTime)
            {
                if (LoopToggle.isOn)//如果是加载，需要加载时设置Toggle状态
                {
                    recordTimer = 0;

                    CMDCopy = new(CurrentRecord.CMDs);
                }
                else
                {
                    CurrentState = RecorderState.Default;
                }
            }
        }
        else
        {
            RecordButton.interactable = true;

            if(CurrentRecord!= null)
            {
                RunButton.interactable = true;
                SaveButton.interactable = true;
            }
            else
            {
                RunButton.interactable = false;
                SaveButton.interactable = false;
            }
            RunButton.GetComponent<Image>().color = new Color(0.4117647f, 0.8980392f, 1f);

        }
        //不需要清除时间

        GetHMSTime(recordTimer);
        if (CurrentRecord != null)
        {
            TimerText.text = $"{GetHMSTime(recordTimer)}  共计{CurrentRecord.CMDs.Count}条指令";
        }
        else
        {
            TimerText.text = $"00:00  共计0条指令";

        }
    }

    public void SwitchRecorder()
    {
        if(CurrentState == RecorderState.Default)
        {
            CurrentState = RecorderState.Recording;
            recordTimer = 0;
            CurrentRecord = new();
            
        }
        else
        {
            CurrentState = RecorderState.Default;
            //保存其他信息
            CurrentRecord.RecordTime = recordTimer;
        }
    }

    public string GetHMSTime(float time)
    {
        float h = Mathf.FloorToInt(time / 3600f);
        float m = Mathf.FloorToInt(time / 60f - h * 60f);
        float s = Mathf.FloorToInt(time - m * 60f - h * 3600f);
        return m.ToString("00") + ":" + s.ToString("00");
    }

    public void AddDanmaku(string content)
    {
        if (CurrentState == RecorderState.Recording)
        {
            CMD cmd = new CMD();
            cmd.Command = content;
            cmd.Time = recordTimer;

            CurrentRecord.CMDs.Add(cmd);
        }



    }

    public void RunCMD()
    {
        if(CurrentState == RecorderState.Default)
        {
            CurrentState = RecorderState.Running;
            recordTimer = 0;
            CurrentState = RecorderState.Running;
            CMDCopy = new(CurrentRecord.CMDs);
            trackManager.ResetTrack();
        }
        else
        {
            CurrentState = RecorderState.Default;
        }
    }


    

    public void SaveCMD()
    {
        if (CurrentRecord != null)
        {
            RecordInfo record = new RecordInfo();
            record.CMDs = new List<CMD>(CurrentRecord.CMDs);
            record.isLoop = LoopToggle.isOn;
            record.RecordTime = CurrentRecord.RecordTime;
            record.UUID = Guid.NewGuid().ToString();

            SavedRecordData.RecordList.Add(record);

            CreateNewPanel(record);

            Debug.Log(JsonConvert.SerializeObject(SavedRecordData));
            PlayerPrefs.SetString("SavedRecordData", JsonConvert.SerializeObject(SavedRecordData));
            PlayerPrefs.Save();
            
        }


    }

    void CreateNewPanel(RecordInfo recordInfo)
    {
        CMDPanel cmdPanel = ((GameObject)GameObject.Instantiate(Resources.Load("CMDPanel"), ScrollContent)).GetComponent<CMDPanel>();
        cmdPanel.UUID = recordInfo.UUID;
        cmdPanel.CMDs = recordInfo.CMDs;
        cmdPanel.recordTimer = recordInfo.RecordTime;
        if (recordInfo.RecordName==null)
        {
            recordInfo.RecordName = recordInfo.UUID;
            cmdPanel.NameInput.text =recordInfo.RecordName;
        }
        else
        {
            cmdPanel.NameInput.text =recordInfo.RecordName;
        }
        
        cmdPanel.IsLoop = recordInfo.isLoop;
        cmdPanel.recorder = this;
        cmdPanel.scriptBind = scriptBind;
        scriptBind.addOption(recordInfo,cmdPanel);

    }
}
