using System;
using System.Threading;
using System.Threading.Tasks;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEngine;

namespace OpenBLive.Runtime
{
    public delegate void QrScannedEventHandler();

    public delegate void QrLoginSucceedEventHandler(BToken bToken);

    public delegate void QrStatusFailed(Exception e);

    public class PollingBToken : IDisposable
    {
        public event QrScannedEventHandler QrScanned;
        public event QrLoginSucceedEventHandler QrLoginSucceed;
        public event QrStatusFailed QrStatusFailed;
        private readonly CancellationTokenSource m_Cancellation;
        private readonly string m_AuthKey;
        /// <summary>
        /// 轮询获取b站的登录token
        /// </summary>
        /// <param name="oauthKey"></param>
        /// <param name="cancellation"></param>
        public PollingBToken(string oauthKey, CancellationTokenSource cancellation = null)
        {
            m_AuthKey = oauthKey;
            m_Cancellation = cancellation ?? new CancellationTokenSource();
        }

        private async Task GetBToken()
        {
            var isScanned = false;
            while (true)
            {
                var loginStatus = await BApi.GetLoginStatus(m_AuthKey);
                try
                {
                    switch (loginStatus)
                    {
                        case LoginStatusReady ready:
                            //登录成功
                            var param = ready.data.Url.Split("?")[1];
                            var nvc = BiliHttpUtility.ParseQueryString(param);
                            BToken bToken = new BToken
                            {
                                code = 0,
                                dedeUserID = long.Parse(nvc.Get("DedeUserID")),
                                dedeUserIDCkMd5 = nvc.Get("DedeUserID__ckMd5"),
                                sessData = nvc.Get("SESSDATA"),
                                biliJct = nvc.Get("bili_jct"),
                            };
                            QrLoginSucceed?.Invoke(bToken);
                            return;
                        case LoginStatusScanning scanning:

                            bToken.code = scanning.data.code;
                            switch (bToken.code)
                            {
                                //case 0:
                                /*case -2:
                                    //oauthKey有问题，终止轮询
                                    var exception = new Exception("oauthKey有问题，终止轮询");
                                    QrStatusFailed?.Invoke(exception);
                                    return;**/
                                case 86090:
                                    //已扫码，未确认
                                    if (isScanned == false)
                                    {
                                        QrScanned?.Invoke();
                                        isScanned = true;
                                    }
                                    break;
                                case 86038:
                                    //二维码过期
                                    
                                    var exception = new Exception("二维码过期");
                                    QrStatusFailed?.Invoke(exception);
                                    return;
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    QrStatusFailed?.Invoke(e);
                    return;
                }


                switch (m_Cancellation)
                {
                    case {IsCancellationRequested: true}:
                    case null:
                        return;
                    default:
                        //每秒轮询一次
                        await Task.Delay(1000);
                        break;
                }
            }
        }

        public void Start()
        {
            var task = GetBToken();
            if (task.Status == TaskStatus.Created)
                task.Start();
        }

        public void Dispose()
        {
            m_Cancellation.Cancel();
        }
    }
}