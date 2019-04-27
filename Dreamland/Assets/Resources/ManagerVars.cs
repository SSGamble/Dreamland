using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理器容器
/// </summary>
// 在 ManagerVars 中声明的变量都可以在 ManagerVarsContainer 里显示
// [CreateAssetMenu(menuName = "CreatManagerVarsContainer")] // 用于生成 Asset 文件
public class ManagerVars : ScriptableObject {

    // 背景图片
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();

    // 获取 管理器容器
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
}
