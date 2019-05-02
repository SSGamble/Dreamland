using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 钻石不足时的动画
/// </summary>
public class Hint : MonoBehaviour {

    private Image bgImg;
    private Text hintTxt;

    private void Awake()
    {
        bgImg = GetComponent<Image>();
        hintTxt = GetComponentInChildren<Text>();

        // 初始隐藏
        bgImg.color = new Color(bgImg.color.r, bgImg.color.g, bgImg.color.b, 0); // 透明
        hintTxt.color = new Color(hintTxt.color.r, hintTxt.color.g, hintTxt.color.b, 0); // 透明

        EventCenter.AddListener<string>(EventDefine.ShowHint, ShowHint);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.ShowHint, ShowHint);
    }

    private void ShowHint(string text)
    {
        StopCoroutine("Dealy"); // 关闭协程，防止多次点击
        transform.localPosition = new Vector3(0, -70, 0); // 初始位置
        transform.DOLocalMoveY(0, 0.3f).OnComplete(()=>{ // 移动动画
            StartCoroutine("Dealy");
        }); 
        bgImg.DOColor(new Color(bgImg.color.r, bgImg.color.g, bgImg.color.b, 0.4f), 0.1f); // 颜色渐显
        hintTxt.DOColor(new Color(hintTxt.color.r, hintTxt.color.g, hintTxt.color.b, 1), 0.1f); // 颜色渐显
    }

    IEnumerator Dealy()
    {
        yield return new WaitForSeconds(1f);
        transform.DOLocalMoveY(70, 0.3f); // 向上移动
        bgImg.DOColor(new Color(bgImg.color.r, bgImg.color.g, bgImg.color.b, 0), 0.1f); // 颜色渐隐
        hintTxt.DOColor(new Color(hintTxt.color.r, hintTxt.color.g, hintTxt.color.b, 0), 0.1f); // 颜色渐隐
    }
}
