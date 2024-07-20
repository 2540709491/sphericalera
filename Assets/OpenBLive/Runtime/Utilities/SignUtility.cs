using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Networking;

namespace OpenBLive.Runtime.Utilities
{
    public static class SignUtility
    {
        #region AccessKey

        /// <summary>
        /// 开放平台的access_key_secret，请妥善保管以防泄露
        /// </summary>
        public static string accessKeySecret = "";

        /// <summary>
        /// 开放平台的access_key_id，请妥善保管以防泄露
        /// </summary>
        public static string accessKeyId = "";

        /// <summary>
        /// 应用的唯一标识。在OAuth2.0认证过程中，client_id的值即为oauth_consumer_key的值。请妥善保管以防泄露
        /// </summary>
        public static string clientId = "";

        /// <summary>
        /// 对应的密钥，访问用户资源时用来验证应用的合法性。在OAuth2.0认证过程中，secret的值即为oauth_consumer_secret的值。请妥善保管以防泄露
        /// </summary>
        public static string secret = "";

        #endregion

        private static Dictionary<string, string> OrderAndMd5(string jsonParam)
        {
            var keyValuePairs = new Dictionary<string, string>
            {
                {"x-bili-content-md5", Md5(jsonParam)},
                {"x-bili-timestamp", DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString("f0")},
                {"x-bili-signature-method", "HMAC-SHA256"},
                {"x-bili-signature-nonce", Guid.NewGuid().ToString()},
                {"x-bili-accesskeyid", accessKeyId},
                {"x-bili-signature-version", "1.0"}
            };
            Dictionary<string, string> sortDic =
                keyValuePairs.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            return sortDic;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        private static string Md5(this string source)
        {
            //MD5类是抽象类
            MD5 md5 = MD5.Create();
            //需要将字符串转成字节数组
            byte[] buffer = Encoding.UTF8.GetBytes(source);
            //加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择
            byte[] md5Buffer = md5.ComputeHash(buffer);
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得

            return md5Buffer.Aggregate<byte, string>(null, (current, b) => current + b.ToString("x2"));
        }

        /// <summary>
        /// 计算签名
        /// </summary>
        private static string CalculateSignature(Dictionary<string, string> keyValuePairs)
        {
            string sig = string.Empty;
            foreach (var item in keyValuePairs)
            {
                if (string.IsNullOrEmpty(sig))
                {
                    sig += item.Key + ":" + item.Value;
                }
                else
                {
                    sig += "\n" + item.Key + ":" + item.Value;
                }
            }

            return HmacSHA256(sig, accessKeySecret);
        }

        private static string HmacSHA256(string message, string secret)
        {
            secret ??= "";
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using var hash256 = new HMACSHA256(keyByte);
            byte[] hash = hash256.ComputeHash(messageBytes);
            StringBuilder builder = new StringBuilder();
            foreach (var t in hash)
            {
                builder.Append(t.ToString("x2"));
            }

            return builder.ToString();
        }


        public static void SetReqHeader(UnityWebRequest webRequest, string jsonParam, string cookie = null)
        {
            var sortDic = OrderAndMd5(jsonParam);
            foreach (var item in sortDic)
            {
                webRequest.SetRequestHeader(item.Key, item.Value);
            }

            var auth = CalculateSignature(sortDic);
            webRequest.SetRequestHeader("Authorization", auth);
            webRequest.SetRequestHeader("Method", "POST");
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            if (cookie != null)
            {
                webRequest.SetRequestHeader("Cookie", cookie);
            }
        }
    }
}