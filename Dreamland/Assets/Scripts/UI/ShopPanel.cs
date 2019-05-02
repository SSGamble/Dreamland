using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPanel : MonoBehaviour {

    private ManagerVars vars;
    private Transform parent;
    private Text nameTxt;
    private Text diamondTxt;
    private Button backBtn;
    private Button selectBtn; // 选择当前皮肤
    private Button buyBtn; // 买皮肤
    private int currentIndex; // 当前选中的皮肤下标

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowShopPanel, ShowShopPanel);
        parent = transform.Find("ScroolRect/Parent");
        diamondTxt = transform.Find("Diamond/DiamongImg/DiamondTxt").GetComponent<Text>();
        nameTxt = transform.Find("NameTxt").GetComponent<Text>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(OnBackBtnClick);
        selectBtn = transform.Find("SelectBtn").GetComponent<Button>();
        selectBtn.onClick.AddListener(OnSelectButtonClick);
        buyBtn = transform.Find("BuyBtn").GetComponent<Button>();
        buyBtn.onClick.AddListener(OnBuyButtonClick);
        vars = ManagerVars.GetManagerVars();
    }

    private void Start()
    {
        Init();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // 当前选中的皮肤下标
        currentIndex = (int)Mathf.Round(parent.transform.localPosition.x / -160.0f);
        if (Input.GetMouseButtonUp(0))
        {
            parent.transform.DOLocalMoveX(currentIndex * -160, 0.2f);
        }
        SetItemSize(currentIndex);
        UpdateUI(currentIndex);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPanel, ShowShopPanel);
    }

    private void Init()
    {
        // 动态设置滚动组件的大小
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count + 2) * 160, 300);
        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {
            GameObject go = Instantiate(vars.skinCurrentPre, parent);
            // 未解锁的皮肤呈灰色
            if (GameManager.Instance.GetSkinUnlocked(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);
        }
        // 打开 Shop 页面时，直接定位到选中的皮肤
        parent.transform.localPosition = new Vector3(GameManager.Instance.GetCurrentSkin() * -160, 0);
    }

    private void ShowShopPanel()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 返回按钮点击事件
    /// </summary>
    private void OnBackBtnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 购买按钮点击事件
    /// </summary>
    private void OnBuyButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        int price = int.Parse(buyBtn.GetComponentInChildren<Text>().text);
        if(price > GameManager.Instance.GetAllDiamond())
        {
            EventCenter.Broadcast(EventDefine.ShowHint, "钻石不足");
            //Debug.Log("钻石不足，不能购买");
            return;
        }
        GameManager.Instance.UpdateAllDiamond(-price);
        GameManager.Instance.SetSkinUnlocked(currentIndex);
        parent.GetChild(currentIndex).GetChild(0).GetComponent<Image>().color = Color.white;
    }

    /// <summary>
    /// 选择按钮点击事件
    /// </summary>
    private void OnSelectButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio); // 播放音效
        EventCenter.Broadcast(EventDefine.ChangeSkin,currentIndex);
        GameManager.Instance.SetSelectSkin(currentIndex);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 根据是否选中，设置皮肤的大小
    /// </summary>
    private void SetItemSize(int index)
    {
        // 遍历所有的子物体
        for (int i = 0; i < parent.childCount; i++)
        {
            if(index == i) // 放大当前选中的皮肤
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else // 缩小没有选中的皮肤
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }

    /// <summary>
    /// 更新 UI
    /// </summary>
    private void UpdateUI(int currentIndex)
    {
        nameTxt.text = vars.skinNameList[currentIndex];
        diamondTxt.text = GameManager.Instance.GetAllDiamond().ToString();
        // 根据皮肤的选中状态，显示相应的按钮
        if(GameManager.Instance.GetSkinUnlocked(currentIndex) == false)
        {
            selectBtn.gameObject.SetActive(false);
            buyBtn.gameObject.SetActive(true);
            buyBtn.GetComponentInChildren<Text>().text = vars.skinPrice[currentIndex].ToString(); // 价格
        }
        else
        {
            selectBtn.gameObject.SetActive(true);
            buyBtn.gameObject.SetActive(false);
        }
    }
}
