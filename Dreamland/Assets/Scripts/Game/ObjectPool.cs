using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
public class ObjectPool : MonoBehaviour {

    // 单例
    private static ObjectPool _instance;
    public static ObjectPool Instance
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

    public int initSpawnCount = 5;

    private List<GameObject> normalPlatformList = new List<GameObject>();
    private List<GameObject> commonPlatformList = new List<GameObject>();
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> winterlPlatformList = new List<GameObject>();
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    private List<GameObject> spikePlatformRightList = new List<GameObject>();
    private List<GameObject> deathEffectList = new List<GameObject>(); // 死亡特效
    private List<GameObject> diamondList = new List<GameObject>();

    private void Awake()
    {
        _instance = this;
        vars = ManagerVars.GetManagerVars();
        Init();
    }

    private void Init()
    {
        // 普通平台
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
        }

        // 通用平台
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.commonPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.commonPlatformGroup[j],ref commonPlatformList);
            }
        }

        // 草地平台
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.grassPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.grassPlatformGroup[j], ref grassPlatformList);
            }
        }

        // 冬季平台
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.winterPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.winterPlatformGroup[j], ref winterlPlatformList);
            }
        }

        // 钉子平台
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
        }

        // 死亡特效
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.deathEffect, ref deathEffectList);
        }

        // 钻石
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObject(vars.diamondPre, ref diamondList);
        }
    }

    /// <summary>
    /// 添加到列表
    /// </summary>
    /// <param name="pre"></param>
    /// <param name="list"></param>
    private GameObject InstantiateObject(GameObject pre,ref List<GameObject> list)
    {
        GameObject go = Instantiate(pre, transform);
        go.SetActive(false);
        list.Add(go);
        return go;
    }

    /// <summary>
    /// 获取单个平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetNormalPlatform()
    {
        for (int i = 0; i < normalPlatformList.Count; i++)
        {
            if(normalPlatformList[i].activeInHierarchy == false)
            {
                return normalPlatformList[i];
            }
        }
        return InstantiateObject(vars.normalPlatformPre, ref normalPlatformList);
    }

    /// <summary>
    /// 获取通用组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetCommonPlatform()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.commonPlatformGroup.Count);
        return InstantiateObject(vars.commonPlatformGroup[ran], ref commonPlatformList);
    }

    /// <summary>
    /// 获取草地组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetGrassPlatform()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.grassPlatformGroup.Count);
        return InstantiateObject(vars.grassPlatformGroup[ran], ref grassPlatformList);
    }

    /// <summary>
    /// 获取通用组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetWinterPlatform()
    {
        for (int i = 0; i < winterlPlatformList.Count; i++)
        {
            if (winterlPlatformList[i].activeInHierarchy == false)
            {
                return winterlPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.winterPlatformGroup.Count);
        return InstantiateObject(vars.winterPlatformGroup[ran], ref winterlPlatformList);
    }

    /// <summary>
    /// 获取左边钉子组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetLeftSpikePlatform()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformLeft, ref spikePlatformLeftList);
    }

    /// <summary>
    /// 获取右边钉子组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetRightSpikePlatform()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }
        return InstantiateObject(vars.spikePlatformRight, ref spikePlatformRightList);
    }

    /// <summary>
    /// 获取死亡特效
    /// </summary>
    /// <returns></returns>
    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < deathEffectList.Count; i++)
        {
            if (deathEffectList[i].activeInHierarchy == false)
            {
                return deathEffectList[i];
            }
        }
        return InstantiateObject(vars.deathEffect, ref deathEffectList);
    }

    /// <summary>
    /// 获取钻石
    /// </summary>
    /// <returns></returns>
    public GameObject GetDiamond()
    {
        for (int i = 0; i < diamondList.Count; i++)
        {
            if (diamondList[i].activeInHierarchy == false)
            {
                return diamondList[i];
            }
        }
        return InstantiateObject(vars.diamondPre, ref diamondList);
    }
}
