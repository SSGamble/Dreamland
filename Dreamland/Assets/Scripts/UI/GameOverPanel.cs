using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour {

    public Text scoreTxt, maxScoreTxt, diamondTxt;
    public Button restartBtn, rankBtn, homeBtn;
    public Image newImg;

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
        if (GameManager.Instance.Score > GameManager.Instance.BestScore())
        {
            newImg.gameObject.SetActive(true);
            maxScoreTxt.text = GameManager.Instance.Score.ToString();
        }
        else { 
            newImg.gameObject.SetActive(false);
            maxScoreTxt.text = GameManager.Instance.BestScore().ToString();
        }
        GameManager.Instance.SaveScore(GameManager.Instance.Score);
        scoreTxt.text = GameManager.Instance.Score.ToString();
        diamondTxt.text = "+ " + GameManager.Instance.Diamond.ToString();
        // 更新总的钻石数量
        GameManager.Instance.UpdateAllDiamond(GameManager.Instance.Diamond);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    private void OnRestartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = true;
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
    /// 主界面
    /// </summary>
    private void OnHomeButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = false;
    }
}
