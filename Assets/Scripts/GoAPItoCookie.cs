using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAPItoCookie : MonoBehaviour
{
    public GameObject gb;
public void GoToAPI()
    {
        Application.OpenURL("https://api.bilibili.com/x/web-interface/nav");
    }
    public void ShowImage()
    {
        gb.SetActive(true);
    }
}
