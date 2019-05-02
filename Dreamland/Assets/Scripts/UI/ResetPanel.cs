using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResetPanel : MonoBehaviour {

    private Button yesBtn;
    private Button noBtn;
    private Image bgImg;
    private GameObject panel;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowResetPanel, ShowResetPanel);

        panel = transform.Find("Panel").gameObject;
        bgImg = transform.Find("Bg").GetComponent<Image>();
        yesBtn = transform.Find("Panel/YesBtn").GetComponent<Button>();
        yesBtn.onClick.AddListener(OnYesButtonClick);
        noBtn = transform.Find("Panel/NoBtn").GetComponent<Button>();
        noBtn.onClick.AddListener(OnNoButtonClick);

        bgImg.color = new Color(bgImg.color.r, bgImg.color.g, bgImg.color.b, 0);
        panel.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowResetPanel, ShowResetPanel);
    }

    private void ShowResetPanel()
    {
        gameObject.SetActive(true);
        bgImg.DOColor(new Color(bgImg.color.r, bgImg.color.g, bgImg.color.b, 0.4f), 0.3f);
        panel.transform.DOScale(Vector3.one, 0.3f);
    }

    private void OnYesButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        GameManager.Instance.ResetDate();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnNoButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        bgImg.DOColor(new Color(bgImg.color.r, bgImg.color.g, bgImg.color.b, 0), 0.3f);
        panel.transform.DOScale(Vector3.zero, 0.3f).OnComplete(()=> {
            gameObject.SetActive(false);
        });
    }
}
