using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;

//https://blog.csdn.net/q493201681/article/details/65936592

/// <summary>
/// 一共可选择三种样式
/// </summary>
public enum enumWinStyle
{
    /// <summary>
    /// 置顶
    /// </summary>
    WinTop,
    /// <summary>
    /// 置顶并且透明
    /// </summary>
    WinTopApha,
    /// <summary>
    /// 置顶透明并且可以穿透
    /// </summary>
    WinTopAphaPenetrate
}
public class WinSetting : MonoBehaviour
{
    public Mouse mouse;
    public SpriteRenderer WindowTop;
    bool isDragging;
    public SpriteRenderer WindowScale;
    bool isScalling;
    public SpriteRenderer PinArea;



    int topWindow = -2;//窗口排序 -1置顶，-2正常

    #region Win函数常量 https://www.lmlphp.com/user/57942/article/item/2107051/
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

    [DllImport("Dwmapi.dll")]
    static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    private const int WS_POPUP = 0x800000;
    private const int GWL_EXSTYLE = -20;
    private const int GWL_STYLE = -16;
    private const int WS_EX_LAYERED = 0x00080000;
    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;
    private const int SWP_SHOWWINDOW = 0x0040;
    private const int LWA_COLORKEY = 0x00000001;
    private const int LWA_ALPHA = 0x00000002;
    private const int WS_EX_TRANSPARENT = 0x20;
    //
    private const int ULW_COLORKEY = 0x00000001;
    private const int ULW_ALPHA = 0x00000002;
    private const int ULW_OPAQUE = 0x00000004;
    private const int ULW_EX_NORESIZE = 0x00000008;
    #endregion
    //
    public string strProduct;//项目名称
    public enumWinStyle WinStyle = enumWinStyle.WinTop;//窗体样式
    //
    public int ResWidth;//窗口宽度
    public int ResHeight;//窗口高度
    //
    public int currentWindowX;//窗口左上角坐标x
    public int currentWindowY;//窗口左上角坐标y

    int oldWindowWidth;
    int oldWindowHeight;
    int oldMouseX;
    int oldMouseY;
    //
    private bool isApha;//是否透明
    private bool isAphaPenetrate;//是否要穿透窗体
    // Use this for initialization
    void Awake()
    {

        ResWidth = PlayerPrefs.GetInt("WindowW", 800);
        ResHeight = PlayerPrefs.GetInt("WindowH", 450);
        currentWindowX = PlayerPrefs.GetInt("WindowX", 0);
        currentWindowY = PlayerPrefs.GetInt("WindowY", 0);

        if (currentWindowX < 0)
        {
            currentWindowX = 0;
        }
        if (currentWindowY < 0)
        {
            currentWindowY = 0;
        }

        if (currentWindowX + ResWidth > Screen.currentResolution.width)
        {
            currentWindowX = Screen.currentResolution.width - ResWidth;
        }
        if (currentWindowY + ResHeight > Screen.currentResolution.height)
        {
            currentWindowY = Screen.currentResolution.height - ResHeight;
        }

        Screen.fullScreen = false;
#if UNITY_EDITOR
        print("编辑模式不更改窗体");
#else
        switch (WinStyle)
        {
            case enumWinStyle.WinTop:
                isApha = false;
                isAphaPenetrate = false;
                break;
            case enumWinStyle.WinTopApha:
                isApha = true;
                isAphaPenetrate = false;
                break;
            case enumWinStyle.WinTopAphaPenetrate:
                isApha = true;
                isAphaPenetrate = true;
                break;
        }
        //
        IntPtr hwnd = FindWindow(null, strProduct);
        //
        if (isApha)
        {
            //去边框并且透明
            SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED);
            int intExTemp = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (isAphaPenetrate)//是否透明穿透窗体
            {
                SetWindowLong(hwnd, GWL_EXSTYLE, intExTemp | WS_EX_TRANSPARENT | WS_EX_LAYERED);//https://docs.microsoft.com/en-us/windows/win32/winmsg/window-features
            }
            //
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_BORDER & ~WS_CAPTION);
            SetWindowPos(hwnd, topWindow, currentWindowX, currentWindowY, ResWidth, ResHeight, SWP_SHOWWINDOW);//窗口排序 -1置顶，0正常 https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
            var margins = new MARGINS() { cxLeftWidth = -1 };
            //
            DwmExtendFrameIntoClientArea(hwnd, ref margins);
        }
        else
        {
            //单纯去边框
            SetWindowLong(hwnd, GWL_STYLE, WS_POPUP);
            SetWindowPos(hwnd, topWindow, currentWindowX, currentWindowY, ResWidth, ResHeight, SWP_SHOWWINDOW);
        }
