using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTheme : MonoBehaviour {

    private SpriteRenderer spriteRenderer; // 精灵渲染器
    private ManagerVars vars; // 管理器容器

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars(); // 获取管理器容器
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        int index = Random.Range(0, vars.bgThemeSpriteList.Count); // 随机背景图
        spriteRenderer.sprite = vars.bgThemeSpriteList[index]; // 设置背景图
    }
}
