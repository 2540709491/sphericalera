using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BilibiliUtilities.Live.Lib;
using BilibiliUtilities.Live.Message;
using UnityEngine;

namespace BilibiliUtilities.Test.LiveLib
{
    public class LiveHandler:IMessageHandler
    {
        //可以放置自己的参数用来使用,比如WPF的window对象
        public bool Param;

        
        public async Task DanmuMessageHandlerAsync(DanmuMessage danmuMessage)
        {
            //GameData.DanmuMessages.Add(danmuMessage);
            Debug.Log($"发送者:{danmuMessage.Username},内容:{danmuMessage.Content}");
            if (!IsNumberAndWord(danmuMessage.Content) || danmuMessage.Content.Length > 3)
            {
                ChatManager.ChatHistory.Add($"<color=#00FFEB>{danmuMessage.Username}: </color>{danmuMessage.Content}");
            }
        }

        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        public static bool IsNumberAndWord(string value)
        {
            Regex r = new Regex(@"^[a-zA-Z0-9+]+$");
            return r.Match(value).Success;
        }



        public async Task AudiencesHandlerAsync(int audiences)
        {
            Debug.Log($"当前人气值:{audiences}");
        }

        public async Task NoticeMessageHandlerAsync(NoticeMessage noticeMessage)
        {
            Debug.Log("通知信息未处理");
        }

        public async Task GiftMessageHandlerAsync(GiftMessage giftMessage)
        {
            //如果礼物不是辣条
            if (giftMessage.GiftId!=1)
            {
                Debug.Log($"{giftMessage.Username}送出了{giftMessage.GiftNum}个{giftMessage.GiftName},价值:{giftMessage.TotalCoin}个{giftMessage.CoinType}");
            }
        }

        public async Task WelcomeMessageHandlerAsync(WelcomeMessage welcomeMessage)
        {
            Debug.Log($"欢迎{welcomeMessage.Username}进入直播间");
        }

        public async Task ComboEndMessageHandlerAsync(ComboEndMessage comboEndMessage)
        {
            Debug.Log($"{comboEndMessage.Username}的{comboEndMessage.GiftName}连击结束了,送出了{comboEndMessage.ComboNum}个,总价值{comboEndMessage.Price}个金瓜子");
        }

        public async Task RoomUpdateMessageHandlerAsync(RoomUpdateMessage roomUpdateMessage)
        {
            Debug.Log($"UP当前粉丝数量{roomUpdateMessage.Fans}");
        }

        public async Task WelcomeGuardMessageHandlerAsync(WelcomeGuardMessage welcomeGuardMessage)
        {
            Debug.Log($"房管{welcomeGuardMessage.Username}进入直播间");
        }

        public async Task LiveStartMessageHandlerAsync(int roomId)
        {
            Debug.Log("直播开始");
        }

        public async Task LiveStopMessageHandlerAsync(int roomId)
        {
            Debug.Log("直播关闭");
        }

        public async Task EntryEffectMessageHandlerAsync(EntryEffectMessage entryEffectMessage)
        {
            Debug.Log($"⚡⚡⚡<特效>⚡⚡⚡{entryEffectMessage.CopyWriting}⚡⚡⚡<特效>⚡⚡⚡");
        }

        public async Task GuardBuyMessageHandlerAsync(GuardBuyMessage guardBuyMessage)
        {
            Debug.Log($"{guardBuyMessage.Username}购买了{guardBuyMessage.Num}月的{guardBuyMessage.GiftName}");
        }

        public async Task UserToastMessageHandlerAsync(UserToastMessage userToastMessage)
        {
            Debug.Log($"{userToastMessage.Username}购买了{userToastMessage.Num}{userToastMessage.Unit}的{userToastMessage.RoleName}");
        }

        public async Task InteractWordMessageHandlerAsync(InteractWordMessage message)
        {
            if (!string.IsNullOrEmpty(message.Medal))
                Debug.Log($"{message.Medal}.{message.MedalLevel}  {message.Username} 进入直播间");
            else
                Debug.Log($"{message.Username} 进入直播间");
        }
    }
}