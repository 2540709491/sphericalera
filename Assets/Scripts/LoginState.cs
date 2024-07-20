using OpenBLive.Runtime.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Task = System.Threading.Tasks.Task;

public class LoginState : MonoBehaviour
{
    // Start is called before the first frame update
    public Danmaku danmaku;
    public GameObject LoginPanel;
    public ChatManager chatManager;
    public Button 手动登录按钮;
    public TextMeshProUGUI 提示;

    private void Awake()
    {
        提示.text = "检查登录状态中......";
        提示.color = Color.yellow;
        手动登录按钮.interactable = false;
    }

    async void Start()
    {
        
        SingLogin.data1 data = SingLogin.GetBSD();
                if(await SingLogin.CheckSign())
                {
                    提示.text = "已登录,正在进入......";
                    提示.color = Color.green;
                    //await Task.Delay(1000);
                    BToken bToken = new BToken();
                    bToken.biliJct = data.bilijct;
                    bToken.sessData = data.sessData;
                    Danmaku.biliToken = bToken;
                    LoginPanel.SetActive(false);
                    danmaku.hasLogin = true;
                }
                else
                {
                    提示.text = "未登录!请扫码或手动在设置面板\\n输入bilijct和sessdata登录";
                    提示.color =new Color(1f, 123f / 255f, 0f, 1f);
                    手动登录按钮.interactable = true;
                    LoginPanel.SetActive(true);
                    danmaku.showQR();
                    
                }

            Debug.Log("sessdata:"+data.sessData);
            Debug.Log("bilijct:"+data.bilijct);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LogOut()
    {
        if (ChatManager.room != null)
        {
            ChatManager.room.Disconnect();

        }
        PlayerPrefs.SetString("sessData", null);
        PlayerPrefs.SetString("biliJct", null);
        Danmaku.biliToken.biliJct = null;
        Danmaku.biliToken.sessData = null;
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }


    public class Rootobject
    {
        public int code { get; set; }
        public string message { get; set; }
        public int ttl { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public bool isLogin { get; set; }
        public int email_verified { get; set; }
        public string face { get; set; }
        public int face_nft { get; set; }
        public Level_Info level_info { get; set; }
        public int mid { get; set; }
        public int mobile_verified { get; set; }
        public float money { get; set; }
        public int moral { get; set; }
        public Official official { get; set; }
        public Officialverify officialVerify { get; set; }
        public Pendant pendant { get; set; }
        public int scores { get; set; }
        public string uname { get; set; }
        public long vipDueDate { get; set; }
        public int vipStatus { get; set; }
        public int vipType { get; set; }
        public int vip_pay_type { get; set; }
        public int vip_theme_type { get; set; }
        public Vip_Label vip_label { get; set; }
        public int vip_avatar_subscript { get; set; }
        public string vip_nickname_color { get; set; }
        public Vip vip { get; set; }
        public Wallet wallet { get; set; }
        public bool has_shop { get; set; }
        public string shop_url { get; set; }
        public int allowance_count { get; set; }
        public int answer_status { get; set; }
        public int is_senior_member { get; set; }
    }

    public class Level_Info
    {
        public int current_level { get; set; }
        public int current_min { get; set; }
        public int current_exp { get; set; }
        public string next_exp { get; set; }
    }

    public class Official
    {
        public int role { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public int type { get; set; }
    }

    public class Officialverify
    {
        public int type { get; set; }
        public string desc { get; set; }
    }

    public class Pendant
    {
        public int pid { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public int expire { get; set; }
        public string image_enhance { get; set; }
        public string image_enhance_frame { get; set; }
    }

    public class Vip_Label
    {
        public string path { get; set; }
        public string text { get; set; }
        public string label_theme { get; set; }
        public string text_color { get; set; }
        public int bg_style { get; set; }
        public string bg_color { get; set; }
        public string border_color { get; set; }
    }

    public class Vip
    {
        public int type { get; set; }
        public int status { get; set; }
        public long due_date { get; set; }
        public int vip_pay_type { get; set; }
        public int theme_type { get; set; }
        public Label label { get; set; }
        public int avatar_subscript { get; set; }
        public string nickname_color { get; set; }
        public int role { get; set; }
        public string avatar_subscript_url { get; set; }
    }

    public class Label
    {
        public string path { get; set; }
        public string text { get; set; }
        public string label_theme { get; set; }
        public string text_color { get; set; }
        public int bg_style { get; set; }
        public string bg_color { get; set; }
        public string border_color { get; set; }
    }

    public class Wallet
    {
        public int mid { get; set; }
        public int bcoin_balance { get; set; }
        public int coupon_balance { get; set; }
        public int coupon_due_time { get; set; }
    }

}
