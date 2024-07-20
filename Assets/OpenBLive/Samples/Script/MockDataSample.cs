using System.Text;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEngine;

namespace OpenBLive.Samples.Script
{
    public class MockDataSample : MonoBehaviour
    {
        public MockData mockData;

        private WebSocketBLiveClient m_WebSocketBLiveClient;

        // Start is called before the first frame update
        void Start()
        {
            WebsocketInfo wsInfo = default;
            m_WebSocketBLiveClient = new WebSocketBLiveClient(wsInfo);
            m_WebSocketBLiveClient.OnDanmaku += WebSocketBLiveClientOnDanmaku;
            m_WebSocketBLiveClient.OnGift += WebSocketBLiveClientOnGift;
            m_WebSocketBLiveClient.OnGuardBuy += WebSocketBLiveClientOnGuardBuy;
            m_WebSocketBLiveClient.OnSuperChat += WebSocketBLiveClientOnSuperChat;
            //设置mock的wss对象
            MockDataUtility.webSocketBLiveClient = m_WebSocketBLiveClient;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void WebSocketBLiveClientOnSuperChat(SuperChat superChat)
        {
            StringBuilder sb = new StringBuilder("收到SC!");
            sb.AppendLine();
            sb.Append("来自用户：");
            sb.AppendLine(superChat.userName);
            sb.Append("留言内容：");
            sb.AppendLine(superChat.message);
            sb.Append("金额：");
            sb.Append(superChat.rmb);
            sb.Append("元");
            Debug.Log(sb);
        }

        private void WebSocketBLiveClientOnGuardBuy(Guard guard)
        {
            StringBuilder sb = new StringBuilder("收到大航海!");
            sb.AppendLine();
            sb.Append("来自用户：");
            sb.AppendLine(guard.userInfo.userName);
            sb.Append("赠送了");
            sb.Append(guard.guardUnit);
            Debug.Log(sb);
        }

        private void WebSocketBLiveClientOnGift(SendGift sendGift)
        {
            StringBuilder sb = new StringBuilder("收到礼物!");
            sb.AppendLine();
            sb.Append("来自用户：");
            sb.AppendLine(sendGift.userName);
            sb.Append("赠送了");
            sb.Append(sendGift.giftNum);
            sb.Append("个");
            sb.Append(sendGift.giftName);
            Debug.Log(sb);
        }

        private void WebSocketBLiveClientOnDanmaku(Dm dm)
        {
            StringBuilder sb = new StringBuilder("收到弹幕!");
            sb.AppendLine();
            sb.Append("用户：");
            sb.AppendLine(dm.userName);
            sb.Append("弹幕内容：");
            sb.Append(dm.msg);
            Debug.Log(sb);
        }
    }
}