using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Danmaku;

public class ResultManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DanmakuResultList.Count > 0)
        {
            DanmakuResult Res = DanmakuResultList[0];

            ResultTip newTip = ((GameObject)GameObject.Instantiate(Resources.Load("ResultTip"), this.transform)).GetComponent<ResultTip>();
            newTip.IsSuccess = Res.IsSuccess;
            newTip.Content = Res.Content;

            DanmakuResultList.RemoveAt(0);
        }
    }
}
