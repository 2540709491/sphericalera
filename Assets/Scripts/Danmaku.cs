using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using NativeWebSocket;
using Newtonsoft.Json;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Recorder;

public class Danmaku : MonoBehaviour
{
    private long m_RoomId;
    public RawImage image;
    public Texture2D icon;
    private string m_QrAuthKey;
    public string accessKeySecret;
    public string accessKeyId;
    private CancellationTokenSource m_Cancellation;
    private PollingBToken m_Polling;

    private WebSocketBLiveClient m_WebSocketBLiveClient;


    public static BToken biliToken;
    public GameObject LoginPanel;
    public Toggle SaveSign;
    public Recorder DanmakuRecorder;
    public Toggle AvoidRepeatToggle;


    bool isSending = false;

    public float sendingTimer = 0;
    public float sendingNextInterval = 0.5f;
    int currentLine;
    public List<string> DanmakuList;
    public static List<DanmakuIn5S> DanmakuIn5SList = new List<DanmakuIn5S>();
    public static List<string> SymbolList = new List<string>() { ".", "。", "，", "!", "?", "~","@","%","#","\\","/" };
    public static int SymbolID = 0;

    public bool hasLogin;

    public static List<DanmakuResult> DanmakuResultList = new List<DanmakuResult>();

    public class DanmakuResult
    {
        public bool IsSuccess;
        public string Content;
    }


    public async void showQR()
    {
        //获取b站扫码接口
        var urlQr = await BApi.GetLoginQrInfo();
        m_QrAuthKey = urlQr.data.qrcode_key;
        //生成二维码地址到图片
        Texture2D texture2D =
            QrCodeUtility.GenerateQrImage(urlQr.data.url, icon, width: 512, height: 512, color: Color.black);
        image.texture = texture2D;
        //开始轮询二维码状态
        StartPolling();
    }

    #region BiliAPI

    private void StartPolling()
    {
        //测试环境的域名现在不可用，默认开启正式环境
        BApi.isTestEnv = false;
        //测试的密钥
        SignUtility.accessKeySecret = accessKeySecret;
        //测试的ID
        SignUtility.accessKeyId = accessKeyId;
        m_Polling = new PollingBToken(m_QrAuthKey);
        m_Polling.QrScanned += PollingBTokenOnQrScanned;
        m_Polling.QrLoginSucceed += PollingBTokenOnQrLoginSucceed;
        m_Polling.QrStatusFailed += PollingBTokenOnQrStatusFailed;
        m_Polling.Start();
    }

    private void PollingBTokenOnQrStatusFailed(Exception e)
    {
        Debug.Log("扫码失败" + e.Message);
        if (!hasLogin)
        {
            ChatManager.room.Disconnect();
            SceneManager.LoadScene("Game");
        }

        if (e.Message == "二维码过期")
        {
            showQR();
        }
    }

    private void PollingBTokenOnQrScanned()
    {
        Debug.Log("二维码被扫了");
    }

    private void PollingBTokenOnQrLoginSucceed(BToken bToken)
    {
        var json = JsonConvert.SerializeObject(bToken);
        Debug.Log("扫码成功 biliJct:" + json);
        if (bToken.code == 0)
        {
            bToken.sessData = WebUtility.UrlEncode(bToken.sessData);
            biliToken = bToken;

            if (SaveSign.isOn)
            {
                PlayerPrefs.SetString("sessData",bToken.sessData);
                PlayerPrefs.SetString("biliJct", bToken.biliJct);
                PlayerPrefs.Save();
            }


            LoginPanel.SetActive(false);
            hasLogin = true;
            //ThreadStart starter = delegate { SendDanmaku("1"); };
            //new Thread(starter).Start();
        }
        else
        {
            if (!hasLogin)
            {
                ChatManager.room.Disconnect();
                SceneManager.LoadScene("Game");
            }
        }
    }

    #endregion

