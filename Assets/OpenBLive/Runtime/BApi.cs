using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace OpenBLive.Runtime
{
    /// <summary>
    /// 各类b站api
    /// </summary>
    public static class BApi
    {
        /// <summary>
        /// 是否为测试环境的api
        /// </summary>
        public static bool isTestEnv;

        /// <summary>
        /// 开放平台域名
        /// </summary>
        private static string OpenLiveDomain =>
            isTestEnv ? "http://test-live-open.biliapi.net" : "https://live-open.biliapi.com";

        /// <summary>
        /// 获取长链地址(需要access_key_id与access_key_secret)
        /// </summary>
        private const string k_WssInfoApi = "/v1/common/websocketInfo";

        /// <summary>
        /// 客户端获取长链地址
        /// </summary>
        private const string k_WssInfoByClientApi = "/v1/common/websocketInfoByClient?csrf=";

        /// <summary>
        /// 房间信息聚合接口 “stream”-流信息 “show”-展示相关 “status”-状态 “area”-分区信息
        /// </summary>
        private const string k_RoomDetailApi = "/v1/common/roomDetail";

        /// <summary>
        /// 根据分区和子分区拉取房间列表和初步信息（在白名单中）一次拉取最多50条
        /// </summary>
        private const string k_RoomListApi = "/v1/common/roomListByArea";

        /// <summary>
        /// 获取房间长号和短号
        /// </summary>
        private const string k_RoomIdApi = "/v1/common/roomIdInfo";

        /// <summary>
        /// 二维码登录入口url
        /// </summary>
        private const string k_QrcodeUrl = "https://passport.bilibili.com/x/passport-login/web/qrcode/generate";//已改

        /// <summary>
        /// 获取登录信息url
        /// </summary>
        private const string k_GetLoginInfoUrl = "https://passport.bilibili.com/x/passport-login/web/qrcode/poll";//已改


        public static async Task<WebsocketInfo> GetWebsocketInfoViaPass(long roomId)
        {
            WebsocketInfo websocketInfo = default;
            var postUrl = OpenLiveDomain + k_WssInfoApi;
            var param = $"{{\"room_id\":{roomId}}}";

            var result = await RequestWebUTF8(postUrl, UnityWebRequest.kHttpVerbPOST, param);

            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    websocketInfo = JsonConvert.DeserializeObject<WebsocketInfo>(result);
                }
                catch (Exception e)
                {
                    Debug.LogError("转换wssInfo失败，返回数据： " + result);
                }
            }


            return websocketInfo;
        }

        public static async Task<WebsocketInfo> GetWebsocketInfoViaBToken(BToken auth)
        {
            WebsocketInfo websocketInfo = default;
            var postUrl = OpenLiveDomain + k_WssInfoByClientApi + auth.biliJct;
            var param = $"{{\"uid\":{123}}}";

            StringBuilder sb = new StringBuilder();
            sb.Append("bili_jct=");
            sb.Append(auth.biliJct);
            sb.Append("; ");
            sb.Append("SESSDATA=");
            sb.Append(auth.sessData);

            var result = await RequestWebUTF8(postUrl, UnityWebRequest.kHttpVerbPOST, param, sb.ToString());
            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    websocketInfo = JsonConvert.DeserializeObject<WebsocketInfo>(result);
                }
                catch (Exception e)
                {
                    Debug.LogError("转换wssInfo失败，返回数据： " + result);
                }
            }

            return websocketInfo;
        }

        /// <summary>
        /// 获取登录状态
        /// </summary>
        /// <param name="qrcode_key"></param>
        /// <returns></returns>
        public static async Task<object> GetLoginStatus(string qrcode_key)
        {
            var postUrl = k_GetLoginInfoUrl + $"?qrcode_key={qrcode_key}";
            var result = await RequestWebUTF8(postUrl, UnityWebRequest.kHttpVerbGET, null);
            Debug.Log("登录检测：" + JsonConvert.SerializeObject(result));
            //if (string.IsNullOrEmpty(result)) return null;
            LoginStatusRootobject root = JsonConvert.DeserializeObject<LoginStatusRootobject>(result);
            if (root.data.code == 0)
            {
                Debug.Log("登录成功");
                var ready = JsonConvert.DeserializeObject<LoginStatusReady>(result);
                return ready;
            }
            Debug.Log("登录失败");
            var scanning = JsonConvert.DeserializeObject<LoginStatusScanning>(result);
            return scanning;
        }


        public class LoginStatusRootobject
        {
            public int code { get; set; }
            public string message { get; set; }
            public int ttl { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public string url { get; set; }
            public string refresh_token { get; set; }
            public long timestamp { get; set; }
            public int code { get; set; }
            public string message { get; set; }
        }


        /// <summary>
        /// 获取b站登录二维码地址
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<LoginUrl> GetLoginQrInfo(string param = null)
        {
            LoginUrl loginUrl = default;
            var result = await RequestWebUTF8(k_QrcodeUrl, UnityWebRequest.kHttpVerbGET, param);
            if (!string.IsNullOrEmpty(result))
            {
                loginUrl = JsonConvert.DeserializeObject<LoginUrl>(result);
            }

            return loginUrl;
        }


        /// <summary>
        /// 获取真实房间号
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static async Task<long> GetRoomIdInfo(long roomId)
        {
            long realRoomId = 0;
            var postUrl = OpenLiveDomain + k_RoomIdApi;
            var param = $"{{\"room_id\":{roomId}}}";

            var result = await RequestWebUTF8(postUrl, UnityWebRequest.kHttpVerbPOST, param);
            if (string.IsNullOrEmpty(result)) return realRoomId;
            var json = JObject.Parse(result);
            realRoomId = json["data"]!["room_id"]!.ToObject<long>();

            return realRoomId;
        }


        public static async Task<string> RequestWebUTF8(string url, string method, string param, string cookie = null)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url);
            webRequest.method = method;
            if (param != null)
            {
                SignUtility.SetReqHeader(webRequest, param, cookie);
                var bytes = System.Text.Encoding.UTF8.GetBytes(param);
                webRequest.uploadHandler = new UploadHandlerRaw(bytes);
            }

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.disposeUploadHandlerOnDispose = true;
            webRequest.disposeDownloadHandlerOnDispose = true;
            await webRequest.SendWebRequest();
            var text = webRequest.downloadHandler.text;

            webRequest.Dispose();
            return text;
        }

        private static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += _ => { tcs.SetResult(null); };
            return ((Task)tcs.Task).GetAwaiter();
        }
    }
}