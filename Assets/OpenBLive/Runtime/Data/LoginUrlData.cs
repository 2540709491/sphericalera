using Newtonsoft.Json;
namespace OpenBLive.Runtime.Data
{
    public struct LoginUrlData
    {
        [JsonProperty("qrcode_key")]
        public string qrcode_key;
        [JsonProperty("url")]
        public string url;
    }
}