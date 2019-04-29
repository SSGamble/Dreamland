using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂载在 MainPanel 上
/// </summary>
public class MainPanel : MonoBehaviour {

    private Button startBtn;
    private Button storeBtn;
    private Button rankBtn;
    private Button soundBtn;

    private void Awake()
    {
        Init();
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
    }

    private void OnStartButtonClick()
    {
        GameManager.Instance.IsGameStart = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel); // 广播事件码
        gameObject.SetActive(false);
    }

    private void OnStoreButtonClick()
    {

    }

    private void OnRankButtonClick()
    {

    }

    private void OnSoundButtonClick()
    {

    }
}
