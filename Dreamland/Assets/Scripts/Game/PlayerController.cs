using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

    private ManagerVars vars;

    private bool isLeft = false; // 是否点击了左边
    private bool isJumping = false; // 是否正在跳跃
    private Vector3 nextPlatformLeft, nextPlatformRight; // 下一个平台

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
    }

    void Update () {

        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsGameStart)
            return;

        // 鼠标监听
        if (Input.GetMouseButtonDown(0) && !isJumping)
        {
            EventCenter.Broadcast(EventDefine.DecidePath);
            isJumping = true;
            Vector3 mousePos = Input.mousePosition; // 鼠标点击的位置

            if (mousePos.x <= Screen.width / 2) // 点击的是左边屏幕
            {
                isLeft = true;
            }
            else // 点击的是右边屏幕
            {
                isLeft = false;
            }
            Jump();
        }
	}

    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump()
    {
        if (isLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.DOMoveX(nextPlatformLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
        }
        else
        {
            transform.localScale = Vector3.one;
            transform.DOMoveX(nextPlatformRight.x, 0.2f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
        }
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Platform")
        {
            isJumping = false;
            Vector3 currentPlatformPos = collision.gameObject.transform.position; // 当前平台的位置
            nextPlatformLeft = new Vector3(currentPlatformPos.x - vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0); // 下一个左平台
            nextPlatformRight = new Vector3(currentPlatformPos.x + vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0); // 下一个右平台
        }
    }
}
