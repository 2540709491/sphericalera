using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using RenderHeads.Media.AVProVideo;
using UnityEngine.Video;

[System.Serializable]
public class LiveStreamData1
{
    public int code;
    public string message;
    public Data1 data;
}

[System.Serializable]
public class Data1
{
    public Durl1[] durl;
}

[System.Serializable]
public class Durl1
{
    public string url;
}

public class 视频流获取与管理 : MonoBehaviour
{
    public MediaPlayer VP;
    public string splurl;
    
    private string GetLiveStreamUrl(string room_id, string quality = "400", string platform = "h5")
    {
        string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299";
        string url = "https://api.live.bilibili.com/room/v1/Room/playUrl";
        
        // 构建请求参数
        var parameters = new Dictionary<string, string>
        {
            { "cid", room_id },
            { "qn", quality },
            { "platform", platform }
        };

        // 构建查询字符串
        string queryString = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

        // 创建请求
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{url}?{queryString}");
        request.Method = "GET";
        request.UserAgent = userAgent;

        try
        {
            // 发送请求并获取响应
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // 读取响应内容
                    string jsonResponse = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    var data = JsonUtility.FromJson<LiveStreamData1>(jsonResponse);

                    if (data.code == 0)
                    {
                        return data.data.durl[0].url;
                    }
                    else
                    {
                        Debug.LogError($"Error: {data.message}");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError($"HTTP Error: {response.StatusCode}");
                    return null;
                }
            }
        }
        catch (WebException e)
        {
            Debug.LogError($"WebException: {e.Message}");
            return null;
        }
    }

    void Start()
    {
        VP = GetComponent<MediaPlayer>();
        string room_id = "694984";
        string stream_url = GetLiveStreamUrl(room_id);
        if (stream_url != null)
        {
            splurl = stream_url;

            VP.OpenMedia(new MediaPath(stream_url, MediaPathType.AbsolutePathOrURL));
            
            VP.Play();
        }

    }

}