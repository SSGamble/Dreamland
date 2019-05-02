using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankPanel : MonoBehaviour {

    public Button closeBtn;
    public Text[] scoreTxtArr;
    public GameObject scoreList;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRankPanel, ShowRankPanel);
        closeBtn.GetComponent<Image>().color = new Color(closeBtn.GetComponent<Image>().color.r, closeBtn.GetComponent<Image>().color.g, closeBtn.GetComponent<Image>().color.b, 0);
        closeBtn.onClick.AddListener(OnCloseButtonClick);
        scoreList.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, ShowRankPanel);
    }

    private void ShowRankPanel()
    {
        gameObject.SetActive(true);
        closeBtn.GetComponent<Image>().DOColor(new Color(closeBtn.GetComponent<Image>().color.r, closeBtn.GetComponent<Image>().color.g, closeBtn.GetComponent<Image>().color.b, 0.4f), 0.3f);
        scoreList.transform.DOScale(Vector3.one, 0.3f);

        // 显示排行榜数据
        int[] scoreArr = GameManager.Instance.BestScoreArr();
        for (int i = 0; i < scoreArr.Length; i++)
        {
            scoreTxtArr[i].text = scoreArr[i].ToString();
        }
    }

    private void OnCloseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        closeBtn.GetComponent<Image>().DOColor(new Color(closeBtn.GetComponent<Image>().color.r, closeBtn.GetComponent<Image>().color.g, closeBtn.GetComponent<Image>().color.b, 0f), 0.3f);
        scoreList.transform.DOScale(Vector3.zero, 0.3f).OnComplete(()=> {
            gameObject.SetActive(false);
        });
    }
}
