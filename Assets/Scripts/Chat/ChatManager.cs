using BilibiliUtilities.Live;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public Danmaku danmaku;
    public GameObject ChatWindow;
    public Text HistoryText;
    public InputField MsgInput;
    public ScrollRect scrollRect;
    
    public bool IsOn;
    public static List<string> ChatHistory = new();
    public static LiveRoom room = null;

    float reconnectTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        ChatWindow.SetActive(false);

        StartConnect();

    }


    public void ReconnectNewRoom()
    {
        if (room != null)
        {
            room.Disconnect();
        }
        //StartConnect();
    }


    async void StartConnect()
    {
        if (room == null)
        {
            var liveHandler = new BilibiliUtilities.Test.LiveLib.LiveHandler();

            //RoomID
            int rid = PlayerPrefs.GetInt("BindRoom", 694984);

            var roomid = rid;
            //第一个参数是直播间的房间号
            //第二个参数是自己实现的处理器
            //第三个参数是可选的,可以是默认的消息分发器,也可以是自己实现的消息分发器
            room = new LiveRoom(roomid, liveHandler);
            //等待连接,该方法会反回是否连接成功
            //或者使用room.Connected,该属性会反馈连接状态
            if (!await room.ConnectAsync())
            {
                Debug.Log("连接失败");
                return;
            }
            Debug.Log(rid + "连接成功");
            //消息由Dispatcher分发到对应的MessageHandler中对应方法
            await room.ReadMessageLoop();
        }
    }


    // Update is called once per frame
    void Update()
    {


        if (IsOn)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (MsgInput.text == "")
                {
                    IsOn = false;
                    ChatWindow.SetActive(false);
                }
                else
                {
                    //发送弹幕
                    if (MsgInput.text.Replace(" ", "") != "")
                    {
                        danmaku.SendDanmaku(MsgInput.text);
                        MsgInput.text = "";
                        MsgInput.ActivateInputField();
                    }
                    else
                    {
                        MsgInput.text = "";
                        MsgInput.ActivateInputField();
                    }

                }


            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                IsOn = true;
                ChatWindow.SetActive(true);
                MsgInput.ActivateInputField();
                scrollRect.verticalNormalizedPosition = 0;
            }

        }

        int maxCount = 50;
        if (ChatHistory.Count > maxCount)
        {
            for (int i = ChatHistory.Count - maxCount - 1; i >= 0; i--)
            {
                ChatHistory.RemoveAt(i);
            }

        }

        string s = "";
        for (int i = 0; i < ChatHistory.Count; i++)
        {
            s += ChatHistory[i] + "\n";
        }
        if (s.Length > 0) s = s.Substring(0, s.Length - 1);
        HistoryText.text = s;



        //重连


        if (!room.Connected())
        {
            reconnectTimer += Time.deltaTime;
            if(reconnectTimer >= 5)
            {
                reconnectTimer = 0;
                int rid = PlayerPrefs.GetInt("BindRoom", 694984);
                ThreadStart starter = delegate { ReconnectAsync(rid); };
                new Thread(starter).Start();
            }
        }
        else
        {
            reconnectTimer = 0;
        }












    }

    async Task ReconnectAsync(int rid)
    {
        Debug.Log("重新连接中...");


        var liveHandler = new BilibiliUtilities.Test.LiveLib.LiveHandler();
        
        var roomid = rid;
        //第一个参数是直播间的房间号
        //第二个参数是自己实现的处理器
        //第三个参数是可选的,可以是默认的消息分发器,也可以是自己实现的消息分发器
        room = new LiveRoom(roomid, liveHandler);
        //等待连接,该方法会反回是否连接成功
        //或者使用room.Connected,该属性会反馈连接状态
        if (!await room.ConnectAsync())
        {
            Debug.Log("连接失败");
            return;
        }
        Debug.Log("连接成功");
        //消息由Dispatcher分发到对应的MessageHandler中对应方法
        await room.ReadMessageLoop();
    }
}
