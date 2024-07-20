using System;
using UnityEngine;
using System.Net;
using Newtonsoft.Json;
using OpenBLive.Runtime.Data;
using System.IO;
using System.Threading.Tasks;
using TMPro;


public class SingLogin : MonoBehaviour
{
    public TMP_InputField bilijctI;
    public TMP_InputField sessdataI;
    public Danmaku danmaku;
    public TextMeshProUGUI tx;

    public void LoginBySign()
    {
        PlayerPrefs.SetString("sessData", sessdataI.text);
        PlayerPrefs.SetString("biliJct", bilijctI.text);
        var sessData = PlayerPrefs.GetString("sessData", null);
        var biliJct = PlayerPrefs.GetString("biliJct", null);
        if (sessData == null)
        {
            upState();
            return;
        }

        var postUrl = "http://api.bilibili.com/x/web-interface/nav";
        //var param = $"{'color': '16777215','fontsize': '25', 'mode': '1','msg': send_meg, 'rnd': '{}'.format(ti),  'roomid': '{}'.format(roomid)   'bubble': '0',  'csrf_token': '复制自己的',  'csrf': '复制自己的',  }";


        var webrequest = (HttpWebRequest)WebRequest.Create(postUrl);
        webrequest.Method = "GET";
        webrequest.UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.75 Safari/537.36";

        var cc = new CookieContainer();
        cc.Add(new Uri(postUrl), new Cookie("bili_jct", biliJct));
        cc.Add(new Uri(postUrl), new Cookie("SESSDATA", sessData));
        webrequest.CookieContainer = cc;

        //webrequest.Headers.Add("bili_jct", auth.biliJct);
        //webrequest.Headers.Add("SESSDATA", auth.sessData);

        webrequest.Headers.Add("origin", "https://live.bilibili.com");
        using (var httpWebResponse = webrequest.GetResponse())
        using (var responseStream = new StreamReader(httpWebResponse.GetResponseStream()))
        {
            var ret = responseStream.ReadToEnd();

            Debug.Log("登陆信息" + ret);

            var root = JsonConvert.DeserializeObject<LoginState.Rootobject>(ret);
            if (root.data is { isLogin: true })
            {
                var bToken = new BToken();
                bToken.biliJct = biliJct;
                bToken.sessData = sessData;
                Danmaku.biliToken = bToken;
                danmaku.hasLogin = true;
                PlayerPrefs.Save();
            }
        }

        upState();
    }

    public class data1
    {
        public string sessData ;
        public string bilijct;
    }
    public static data1 GetBSD()
    {
        var sessData = Danmaku.biliToken.sessData;
        var biliJct = Danmaku.biliToken.biliJct;
        var data = new data1();
        if (sessData== null)
        {
            sessData = PlayerPrefs.GetString("sessData", null);
            biliJct = PlayerPrefs.GetString("biliJct", null);

        }

        data.sessData = sessData;
        data.bilijct = biliJct;
        return data;
    }
    async public static Task<bool> CheckSign()
    {
        var sessData = Danmaku.biliToken.sessData;
        var biliJct = Danmaku.biliToken.biliJct;
        if (sessData == null || biliJct == null)
        {
            sessData = PlayerPrefs.GetString("sessData", null);
            biliJct = PlayerPrefs.GetString("biliJct", null);
        }

        var postUrl = "http://api.bilibili.com/x/web-interface/nav"; // 确保URL是正确的

        // 创建请求并设置必要的属性
        var webrequest = (HttpWebRequest)WebRequest.Create(postUrl);
        webrequest.Method = "GET";
        webrequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.75 Safari/537.36";

        // 设置Cookie
        var cc = new CookieContainer();
        cc.Add(new Uri(postUrl), new Cookie("bili_jct", biliJct));
        cc.Add(new Uri(postUrl), new Cookie("SESSDATA", sessData));
        webrequest.CookieContainer = cc;

        // 设置请求头
        webrequest.Headers.Add("origin", "https://live.bilibili.com"); // 确保引号是正确的

        try
        {
            // 异步获取响应
            using (var httpResponse = (HttpWebResponse)await webrequest.GetResponseAsync())
            using (var responseStream = new StreamReader(httpResponse.GetResponseStream()))
            {
                var ret = await responseStream.ReadToEndAsync(); // 异步读取响应流

                Debug.Log("检查登录信息: " + ret);

                var root = JsonConvert.DeserializeObject<LoginState.Rootobject>(ret);
                return root.data != null && root.data.isLogin;
            }
        }
        catch (WebException ex)
        {
            // 处理可能的网络异常
            Debug.LogError("Web request failed: " + ex.Message);
            return false;
        }
    }
    async public void upState()
    {
        if (await CheckSign())
        {
            tx.text = "登录状态:已登录";
            tx.color = Color.green;
            bilijctI.text = PlayerPrefs.GetString("biliJct", null);
            sessdataI.text = PlayerPrefs.GetString("sessData", null);

        }
        else
        {
            tx.text = "登录状态:未登录";
            tx.color = Color.HSVToRGB(55, 100, 100);
            bilijctI.text = null;
            sessdataI.text = null;
            
        }
    }

    private void Start()
    {
  

        upState();
    }
}