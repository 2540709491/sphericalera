using System;
using System.Threading.Tasks;
using OpenBLive.Runtime.Data;
using NativeWebSocket;
using UnityEngine;

namespace OpenBLive.Runtime
{
    public class WebSocketBLiveClient : BLiveClient
    {
        private readonly WebsocketInfo m_WebsocketInfo;
        public WebSocket ws;

        public WebSocketBLiveClient(WebsocketInfo websocketInfo)
        {
            m_WebsocketInfo = websocketInfo;
            base.token = m_WebsocketInfo.data.authBody;
        }
        public override async void Connect()
        {
            
            //尝试释放已连接的ws
            if (ws != null && ws.State != WebSocketState.Closed)
            {
                await ws.Close();
            }
            var url = ("wss://" + m_WebsocketInfo.data.host[0] + "/sub");
            ws = new WebSocket(url);
            ws.OnOpen += OnOpen;
            ws.OnMessage += data =>
            {
                
                ProcessPacket(data);
            };
            ws.OnError += msg => { Debug.LogError("WebSocket Error Message: " + msg); };
            ws.OnClose += code => { Debug.Log("WebSocket Close: " + code); };
            await ws.Connect();
        }

        public override void Disconnect()
        {
            ws?.Close();
            ws = null;
        }

        public override void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }

        public override void Send(byte[] packet)
        {
            if (ws.State == WebSocketState.Open)
                ws.Send(packet);
        }



        public override void Send(Packet packet) => Send(packet.ToBytes);
        public override Task SendAsync(byte[] packet) => Task.Run(() => Send(packet));
        protected override Task SendAsync(Packet packet) => SendAsync(packet.ToBytes);
    }
}