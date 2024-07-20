using System;
using TMPro;
using UnityEngine;

public class 版本号显示 : MonoBehaviour
{
    public string otherinfo;
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = otherinfo!="" ? $"球纪元种田工具{Application.version}_{otherinfo}" : $"老六直播种田工具{Application.version}";
    }
}
