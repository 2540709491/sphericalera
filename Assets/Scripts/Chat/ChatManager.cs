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
            //��һ��������ֱ����ķ����
            //�ڶ����������Լ�ʵ�ֵĴ�����
            //�����������ǿ�ѡ��,������Ĭ�ϵ���Ϣ�ַ���,Ҳ�������Լ�ʵ�ֵ���Ϣ�ַ���
            room = new LiveRoom(roomid, liveHandler);
            //�ȴ�����,�÷����ᷴ���Ƿ����ӳɹ�
            //����ʹ��room.Connected,�����Իᷴ������״̬
            if (!await room.ConnectAsync())
            {
                Debug.Log("����ʧ��");
                return;
            }
            Debug.Log(rid + "���ӳɹ�");
            //��Ϣ��Dispatcher�ַ�����Ӧ��MessageHandler�ж�Ӧ����
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
                    //���͵�Ļ
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



        //����


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
        Debug.Log("����������...");


        var liveHandler = new BilibiliUtilities.Test.LiveLib.LiveHandler();
        
        var roomid = rid;
        //��һ��������ֱ����ķ����
        //�ڶ����������Լ�ʵ�ֵĴ�����
        //�����������ǿ�ѡ��,������Ĭ�ϵ���Ϣ�ַ���,Ҳ�������Լ�ʵ�ֵ���Ϣ�ַ���
        room = new LiveRoom(roomid, liveHandler);
        //�ȴ�����,�÷����ᷴ���Ƿ����ӳɹ�
        //����ʹ��room.Connected,�����Իᷴ������״̬
        if (!await room.ConnectAsync())
        {
            Debug.Log("����ʧ��");
            return;
        }
        Debug.Log("���ӳɹ�");
        //��Ϣ��Dispatcher�ַ�����Ӧ��MessageHandler�ж�Ӧ����
        await room.ReadMessageLoop();
    }
}