    public void SendDanmaku(string content)
    {
        if (AvoidRepeatToggle.isOn)
        {
            string originalContent = content;
            for (int i = DanmakuIn5SList.Count - 1; i >= 0; i--)
            {
                if (DanmakuIn5SList[i].Time < Time.time - 5.1)
                {
                    DanmakuIn5SList.RemoveAt(i);
                }
            }


            for (int i = DanmakuIn5SList.Count - 1; i >= 0; i--)
            {
                if (DanmakuIn5SList[i].Danmaku == content)
                {
                    //排除黑名单
                    if (content!="J")
                    {
                        content += SymbolList[SymbolID];
                        SymbolID++;
                        if (SymbolID >= SymbolList.Count - 1)
                        {
                            SymbolID = 0;
                        }
                    }

                }
            }

            DanmakuIn5S danmakuIn5S = new DanmakuIn5S() { Danmaku = originalContent, Time = Time.time };
            DanmakuIn5SList.Add(danmakuIn5S);
        }

        DanmakuRecorder.AddDanmaku(content);


        if (hasLogin)
        {
            DanmakuList.Add(content);
        }
        else
        {
            Debug.Log("未登录。待发弹幕: " + content);
        }
    }


    public class DanmakuIn5S
    {
        public string Danmaku;
        public float Time;
    }


    private static void SendCurrent(string content, int randomSeed, int roomID)
    {
        //Thread.Sleep(100);
        Debug.Log("发送弹幕...");
        BToken auth = biliToken;
        string s;
        var postUrl = "https://api.live.bilibili.com/msg/send";
        //var param = $"{'color': '16777215','fontsize': '25', 'mode': '1','msg': send_meg, 'rnd': '{}'.format(ti),  'roomid': '{}'.format(roomid)   'bubble': '0',  'csrf_token': '复制自己的',  'csrf': '复制自己的',  }";
        var param =
            $"color=5566168&fontsize=25&mode=1&msg={HttpUtility.UrlEncode(content)}&rnd={randomSeed}&roomid={roomID}&csrf_token={auth.biliJct}&csrf={auth.biliJct}";


        HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(postUrl);
        webrequest.Method = "post";
        webrequest.UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.75 Safari/537.36";

        byte[] postdatabyte = Encoding.UTF8.GetBytes(param);
        webrequest.ContentLength = postdatabyte.Length;
        webrequest.ContentType = "application/x-www-form-urlencoded";
        webrequest.Referer = $"https://live.bilibili.com/" +
                             $"{roomID}?broadcast_type=0&spm_id_from=333.1007.top_right_bar_window_dynamic.content.click";

        CookieContainer cc = new CookieContainer();
        cc.Add(new Uri(postUrl), new Cookie("bili_jct", auth.biliJct));
        cc.Add(new Uri(postUrl), new Cookie("SESSDATA", auth.sessData));
        webrequest.CookieContainer = cc;

        //webrequest.Headers.Add("bili_jct", auth.biliJct);
        //webrequest.Headers.Add("SESSDATA", auth.sessData);

        webrequest.Headers.Add("origin", "https://live.bilibili.com");

        Stream stream;
        stream = webrequest.GetRequestStream();
        stream.Write(postdatabyte, 0, postdatabyte.Length);
        stream.Close();


        using (var httpWebResponse = webrequest.GetResponse())
        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream()))
        {
            String ret = responseStream.ReadToEnd();

            //T result = XmlDeserialize<T>(ret);

            Debug.Log("发送弹幕 " + content + " 返回:" + ret);

            DanmakuResult result = new DanmakuResult();
            result.Content = content;
            if (result.Content.Length > 4)
            {
                result.Content = result.Content.Substring(0, 4) + "...";
            }

            if (ret.Contains("\"code\":0"))
            {
                result.IsSuccess = true;
            }

            DanmakuResultList.Add(result);
        }
    }

    //弹幕返回值 code=0发送成功，code=10031您发送弹幕的频率过快


    private void Update()
    {
        sendingTimer -= Time.deltaTime;
        if (sendingTimer <= 0)
        {
            if (DanmakuList.Count > 0)
            {
                int ran = UnityEngine.Random.Range(100000, 200000);
                string content = DanmakuList[0];

                //RoomID
                int roomID = PlayerPrefs.GetInt("BindRoom", 694984);

                ThreadStart starter = delegate { SendCurrent(content, ran, roomID); };
                new Thread(starter).Start();

                DanmakuList.RemoveAt(0);

                sendingTimer = 0.5f + UnityEngine.Random.Range(0f, 0.5f);
            }
            else
            {
                sendingTimer = 0;
            }
        }
    }
}