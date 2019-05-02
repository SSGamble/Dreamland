using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 序列化
public class GameData {

    /// <summary>
    /// 是否再来一次游戏
    /// </summary>
    public static bool IsAgainGame = false;

    private bool isFirstGame; // 是否第一次开始游戏
    private bool isMusicOn; // 音乐开关
    private int[] bestScoreArr; // 排行榜
    private int selectSkin; // 当前选择的皮肤
    private bool[] skinUnlocked; // 没有解锁的皮肤
    private int diamondCount; // 钻石

    public bool IsFirstGame
    {
        get
        {
            return isFirstGame;
        }

        set
        {
            isFirstGame = value;
        }
    }

    public bool IsMusicOn
    {
        get
        {
            return isMusicOn;
        }

        set
        {
            isMusicOn = value;
        }
    }

    public int[] BestScoreArr
    {
        get
        {
            return bestScoreArr;
        }

        set
        {
            bestScoreArr = value;
        }
    }

    public int SelectSkin
    {
        get
        {
            return selectSkin;
        }

        set
        {
            selectSkin = value;
        }
    }

    public bool[] SkinUnlocked
    {
        get
        {
            return skinUnlocked;
        }

        set
        {
            skinUnlocked = value;
        }
    }

    public int DiamondCount
    {
        get
        {
            return diamondCount;
        }

        set
        {
            diamondCount = value;
        }
    }
}
