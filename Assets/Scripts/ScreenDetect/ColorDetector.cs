using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorDetector : MonoBehaviour
{
    public WinSetting winSetting;
    public ChatManager chatManager;
    public Manager manager;
    public bool isWaitToClick;

    public TMP_Dropdown dropdown;
    //public Button AddButton;
    public Mouse mouse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //winSetting.GetMouseRealPos(out int x, out int y);
        //Color c = GetPixelColor(x,y);
        //Debug.Log(c.r +" " + c.g + " " + c.b);
    }

    public void AddPointButton()
    {
        isWaitToClick = true;
    }

    public void ClearAllPoints()
    {
        isWaitToClick = false;
        GameObject[] points = GameObject.FindGameObjectsWithTag("DetectPoint");
        foreach(GameObject point in points)
        {
            GameObject.Destroy(point);
        }




    }


    public void AddPoint()
    {
        DetectPoint point = ((GameObject)GameObject.Instantiate(Resources.Load("DetectPoint"))).GetComponent<DetectPoint>();
        point.transform.position = mouse.transform.position;

        winSetting.GetMouseRealPos(out int x, out int y);
        point.RealPosX = x;
        point.RealPosY = y;
        point.SavedColor = GetPixelColor(x, y);
        point.chatManager = this.chatManager;
        point.manager = manager;
        point.CmdPanel = dropdown.GetComponent<ScriptBind>().cmds[dropdown.value];


    }













    static public Color GetPixelColor(int x, int y)//https://www.orcode.com/question/1032624_k52683.html
    {
        IntPtr hdc = GetDC(IntPtr.Zero);
        uint pixel = GetPixel(hdc, x, y);
        ReleaseDC(IntPtr.Zero, hdc);
        System.Drawing.Color color = System.Drawing.Color.FromArgb((int)(pixel & 0x000000FF),
                     (int)(pixel & 0x0000FF00) >> 8,
                     (int)(pixel & 0x00FF0000) >> 16);
        return new Color(color.R / 255f, color.G / 255f, color.B / 255f);
    }

    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("gdi32.dll")]
    static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
}
