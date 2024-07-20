using OpenBLive.Runtime.Data;
using UnityEngine;

namespace OpenBLive.Runtime.Utilities
{
    public static class MockDataUtility
    {
        public static WebSocketBLiveClient webSocketBLiveClient;
        public static void Mock(this Dm dm)
        {
            webSocketBLiveClient?.Mock(dm);
        }
        public static void Mock(this SendGift sendGift)
        {
            webSocketBLiveClient?.Mock(sendGift);
        }
        public static void Mock(this SuperChat superChat)
        {
            webSocketBLiveClient?.Mock(superChat);
        }
        public static void Mock(this Guard guard)
        {
            webSocketBLiveClient?.Mock(guard);
        }
        public static void Mock(this SuperChatDel superChatDel)
        {
            webSocketBLiveClient?.Mock(superChatDel);
        }
    }
}