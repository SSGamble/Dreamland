using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂载在 MainPanel 上
/// </summary>
public class MainPanel : MonoBehaviour {

    private ManagerVars vars;

    private Button startBtn;
    private Button storeBtn;
    private Button rankBtn;
    private Button soundBtn;
    private Button resetBtn;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.ShowMainPanel, ShowMainPanel);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        Init();
    }

    private void Start()
    {
        if (GameData.IsAgainGame)
        {
            EventCenter.Broadcast(EventDefine.ShowGamePanel);
            gameObject.SetActive(false);
        }
        Sound();
        ChangeSkin(GameManager.Instance.GetCurrentSkin());
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowMainPanel, ShowMainPanel);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }

    /// <summary>
    /// 初始化信息
    /// </summary>
    private void Init()
    {
        startBtn = transform.Find("StartBtn").GetComponent<Button>(); // 找到按钮
        startBtn.onClick.AddListener(OnStartButtonClick); // 添加监听事件
        storeBtn = transform.Find("BtnPanel/StoreBtn").GetComponent<Button>();
        storeBtn.onClick.AddListener(OnStoreButtonClick);
        rankBtn = transform.Find("BtnPanel/RankBtn").GetComponent<Button>();
        rankBtn.onClick.AddListener(OnRankButtonClick);
        soundBtn = transform.Find("BtnPanel/SoundBtn").GetComponent<Button>();
        soundBtn.onClick.AddListener(OnSoundButtonClick);
        resetBtn = transform.Find("BtnPanel/ResetBtn").GetComponent<Button>();
        resetBtn.onClick.AddListener(OnResetButtonClick);
    }

    private void ShowMainPanel()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void OnStartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        GameManager.Instance.IsGameStart = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel); // 广播事件码
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 商店
    /// </summary>
    private void OnStoreButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        EventCenter.Broadcast(EventDefine.ShowShopPanel);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    /// <summary>
    /// 声音开关
    /// </summary>
    private void OnSoundButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效

        GameManager.Instance.SetIsMusicOn(!GameManager.Instance.GetIsMusicOn());
        Sound();
    }

    private void Sound()
    {
        if (GameManager.Instance.GetIsMusicOn())
        {
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOn;
        }
        else
        {
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOff;
        }

        EventCenter.Broadcast(EventDefine.IsMusicOn, GameManager.Instance.GetIsMusicOn());
    }

    /// <summary>
    /// 重置游戏数据
    /// </summary>
    private void OnResetButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        EventCenter.Broadcast(EventDefine.ShowResetPanel);
    }

    /// <summary>
    /// 更换 Shop 按钮的皮肤图片
    /// </summary>
    private void ChangeSkin(int skinIndex)
    {
        storeBtn.transform.GetChild(0).GetComponent<Image>().sprite = vars.skinSpriteList[skinIndex];
    }
}
