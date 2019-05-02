using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

    private Button pauseBtn;
    private Button playBtn;
    private Text diamondTxt;
    private Text scoreTxt;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGamePanel, ShowGamePanel); // 添加事件监听
        EventCenter.AddListener<int>(EventDefine.UpdateScoreUI, UpdateScoreUI); 
        EventCenter.AddListener<int>(EventDefine.UpdateDiamondUI, UpdateDiamondUI); 
        Init();
    }

    private void Init()
    {
        pauseBtn = transform.Find("PauseBtn").GetComponent<Button>();
        pauseBtn.onClick.AddListener(OnPauseButtonClick);

        playBtn = transform.Find("PlayBtn").GetComponent<Button>();
        playBtn.onClick.AddListener(OnPlayButtonClick);
        playBtn.gameObject.SetActive(false); // 默认隐藏开始按钮

        diamondTxt = transform.Find("Damond/DamondTxt").GetComponent<Text>();
        scoreTxt = transform.Find("ScoreTxt").GetComponent<Text>();

        gameObject.SetActive(false); // 默认隐藏游戏界面
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, ShowGamePanel); // 移除事件监听
        EventCenter.RemoveListener<int>(EventDefine.UpdateScoreUI, UpdateScoreUI);
        EventCenter.RemoveListener<int>(EventDefine.UpdateDiamondUI, UpdateDiamondUI);
    }

    private void ShowGamePanel()
    {
        gameObject.SetActive(true);
    }

    private void OnPauseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        Time.timeScale = 0; // 暂停游戏
        GameManager.Instance.IsPause = true;
    }

    private void OnPlayButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
        Time.timeScale = 1; // 开始游戏
        GameManager.Instance.IsPause = false;
    }

    /// <summary>
    /// 更新分数显示
    /// </summary>
    private void UpdateScoreUI(int score)
    {
        scoreTxt.text = score.ToString();
    }

    private void UpdateDiamondUI(int diamond)
    {
        diamondTxt.text = diamond.ToString();
    }
}