#endif










    }

    private void Update()
    {
      
        if (WindowTop.bounds.Intersects(mouse.GetComponent<SpriteRenderer>().bounds))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isDragging = true;
                //oldX = currentWindowX;
                //oldY = currentWindowY;
                if(GetMouseRealPos(out int x, out int y))
                {
                    oldMouseX = x;
                    oldMouseY = y;

                    RECT WinR;
                    GetWindowRect(GetActiveWindow(), out WinR);
                    currentWindowX = WinR.Left;
                    currentWindowY = WinR.Top;
                }

            }
        }
        if (WindowScale.bounds.Intersects(mouse.GetComponent<SpriteRenderer>().bounds))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isScalling = true;

                RECT WinR;
                GetWindowRect(GetActiveWindow(), out WinR);
                oldWindowWidth = WinR.Right - WinR.Left;
                oldWindowHeight = WinR.Bottom - WinR.Top;

                if (GetMouseRealPos(out int x, out int y))
                {
                    oldMouseX = x;
                    oldMouseY = y;

                    currentWindowX = WinR.Left;
                    currentWindowY = WinR.Top;
                }

            }



        }
        if (PinArea.bounds.Intersects(mouse.GetComponent<SpriteRenderer>().bounds))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(topWindow == -2)
                {
                    topWindow = -1;
                    PinArea.color = new Color(1, 0.2966457f, 0.2966457f);
                }
                else
                {
                    topWindow = -2;
                    PinArea.color = new Color(0.9245283f, 0.9245283f, 0.9245283f);
                }
                IntPtr hwnd = FindWindow(null, strProduct);
                SetWindowPos(hwnd, topWindow, currentWindowX, currentWindowY, ResWidth, ResHeight, SWP_SHOWWINDOW);
            }



        }



        



        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isDragging = false;
            isScalling = false;
            PlayerPrefs.Save();
        }

        if (isDragging)
        {
            if (GetMouseRealPos(out int newMouseX, out int newMouseY))
            {
                int offsetX = newMouseX - oldMouseX;
                int offsetY = newMouseY - oldMouseY;

                currentWindowX += offsetX;
                currentWindowY += offsetY;

                //设置窗口

                IntPtr hwnd = FindWindow(null, strProduct);

                RECT WinR;
                GetWindowRect(GetActiveWindow(), out WinR);
                ResWidth = WinR.Right - WinR.Left;
                ResHeight = WinR.Bottom - WinR.Top;

                if(currentWindowX < 0)
                {
                    currentWindowX = 0;
                }
                if (currentWindowY < 0)
                {
                    currentWindowY = 0;
                }

                if (currentWindowX + ResWidth > Screen.currentResolution.width)
                {
                    currentWindowX = Screen.currentResolution.width - ResWidth;
                }
                if (currentWindowY + ResHeight > Screen.currentResolution.height)
                {
                    currentWindowY = Screen.currentResolution.height - ResHeight;
                }




                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_BORDER & ~WS_CAPTION);
                SetWindowPos(hwnd, topWindow, currentWindowX, currentWindowY, ResWidth, ResHeight, SWP_SHOWWINDOW);
                var margins = new MARGINS() { cxLeftWidth = -1 };
                //
                DwmExtendFrameIntoClientArea(hwnd, ref margins);

                //每一帧变动
                oldMouseX = newMouseX;
                oldMouseY = newMouseY;

                Debug.Log(currentWindowX + " " + currentWindowY);

                PlayerPrefs.SetInt("WindowX", currentWindowX);
                PlayerPrefs.SetInt("WindowY", currentWindowY);
            }


        }
        if (isScalling)
        {
            if (GetMouseRealPos(out int newMouseX, out int newMouseY))
            {
                int offsetY = newMouseY - oldMouseY;
                int offsetX = newMouseX - oldMouseX;

                //设置窗口
                IntPtr hwnd = FindWindow(null, strProduct);

                //RECT WinR;
                //GetWindowRect(GetActiveWindow(), out WinR);

                ResWidth = oldWindowWidth + offsetX;
                ResHeight = oldWindowHeight + offsetY;

                int minWidth = 500;
                if(ResWidth < minWidth)
                {
                    ResWidth = minWidth;
                    ResHeight = (int)(minWidth / 16f * 9f);
                }
                int maxWidth = Screen.currentResolution.width;
                if (ResWidth > maxWidth)
                {
                    ResWidth = maxWidth;
                    ResHeight = (int)(maxWidth / 16f * 9f);
                }



                if (ResWidth / 16f * 9f > ResHeight)
                {
                    ResHeight = (int)(ResWidth / 16f * 9f);
                }
                else
                {
                    ResWidth = (int)(ResHeight / 9f * 16f);
                }
               

                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_BORDER & ~WS_CAPTION);
                SetWindowPos(hwnd, topWindow, currentWindowX, currentWindowY, ResWidth, ResHeight, SWP_SHOWWINDOW);
                var margins = new MARGINS() { cxLeftWidth = -1 };
                //
                DwmExtendFrameIntoClientArea(hwnd, ref margins);

                //这里的old指的是拖动前的
                //oldMouseX = newMouseX;
                //oldMouseY = newMouseY;

                PlayerPrefs.SetInt("WindowH", ResHeight);
                PlayerPrefs.SetInt("WindowW", ResWidth);
            }


        }



        if (GetCursorPos(out POINT lpPoint))
        {
            //Debug.Log(lpPoint.X + " " + lpPoint.Y);



        }
        else
        {
            Debug.Log("没有鼠标坐标");

        }

    }




    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
    [StructLayout(LayoutKind.Sequential)]


    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }



    public bool GetMouseRealPos(out int x, out int y)
    {
        x = 0;
        y = 0;
        if (GetCursorPos(out POINT lpPoint))
        {
            x = lpPoint.X;
            y = lpPoint.Y;
            return true;



        }

        return false;
    }


    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);


    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }







    //最小化
    //https://blog.csdn.net/yf160702/article/details/118695097


    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    //[DllImport("user32.dll")]
    //static extern IntPtr GetForegroundWindow();

    const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}
    const int SW_SHOWMAXIMIZED = 3;//最大化
    const int SW_SHOWRESTORE = 1;//还原

    public void OnClickMinimize()
    { //最小化 
      //Debug.Log("GetForegroundWindow = " + GetForegroundWindow() + " ParenthWnd =  " + ParenthWnd);
      // ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
        IntPtr hwnd = FindWindow(null, strProduct);
        ShowWindow(hwnd, SW_SHOWMINIMIZED);

        //Invoke("OnClickMaximize", 5.0f) ;
    }

    /**public void OnClickMaximize()
    {
        //最大化
        //ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
        IntPtr hwnd = FindWindow(null, strProduct);
        ShowWindow(hwnd, SW_SHOWMAXIMIZED);
    }

    public void OnClickRestore()
    {
        //还原
        ShowWindow(GetForegroundWindow(), SW_SHOWRESTORE);
    }*/

    public void OnClose()
    {
        IntPtr hwnd = FindWindow(null, strProduct);
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP);
        Application.Quit();

        //System.Environment.Exit(0);
    }
}