using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

    public Text scoreTxt, maxScoreTxt, diamondTxt;
    public Button restartBtn, rankBtn, homeBtn;

    private void Awake()
    {
        restartBtn.onClick.AddListener(OnRestartButtonClick);
        rankBtn.onClick.AddListener(OnRankButtonClick);
        homeBtn.onClick.AddListener(OnHomeButtonClick);
        EventCenter.AddListener(EventDefine.ShowGameOverPanel, ShowGameOverPanel);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, ShowGameOverPanel);
    }

    private void ShowGameOverPanel()
    {
        scoreTxt.text = GameManager.Instance.Score.ToString();
        diamondTxt.text = "+ " + GameManager.Instance.Diamond.ToString();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    private void OnRestartButtonClick()
    {

    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void OnRankButtonClick()
    {

    }

    /// <summary>
    /// 主界面
    /// </summary>
    private void OnHomeButtonClick()
    {

    }
}
