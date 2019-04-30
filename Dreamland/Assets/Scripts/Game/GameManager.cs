using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        _instance = this;
        EventCenter.AddListener(EventDefine.AddScore, AddScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddDiamond);
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
}
