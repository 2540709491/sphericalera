using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TrackManager trackManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trackManager.TrackList.Count > 0)
        {
            //int id = trackManager.GetLastReachedID();


            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, trackManager.TrackList[trackManager.TrackList.Count - 1].Pos, trackManager.MoveSpeed / 1000 * Time.deltaTime * 60f);


        }
        else
        {

        }


    }
}
