using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour {

    // 单例
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private ManagerVars vars;

    // 游戏是否开始
    public bool IsGameStart { get; set; }
    // 游戏是否结束
	public bool IsGameOver { get; set; }
    // 游戏是否暂停
    public bool IsPause { get; set; }
    // 玩家是否开始移动
    public bool IsPlayerMove { get; set; }
    // 游戏分数
    private int score;
    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }
    // 钻石
    public int Diamond
    {
        get
        {
            return diamond;
        }

        set
        {
            diamond = value;
        }
    }
    private int diamond;

    private GameData data;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        _instance = this;
        EventCenter.AddListener(EventDefine.AddScore, AddScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddDiamond);

        if (GameData.IsAgainGame)
        {
            IsGameStart = true;
        }

        InitData();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddDiamond);
    }

    private void PlayerMove()
    {
        IsPlayerMove = true;
    }

    private void AddScore()
    {
        if (!IsGameStart || IsGameOver || IsPause)
            return;
        Score++;
        EventCenter.Broadcast(EventDefine.UpdateScoreUI, Score);
    }

    private void AddDiamond()
    {
        Diamond++;
        EventCenter.Broadcast(EventDefine.UpdateDiamondUI, Diamond);
    }

    private bool isFirstGame; // 是否第一次开始游戏
    private bool isMusicOn; // 音乐开关
    private int[] bestScoreArr; // 排行榜
    private int selectSkin; // 当前选择的皮肤
    private bool[] skinUnlocked; // 解锁的皮肤
    private int diamondCount; // 钻石总数

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitData()
    {
        Read();
        if(data != null) // 不是第一次游戏
        {
            isFirstGame = data.IsFirstGame;
        }
        else // 第一次游戏
        {
            isFirstGame = true;
        }

        // 初始化数据
        if (isFirstGame)
        {
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            diamondCount = 10;
            data = new GameData();
            Save();
        }
        else
        {
            isMusicOn = data.IsMusicOn;
            bestScoreArr = data.BestScoreArr;
            selectSkin = data.SelectSkin;
            skinUnlocked = data.SkinUnlocked;
            diamondCount = data.DiamondCount;
        }
    }

    /// <summary>
    /// 保存成绩
    /// </summary>
    /// <param name="score"></param>
    public void SaveScore(int score)
    {
        List<int> list = bestScoreArr.ToList(); // 数组 转 List
        list.Sort((x, y) => (-x.CompareTo(y))); // 从大到小排序 list
        bestScoreArr = list.ToArray(); // List 转 数组

        // 插入数据
        int index = -1;
        for (int i = 0; i < bestScoreArr.Length; i++)
        {
            if(score > bestScoreArr[i])
            {
                index = i;
            }
        }
        if (index == -1) return;
        for (int i = bestScoreArr.Length-1; i >index; i--)
        {
            bestScoreArr[i] = bestScoreArr[i - 1];
        }
        bestScoreArr[index] = score;
        Save();
    }

    /// <summary>
    /// 获取最好成绩
    /// </summary>
    /// <returns></returns>
    public int BestScore()
    {
        return bestScoreArr.Max();
    }

    /// <summary>
    /// 获取最好成绩数组
    /// </summary>
    /// <returns></returns>
    public int[] BestScoreArr()
    {
        List<int> list = bestScoreArr.ToList(); // 数组 转 List
        list.Sort((x, y) => (-x.CompareTo(y))); // 从大到小排序 list
        bestScoreArr = list.ToArray(); // List 转 数组
        return bestScoreArr;
    }

    /// <summary>
    /// 存储数据,文件
    /// </summary>
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using(FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.IsFirstGame = isFirstGame;
                data.IsMusicOn = isMusicOn;
                data.BestScoreArr = bestScoreArr;
                data.SelectSkin = selectSkin;
                data.SkinUnlocked = skinUnlocked;
                data.DiamondCount = diamondCount;

                bf.Serialize(fs, data); // 序列化
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data",FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs); // 反序列化
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 重置数据
    /// </summary>
    public void ResetDate()
    {
        isFirstGame = false;
        isMusicOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinUnlocked = new bool[vars.skinSpriteList.Count];
        skinUnlocked[0] = true;
        diamondCount = 10;
        Save();
    }

    /// <summary>
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }

    /// <summary>
    /// 设置当前皮肤解锁
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnlocked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }

    /// <summary>
    /// 获取所拥有的钻石数量
    /// </summary>
    /// <returns></returns>
    public int GetAllDiamond()
    {
        return diamondCount;
    }

    /// <summary>
    /// 更新总钻石数量
    /// </summary>
    /// <param name="value"></param>
    public void UpdateAllDiamond(int value)
    {
        diamondCount += value;
        Save();
    }

    /// <summary>
    /// 设置当前选择皮肤的下标
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectSkin(int index)
    {
        selectSkin = index;
        Save();
    }

    /// <summary>
    /// 获取当前选中的皮肤
    /// </summary>
    /// <returns></returns>
    public int GetCurrentSkin()
    {
        return selectSkin;
    }

    /// <summary>
    /// 设置音效是否开启
    /// </summary>
    /// <param name="value"></param>
    public void SetIsMusicOn(bool value)
    {
        isMusicOn = value;
        Save();
    }

    /// <summary>
    /// 获取音效是否开启
    /// </summary>
    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }
}
