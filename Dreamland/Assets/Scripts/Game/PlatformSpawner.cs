using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台主题枚举
/// </summary>
public enum PlatformGroupType
{
    Grass,
    Winter
}

/// <summary>
/// 平台生成类
/// </summary>
public class PlatformSpawner : MonoBehaviour {

    private ManagerVars vars; // 管理器容器

    public Vector3 startSpawnPos; // 第一个平台的生成位置
    private int spawnPlatformCount; // 平台数量
    private Vector3 platformSpawnPos; // 平台生成位置
    private bool isLeftSpawn = false; // 是否是向左生成的平台，两种情况，左，右

    private Sprite selectPlatformSprite; // 选中的平台图

    private PlatformGroupType groupType; // 平台主题

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }

    void Start () {
        RandomPlatformTheme(); // 随机平台主题
        platformSpawnPos = startSpawnPos; // 第一个平台生成的位置

        for(int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5; // 生成 5 个平台
            DecidePath(); // 确定平台的生成路径
        }

        Instantiate(vars.characterPre); // 生成 Player
	}

    /// <summary>
    /// 随机平台主题
    /// </summary>
    private void RandomPlatformTheme()
    {
        int index = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[index];

        if(index == 2)
        {
            groupType = PlatformGroupType.Grass;
        }
        else
        {
            groupType = PlatformGroupType.Winter;
        }
    }

    /// <summary>
    /// 确定平台的生成路径
    /// </summary>
    private void DecidePath()
    {
        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            isLeftSpawn = !isLeftSpawn; // 反转生成方向
            spawnPlatformCount = Random.Range(1, 4); // 随机生成几个平台
            SpawnPlatform();
        }
    }

    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        if (spawnPlatformCount >= 1) // 生成单个平台
        {
            SpawnNormalPlatform();
        }
        else if (spawnPlatformCount == 0) // 生成组合平台
        {
            int index = Random.Range(0, 3);
            if(index == 0) // 生成通用组合平台
            {
                SpawnCommonPlatformGroup();
            }
            else if(index == 1) // 生成主题组合平台
            {
                switch (groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup();
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup();
                        break;
                    default:
                        break;
                }
            }
            else // 生成带钉子的组合平台
            {
                int dir = -1; // 方向，左边钉子还是右边钉子
                if (isLeftSpawn)
                {
                    dir = 0; // 生成右边方向的钉子
                }
                else
                {
                    dir = 1; // 生成左边方向的钉子
                }
                SpawnSpikePlatformp(dir);
            }
        }

        if (isLeftSpawn) // 向左生成，-x，+y
        {
            platformSpawnPos = new Vector3(platformSpawnPos.x - vars.nextXPos, platformSpawnPos.y + vars.nextYPos, 0);
        }
        else // 向右生成，双正
        {
            platformSpawnPos = new Vector3(platformSpawnPos.x + vars.nextXPos, platformSpawnPos.y + vars.nextYPos, 0);
        }
    }

    /// <summary>
    /// 生成普通平台（单个）
    /// </summary>
    private void SpawnNormalPlatform()
    {
        GameObject go = Instantiate(vars.normalPlatformPre, transform); // 初始化平台
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite); // 随机平台样式
    }

    /// <summary>
    /// 生成通用组合平台
    /// </summary>
    private void SpawnCommonPlatformGroup()
    {
        int index = Random.Range(0, vars.commonPlatformGroup.Count);
        GameObject go = Instantiate(vars.commonPlatformGroup[index], transform);
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite);
    }

    /// <summary>
    /// 生成草地组合平台
    /// </summary>
    private void SpawnGrassPlatformGroup()
    {
        int index = Random.Range(0, vars.grassPlatformGroup.Count);
        GameObject go = Instantiate(vars.grassPlatformGroup[index], transform);
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite);
    }

    /// <summary>
    /// 生成冬季组合平台
    /// </summary>
    private void SpawnWinterPlatformGroup()
    {
        int index = Random.Range(0, vars.winterPlatformGroup.Count);
        GameObject go = Instantiate(vars.winterPlatformGroup[index], transform);
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite);
    }

    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    private void SpawnSpikePlatformp(int dir)
    {
        GameObject temp;
        if (dir == 0)
        {
            temp = Instantiate(vars.spikePlatformRight, transform);
        }
        else
        {
            temp = Instantiate(vars.spikePlatformLeft, transform);
        }
        temp.transform.position = platformSpawnPos;
        temp.GetComponent<PlatformScript>().Init(selectPlatformSprite);
    }
}
