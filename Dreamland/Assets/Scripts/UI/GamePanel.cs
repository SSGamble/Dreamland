using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

    private Button pauseBtn;
    private Button playBtn;
    private Text damondTxt;
    private Text scoreTxt;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGamePanel, ShowGamePanel); // 添加事件监听
        Init();
    }

    private void Init()
    {
        pauseBtn = transform.Find("PauseBtn").GetComponent<Button>();
        pauseBtn.onClick.AddListener(OnPauseButtonClick);

        playBtn = transform.Find("PlayBtn").GetComponent<Button>();
        playBtn.onClick.AddListener(OnPlayButtonClick);
        playBtn.gameObject.SetActive(false); // 默认隐藏开始按钮

        damondTxt = transform.Find("Damond/DamondTxt").GetComponent<Text>();
        scoreTxt = transform.Find("ScoreTxt").GetComponent<Text>();

        gameObject.SetActive(false); // 默认隐藏游戏界面
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, ShowGamePanel); // 移除事件监听
    }

    private void ShowGamePanel()
    {
        gameObject.SetActive(true);
    }

    private void OnPauseButtonClick()
    {
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }

    private void OnPlayButtonClick()
    {
        playBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
    }
}
