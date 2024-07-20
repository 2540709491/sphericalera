using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackManager : MonoBehaviour
{
    public float Delay;
    public float MoveSpeed;

    public GameObject Player;
    public Manager manager;
    
    static bool isFirstClick = true;
    static Vector3 playerPos;


    public LineRenderer lineRenderer;
    public List<TrackInfo> TrackList = new List<TrackInfo>();

    public class TrackInfo
    {
        public float Time;
        public Vector3 Pos;
        public bool HasReached = false;//暂时不用
    }


    public void SetSpeed(TMP_InputField input)
    {
        MoveSpeed = float.Parse(input.text);
    }
    public void SetDelay(TMP_InputField input)
    {
        Delay = float.Parse(input.text);
    }

    public void SetMainRoom()
    {
        if (manager.PlayerBlock != null)
        {
            manager.MainRoom = manager.PlayerBlock;
        }
    }

    public void TpMainRoom()
    {
        if (manager.MainRoom != null)
        {
            ResetTrack();
            AddTrack(manager.MainRoom);
        }
    }

    public void SetTrackEnable(Toggle toggle)
    {
        if (toggle.isOn)
        {
            lineRenderer.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            lineRenderer.transform.position = new Vector3(0, 1000, 0);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        UpdateLineRender();
        Player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //清除时间过长的
        int count = TrackList.Count;
        if (count > 0)
        {
            TrackList[TrackList.Count - 1].Time = Time.time;//保留最后一个点
            for (int i = TrackList.Count - 1; i >= 0; i--)
            {
                if (TrackList[i].Time < Time.time - 25)
                {
                    TrackList.RemoveAt(i);
                }
            }





        }
        else
        {
            Player.SetActive(false);
            isFirstClick = true;
        }

        UpdateLineRender();

        playerPos = Player.transform.localPosition;

    }


    public void AddTrack(Block block)
    {
        if (isFirstClick)
        {
            isFirstClick = false;

            Player.SetActive(true);
            Player.transform.localPosition = block.transform.position;

        }
        float time = Time.time + Delay;
        Vector3 blockPos = block.transform.position;
        AddTrackWithDelay(blockPos, time);
        /*
        ThreadStart threadStart = new ThreadStart(() =>
        {
            AddTrackWithDelay(blockPos, time);
        });
        Thread thread = new Thread(threadStart);
        thread.Start();*/




    }

    void AddTrackWithDelay(Vector3 blockPos, float time)
    {
        //Thread.Sleep((int)(Delay * 1000));

        TrackInfo trackInfo = new TrackInfo();
        trackInfo.Time = time;
        trackInfo.Pos = blockPos;

        if (TrackList.Count > 0)
        {
            //寻找上一个已经被reach的点
            int lastReached = GetLastReachedID();

            //移除剩余点
            if (TrackList.Count > lastReached + 1)
            {
                for (int i = TrackList.Count - 1; i > lastReached; i--)
                {
                    TrackList.RemoveAt(i);
                }
            }

            //在当前玩家处创建新路径点
            TrackInfo middleTrackInfo = new TrackInfo();
            middleTrackInfo.Time = time;
            middleTrackInfo.Pos = playerPos;
            middleTrackInfo.HasReached = true;


            TrackList.Add(middleTrackInfo);




        }




        TrackList.Add(trackInfo);
    }

    public int GetLastReachedID()
    {
        for (int i = TrackList.Count - 1; i >= 0; i--)
        {
            if (TrackList[i].HasReached)
            {
                return i;
            }
        }

        return 0;
    }



    public void ResetTrack()
    {
        isFirstClick = true;
        TrackList = new List<TrackInfo>();

        UpdateLineRender();
    }
    void UpdateLineRender()
    {
        Vector3[] Pos = new Vector3[TrackList.Count];
        for(int i = 0; i < TrackList.Count; i++)
        {
            Pos[i] = TrackList[i].Pos;
        }
        lineRenderer.positionCount = Pos.Length;
        lineRenderer.SetPositions(Pos);
    }
}
