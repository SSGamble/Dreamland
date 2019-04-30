using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    public SpriteRenderer[] spriteRenderers;
    public GameObject obstacle; // 需要参与随机的障碍物平台，即将障碍物生成在左边或者右边
    private bool isStartTimer; // 掉落的计时器
    private float fallTime; // 掉落时间
    private Rigidbody2D rigi;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStart == false || !GameManager.Instance.IsPlayerMove) return;
        if (isStartTimer)
        {
            fallTime -= Time.deltaTime;
            if (fallTime < 0) // 倒计时结束
            {
                // 掉落
                isStartTimer = false;
                if (rigi.bodyType != RigidbodyType2D.Dynamic)
                {
                    rigi.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(DealyHide());
                }
            }
        }
        if(transform.position.y - Camera.main.transform.position.y < -6)
        {
            StartCoroutine(DealyHide());
        }
    }

    public void Init(Sprite sprite,float fallTime,int obstacleDir)
    {
        rigi.bodyType = RigidbodyType2D.Static;
        this.fallTime = fallTime;
        isStartTimer = true;
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

    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
