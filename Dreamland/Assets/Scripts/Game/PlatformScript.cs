using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    public SpriteRenderer[] spriteRenderers;
    public GameObject obstacle; // 需要参与随机的障碍物平台，即将障碍物生成在左边或者右边

    public void Init(Sprite sprite,int obstacleDir)
    {
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }
        if (obstacleDir == 0) // 右边
        {
            if (obstacle != null)
            {
                obstacle.transform.localPosition = new Vector3(
                    -obstacle.transform.localPosition.x,
                    obstacle.transform.localPosition.y,
                    0);
            }
        }
    }
}
