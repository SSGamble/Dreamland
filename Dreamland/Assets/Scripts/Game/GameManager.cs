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

    private void Awake()
    {
        _instance = this;
    }
}
