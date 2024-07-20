using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BindManager : MonoBehaviour
{
    public TMP_InputField RoomInput;
    // Start is called before the first frame update
    void Start()
    {
        int room = PlayerPrefs.GetInt("BindRoom", 694984);
        RoomInput.text = room.ToString();


    }

    public void OnEndEdit()
    {
        int roomID = 694984;
        if (int.TryParse(RoomInput.text, out int rid))
        {
            roomID = rid;
        }

        PlayerPrefs.SetInt("BindRoom", roomID);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("BindRoom"));
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
