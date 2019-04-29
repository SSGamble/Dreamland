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

    private PlatformGroupType groupType; // 平台主题
    private ManagerVars vars; // 管理器容器

    public Vector3 startSpawnPos; // 第一个平台的生成位置
    private int spawnPlatformCount; // 平台数量
    private Vector3 platformSpawnPos; // 平台生成位置
    private bool isLeftSpawn = false; // 是否是向左生成的平台，两种情况，左，右
    private bool spikeSpawnLeft; // 钉子组合平台是否生成在左边
    private Vector3 spikePlatformPos; // 钉子方向平台的位置
    private Sprite selectPlatformSprite; // 选中的平台图
    private int spikeAfterPlatformCount; // 钉子之后生成的平台数量
    private bool isSpawnSpike;

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
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }

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
        int obstacleIndex = Random.Range(0, 2); // 将障碍物生成在左边0或者右边1

        if (spawnPlatformCount >= 1) // 生成单个平台
        {
            SpawnNormalPlatform(obstacleIndex);
        }
        else if (spawnPlatformCount == 0) // 生成组合平台
        {
            int index = Random.Range(0, 3);
            if(index == 0) // 生成通用组合平台
            {
                SpawnCommonPlatformGroup(obstacleIndex);
            }
            else if(index == 1) // 生成主题组合平台
            {
                switch (groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(obstacleIndex);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(obstacleIndex);
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

                SpawnSpikePlatformp(dir, obstacleIndex);
                isSpawnSpike = true;
                spikeAfterPlatformCount = 4;
                // 在钉子后生成一些普通平台，用于迷惑玩家,下一个普通平台的位置
                if (spikeSpawnLeft) // 钉子在左边
                {
                    spikePlatformPos = new Vector3(platformSpawnPos.x - 1.65f, platformSpawnPos.y + 0.637f, 0); ;
                }
                else // 钉子在右边
                {
                    spikePlatformPos = new Vector3(platformSpawnPos.x + 1.65f, platformSpawnPos.y + 0.637f, 0); ;
                }
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
    private void SpawnNormalPlatform(int obstacleIndex)
    {
        // GameObject go = Instantiate(vars.normalPlatformPre, transform); // 初始化平台
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, obstacleIndex); // 随机平台样式
        go.SetActive(true);
    }

    /// <summary>
    /// 生成通用组合平台
    /// </summary>
    private void SpawnCommonPlatformGroup(int obstacleIndex)
    {
        // int index = Random.Range(0, vars.commonPlatformGroup.Count);
        // GameObject go = Instantiate(vars.commonPlatformGroup[index], transform);
        GameObject go = ObjectPool.Instance.GetCommonPlatform();
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, obstacleIndex);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成草地组合平台
    /// </summary>
    private void SpawnGrassPlatformGroup(int obstacleIndex)
    {
        // int index = Random.Range(0, vars.grassPlatformGroup.Count);
        // GameObject go = Instantiate(vars.grassPlatformGroup[index], transform);
        GameObject go = ObjectPool.Instance.GetGrassPlatform();
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, obstacleIndex);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成冬季组合平台
    /// </summary>
    private void SpawnWinterPlatformGroup(int obstacleIndex)
    {
        // int index = Random.Range(0, vars.winterPlatformGroup.Count);
        // GameObject go = Instantiate(vars.winterPlatformGroup[index], transform);
        GameObject go = ObjectPool.Instance.GetWinterPlatform();
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, obstacleIndex);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    private void SpawnSpikePlatformp(int dir,int obstacleIndex)
    {
        GameObject temp;
        if (dir == 0) // 钉子在右边
        {
            spikeSpawnLeft = false;
            // temp = Instantiate(vars.spikePlatformRight, transform);
            temp = ObjectPool.Instance.GeRightSpikePlatform();

        }
        else // 钉子在左边
        {
            spikeSpawnLeft = true;
            // temp = Instantiate(vars.spikePlatformLeft, transform);
            temp = ObjectPool.Instance.GeLeftSpikePlatform();
        }
        temp.transform.position = platformSpawnPos;
        temp.GetComponent<PlatformScript>().Init(selectPlatformSprite, obstacleIndex);
        temp.SetActive(true);
    }

    /// <summary>
    /// 生成钉子平台之后需要生成的平台
    /// 包括钉子方向 和 原来的方向
    /// </summary>
    private void AfterSpawnSpike()
    {
        if (spikeAfterPlatformCount > 0)
        {
            spikeAfterPlatformCount--;
            for(int i = 0; i < 2; i++)
            {
                // GameObject temp = Instantiate(vars.normalPlatformPre,transform);
                GameObject temp = ObjectPool.Instance.GetNormalPlatform();
                if (i == 0) // 生成原来方向的平台
                {
                    // 如果钉子在左边，原先的路径就是在右边
                    temp.transform.position = platformSpawnPos;
                    if (spikeSpawnLeft)
                    {
                        // 下一个平台的生成位置
                        platformSpawnPos = new Vector3(platformSpawnPos.x + vars.nextXPos,
                            platformSpawnPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        platformSpawnPos = new Vector3(platformSpawnPos.x - vars.nextXPos,
                            platformSpawnPos.y + vars.nextYPos, 0);
                    }
                }
                else // 生成钉子方向的平台
                {
                    temp.transform.position = spikePlatformPos;
                    if (spikeSpawnLeft)
                    {
                        // 下一个平台的生成位置
                        spikePlatformPos = new Vector3(spikePlatformPos.x - vars.nextXPos,
                            spikePlatformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spikePlatformPos = new Vector3(spikePlatformPos.x + vars.nextXPos,
                            spikePlatformPos.y + vars.nextYPos, 0);
                    }
                }
                temp.GetComponent<PlatformScript>().Init(selectPlatformSprite, 1);
                temp.SetActive(true);
            }
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
}
