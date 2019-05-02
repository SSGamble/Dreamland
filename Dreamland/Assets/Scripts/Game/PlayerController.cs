using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    private ManagerVars vars;

    private Rigidbody2D playerRigi;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public Transform rayDown,rayLeft,rayRight; // 玩家身上的射线监测点
    public LayerMask platformLayer,obstacleLayer; // 平台层，障碍层

    private bool isMove = false; // 开始移动了
    private bool isLeft = false; // 是否点击了左边
    private bool isJumping = false; // 是否正在跳跃
    private Vector3 nextPlatformLeft, nextPlatformRight; // 下一个平台
    private GameObject lastHitGo = null; // 防止在一个平台上广播多次事件码

    private void Awake()
    {
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        vars = ManagerVars.GetManagerVars();
        playerRigi = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ChangeSkin(GameManager.Instance.GetCurrentSkin());
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }

    /// <summary>
    /// 移动端 和 PC端 适配的，是否点击 UI 界面
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {
        // 创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        // 向点击位置发射一条射线，检测是否点击的是 UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

    void Update () {
        // 绘制射线
        //Debug.DrawRay(rayDown.position, Vector2.down * 1,Color.red);
        //Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);
        //Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);

        // 平台适配
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) // 移动端
        //{
        //    int fingerId = Input.GetTouch(0).fingerId;
        //    if (EventSystem.current.IsPointerOverGameObject(fingerId)) // 是否点击 UI 界面，移动端
        //        return;
        //}
        //else
        //{
        //    if (EventSystem.current.IsPointerOverGameObject()) // 是否点击 UI 界面，PC 端
        //        return;
        //}

        // 是否点击 UI 界面
        if (IsPointerOverGameObject(Input.mousePosition)) return;

        if (GameManager.Instance.IsGameOver || !GameManager.Instance.IsGameStart || GameManager.Instance.IsPause)
            return;

        // 鼠标监听
        if (Input.GetMouseButtonDown(0) && !isJumping && nextPlatformLeft != Vector3.zero)
        {
            if (!isMove)
            {
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true;
            }
            audioSource.PlayOneShot(vars.jump); // 播放音效
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

        // 游戏结束，正在下落，没有检测到平台
        if (playerRigi.velocity.y < 0 && !IsRayPlatform() && !GameManager.Instance.IsGameOver)
        {
            audioSource.PlayOneShot(vars.fall); // 播放音效
            // Player 处理
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            // 调用游戏结束面板
            StartCoroutine(DealyShowGameOverPanel());
        }

        // 游戏结束，障碍物检测
        if (isJumping && IsRayObstacle() && !GameManager.Instance.IsGameOver)
        {
            audioSource.PlayOneShot(vars.hit); // 播放音效
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.transform.position = transform.position;
            go.SetActive(true);
            GameManager.Instance.IsGameOver = true;
            spriteRenderer.enabled = false; // 不显示玩家
            // 调用游戏结束面板
            StartCoroutine(DealyShowGameOverPanel());
        }

        // 游戏结束，和平台一起掉下去
        if(transform.position.y-Camera.main.transform.position.y < -6 && !GameManager.Instance.IsGameOver)
        {
            audioSource.PlayOneShot(vars.fall); // 播放音效
            GameManager.Instance.IsGameOver = true;
            StartCoroutine(DealyShowGameOverPanel());
        }
    }

    IEnumerator DealyShowGameOverPanel()
    {
        yield return new WaitForSeconds(1f);
        // 调用游戏结束面板
        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
    }

    /// <summary>
    /// 更换皮肤
    /// </summary>
    /// <param name="skinIndex"></param>
    private void ChangeSkin(int skinIndex)
    {
        spriteRenderer.sprite = vars.playerSpriteList[skinIndex];
    }

    /// <summary>
    /// 是否检测到平台
    /// </summary>
    /// <returns></returns>
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null && hit.collider.tag == "Platform")
        {
            // 防止在一个平台上广播多次事件码
            if (lastHitGo != hit.collider.gameObject)
            {
                if (lastHitGo == null)
                {
                    lastHitGo = hit.collider.gameObject;
                    return true;
                }
                EventCenter.Broadcast(EventDefine.AddScore);
                lastHitGo = hit.collider.gameObject;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否检测到障碍物
    /// </summary>
    /// <returns></returns>
    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);

        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Obstacle")
            {
                print("left: " + leftHit.collider.tag);
                return true;
            }
        }

        if (rightHit.collider != null && rightHit.collider.tag == "Obstacle")
        {
            print("right: " + rightHit.collider.tag);
            return true;
        }
        return false;
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

    /// <summary>
    /// 吃钻石
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "PickUp")
        {
            audioSource.PlayOneShot(vars.diamond); // 播放音效
            EventCenter.Broadcast(EventDefine.AddDiamond);
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 音效是否开启
    /// </summary>
    /// <param name="value"></param>
    private void IsMusicOn(bool value)
    {
        audioSource.mute = !value;
    }
}
